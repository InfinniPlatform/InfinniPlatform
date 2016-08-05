using InfinniPlatform.Owin.Modules;
using InfinniPlatform.Sdk.Logging;
using InfinniPlatform.Sdk.Settings;

using Microsoft.Owin.Security.Facebook;

using Owin;

namespace InfinniPlatform.Authentication.Modules
{
    /// <summary>
    /// Модуль хостинга обработчика запросов к подсистеме внешней аутентификации через Facebook.
    /// </summary>
    internal sealed class ExternalAuthFacebookOwinHostingModule : IOwinHostingModule
    {
        public ExternalAuthFacebookOwinHostingModule(IAppConfiguration appConfiguration)
        {
            _settings = appConfiguration.GetSection<ExternalAuthFacebookOwinHostingModuleSettings>(ExternalAuthFacebookOwinHostingModuleSettings.SectionName);
        }


        private readonly ExternalAuthFacebookOwinHostingModuleSettings _settings;


        public OwinHostingModuleType ModuleType => OwinHostingModuleType.ExternalAuth;


        public void Configure(IAppBuilder builder, IOwinHostingContext context, ILog log)
        {
            if (_settings.Enable)
            {
                builder.UseFacebookAuthentication(new FacebookAuthenticationOptions
                {
                    AppId = _settings.ClientId,
                    AppSecret = _settings.ClientSecret
                });
            }
        }
    }
}