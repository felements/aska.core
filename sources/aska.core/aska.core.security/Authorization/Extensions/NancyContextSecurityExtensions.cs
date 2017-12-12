using System;

namespace aska.core.security.Authorization.Extensions
{
    public static class NancyContextSecurityExtensions
    {
        public static Func<NancyContext, string> GetDeviceId =
            context => context.Request.Headers[Web.Headers.DeviceId].Any()
                ? context.Request.Headers[Web.Headers.DeviceId].First().Limit(100)
                : "anonymous";
    }
}