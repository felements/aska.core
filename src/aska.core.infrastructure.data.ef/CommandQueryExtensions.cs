using aska.core.infrastructure.data.CommandQuery.Interfaces;
using aska.core.infrastructure.data.ef.Context;
using aska.core.infrastructure.data.ef.Query;
using aska.core.infrastructure.data.Store;
using Microsoft.Extensions.DependencyInjection;

namespace aska.core.infrastructure.data.ef
{
    public static class CommandQueryExtensions {
        public static data.CommandQueryExtensions.Builder WithEfDatabaseQuery<TContext>(this data.CommandQueryExtensions.Builder builder)
        {
            builder.Services.AddTransient(typeof(IQuery<,>), typeof(EfDatabaseQuery<,>));
            builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();
            //TODO: unitOfWork

            return builder;
        }
    }
}