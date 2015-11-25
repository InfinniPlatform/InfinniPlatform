using InfinniPlatform.Esia.Middleware;
using InfinniPlatform.Owin.Modules;
using InfinniPlatform.Sdk.Environment.Settings;

using Owin;

namespace InfinniPlatform.Authentication.Modules
{
    /// <summary>
    /// Модуль хостинга обработчика запросов к подсистеме внешней аутентификации через ЕСИА.
    /// </summary>
    internal sealed class ExternalAuthEsiaOwinHostingModule : IOwinHostingModule
    {
        public OwinHostingModuleType ModuleType => OwinHostingModuleType.ExternalAuth;


        public void Configure(IAppBuilder builder, IOwinHostingContext context)
        {
            if (AppSettings.GetValue("AppServerAuthEsiaEnable", false))
            {
                var server = AppSettings.GetValue("AppServerAuthEsiaServer");
                var clientId = AppSettings.GetValue("AppServerAuthEsiaClientId");
                var clientSecret = AppSettings.GetValue("AppServerAuthEsiaClientSecret");

                builder.UseEsiaAuthentication(new EsiaAuthenticationOptions
                {
                    Server = server,
                    ClientId = clientId,
                    ClientSecretCert = clientSecret
                });
            }
        }
    }
}