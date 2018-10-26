

using System.Linq;
using kd.misc;
using kd.misc.Constants;

namespace kd.infrastructure.Store.Seed
{
    public class DataSeedProcessor
    {
        public static void Seed(IDbContext context)
        {
            AssemblyExtensions.GetDerivedTypes<IDataSeed>(Namespace.AssemblyNamePrefix)
                .OrderBy(x => (x.GetCustomAttributes(typeof (SeedOrderAttribute), false).FirstOrDefault() as SeedOrderAttribute)?.Order) //todo
                .ActionPerInstance<IDataSeed>(x => x.Seed(context));
        } 
    }
}