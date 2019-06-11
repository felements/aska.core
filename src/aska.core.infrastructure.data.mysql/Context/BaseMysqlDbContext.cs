﻿using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using aska.core.common;
using aska.core.infrastructure.data.ef.Context;
using aska.core.infrastructure.data.mysql.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using NLog;

namespace aska.core.infrastructure.data.mysql.Context
{
    public class BaseMysqlDbContext : 
        DbContext, IDbContext, IMysqlDbContextExtendedOperations, IDbContextMigrate, IDbContextMetadata
    {
        private readonly IConnectionStringProvider _connectionStringProvider;

        /// <example>
        /// connstring - "server=localhost;database=kovalevskaya_design;user id=kovalevskaya_design;password=kovalevskaya_design"
        /// </example>
        /// <param name="connectionStringProvider"></param>
        protected BaseMysqlDbContext(IConnectionStringProvider connectionStringProvider)
        {
            _connectionStringProvider = connectionStringProvider;
        }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(_connectionStringProvider.Get());
        }
       

        #region IDbContext implementation

        
        DbSet<T> IDbContext.GetDbSet<T>()
        {
            return base.Set<T>();
        }

        Task<int> IDbContext.SaveChangesAsync(CancellationToken ct)
        {
            return base.SaveChangesAsync(ct);
        }
        #endregion 

        #region mysql extended ops

        string IMysqlDbContextExtendedOperations.GetTableName<T>()
        {
            return GetTableName(typeof(T), this);
        }

        void IMysqlDbContextExtendedOperations.ExecuteRawSqlCommand(string command)
        {
            if (string.IsNullOrWhiteSpace(command)) throw new ArgumentOutOfRangeException(nameof(command));
            Database.ExecuteSqlCommand(command);
        }

        IQueryable<T> IMysqlDbContextExtendedOperations.ExecuteRawSqlQuery<T>(string query, params object[] parameters)
        {
            if (string.IsNullOrWhiteSpace(query)) throw new ArgumentOutOfRangeException(nameof(query));
            return Set<T>().FromSql(query, parameters);
        }
        
        void IMysqlDbContextExtendedOperations.TruncateTable<T>()
        {
            var tblName = GetTableName(typeof(T), this).ToLower();

            //todo:  There is an issue with sql cmd parameters formatting. Using string.format as a workaround.
            var result = this.Database.ExecuteSqlCommand(string.Format("TRUNCATE `{0}`;", tblName));
        }

        /// <summary>
        /// returns the related table name, that has been generated by EF for requesing entity type  
        /// </summary>
        /// <param name="type">Entity type</param>
        /// <param name="context">DB Context</param>
        /// <returns></returns>
        private static string GetTableName(Type type, DbContext context)
        {
            var mapping = context.Model.FindEntityType(type).Relational();
            var schema = mapping.Schema;
            var tableName = mapping.TableName;
            return tableName;
        }
        #endregion
        
        #region migration

        /// <summary>
        /// perform all migration actions
        /// </summary>
        /// <returns></returns>
        string[] IDbContextMigrate.Migrate()
        {
            var pending = Database.GetPendingMigrations();
            Database.Migrate();
            return pending.ToArray();
        }        
        #endregion

        #region metadata
        
        /// <summary>
        /// Get list of entities types attached to the db context
        /// </summary>
        public virtual Type[] GetEntityTypes() => Model.GetEntityTypes().Select(x=>x.ClrType).ToArray();

        #endregion
    }

}