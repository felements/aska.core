namespace aska.core.infrastructure.Store.Seed
{
    public interface IDataSeed
    {
        void Seed(IDbContext context);
    }
}