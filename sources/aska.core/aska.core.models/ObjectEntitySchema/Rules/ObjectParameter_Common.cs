using System;
using System.Linq.Expressions;

namespace aska.core.models.ObjectEntitySchema.Rules
{
    public partial class ObjectParameter
    {
        public static Expression<Func<ObjectEntitySchema.ObjectParameter, bool>> ActiveRule = (obj) => obj.ParameterKey != ObjectParameterKey.Unknown;
        public static Expression<Func<ObjectEntitySchema.ObjectParameter, bool>> BuiltinRule = (obj) => obj.BuiltIn;
    }
}