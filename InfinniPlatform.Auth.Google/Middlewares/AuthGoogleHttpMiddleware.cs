using InfinniPlatform.Http.Middlewares;
using Microsoft.AspNetCore.Builder;

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
        private readonly AuthGoogleHttpMiddlewareSettings _settings;

        public AuthGoogleHttpMiddleware(AuthGoogleHttpMiddlewareSettings settings) : base(HttpMiddlewareType.ExternalAuthentication)
        {
            _settings = settings;
        }


        public override void Configure(IApplicationBuilder builder)
        {
            if (_settings.Enable)
            {
                builder.UseGoogleAuthentication(new GoogleOptions
                                                {
                                                    ClientId = _settings.ClientId,
                                                    ClientSecret = _settings.ClientSecret
                                                });
            }
        }
    }
}