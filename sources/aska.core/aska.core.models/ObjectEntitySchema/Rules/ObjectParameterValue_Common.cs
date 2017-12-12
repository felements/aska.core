using System;
using System.Linq.Expressions;

namespace aska.core.models.ObjectEntitySchema.Rules
{
    public partial class ObjectParameterValue
    {
        public static Expression<Func<ObjectEntitySchema.ObjectParameterValue, bool>> ActiveRule = (obj) => true;
    }
}