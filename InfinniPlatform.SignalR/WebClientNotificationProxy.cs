using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace InfinniPlatform.SignalR
{
	/// <summary>
	/// Клиент точки обмена для организации оповещения Web-клиентов.
	/// </summary>
	public sealed class WebClientNotificationProxy
	{
		/// <summary>
		/// Оповестить клиентов.
		/// </summary>
		/// <param name="routingKey">Ключ маршрутизации.</param>
		/// <param name="message">Сообщение.</param>
		public void Notify(string routingKey, object message)
		{
			var context = GlobalHost.ConnectionManager.GetHubContext<WebClientNotificationHub>();

			if (context != null && context.Clients != null && context.Clients.All != null)
			{
				IClientProxy clientProxy = context.Clients.All;
				clientProxy.Invoke(routingKey, message);
			}
		}
	}
}