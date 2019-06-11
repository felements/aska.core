using aska.core.infrastructure.data.CommandQuery.Interfaces;
using aska.core.infrastructure.data.ef.Context;
using aska.core.infrastructure.data.ef.Query;
using aska.core.infrastructure.data.Store;
using Microsoft.Extensions.DependencyInjection;

namespace aska.core.infrastructure.data.ef
{
    public static class CommandQueryExtensions {
        public static data.CommandQueryExtensions.Builder WithEfDatabaseQuery(this data.CommandQueryExtensions.Builder builder) 
        {
            builder.Services
                .AddTransient(typeof(IQuery<,>), typeof(EfDatabaseNoTrackingQuery<,>))
                .AddTransient<IUnitOfWork, UnitOfWork>()//todo
                .AddSingleton<IQueryableEntityProvider, DbContextQueryableEntityProvider>();//todo: check on multiple additions
            
            return builder;
        }
    }
}