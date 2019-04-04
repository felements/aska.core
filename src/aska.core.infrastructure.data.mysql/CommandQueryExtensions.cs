using System;
using aska.core.infrastructure.data.ef.Context;
using aska.core.infrastructure.data.mysql.Context;
using Microsoft.Extensions.DependencyInjection;

namespace aska.core.infrastructure.data.mysql
{
    public static class CommandQueryExtensions {
        public static data.CommandQueryExtensions.Builder WithMysqlDatabaseContext<TContext>(
            this data.CommandQueryExtensions.Builder builder, 
            Func<IServiceProvider, TContext> factory) 
            where TContext: class, IDbContext, IMysqlDbContextExtendedOperations, IDbContextMigrate
        {
            
            builder.Services.AddTransient<IDbContext, TContext>(factory);
            builder.Services.AddTransient<IMysqlDbContextExtendedOperations, TContext>(factory);
            builder.Services.AddTransient<IDbContextMigrate, TContext>(factory);

            return builder;
        }
    }
}