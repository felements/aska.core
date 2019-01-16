using aska.core.infrastructure.data.Model;

namespace aska.core.infrastructure.data.CommandQuery.Interfaces
{
    public interface ISpecification<in T>
         where T : IEntity
    {
        bool IsSatisfiedBy(T o);
    }
}