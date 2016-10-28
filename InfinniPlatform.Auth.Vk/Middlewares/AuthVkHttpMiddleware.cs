using Duke.Owin.VkontakteMiddleware;

using InfinniPlatform.Http.Middlewares;

using Owin;

namespace InfinniPlatform.Auth.Vk.Middlewares
{
    /// <summary>
    /// Промежуточный слой обработки HTTP запросов приложения для аутентификации пользователя через ВКонтакте.
    /// </summary>
    internal sealed class AuthVkHttpMiddleware : HttpMiddleware
    {
        public AuthVkHttpMiddleware(AuthVkHttpMiddlewareSettings settings) : base(HttpMiddlewareType.ExternalAuthentication)
        {
            _settings = settings;
        }


        private readonly AuthVkHttpMiddlewareSettings _settings;


        public override void Configure(IAppBuilder builder)
        {
            if (_settings.Enable)
            {
                builder.UseVkontakteAuthentication(new VkAuthenticationOptions
                                                   {
                                                       AppId = _settings.ClientId,
                                                       AppSecret = _settings.ClientSecret
                                                   });
            }
        }
    }
}