using System.Security.Claims;

using Microsoft.Owin;
using Microsoft.Owin.Security;

namespace InfinniPlatform.Esia.Middleware
{
	/// <summary>
	/// Настройки слоя OWIN для аутентификации пользователей с использованием Единой системы идентификации и аутентификации (ЕСИА).
	/// </summary>
	public sealed class EsiaAuthenticationOptions : AuthenticationOptions
	{
		public EsiaAuthenticationOptions()
			: base(Constants.DefaultAuthenticationType)
		{
			CallbackPath = Constants.DefaultCallbackPath;
			AuthenticationMode = AuthenticationMode.Passive;
			Description.Caption = Constants.DefaultAuthenticationType;
			StateDataFormat = Constants.DefaultStateDataFormat;
		}


		/// <summary>
		/// Адрес сервера аутентификации.
		/// </summary>
		public string Server { get; set; }

		/// <summary>
		/// Идентификатор системы клиента.
		/// </summary>
		public string ClientId { get; set; }

		/// <summary>
		/// Отпечаток сертификата с приватным ключем клиента.
		/// </summary>
		public string ClientSecretCert { get; set; }

		/// <summary>
		/// Адрес для получения ответа от сервера аутентификации.
		/// </summary>
		public PathString CallbackPath { get; set; }

		/// <summary>
		/// Тип аутентификации для создания <see cref="ClaimsIdentity"/>.
		/// </summary>
		public string SignInAsAuthenticationType { get; set; }

		/// <summary>
		/// Провайдер точек расширения процесса аутентификации.
		/// </summary>
		public EsiaAuthenticationProvider Provider { get; set; }

		/// <summary>
		/// Сервис для шифрования информации о сессии пользователя.
		/// </summary>
		public ISecureDataFormat<AuthenticationProperties> StateDataFormat { get; set; }
	}
}