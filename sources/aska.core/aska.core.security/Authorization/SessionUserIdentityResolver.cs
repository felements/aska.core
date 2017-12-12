using System.Collections.Generic;
using aska.core.security.Authorization.Extensions;

namespace aska.core.security.Authorization
{
    public class SessionUserIdentityResolver  : IUserIdentityResolver
    {
        private readonly IUserSessionStore _sessionStore;
        public SessionUserIdentityResolver(IUserSessionStore sessionStore)
        {
            _sessionStore = sessionStore;
        }

        /// <summary>
        /// Gets the <see cref="T:Nancy.Security.IUserIdentity" /> from username and claims.
        /// </summary>
        /// <param name="userName">The username.</param>
        /// <param name="claims">The claims.</param>
        /// <param name="context">Current <see cref="T:Nancy.NancyContext" />.</param>
        /// <returns>A populated <see cref="T:Nancy.Security.IUserIdentity" />, or <c>null</c></returns>
        public IUserIdentity GetUser(string userName, IEnumerable<string> claims, NancyContext context)
        {
            var deviceId = NancyContextSecurityExtensions.GetDeviceId(context);
            if (!_sessionStore.Get(userName, deviceId)) return null;

            return (IUserIdentity)new SessionUserIdentityResolver.TokenUserIdentity(userName, claims);
        }

        private class TokenUserIdentity : IUserIdentity
        {
            public string UserName { get; private set; }

            public IEnumerable<string> Claims { get; private set; }

            public TokenUserIdentity(string userName, IEnumerable<string> claims)
            {
                this.UserName = userName;
                this.Claims = claims;
            }
        }
    }
}