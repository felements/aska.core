namespace aska.core.security.Authorization.Extensions
{
    public static class UserExtensions
    {
        public static bool IsAuthenticated(this IUserIdentity uid)
        {
            return uid != null && !string.IsNullOrWhiteSpace(uid.UserName);
        }
    }
}