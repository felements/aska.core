using System;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using Google.Apis.Auth;
using kd.domainmodel.Authentication;
using kd.domainmodel.specification.User;
using kd.domainmodel.User;
using kd.infrastructure.CommandQuery.Interfaces;
using kd.misc.Constants;
using Nancy;
using NLog;

namespace kd.services.security.IdentityResolver
{
    public class GoogleIdTokenIdentityResolver : IIdentityResolver
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private const string BearerDeclaration = "Bearer";

        private readonly IQueryFactory _queryFactory;
        private readonly ICommandFactory _commandFactory;

        public GoogleIdTokenIdentityResolver(
            IQueryFactory queryFactory,
            ICommandFactory commandFactory)
        {
            _queryFactory = queryFactory;
            _commandFactory = commandFactory;
        }


        public async Task<ClaimsPrincipal> GetOrCreateUserIdentityAsync(IdTokenData token, string[] allowedAccounts)
        {
            try
            {
                GoogleJsonWebSignature.Payload validationResult = await ValidateTokenAsync(token);

                // try to find related user record
                var user = FindUserByEmail(validationResult.Email, _queryFactory);
                if (user == null)
                {
                    Logger.Warn($"Cannot find related user record for token with ID #{validationResult.Email}");
                    if (!allowedAccounts.Contains(validationResult.Email)) return null;

                    var pr = new UserPrincipal(validationResult.Email)
                    {
                        DisplayName = validationResult.Name
                    };
                    _commandFactory.GetCreateCommand<UserPrincipal>().Execute(pr);
                    Logger.Debug($"Created account record for the credential from precompiled permitted list - {token.Email} - {token.DisplayName}");
                }

                var result = new ClaimsPrincipal(new GenericIdentity(validationResult.Email));
                return result;
            }
            catch (InvalidJwtException jwtException)
            {
                Logger.Warn(
                    string.Format("Failed processing JWT for {0}: {1}", token.Email, jwtException.Message),
                    jwtException);
                return null;
            }
            catch (Exception e)
            {
                Logger.Error(e, "Something went wrong when we tried to validate Google id-token.");
                return null;
            }
        }

        /// <summary>
        /// Handles "Authentication Bearer {TOKEN}" headers to fetch identity data
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public ClaimsPrincipal GetUserIdentity(NancyContext context)
        {
            var authorizationHeader = context.Request.Headers.Authorization;
            if (string.IsNullOrWhiteSpace(authorizationHeader)) return null;

            var jwt = authorizationHeader.Substring(BearerDeclaration.Length);
            if (string.IsNullOrWhiteSpace(jwt)) return null;


            var validateTask = ValidateTokenAsync(new IdTokenData(jwt, string.Empty));
            validateTask.Wait();
            var result = validateTask.Result;
            
            return result != null ? new ClaimsPrincipal(new GenericIdentity(result.Email)) : null;
        }


        private async Task<GoogleJsonWebSignature.Payload> ValidateTokenAsync(IdTokenData token)
        {
            try
            {
                GoogleJsonWebSignature.Payload validationResult = await GoogleJsonWebSignature.ValidateAsync(token.IdToken);
                if (validationResult == null) return null;

                if ((string)validationResult.Audience != Configuration.GoogleAuthentication.ClientId
                    /*|| (!string.IsNullOrWhiteSpace(token.Id) && (string) valudationResult.Email != token.Id)*/
                    || !validationResult.EmailVerified)
                {
                    Logger.Warn($"JWT Audience mismatch - #{validationResult.Email}");
                    return null;
                }

                return validationResult;
            }
            catch (InvalidJwtException jwtException)
            {
                Logger.Warn($"Failed processing JWT for {token.Email}: {jwtException.Message}", jwtException);
                return null;
            }
        }

        private UserPrincipal FindUserByEmail(string email, IQueryFactory queryFactory)
        {
            return queryFactory.GetQuery<UserPrincipal, UserPrincipalByEmailSpecification>()
                .Where(new UserPrincipalByEmailSpecification(email))
                .Where(UserPrincipal.IsActiveRule)
                .SingleOrDefault();
        }
        
    }
}