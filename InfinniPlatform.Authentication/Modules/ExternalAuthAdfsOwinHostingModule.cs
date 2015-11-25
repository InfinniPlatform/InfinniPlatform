using InfinniPlatform.Owin.Modules;
using InfinniPlatform.Sdk.Environment.Settings;

using Microsoft.Owin.Security;
using Microsoft.Owin.Security.WsFederation;

using Owin;

namespace InfinniPlatform.Authentication.Modules
{
    /// <summary>
    /// Модуль хостинга обработчика запросов к подсистеме внешней аутентификации через ADFS.
    /// </summary>
    /// <remarks>Аутентификация осуществляется через службу ADFS по протоколу WS-Federation.</remarks>
    internal sealed class ExternalAuthAdfsOwinHostingModule : IOwinHostingModule
    {
        private const string ResourceUri = "https://InfinniPlatform";
        private const string MetadataUri = "https://{0}/FederationMetadata/2007-06/FederationMetadata.xml";


        public OwinHostingModuleType ModuleType => OwinHostingModuleType.ExternalAuth;


        public void Configure(IAppBuilder builder, IOwinHostingContext context)
        {
            if (AppSettings.GetValue("AppServerAuthAdfsEnable", false))
            {
                var adfsServer = AppSettings.GetValue("AppServerAuthAdfsServer");

                builder.UseWsFederationAuthentication(new WsFederationAuthenticationOptions
                {
                    Caption = WsFederationAuthenticationDefaults.Caption,
                    AuthenticationType = WsFederationAuthenticationDefaults.AuthenticationType,
                    AuthenticationMode = AuthenticationMode.Passive,
                    MetadataAddress = string.Format(MetadataUri, adfsServer),
                    Wtrealm = ResourceUri
                });
            }
        }
    }
}