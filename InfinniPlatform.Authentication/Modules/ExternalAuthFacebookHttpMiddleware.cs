using InfinniPlatform.Http.Middlewares;
using InfinniPlatform.Sdk.Settings;

using Microsoft.Owin.Security.Facebook;

using Owin;

namespace InfinniPlatform.Authentication.Modules
{
    /// <summary>
    /// Модуль хостинга обработчика запросов к подсистеме внешней аутентификации через Facebook.
    /// </summary>
    internal sealed class ExternalAuthFacebookHttpMiddleware : HttpMiddleware
    {
        public ExternalAuthFacebookHttpMiddleware(IAppConfiguration appConfiguration) : base(HttpMiddlewareType.ExternalAuthentication)
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