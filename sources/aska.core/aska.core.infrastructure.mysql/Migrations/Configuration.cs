namespace aska.core.infrastructure.mysql.Migrations
{
    public sealed class Configuration : DbMigrationsConfiguration<Context.RegularEnityDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;

            // NOTE!
            // https://bugs.mysql.com/bug.php?id=69649
            // Prevent adding 'dbo.' prefix to the migration data
            CodeGenerator = new MySql.Data.Entity.MySqlMigrationCodeGenerator();
            SetSqlGenerator("MySql.Data.MySqlClient", new MySql.Data.Entity.MySqlMigrationSqlGenerator());
            SetHistoryContextFactory("MySql.Data.MySqlClient", (conn, schema) => new MySqlHistoryContext(conn, schema)); //here s the thing.
        }

        protected override void Seed(Context.RegularEnityDbContext context)
        {
            //  This method will be called after migrating to the latest version.
            SeedMarker s = new SeedMarker();//to force load Seed assembly
            DataSeedProcessor.Seed(context);
        }
    }
}
