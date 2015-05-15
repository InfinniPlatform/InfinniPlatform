using System.Threading.Tasks;

namespace InfinniPlatform.Esia.Middleware
{
	/// <summary>
	/// Провайдер точек расширения процесса аутентификации.
	/// </summary>
	public interface IEsiaAuthenticationProvider
	{
		/// <summary>
		/// Обрабатывает событие успешной аутентификации пользователя.
		/// </summary>
		Task Authenticated(EsiaAuthenticatedContext context);

		/// <summary>
		/// Обрабатывает событие перенаправления пользователя на запрашиваемый ресурс.
		/// </summary>
		Task ReturnEndpoint(EsiaReturnEndpointContext context);

		/// <summary>
		/// Обрабатывает событие перенаправления пользователя на страницу аутентификации.
		/// </summary>
		void ApplyRedirect(EsiaApplyRedirectContext context);
	}
}