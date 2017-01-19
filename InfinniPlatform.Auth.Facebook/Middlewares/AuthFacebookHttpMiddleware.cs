using InfinniPlatform.Http.Middlewares;
using Microsoft.AspNetCore.Builder;

namespace InfinniPlatform.Auth.Facebook.Middlewares
{
    /// <summary>
    ///     Промежуточный слой обработки HTTP запросов приложения для аутентификации пользователя через Facebook.
    /// </summary>
    internal class AuthFacebookHttpMiddleware : HttpMiddleware
    {
        private readonly AuthFacebookHttpMiddlewareSettings _settings;

        public AuthFacebookHttpMiddleware(AuthFacebookHttpMiddlewareSettings settings) : base(HttpMiddlewareType.ExternalAuthentication)
        {
            _settings = settings;
        }


        public override void Configure(IApplicationBuilder builder)
        {
            if (_settings.Enable)
            {
                builder.UseFacebookAuthentication(new FacebookOptions
                                                  {
                                                      AppId = _settings.ClientId,
                                                      AppSecret = _settings.ClientSecret
                                                  });
            }
        }
    }
}