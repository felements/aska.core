using kd.domainmodel.Entity;

namespace kd.infrastructure.CommandQuery.Interfaces
{
    public interface ISpecification<in T>
         where T : IEntity
    {
        bool IsSatisfiedBy(T o);
    }
}