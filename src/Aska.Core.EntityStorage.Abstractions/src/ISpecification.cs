namespace Aska.Core.EntityStorage.Abstractions
{
    public interface ISpecification<in T>
    {
        bool IsSatisfiedBy(T o);
    }
}