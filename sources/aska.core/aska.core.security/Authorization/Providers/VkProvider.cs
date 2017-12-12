using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using System.Security.Authentication;
using aska.core.security.Authorization.Providers.Vk;

namespace aska.core.security.Authorization.Providers
{
    // REFERENCE: https://vk.com/dev/access_token
    public class VkProvider : BaseOAuth20Provider<AccessTokenResult>
    {
        private const string BaseUrl = "https://oauth.vk.com/";
        private const string ApiUrl = "https://api.vk.com/";
        private bool _isMobile;

        public VkProvider(ProviderParams providerParams) : this("Vk", providerParams)
        {
        }

        protected VkProvider(string name, ProviderParams providerParams) : base(name, providerParams)
        {
            DisplayType = DisplayType.Unknown;
            IsMobile = false;
        }

        #region BaseOAuth20Token<AccessTokenResult> Implementation

        protected override string CreateRedirectionQuerystringParameters(Uri callbackUri, string state)
        {
            if (callbackUri == null)
            {
                throw new ArgumentNullException("callbackUri");
            }

            if (string.IsNullOrEmpty(state))
            {
                throw new ArgumentNullException("state");
            }

            var display = DisplayType == DisplayType.Unknown
                              ? string.Empty
                              : "&display=" + DisplayType.ToString().ToLowerInvariant();

            // REFERENCE: https://vk.com/dev/authcode_flow_user?f=1.%20%D0%9E%D1%82%D0%BA%D1%80%D1%8B%D1%82%D0%B8%D0%B5%20%D0%B4%D0%B8%D0%B0%D0%BB%D0%BE%D0%B3%D0%B0%20%D0%B0%D0%B2%D1%82%D0%BE%D1%80%D0%B8%D0%B7%D0%B0%D1%86%D0%B8%D0%B8
            return string.Format("client_id={0}&redirect_uri={1}{2}{3}{4}{5}{6}", PublicApiKey,
                    callbackUri.AbsoluteUri,
                    GetScope(),
                    GetQuerystringState(state),
                    display,
                    "&response_type=code",
                    "&v=5.63")
                .ToLowerInvariant();
        }


        public override RedirectToAuthenticateSettings RedirectToAuthenticate(Uri callbackUri)
        {
            if (callbackUri == (Uri)null) throw new ArgumentNullException(nameof(callbackUri));
            if (this.AuthenticateRedirectionUrl == (Uri)null) throw new SimpleAuthentication.Core.Exceptions.AuthenticationException("AuthenticationRedirectUrl has no value. Please set the authentication Url location to redirect to.");
            if (string.IsNullOrEmpty(this.PublicApiKey)) throw new SimpleAuthentication.Core.Exceptions.AuthenticationException("PublicApiKey has no value. Please set this value.");

            var state = Guid.NewGuid().ToString();
            var uriString = string.Format("{0}/authorize?{1}", (object)this.AuthenticateRedirectionUrl.AbsoluteUri, (object)this.CreateRedirectionQuerystringParameters(callbackUri, state));
            //todo: https://oauth.vk.com/authorize

            this.TraceSource.TraceInformation("Vk redirection uri: {0}.", (object)uriString);
            return new RedirectToAuthenticateSettings()
            {
                RedirectUri = new Uri(uriString),
                State = state
            };

            //return base.RedirectToAuthenticate(callbackUri);
        }


        protected override string RetrieveAuthorizationCode(NameValueCollection queryStringParameters)
        {
            if (queryStringParameters == null)
            {
                throw new ArgumentNullException("queryStringParameters");
            }

            if (queryStringParameters.Count <= 0)
            {
                throw new ArgumentOutOfRangeException("queryStringParameters");
            }

            // Is this a vk callback?
            var code = queryStringParameters["code"];

            // Maybe we have an error?
            var errorReason = queryStringParameters["error_reason"];
            var error = queryStringParameters["error"];
            var errorDescription = queryStringParameters["error_description"];
            if (!string.IsNullOrEmpty(errorReason) &&
                !string.IsNullOrEmpty(error) &&
                !string.IsNullOrEmpty(errorDescription))
            {
                var errorMessage = string.Format("Reason: {0}. Error: {1}. Description: {2}.",
                                                 string.IsNullOrEmpty(errorReason) ? "-no error reason-" : errorReason,
                                                 string.IsNullOrEmpty(error) ? "-no error-" : error,
                                                 string.IsNullOrEmpty(errorDescription)
                                                     ? "-no error description-"
                                                     : errorDescription);
                TraceSource.TraceVerbose(errorMessage);
                throw new AuthenticationException(errorMessage);
            }

            if (string.IsNullOrEmpty(code))
            {
                const string errorMessage = "No code parameter provided in the response query string from Vk.";
                TraceSource.TraceError(errorMessage);
                throw new AuthenticationException(errorMessage);
            }

            return code;
        }

        protected override IRestResponse<AccessTokenResult> ExecuteRetrieveAccessToken(string authorizationCode,
                                                                                       Uri redirectUri)
        {
            if (string.IsNullOrEmpty(authorizationCode))
            {
                throw new ArgumentNullException("authorizationCode");
            }

            if (redirectUri == null ||
                string.IsNullOrEmpty(redirectUri.AbsoluteUri))
            {
                throw new ArgumentNullException("redirectUri");
            }

            var restRequest = new RestRequest("access_token");
            restRequest.AddParameter("client_id", PublicApiKey);
            restRequest.AddParameter("client_secret", SecretApiKey);
            restRequest.AddParameter("code", authorizationCode);
            restRequest.AddParameter("redirect_uri", redirectUri.AbsoluteUri.ToLowerInvariant());
            restRequest.AddHeader("Content-Type", "application/json");
            restRequest.AddParameter("format", "json");

            var restClient = RestClientFactory.CreateRestClient(BaseUrl);
            TraceSource.TraceVerbose("Retrieving Access Token endpoint: {0}",
                                     restClient.BuildUri(restRequest).AbsoluteUri);

            return restClient.Execute<AccessTokenResult>(restRequest);
        }

