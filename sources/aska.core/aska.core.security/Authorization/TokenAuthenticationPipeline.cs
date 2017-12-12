namespace aska.core.security.Authorization
{
    public class TokenAuthenticationPipeline : IApplicationStartup
    {
        private readonly ILifetimeScope _container;
        public TokenAuthenticationPipeline(ILifetimeScope container)
        {
            _container = container;
        }

        //public void Initialize(IPipelines pipelines, NancyContext context)
        //{
        //    TokenAuthentication.Enable(pipelines, new TokenAuthenticationConfiguration(_container.Resolve<ITokenizer>()));
        //}

        public void Initialize(IPipelines pipelines)
        {
            TokenAuthentication.Enable(pipelines, new TokenAuthenticationConfiguration(_container.Resolve<ITokenizer>(), _container.Resolve<IUserIdentityResolver>()));
        }
    }
}