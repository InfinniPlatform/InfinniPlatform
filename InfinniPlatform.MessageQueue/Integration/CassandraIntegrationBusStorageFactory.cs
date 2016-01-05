using InfinniPlatform.Cassandra;
using InfinniPlatform.Core.Factories;
using InfinniPlatform.Core.MessageQueue.Integration;
using InfinniPlatform.Core.Serialization;

namespace InfinniPlatform.MessageQueue.Integration
{
	/// <summary>
	/// Фабрика для создания хранилища подписок интеграционной шины на базе Apache Cassandra.
	/// </summary>
	public sealed class CassandraIntegrationBusStorageFactory : IIntegrationBusStorageFactory
	{
		public CassandraIntegrationBusStorageFactory(IColumnFamilyDatabaseFactory factory)
		{
			_subscriptionStorage = new CassandraIntegrationBusSubscriptionStorage(factory, JsonObjectSerializer.Default);
		}


		private readonly CassandraIntegrationBusSubscriptionStorage _subscriptionStorage;


		/// <summary>
		/// Создать хранилище подписок интеграционной шины.
		/// </summary>
		public IIntegrationBusSubscriptionStorage CreateSubscriptionStorage()
		{
			return _subscriptionStorage;
		}

		/// <summary>
		/// Создать менеджер для хранилища подписок интеграционной шины.
		/// </summary>
		public IIntegrationBusSubscriptionStorageManager CreateSubscriptionStorageManager()
		{
			return _subscriptionStorage;
		}
	}
}