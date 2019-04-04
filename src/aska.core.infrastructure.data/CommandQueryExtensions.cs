using aska.core.infrastructure.data.CommandQuery;
using aska.core.infrastructure.data.CommandQuery.Interfaces;
using aska.core.infrastructure.data.CommandQuery.Query;
using aska.core.infrastructure.data.CommandQuery.Specification;
using Microsoft.Extensions.DependencyInjection;

namespace aska.core.infrastructure.data
{
    public static class CommandQueryExtensions {
        public static Builder AddCommandQuery(this IServiceCollection services)
        {
            services.AddTransient<IQueryFactory, QueryFactory>();
            services.AddTransient<ICommandFactory, CommandFactory>();

            services.AddTransient(typeof(IExpressionSpecification<>), typeof(ExpressionSpecification<>));
            services.AddTransient(typeof(ByIdExpressionSpecification<>));

            return new Builder(services);
        }
        
        public class Builder
        {
            public readonly IServiceCollection Services;
            
            public Builder(IServiceCollection services)
            {
                Services = services;
            }
        }
    }
}