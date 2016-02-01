using InfinniPlatform.Core.Factories;
using InfinniPlatform.Core.MessageQueue.Integration;

namespace InfinniPlatform.MessageQueue.Integration
{
	/// <summary>
	/// Фабрика для создания интеграционной шины.
	/// </summary>
	public sealed class IntegrationBusFactory : IIntegrationBusFactory
	{
		public IntegrationBusFactory(IMessageQueueFactory queueFactory, IIntegrationBusStorageFactory subscriptionStorageFactory)
		{
			_integrationBus = new IntegrationBus(queueFactory, subscriptionStorageFactory,
												 new EmptyIntegrationBusSecurityTokenValidator(),
												 new IntegrationBusSubscriptionValidator());
		}


		private readonly IIntegrationBus _integrationBus;


		/// <summary>
		/// Создать интеграционную шину.
		/// </summary>
		public IIntegrationBus CreateIntegrationBus()
		{
			return _integrationBus;
		}
	}
}