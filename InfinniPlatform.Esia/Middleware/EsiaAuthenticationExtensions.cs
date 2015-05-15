using Owin;

namespace InfinniPlatform.Esia.Middleware
{
	/// <summary>
	/// Методы расширения слоя OWIN для аутентификации пользователей с использованием Единой системы идентификации и аутентификации (ЕСИА).
	/// </summary>
	public static class EsiaAuthenticationExtensions
	{
		public static IAppBuilder UseEsiaAuthentication(this IAppBuilder app, EsiaAuthenticationOptions options)
		{
			return app.Use(typeof(EsiaAuthenticationMiddleware), options, app);
		}
	}
}