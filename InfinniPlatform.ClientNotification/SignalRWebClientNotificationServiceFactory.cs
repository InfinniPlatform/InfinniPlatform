using InfinniPlatform.Api.ClientNotification;
using InfinniPlatform.Factories;

namespace InfinniPlatform.ClientNotification
{
	/// <summary>
	/// Фабрика для создания сервиса оповещения Web-клиентов на базе ASP.NET SignalR.
	/// </summary>
	public sealed class SignalRWebClientNotificationServiceFactory : IWebClientNotificationServiceFactory
	{
		public SignalRWebClientNotificationServiceFactory()
		{
			_notificationService = new SignalRWebClientNotificationService();
		}


		private readonly SignalRWebClientNotificationService _notificationService;


		/// <summary>
		/// Создать сервис оповещения Web-клиентов.
		/// </summary>
		public IWebClientNotificationService CreateClientNotificationService()
		{
			return _notificationService;
		}
	}
}