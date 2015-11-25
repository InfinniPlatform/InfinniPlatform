using InfinniPlatform.Owin.Modules;
using InfinniPlatform.Sdk.Environment.Settings;

using Microsoft.Owin.Security.Facebook;

using Owin;

namespace InfinniPlatform.Authentication.Modules
{
    /// <summary>
    /// Модуль хостинга обработчика запросов к подсистеме внешней аутентификации через Facebook.
    /// </summary>
    internal sealed class ExternalAuthFacebookOwinHostingModule : IOwinHostingModule
    {
        public OwinHostingModuleType ModuleType => OwinHostingModuleType.ExternalAuth;


        public void Configure(IAppBuilder builder, IOwinHostingContext context)
        {
            if (AppSettings.GetValue("AppServerAuthFacebookEnable", false))
            {
                var clientId = AppSettings.GetValue("AppServerAuthFacebookClientId");
                var clientSecret = AppSettings.GetValue("AppServerAuthFacebookClientSecret");

                builder.UseFacebookAuthentication(new FacebookAuthenticationOptions
                {
                    AppId = clientId,
                    AppSecret = clientSecret
                });
            }
        }
    }
}