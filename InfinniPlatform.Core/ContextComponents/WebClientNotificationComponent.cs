using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfinniPlatform.Api.ClientNotification;
using InfinniPlatform.Api.ContextComponents;
using InfinniPlatform.Factories;

namespace InfinniPlatform.ContextComponents
{
	/// <summary>
	///   Компонент для осуществления клиентской нотификации из контекста
	/// </summary>
	public sealed class WebClientNotificationComponent : IWebClientNotificationComponent
	{
		private readonly IWebClientNotificationServiceFactory _factory;

		public WebClientNotificationComponent(IWebClientNotificationServiceFactory factory)
		{
			_factory = factory;
		}

		public void Notify(string routingKey, object message)
		{
			var service = _factory.CreateClientNotificationService();
			service.Notify(routingKey,message);
		}
	}
}
