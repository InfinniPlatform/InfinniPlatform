using System;
using System.Security.Cryptography.X509Certificates;

using Microsoft.Owin;
using Microsoft.Owin.Logging;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataHandler;
using Microsoft.Owin.Security.DataProtection;
using Microsoft.Owin.Security.Infrastructure;

using Owin;

using InfinniPlatform.Esia.Implementation.Protocol;
using InfinniPlatform.Esia.Properties;

namespace InfinniPlatform.Esia.Middleware
{
	/// <summary>
	/// Слой OWIN для аутентификации пользователей с использованием Единой системы идентификации и аутентификации (ЕСИА).
	/// </summary>
	public sealed class EsiaAuthenticationMiddleware : AuthenticationMiddleware<EsiaAuthenticationOptions>
	{
		public EsiaAuthenticationMiddleware(OwinMiddleware next, EsiaAuthenticationOptions options, IAppBuilder app)
			: base(next, options)
		{
			// Проверка параметров

			if (options == null)
			{
				throw new ArgumentNullException("options");
			}

			if (string.IsNullOrWhiteSpace(options.Server))
			{
				throw new ArgumentException(Resources.ServerCannotBeNullOrWhiteSpace, "options");
			}

			if (string.IsNullOrWhiteSpace(options.ClientId))
			{
				throw new ArgumentException(Resources.ClientIdCannotBeNullOrWhiteSpace, "options");
			}

			if (string.IsNullOrWhiteSpace(options.ClientSecretCert))
			{
				throw new ArgumentException(Resources.ClientSecretCertCannotBeNullOrWhiteSpace, "options");
			}

			if (options.CallbackPath == null || string.IsNullOrWhiteSpace(options.CallbackPath.Value))
			{
				throw new ArgumentException(Resources.CallbackPathCannotBeNullOrWhiteSpace, "options");
			}

			// Установка значений по умолчанию

			if (string.IsNullOrEmpty(Options.SignInAsAuthenticationType))
			{
				Options.SignInAsAuthenticationType = app.GetDefaultSignInAsAuthenticationType();
			}

			if (Options.Provider == null)
			{
				Options.Provider = new EsiaAuthenticationProvider();
			}

			if (Options.StateDataFormat == null)
			{
				var dataProtector = app.CreateDataProtector(typeof(EsiaAuthenticationMiddleware).FullName, Options.AuthenticationType);

				Options.StateDataFormat = new PropertiesDataFormat(dataProtector);
			}

			// Инициализация приватных переменных

			var clientSecretCert = FindCertificate(Options.ClientSecretCert);

			_authRequestBuilder = new AuthRequestBuilder(Options.Server, Options.ClientId, clientSecretCert);
			_authResponseParser = new AuthResponseParser(clientSecretCert);
			_logger = app.CreateLogger<EsiaAuthenticationMiddleware>();
		}


		private readonly AuthRequestBuilder _authRequestBuilder;
		private readonly AuthResponseParser _authResponseParser;
		private readonly ILogger _logger;


		protected override AuthenticationHandler<EsiaAuthenticationOptions> CreateHandler()
		{
			return new EsiaAuthenticationHandler(_authRequestBuilder, _authResponseParser, _logger);
		}


		private static X509Certificate2 FindCertificate(string certificateThumbprint)
		{
			certificateThumbprint = certificateThumbprint.Trim().Replace(" ", "");

			var store = new X509Store(StoreName.My, StoreLocation.LocalMachine);
			store.Open(OpenFlags.ReadOnly);
			var certificates = store.Certificates.Find(X509FindType.FindByThumbprint, certificateThumbprint, false);
			store.Close();

			if (certificates.Count <= 0)
			{
				throw new ArgumentException(string.Format(Resources.CertificateNotFound, certificateThumbprint), "certificateThumbprint");
			}

			return certificates[0];
		}
	}
}