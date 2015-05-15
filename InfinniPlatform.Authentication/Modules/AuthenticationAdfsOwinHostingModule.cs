using System;

using Microsoft.Owin.Security;
using Microsoft.Owin.Security.WsFederation;

using Owin;

using InfinniPlatform.Authentication.Properties;
using InfinniPlatform.Hosting;
using InfinniPlatform.Owin.Modules;

namespace InfinniPlatform.Authentication.Modules
{
    /// <summary>
    /// Модуль хостинга подсистемы аутентификации ADFS на базе OWIN.
    /// </summary>
    /// <remarks>
    /// Аутентификация осуществляется через службу ADFS по протоколу WS-Federation.
    /// </remarks>
    public sealed class AuthenticationAdfsOwinHostingModule : OwinHostingModule
    {
        public AuthenticationAdfsOwinHostingModule(string adfsServer)
        {
            if (string.IsNullOrWhiteSpace(adfsServer))
            {
                throw new ArgumentNullException("adfsServer", Resources.AuthenticationActiveDirectoryAdfsServerNameCannotBeNullOrWhiteSpace);
            }

            _adfsServer = adfsServer;
        }


        private readonly string _adfsServer;

        private const string ResourceUri = "https://InfinniPlatform";
        private const string MetadataUri = "https://{0}/FederationMetadata/2007-06/FederationMetadata.xml";


        public override void Configure(IAppBuilder builder, IHostingContext context)
        {
            builder.UseWsFederationAuthentication(new WsFederationAuthenticationOptions
                                                  {
                                                      Caption = WsFederationAuthenticationDefaults.Caption,
                                                      AuthenticationType = WsFederationAuthenticationDefaults.AuthenticationType,
                                                      AuthenticationMode = AuthenticationMode.Passive,
                                                      MetadataAddress = string.Format(MetadataUri, _adfsServer),
                                                      Wtrealm = ResourceUri
                                                  });
        }
    }
}