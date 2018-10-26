using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Autofac;
using ferriswheel.datamodel;
using ferriswheel.datamodel.Content;
using ferriswheel.datamodel.specification.Content;
using ferriswheel.infrastructure.CommandQuery.Interfaces;
using ferriswheel.services.config.BasicConfiguration;
using NLog;

namespace ferriswheel.services.config
{
    internal class MergedConfigurationProvider : IConfigurationProvider
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly ILifetimeScope _scope;
        private Dictionary<string, ConfigValueEntity> _configurationCache = new Dictionary<string, ConfigValueEntity>();
        private readonly ReaderWriterLockSlim _cacheLock = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);
        private readonly Dictionary<ConfigurationProvider, Func<ILifetimeScope, ConfigValueEntity[]>> _configurationProviders = 
            new Dictionary<ConfigurationProvider, Func<ILifetimeScope, ConfigValueEntity[]>>
        {
            // note: providers order affects the priority in case of intersections.
            {ConfigurationProvider.Precompiled, PrecompiledConfigurationSource },
            {ConfigurationProvider.File, ConfigFileConfigurationSource },
            {ConfigurationProvider.Database, DbConfigurationSource }
        };

        [Obsolete("Use with care.")]
        public MergedConfigurationProvider()
        {
            // initially loading data without access to DB, cause DB params can be stored in app config file
            // and we need to read this params before DB context initialization
            ReloadCache(ConfigurationProvider.NoDb);
        }

        public MergedConfigurationProvider(ILifetimeScope scope)
        {
            _scope = scope;
            ReloadCache(ConfigurationProvider.All);
        }

        public ConfigValueEntity Get(string key)
        {
            Args.EnsureNotEmpty(key);

            var normalizedKey = (key ?? string.Empty).ToLowerInvariant();

            _cacheLock.EnterUpgradeableReadLock();
            if (!_configurationCache.TryGetValue(normalizedKey, out var entity))
            {
                Logger.Warn("Not found configuration parameter w. key '{0}'", normalizedKey);
                entity = Set(key, string.Empty, ConfigValueType.PlainString);
            }
            _cacheLock.ExitUpgradeableReadLock();
            return entity ?? ConfigValueEntity.Empty;
        }

        public ConfigValueEntity[] Get()
        {
            _cacheLock.EnterReadLock();
            var values = _configurationCache.Values.ToArray();
            _cacheLock.ExitReadLock();
            return values;
        }

        public void Set(ConfigValueEntity value)
        {
            Args.EnsureNotNull(value, nameof(value));

            _cacheLock.EnterWriteLock();
            try
            {
                // skip modifying read-only params
                if (_configurationCache.TryGetValue(value.Key, out var existingEntity) &&
                    !existingEntity.IsEditable)
                {
                    Logger.Warn("Attempt to modify readonly parameter '{0}'", value.Key);
                    return;
                }

                var entity = _scope.Resolve<IQueryFactory>()
                                 .GetQuery<ConfigValueEntity, ConfigValueByKeySpecification>()
                                 .Where(new ConfigValueByKeySpecification(value.Key))
                                 .SingleOrDefault() ?? new ConfigValueEntity(value.Key);

                entity.Value = value.Value;
                entity.ValueType = value.ValueType;
                entity.IsEditable = true;

                _scope.Resolve<ICommandFactory>().GetUpdateCommand<ConfigValueEntity>().Execute(entity);
                Logger.Debug("Updated/created configuration value with key [{0}] == '{1}'", value.Key, value.Value);

                ReloadCache();
            }
            catch (Exception e)
            {
                Logger.Error(e, "Error while updating configuration value.");
            }
            finally
            {
                _cacheLock.ExitWriteLock();
            }
        }

        public ConfigValueEntity Set(string key, string value, ConfigValueType valueType = ConfigValueType.PlainString)
        {
            var entity = new ConfigValueEntity(key, value, true, valueType);
            Set(entity);
            return entity;
        }

        public void Reload(ConfigurationProvider enabledProviders = ConfigurationProvider.All)
        {
            _cacheLock.EnterWriteLock();
            try
            {
                ReloadCache(enabledProviders);
            }
            catch (Exception e)
            {
                Logger.Error(e, "Error while reloading configuration values.");
            }
            finally
            {
                _cacheLock.ExitWriteLock();
            }
        }

        private void ReloadCache( ConfigurationProvider enabledProviders = ConfigurationProvider.All )
        {
            Logger.Debug("Filling configuration cache using providers: {0}", enabledProviders.ToString("G"));

            if (_configurationCache.Any())
            {
                _configurationCache.Clear();
                Logger.Debug("Droppped configuration cache.");
            }

            // fetch values from all enabled providers
            var values = new List<ConfigValueEntity>();
            var providers = _configurationProviders.Where(p => enabledProviders.HasFlag(p.Key)).Select(p=>p.Value);
            foreach (var provider in providers)
                values.AddRange(provider(_scope).Where(v => values.All(e=>e.Key != v.Key)));

            _configurationCache = values.ToDictionary(k => k.Key, v => v);

            Logger.Debug("Loaded {0} configuration values using providers: {1}.", _configurationCache.Count, enabledProviders.ToString("G"));
        }


        private static readonly Func<ILifetimeScope, ConfigValueEntity[]> DbConfigurationSource = 
            (scope) => scope.Resolve<IQueryFactory>().GetQuery<ConfigValueEntity>().OrderBy(x => x.Key).All().ToArray();

        private static readonly Func<ILifetimeScope, ConfigValueEntity[]> ConfigFileConfigurationSource =
            (scope) => BasicConfigurationSection.Instance.Parameters.Select(p=> new ConfigValueEntity(p.Key, p.Value)).ToArray() ;

        private static readonly Func<ILifetimeScope, ConfigValueEntity[]> PrecompiledConfigurationSource =
            (scope) => new ConfigValueEntity[]
            {
                //new ConfigValueEntity(Configuration.GoogleAuthentication.PredefinedCredentials, ArrayString.Create("dmitry.mitichenko@gmail.com", "info@baxx.pro")), 
            };



        #region typed values

        public int GetInt(string key)
        {
            var valueRaw = this.Get(key);
            if (string.IsNullOrWhiteSpace(valueRaw)) return default(int);

            if (!int.TryParse(valueRaw.Value, out var value))
            {
                Logger.Warn("Unable to parse value of parameter '{0}' == '{1}'", key, valueRaw);
            }
            return value;
        }

        public bool GetBool(string key)
        {
            var valueRaw = this.Get(key);

            if (!bool.TryParse(valueRaw.Value, out var value))
            {
                Logger.Warn("Unable to parse value of parameter '{0}' == '{1}'", key, valueRaw);
            }
            return value;
        }

        public string GetStringOrDefault(string key, string defaultValue)
        {
            var value = Get(key);
            return string.IsNullOrWhiteSpace(value) ? defaultValue : value;
        }

        #endregion
    }
}
