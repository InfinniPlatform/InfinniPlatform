using InfinniPlatform.Factories;

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
												 new IntegrationBusSecurityTokenValidator(),
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