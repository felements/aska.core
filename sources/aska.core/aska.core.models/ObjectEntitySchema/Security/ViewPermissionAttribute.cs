using System;
using aska.core.models.Variants;

namespace aska.core.models.ObjectEntitySchema.Security
{
    [AttributeUsage(AttributeTargets.Enum | AttributeTargets.Field, AllowMultiple = true)]
    public class ViewPermissionAttribute : Attribute
    {
        public ViewPermissionAttribute(UserClaim claim)
        {
            Claim = claim;
        }

        public UserClaim Claim { get; set; }
    }
}