using System;
using aska.core.infrastructure.data.ef.Context;
using aska.core.infrastructure.data.ef.Query;
using aska.core.infrastructure.data.mysql.Context;
using Microsoft.Extensions.DependencyInjection;

namespace aska.core.infrastructure.data.mysql
{
    public static class CommandQueryExtensions {
        public static data.CommandQueryExtensions.Builder WithMysqlDatabaseContext<TContext>(
            this data.CommandQueryExtensions.Builder builder) 
            where TContext: class, IDbContext, IMysqlDbContextExtendedOperations, IDbContextMigrate
        {
            builder.Services
                .AddTransient<IDbContext, TContext>()
                .AddTransient<IMysqlDbContextExtendedOperations, TContext>()
                .AddTransient<IDbContextMigrate, TContext>()
                .AddTransient<TContext>();

            
            return builder;
        }

        public static data.CommandQueryExtensions.Builder WithDefaultConnectionStringProvider(
            this data.CommandQueryExtensions.Builder builder, string name)
        {
            builder.Services.AddSingleton<IConnectionStringProvider, DefaultMysqlConnectionStringProvider>(
                provider => DefaultMysqlConnectionStringProvider.Create(name) );
            
            return builder;
        }
    }
}