using InfinniPlatform.Http.Middlewares;

using Microsoft.Owin.Security.Facebook;

using Owin;

namespace InfinniPlatform.Auth.Facebook.Middlewares
{
    /// <summary>
    /// Промежуточный слой обработки HTTP запросов приложения для аутентификации пользователя через Facebook.
    /// </summary>
    internal class AuthFacebookHttpMiddleware : HttpMiddleware
    {
        public AuthFacebookHttpMiddleware(AuthFacebookHttpMiddlewareSettings settings) : base(HttpMiddlewareType.ExternalAuthentication)
        {
            _settings = settings;
        }


        private readonly AuthFacebookHttpMiddlewareSettings _settings;


        public override void Configure(IAppBuilder builder)
        {
            if (_settings.Enable)
            {
                builder.UseFacebookAuthentication(new FacebookAuthenticationOptions
                                                  {
                                                      AppId = _settings.ClientId,
                                                      AppSecret = _settings.ClientSecret
                                                  });
            }
        }
    }
}