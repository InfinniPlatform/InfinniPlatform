using InfinniPlatform.Owin.Middleware;
using InfinniPlatform.Sdk.Hosting;
using InfinniPlatform.Sdk.Settings;

using Microsoft.Owin.Security.Facebook;

using Owin;

namespace InfinniPlatform.Authentication.Modules
{
    /// <summary>
    /// Модуль хостинга обработчика запросов к подсистеме внешней аутентификации через Facebook.
    /// </summary>
    internal sealed class ExternalAuthFacebookOwinHostingMiddleware : OwinHostingMiddleware
    {
        public ExternalAuthFacebookOwinHostingMiddleware(IAppConfiguration appConfiguration) : base(HostingMiddlewareType.ExternalAuthentication)
        {
            _settings = appConfiguration.GetSection<ExternalAuthFacebookOwinHostingModuleSettings>(ExternalAuthFacebookOwinHostingModuleSettings.SectionName);
        }


        private readonly ExternalAuthFacebookOwinHostingModuleSettings _settings;


        public override void Configure(IAppBuilder builder)
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