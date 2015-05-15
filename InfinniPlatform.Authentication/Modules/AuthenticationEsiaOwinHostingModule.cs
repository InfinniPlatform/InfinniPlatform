using System;

using Owin;

using InfinniPlatform.Authentication.Properties;
using InfinniPlatform.Esia.Middleware;
using InfinniPlatform.Hosting;
using InfinniPlatform.Owin.Modules;

namespace InfinniPlatform.Authentication.Modules
{
	/// <summary>
	/// Модуль хостинга подсистемы аутентификации ЕСИА на базе OWIN.
	/// </summary>
	public sealed class AuthenticationEsiaOwinHostingModule : OwinHostingModule
	{
		/// <summary>
		/// Конструктор.
		/// </summary>
		/// <param name="server">Адрес сервера аутентификации ЕСИА.</param>
		/// <param name="clientId">Идентификатор системы клиента ЕСИА.</param>
		/// <param name="clientSecretCert">Отпечаток сертификата с приватным ключем клиента ЕСИА.</param>
		/// <exception cref="ArgumentNullException"></exception>
		public AuthenticationEsiaOwinHostingModule(string server, string clientId, string clientSecretCert)
		{
			if (string.IsNullOrEmpty(server))
			{
				throw new ArgumentNullException("server", Resources.AuthenticationEsiaServerCannotBeNullOrWhiteSpace);
			}

			if (string.IsNullOrEmpty(clientId))
			{
				throw new ArgumentNullException("clientId", Resources.AuthenticationEsiaClientIdCannotBeNullOrWhiteSpace);
			}

			if (string.IsNullOrEmpty(clientSecretCert))
			{
				throw new ArgumentNullException("clientSecretCert", Resources.AuthenticationEsiaClientSecretCertCannotBeNullOrWhiteSpace);
			}

			_server = server;
			_clientId = clientId;
			_clientSecretCert = clientSecretCert;
		}


		private readonly string _server;
		private readonly string _clientId;
		private readonly string _clientSecretCert;


		public override void Configure(IAppBuilder builder, IHostingContext context)
		{
			builder.UseEsiaAuthentication(new EsiaAuthenticationOptions
										  {
											  Server = _server,
											  ClientId = _clientId,
											  ClientSecretCert = _clientSecretCert
										  });
		}
	}
}