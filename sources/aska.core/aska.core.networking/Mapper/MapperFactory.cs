using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace aska.core.networking.Mapper
{
    public class MapperFactory
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public static IMapper Create(IEnumerable<Profile> profiles )
        {
            var mapperConfiguration = new MapperConfiguration(cfg =>
            {
                foreach (var profile in profiles)
                {
                    cfg.AddProfile(profile);
                }
            });

            var mapper = mapperConfiguration.CreateMapper();

            AssertAllSourcePropertiesMapped(mapper);
            return mapper;
        }

        public static void AssertAllSourcePropertiesMapped(IMapper mapper)
        {
            var sb = new StringBuilder();
            var typeMaps = mapper.ConfigurationProvider.GetAllTypeMaps();

            Logger.Debug("MapperFactory: verifying {0} mapper type maps...", typeMaps.Length);
            foreach (var map in typeMaps)
            {
                // Here is hack, because source member mappings are not exposed
                Type t = typeof(TypeMap);
                var configs = t.GetField("_sourceMemberConfigs", BindingFlags.Instance | BindingFlags.NonPublic);
                var mappedSourceProperties = ((IEnumerable<SourceMemberConfig>)configs.GetValue(map)).Select(m => m.SourceMember);

                var mappedProperties = map.GetPropertyMaps().Select(m => m.SourceMember).Concat(mappedSourceProperties);
                var properties = map.SourceType.GetProperties(BindingFlags.Instance | BindingFlags.Public).Where(prop => prop.GetIndexParameters().Length == 0);
                
                foreach (var propertyInfo in properties)
                {
                    if (!mappedProperties.Contains(propertyInfo))
                    {
                        sb.AppendLine(string.Format(" - property '{0}' of type '{1}' is not mapped at {2}", propertyInfo, map.SourceType, map.Profile.Name));
                    }
                }
            }

            var report = sb.ToString();
            if (string.IsNullOrWhiteSpace(report))
            {
                Logger.Debug("MapperFactory: all mapper type maps are correct");
            }
            else
            {
                Logger.Fatal("MapperFactory: found not mapped properties\n\r" + report);
                throw new Exception("MapperFactory: Found not mapped properties. Should be fixed.");
            }
        }
    }
}