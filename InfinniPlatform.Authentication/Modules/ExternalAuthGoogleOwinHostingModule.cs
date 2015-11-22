using InfinniPlatform.Owin.Modules;
using InfinniPlatform.Sdk.Environment.Settings;

using Microsoft.Owin.Security.Google;

using Owin;

namespace InfinniPlatform.Authentication.Modules
{
    /// <summary>
    /// Модуль хостинга обработчика запросов к подсистеме внешней аутентификации через Google.
    /// </summary>
    /// <remarks>
    /// Информация о реализации Google OAuth: "https://developers.google.com/+/api/oauth"
    /// Информация о создании проекта Google:"https://developers.google.com/console/help/#creatingdeletingprojects"
    /// После создания проекта нужно не забыть включить "Google+ API".
    /// </remarks>
    internal sealed class ExternalAuthGoogleOwinHostingModule : IOwinHostingModule
    {
        public OwinHostingModuleType ModuleType => OwinHostingModuleType.ExternalAuth;

        public void Configure(IAppBuilder builder, IOwinHostingContext context)
        {
            if (AppSettings.GetValue("AppServerAuthGoogleEnable", false))
            {
                var clientId = AppSettings.GetValue("AppServerAuthGoogleClientId");
                var clientSecret = AppSettings.GetValue("AppServerAuthGoogleClientSecret");

                builder.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions
                {
                    ClientId = clientId,
                    ClientSecret = clientSecret
                });
            }
        }
    }
}