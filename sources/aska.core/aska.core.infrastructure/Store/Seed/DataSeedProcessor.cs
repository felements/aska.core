namespace aska.core.infrastructure.Store.Seed
{
    public class DataSeedProcessor
    {
        public static void Seed(IDbContext context)
        {
            AssemblyExtensions.GetDerivedTypes<IDataSeed>(Namespace.AssemblyNamePrefix)
                .OrderBy(x => (x.GetCustomAttributes(typeof (SeedOrderAttribute), false).FirstOrDefault() as SeedOrderAttribute).Let(a => a.Order, 0))
                .ActionPerInstance<IDataSeed>(x => x.Seed(context));
        } 
    }
}