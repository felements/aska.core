using System.Collections.Generic;
using System.Linq;

namespace aska.core.security.Authorization.Extensions
{
    public static class RoleExtensions
    {
        public static void RequiresClaims(this INancyModule module, params UserClaim[] requiredClaims)
        {
            module.RequiresClaims(requiredClaims.Select(x=>x.ToString("G")).AsEnumerable<string>());
        }

        public static void RequiresAnyClaim(this INancyModule module, IEnumerable<UserClaim> requiredClaims)
        {
            module.RequiresAnyClaim(requiredClaims.Select(x => x.ToString("G")).AsEnumerable<string>());
        }
    }
}