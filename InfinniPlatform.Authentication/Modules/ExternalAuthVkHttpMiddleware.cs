using Duke.Owin.VkontakteMiddleware;

using InfinniPlatform.Http.Middlewares;
using InfinniPlatform.Sdk.Settings;

using Owin;

namespace InfinniPlatform.Authentication.Modules
{
    /// <summary>
    /// Модуль хостинга обработчика запросов к подсистеме внешней аутентификации через ВКонтакте.
    /// </summary>
    internal sealed class ExternalAuthVkHttpMiddleware : HttpMiddleware
    {
        public ExternalAuthVkHttpMiddleware(IAppConfiguration appConfiguration) : base(HttpMiddlewareType.ExternalAuthentication)
        {
            _settings = appConfiguration.GetSection<ExternalAuthVkOwinHostingModuleSettings>(ExternalAuthVkOwinHostingModuleSettings.SectionName);
        }


        private readonly ExternalAuthVkOwinHostingModuleSettings _settings;


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