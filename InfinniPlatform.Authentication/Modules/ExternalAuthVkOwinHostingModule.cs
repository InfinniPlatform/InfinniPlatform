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
        public OwinHostingModuleType ModuleType => OwinHostingModuleType.ExternalAuth;


        public void Configure(IAppBuilder builder, IOwinHostingContext context)
        {
            if (AppSettings.GetValue("AppServerAuthVkEnable", false))
            {
                var clientId = AppSettings.GetValue("AppServerAuthVkClientId");
                var clientSecret = AppSettings.GetValue("AppServerAuthVkClientSecret");

                builder.UseVkontakteAuthentication(new VkAuthenticationOptions
                {
                    AppId = clientId,
                    AppSecret = clientSecret
                });
            }
        }
    }
}