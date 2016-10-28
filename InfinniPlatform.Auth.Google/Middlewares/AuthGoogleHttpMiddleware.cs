using InfinniPlatform.Http.Middlewares;

using Microsoft.Owin.Security.Google;

using Owin;

namespace InfinniPlatform.Auth.Google.Middlewares
{
    /// <summary>
    /// Промежуточный слой обработки HTTP запросов приложения для аутентификации пользователя через Google.
    /// </summary>
    /// <remarks>
    /// Информация о реализации Google OAuth: "https://developers.google.com/+/api/oauth".
    /// Информация о создании проекта Google:"https://developers.google.com/console/help/#creatingdeletingprojects".
    /// После создания проекта нужно не забыть включить "Google+ API".
    /// </remarks>
    internal class AuthGoogleHttpMiddleware : HttpMiddleware
    {
        public AuthGoogleHttpMiddleware(AuthGoogleHttpMiddlewareSettings settings) : base(HttpMiddlewareType.ExternalAuthentication)
        {
            _settings = settings;
        }


        private readonly AuthGoogleHttpMiddlewareSettings _settings;


        public override void Configure(IAppBuilder builder)
        {
            if (_settings.Enable)
            {
                builder.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions
                                                {
                                                    ClientId = _settings.ClientId,
                                                    ClientSecret = _settings.ClientSecret
                                                });
            }
        }
    }
}