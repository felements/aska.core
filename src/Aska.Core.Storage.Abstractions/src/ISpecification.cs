namespace Aska.Core.Storage.Abstractions
{
    public interface ISpecification<in T>
    {
        bool IsSatisfiedBy(T o);
    }
}