namespace Aska.Core.EntityStorage.DemoApp.Specification
{
    public interface ITestEntity<out TKey>
    {
        TKey Id { get; }
    }
}