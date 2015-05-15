using InfinniPlatform.Api.ClientNotification;


namespace InfinniPlatform.Factories
{
	/// <summary>
	/// Фабрика для создания сервиса оповещения Web-клиентов.
	/// </summary>
	public interface IWebClientNotificationServiceFactory
	{
		/// <summary>
		/// Создать сервис оповещения Web-клиентов.
		/// </summary>
		IWebClientNotificationService CreateClientNotificationService();
	}
}