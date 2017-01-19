using InfinniPlatform.Http.Middlewares;
using Microsoft.AspNetCore.Builder;

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


        public override void Configure(IApplicationBuilder builder)
        {
            if (_settings.Enable)
            {
                //TODO WsFederation middleware is blocked by https://github.com/dotnet/corefx/issues/1132.
                //                builder.UseWsFederationAuthentication(new WsFederationAuthenticationOptions
                //                                                      {
                //                                                          Caption = WsFederationAuthenticationDefaults.Caption,
                //                                                          AuthenticationType = WsFederationAuthenticationDefaults.AuthenticationType,
                //                                                          AuthenticationMode = AuthenticationMode.Passive,
                //                                                          MetadataAddress = string.Format(MetadataUri, _settings.Server),
                //                                                          Wtrealm = _settings.ResourceUri
                //                                                      });
            }
        }
    }
}