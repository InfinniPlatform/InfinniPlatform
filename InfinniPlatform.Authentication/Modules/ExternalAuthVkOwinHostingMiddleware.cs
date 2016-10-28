using Duke.Owin.VkontakteMiddleware;

using InfinniPlatform.Owin.Middleware;
using InfinniPlatform.Sdk.Hosting;
using InfinniPlatform.Sdk.Settings;

using Owin;

namespace InfinniPlatform.Authentication.Modules
{
    /// <summary>
    /// Модуль хостинга обработчика запросов к подсистеме внешней аутентификации через ВКонтакте.
    /// </summary>
    internal sealed class ExternalAuthVkOwinHostingMiddleware : OwinHostingMiddleware
    {
        public ExternalAuthVkOwinHostingMiddleware(IAppConfiguration appConfiguration) : base(HostingMiddlewareType.ExternalAuthentication)
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