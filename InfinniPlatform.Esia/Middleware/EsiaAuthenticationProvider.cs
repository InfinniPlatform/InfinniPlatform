using System;
using System.Threading.Tasks;

namespace InfinniPlatform.Esia.Middleware
{
	/// <summary>
	/// Провайдер точек расширения процесса аутентификации.
	/// </summary>
	public sealed class EsiaAuthenticationProvider : IEsiaAuthenticationProvider
	{
		public EsiaAuthenticationProvider()
		{
			OnAuthenticated = context => Constants.NullTask;
			OnReturnEndpoint = context => Constants.NullTask;
			OnApplyRedirect = context => context.Response.Redirect(context.RedirectUri);
		}


		/// <summary>
		/// Обработчик события успешной аутентификации пользователя.
		/// </summary>
		public Func<EsiaAuthenticatedContext, Task> OnAuthenticated { get; set; }

		/// <summary>
		/// Обработчик события перенаправления пользователя на запрашиваемый ресурс.
		/// </summary>
		public Func<EsiaReturnEndpointContext, Task> OnReturnEndpoint { get; set; }

		/// <summary>
		/// Обработчик события перенаправления пользователя на страницу аутентификации.
		/// </summary>
		public Action<EsiaApplyRedirectContext> OnApplyRedirect { get; set; }


		/// <summary>
		/// Обрабатывает событие успешной аутентификации пользователя.
		/// </summary>
		public Task Authenticated(EsiaAuthenticatedContext context)
		{
			return OnAuthenticated(context);
		}

		/// <summary>
		/// Обрабатывает событие перенаправления пользователя на запрашиваемый ресурс.
		/// </summary>
		public Task ReturnEndpoint(EsiaReturnEndpointContext context)
		{
			return OnReturnEndpoint(context);
		}

		/// <summary>
		/// Обрабатывает событие перенаправления пользователя на страницу аутентификации.
		/// </summary>
		public void ApplyRedirect(EsiaApplyRedirectContext context)
		{
			OnApplyRedirect(context);
		}
	}
}