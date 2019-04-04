namespace aska.core.infrastructure.data.ef.Context
{
    public interface IDbContextMigrate
    {
        string[] Migrate();
    }
}