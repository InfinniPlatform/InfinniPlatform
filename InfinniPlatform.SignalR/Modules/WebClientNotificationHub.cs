using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace InfinniPlatform.SignalR.Modules
{
	/// <summary>
	/// Точка обмена для организации оповещения Web-клиентов.
	/// </summary>
	public sealed class WebClientNotificationHub : Hub
	{
		/// <summary>
		/// Оповестить клиентов.
		/// </summary>
		/// <param name="routingKey">Ключ маршрутизации.</param>
		/// <param name="message">Сообщение.</param>
		public void Notify(string routingKey, object message)
		{
			IClientProxy clientProxy = Clients.All;
			clientProxy.Invoke(routingKey, message);
		}
	}
}