using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Provider;

namespace InfinniPlatform.Esia.Middleware
{
	/// <summary>
	/// Контекст для обработки события перенаправления пользователя на запрашиваемый ресурс.
	/// </summary>
	public sealed class EsiaReturnEndpointContext : ReturnEndpointContext
	{
		public EsiaReturnEndpointContext(IOwinContext context, AuthenticationTicket ticket)
			: base(context, ticket)
		{
		}
	}
}