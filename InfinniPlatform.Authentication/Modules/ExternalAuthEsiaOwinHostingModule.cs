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
        public ExternalAuthEsiaOwinHostingModule(IAppConfiguration appConfiguration)
        {
            _settings = appConfiguration.GetSection<ExternalAuthEsiaOwinHostingModuleSettings>(ExternalAuthEsiaOwinHostingModuleSettings.SectionName);
        }


        private readonly ExternalAuthEsiaOwinHostingModuleSettings _settings;


        public OwinHostingModuleType ModuleType => OwinHostingModuleType.ExternalAuth;


        public void Configure(IAppBuilder builder, IOwinHostingContext context)
        {
            if (_settings.Enable)
            {
                builder.UseEsiaAuthentication(new EsiaAuthenticationOptions
                {
                    Server = _settings.Server,
                    ClientId = _settings.ClientId,
                    ClientSecretCert = _settings.ClientSecret
                });
            }
        }
    }
}