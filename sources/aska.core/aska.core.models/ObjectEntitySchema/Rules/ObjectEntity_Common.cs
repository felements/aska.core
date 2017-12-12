using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using aska.core.models.ObjectEntitySchema.Security;

namespace aska.core.models.ObjectEntitySchema.Rules
{
    public partial class ObjectEntity
    {
        private static readonly Lazy<ObjectType[]> PublicTypes = new Lazy<ObjectType[]>(() => Enum.GetValues(typeof(ObjectType)).Cast<ObjectType>().Where(t=>EnumExtensions.GetAttributes<IsPublicAttribute>(t).Any()).ToArray(), LazyThreadSafetyMode.ExecutionAndPublication);
        
        public static Expression<Func<ObjectEntitySchema.ObjectEntity, bool>> ActiveRule = (obj) => !obj.IsDeleted;
        public static Expression<Func<ObjectEntitySchema.ObjectEntity, bool>> IsPublicRule = (obj) => !obj.IsDeleted && PublicTypes.Value.Contains(obj.Type);


        public static Expression<Func<ObjectEntitySchema.ObjectEntity, string>> SortValueSelector = (obj) => obj.Title;
    }
}