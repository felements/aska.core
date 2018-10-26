using System.Security.Claims;
using System.Threading.Tasks;
using kd.domainmodel.Authentication;
using Nancy;

namespace kd.services.security.IdentityResolver
{
    public interface IIdentityResolver
    {
        ClaimsPrincipal GetUserIdentity(NancyContext context);
        Task<ClaimsPrincipal> GetOrCreateUserIdentityAsync(IdTokenData token, string[] allowedCredentials);
    }
}