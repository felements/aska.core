namespace kd.infrastructure.Store.Seed
{
    public interface IDataSeed
    {
        void Seed(IDbContext context);
    }
}