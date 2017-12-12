namespace aska.core.infrastructure.mysql.Extensions
{
    public static class DbModelBuilderExtensions
    {
        public static void RegisterDerivedTypes<TBase>(this DbModelBuilder builder, string assembliesNamePrefix)
        {
            var entityMethod = typeof(DbModelBuilder).GetMethod("Entity");

            var types = AssemblyExtensions.GetDerivedTypes<TBase>(assembliesNamePrefix);

            foreach (var type in types)
            {
                var generic = entityMethod.MakeGenericMethod(type);
                var entity = generic.Invoke(builder, null);
            }
        }
    }
}