using Duke.Owin.VkontakteMiddleware;

using InfinniPlatform.Owin.Modules;
using InfinniPlatform.Sdk.Environment.Settings;

using Owin;

namespace InfinniPlatform.Authentication.Modules
{
    /// <summary>
    /// Модуль хостинга обработчика запросов к подсистеме внешней аутентификации через ВКонтакте.
    /// </summary>
    internal sealed class ExternalAuthVkOwinHostingModule : IOwinHostingModule
    {
        public ExternalAuthVkOwinHostingModule(IAppConfiguration appConfiguration)
        {
            _settings = appConfiguration.GetSection<ExternalAuthVkOwinHostingModuleSettings>(ExternalAuthVkOwinHostingModuleSettings.SectionName);
        }


        private readonly ExternalAuthVkOwinHostingModuleSettings _settings;


        public OwinHostingModuleType ModuleType => OwinHostingModuleType.ExternalAuth;


        public void Configure(IAppBuilder builder, IOwinHostingContext context)
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