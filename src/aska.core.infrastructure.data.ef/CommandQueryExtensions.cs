using aska.core.infrastructure.data.CommandQuery.Interfaces;
using aska.core.infrastructure.data.ef.Context;
using aska.core.infrastructure.data.ef.Query;
using aska.core.infrastructure.data.Store;
using Microsoft.Extensions.DependencyInjection;

namespace aska.core.infrastructure.data.ef
{
    public static class CommandQueryExtensions {
        public static data.CommandQueryExtensions.Builder WithEfDatabaseQuery<TContext>(this data.CommandQueryExtensions.Builder builder, string assemblyNamePrefix) 
            where TContext: class, IDbContext
        {
            builder.Services
                .AddTransient(typeof(IQuery<,>), typeof(EfDatabaseQuery<,>))

                .AddTransient<IUnitOfWork, UnitOfWork>()
                .AddSingleton<IDbContextEntityTypesProvider<TContext>, DefaultDbContextEntityTypesProvider<TContext>>(
                    provider => new DefaultDbContextEntityTypesProvider<TContext>(assemblyNamePrefix))
                .AddTransient<IQueryableEntityProvider, DbContextQueryableEntityProvider<TContext>>();

            //TODO: support multiple dbContexts for IQueryableEntityProvider<TContext>
            return builder;
        }
    }
}