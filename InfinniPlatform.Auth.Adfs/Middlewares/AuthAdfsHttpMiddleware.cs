using InfinniPlatform.Http.Middlewares;

using Microsoft.Owin.Security;
using Microsoft.Owin.Security.WsFederation;

using Owin;

namespace InfinniPlatform.Auth.Adfs.Middlewares
{
    /// <summary>
    /// Промежуточный слой обработки HTTP запросов приложения для аутентификации пользователя через ADFS.
    /// </summary>
    /// <remarks>
    /// Аутентификация осуществляется через службу ADFS по протоколу WS-Federation.
    /// </remarks>
    internal class AuthAdfsHttpMiddleware : HttpMiddleware
    {
        private const string MetadataUri = "https://{0}/FederationMetadata/2007-06/FederationMetadata.xml";


        public AuthAdfsHttpMiddleware(AuthAdfsHttpMiddlewareSettings settings) : base(HttpMiddlewareType.ExternalAuthentication)
        {
            _settings = settings;
        }


        private readonly AuthAdfsHttpMiddlewareSettings _settings;


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