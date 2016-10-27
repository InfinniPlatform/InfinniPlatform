using InfinniPlatform.Owin.Middleware;
using InfinniPlatform.Sdk.Hosting;
using InfinniPlatform.Sdk.Settings;

using Microsoft.Owin.Security;
using Microsoft.Owin.Security.WsFederation;

using Owin;

namespace InfinniPlatform.Authentication.Modules
{
    /// <summary>
    /// Модуль хостинга обработчика запросов к подсистеме внешней аутентификации через ADFS.
    /// </summary>
    /// <remarks>Аутентификация осуществляется через службу ADFS по протоколу WS-Federation.</remarks>
    internal sealed class ExternalAuthAdfsOwinHostingMiddleware : OwinHostingMiddleware
    {
        private const string MetadataUri = "https://{0}/FederationMetadata/2007-06/FederationMetadata.xml";


        public ExternalAuthAdfsOwinHostingMiddleware(IAppConfiguration appConfiguration) : base(HostingMiddlewareType.ExternalAuthentication)
        {
            _settings = appConfiguration.GetSection<ExternalAuthAdfsOwinHostingModuleSettings>(ExternalAuthAdfsOwinHostingModuleSettings.SectionName);
        }


        private readonly ExternalAuthAdfsOwinHostingModuleSettings _settings;


        public override void Configure(IAppBuilder builder)
        {
            if (_settings.Enable)
            {
                builder.UseWsFederationAuthentication(new WsFederationAuthenticationOptions
                {
                    Caption = WsFederationAuthenticationDefaults.Caption,
                    AuthenticationType = WsFederationAuthenticationDefaults.AuthenticationType,
                    AuthenticationMode = AuthenticationMode.Passive,
                    MetadataAddress = string.Format(MetadataUri, _settings.Server),
                    Wtrealm = _settings.ResourceUri
                });
            }
        }
    }
}