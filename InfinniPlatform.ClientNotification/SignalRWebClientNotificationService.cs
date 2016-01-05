using InfinniPlatform.Core.ClientNotification;
using InfinniPlatform.SignalR;
using InfinniPlatform.SignalR.Modules;

namespace InfinniPlatform.ClientNotification
{
	/// <summary>
	/// Сервис оповещения Web-клиентов на базе ASP.NET SignalR.
	/// </summary>
	sealed class SignalRWebClientNotificationService : IWebClientNotificationService
	{
		public SignalRWebClientNotificationService()
		{
			_notificationProxy = new WebClientNotificationProxy();
		}


		private readonly WebClientNotificationProxy _notificationProxy;


		/// <summary>
		/// Оповестить клиентов.
		/// </summary>
		/// <param name="routingKey">Ключ маршрутизации.</param>
		/// <param name="message">Сообщение.</param>
		public void Notify(string routingKey, object message)
		{
			_notificationProxy.Notify(routingKey, message);
		}
	}
}