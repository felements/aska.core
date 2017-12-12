using System;
using System.Linq;
using aska.core.models.User;

namespace aska.core.models.ObjectEntitySchema.Specifications
{
    public class RoleByNameSpecification : ExpressionSpecification<UserRole>
    {
        public RoleByNameSpecification(string name) : base(role => role.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase))
        {
        }
        public RoleByNameSpecification(string[] names) : base(role => names.Contains(role.Name))
        {
        }
    }
}