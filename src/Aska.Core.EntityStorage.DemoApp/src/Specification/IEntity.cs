namespace Aska.Core.EntityStorage.DemoApp.Specification
{
    public interface IEntity<out TKey>
    {
        TKey Id { get; }
    }
}