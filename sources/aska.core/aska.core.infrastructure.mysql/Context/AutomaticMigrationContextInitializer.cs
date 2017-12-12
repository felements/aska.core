using aska.core.infrastructure.mysql.Migrations;

namespace aska.core.infrastructure.mysql.Context
{
    // automatic migrations : http://habrahabr.ru/post/143292/
    public class AutomaticMigrationContextInitializer : MigrateDatabaseToLatestVersion<RegularEnityDbContext, Configuration>
    {
        public override void InitializeDatabase(RegularEnityDbContext context)
        {
            base.InitializeDatabase(context);
        }
    }
}