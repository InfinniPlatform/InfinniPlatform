using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Provider;

namespace InfinniPlatform.Esia.Middleware
{
	/// <summary>
	/// Контекст для обработки события перенаправления пользователя на страницу аутентификации.
	/// </summary>
	public sealed class EsiaApplyRedirectContext : BaseContext<EsiaAuthenticationOptions>
	{
		public EsiaApplyRedirectContext(IOwinContext context, EsiaAuthenticationOptions options, AuthenticationProperties properties, string redirectUri)
			: base(context, options)
		{
			RedirectUri = redirectUri;
			Properties = properties;
		}

		/// <summary>
		/// Адрес страницы аутентификации.
		/// </summary>
		public string RedirectUri { get; private set; }

		/// <summary>
		/// Информация о сессии пользователя.
		/// </summary>
		public AuthenticationProperties Properties { get; private set; }
	}
}