        protected override AccessToken MapAccessTokenResultToAccessToken(AccessTokenResult accessTokenResult)
        {
            if (accessTokenResult == null)
            {
                throw new ArgumentNullException("accessTokenResult");
            }

            if (string.IsNullOrEmpty(accessTokenResult.access_token) ||
                accessTokenResult.expires_in <= 0)
            {
                var errorMessage =
                    string.Format(
                        "Retrieved a Vk Access Token but there's an error with either the access_token and/or expires_on parameters. Access Token: {0}. Expires In: {1}.",
                        string.IsNullOrEmpty(accessTokenResult.access_token)
                            ? "-no access token-"
                            : accessTokenResult.access_token,
                        accessTokenResult.expires_in.ToString());

                TraceSource.TraceError(errorMessage);
                throw new AuthenticationException(errorMessage);
            }

            return new AccessToken
            {
                PublicToken = accessTokenResult.access_token,
                ExpiresOn = DateTime.UtcNow.AddSeconds(accessTokenResult.expires_in)
            };
        }


        /// <summary>
        /// call VK API method users.get <see cref="https://vk.com/dev/users.get"/>
        /// </summary>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        protected override UserInformation RetrieveUserInformation(AccessToken accessToken)
        {
            if (accessToken == null)
            {
                throw new ArgumentNullException("accessToken");
            }

            if (string.IsNullOrEmpty(accessToken.PublicToken))
            {
                throw new ArgumentException("accessToken.PublicToken");
            }

            IRestResponse<ProfileInfoResult> response;

            try
            {
                var restRequest = new RestRequest("method/users.get");
                restRequest.AddParameter("access_token", accessToken.PublicToken);
                restRequest.AddParameter("v", "5.63");
                restRequest.AddParameter("fields", "photo_50,sex,bdate");

                var restClient = RestClientFactory.CreateRestClient(ApiUrl);

                TraceSource.TraceVerbose("Retrieving user information. Vk Endpoint: {0}",
                                         restClient.BuildUri(restRequest).AbsoluteUri);

                response = restClient.Execute<ProfileInfoResult>(restRequest);
            }
            catch (Exception exception)
            {
                var authenticationException =
                    new AuthenticationException("Failed to retrieve any data from the Vk Api.", exception);
                var errorMessage = authenticationException.RecursiveErrorMessages();
                TraceSource.TraceError(errorMessage);
                throw new AuthenticationException(errorMessage, exception);
            }

            if (response == null ||
                response.StatusCode != HttpStatusCode.OK ||
                response.Data == null ||
                response.Data.error != null)
            {
                var errorMessage = string.Format(
                    "Failed to obtain data from the Vk api OR the the response was not an HTTP Status 200 OK. Response Status: {0} . Response Description: {1} {3}. Error Message: {2}.",
                    response == null ? "-- null response --" : response.StatusCode.ToString(),
                    response == null ? string.Empty : response.StatusDescription ,
                    response == null
                        ? string.Empty
                        : response.ErrorException == null
                              ? "--no error exception--"
                              : response.ErrorException.RecursiveErrorMessages(),
                    response != null && response.Data != null && response.Data.error != null ? "("+ response.Data.error.error_code + ") " + response.Data.error.error_msg : string.Empty);

                TraceSource.TraceError(errorMessage);
                throw new AuthenticationException(errorMessage);
            }


            var responseData = response.Data.response[0];
            var name = (string.IsNullOrEmpty(responseData.first_name)
                            ? string.Empty
                            : responseData.first_name) + " " +
                       (string.IsNullOrEmpty(responseData.last_name)
                            ? string.Empty
                            : responseData.last_name).Trim();

            var userInformation = new UserInformation
            {
                Id = responseData.id,
                Name = name,
                //Email = response.Data.,
                //Locale = response.Data.Locale,
                //UserName = response.Data.,
                Gender = toGenderType(responseData.sex),
                Picture = responseData.photo_50
                
            };

            return userInformation;
        }

        /// <summary>
        ///   1 — женский;
        ///   2 — мужской;
        ///   0 — пол не указан. 
        /// </summary>
        private GenderType toGenderType(int type)
        {
            switch (type)
            {
                case 1: return GenderType.Female;
                case 2: return GenderType.Male;
                default: return GenderType.Unknown;
            }

        }

        #endregion


        /// <summary>
        /// Are we on a mobile device?
        /// </summary>
        /// <remarks>This will also have the side-effect of auto setting the AuthenticateRedirectionUrl property, based on this set value.</remarks>
        public bool IsMobile
        {
            get { return _isMobile; }
            set
            {
                _isMobile = value;

                // Now auto set the redirection url. 
                var url = string.Format("https://oauth.vk.com/",
                    IsMobile
                        ? "m"
                        : "www");
                AuthenticateRedirectionUrl = new Uri(url);
            }
        }

        public DisplayType DisplayType { get; set; }

        public override IEnumerable<string> DefaultScopes
        {
            get { return new string[] {/* "account", "email"*/ }; }
        }

        public override string ScopeSeparator
        {
            get { return ","; }
        }
    }
}