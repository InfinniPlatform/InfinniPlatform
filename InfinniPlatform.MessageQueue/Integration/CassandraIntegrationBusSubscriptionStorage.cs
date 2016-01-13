using System;
using System.Linq;

using InfinniPlatform.Cassandra;
using InfinniPlatform.Cassandra.DataAdapter;
using InfinniPlatform.Cassandra.Repository;
using InfinniPlatform.Cassandra.Storage;
using InfinniPlatform.Core.MessageQueue.Integration;
using InfinniPlatform.Sdk.Serialization;

namespace InfinniPlatform.MessageQueue.Integration
{
	/// <summary>
	/// Хранилище подписок интеграционной шины на базе Apache Cassandra.
	/// </summary>
	sealed class CassandraIntegrationBusSubscriptionStorage : IIntegrationBusSubscriptionStorage, IIntegrationBusSubscriptionStorageManager
	{
		public CassandraIntegrationBusSubscriptionStorage(IColumnFamilyDatabaseFactory factory, IObjectSerializer serializer)
		{
			if (factory == null)
			{
				throw new ArgumentNullException("factory");
			}

			if (serializer == null)
			{
				throw new ArgumentNullException("serializer");
			}

			_dataAdapter = factory.CreateDataAdapter();
			_repository = factory.CreateRepository();
			_storage = factory.CreateStorage();
			_serializer = serializer;
		}


		private readonly IColumnFamilyDataAdapter _dataAdapter;
		private readonly IColumnFamilyRepository _repository;
		private readonly IColumnFamilyStorage _storage;
		private readonly IObjectSerializer _serializer;

		private const string Table = "integrationbussubscriptionstorage";
		private static readonly Column IdColumn = new Column("id", ColumnType.String, true);
		private static readonly Column TimeColumn = new Column("time", ColumnType.DateTime);
		private static readonly Column ExchangeTypeColumn = new Column("exchangetype", ColumnType.String);
		private static readonly Column ExchangeNameColumn = new Column("exchangename", ColumnType.String);
		private static readonly Column QueueNameColumn = new Column("queuename", ColumnType.String);
		private static readonly Column RoutingKeyColumn = new Column("routingkey", ColumnType.String);
		private static readonly Column RoutingHeadersColumn = new Column("routingheaders", ColumnType.Blob);
		private static readonly Column AddressColumn = new Column("address", ColumnType.String);


		private static readonly SelectQueryStatement SelectSubscriptionAddressQuery
			= new SelectQueryStatement
				  {
					  Table = Table,
					  Columns = new[] { AddressColumn.Name },
					  Where = new[] { new KeyFilter(IdColumn.Name) }
				  };

		/// <summary>
		/// Получить aдрес сервиса подписчика для отправки сообщений из интеграционной шины.
		/// </summary>
		/// <param name="consumerId">Уникальный идентификатор обработчика очереди сообщений.</param>
		public string GetSubscriptionAddress(string consumerId)
		{
			if (consumerId == null)
			{
				throw new ArgumentNullException("consumerId");
			}

			var rows = _repository.Fetch(SelectSubscriptionAddressQuery, new object[] { consumerId });

			if (rows != null)
			{
				var dataRow = rows.FirstOrDefault();

				if (dataRow != null)
				{
					return dataRow.GetValue<string>(0);
				}
			}

			return null;
		}


		private static readonly InsertQueryStatement InsertSubscriptionQuery
			= new InsertQueryStatement
				  {
					  Table = Table,
					  Columns = new[]
						            {
							            IdColumn.Name,
							            TimeColumn.Name,
							            ExchangeTypeColumn.Name,
							            ExchangeNameColumn.Name,
							            QueueNameColumn.Name,
							            RoutingKeyColumn.Name,
							            RoutingHeadersColumn.Name,
							            AddressColumn.Name
						            }
				  };

		/// <summary>
		/// Сохранить информацию о подписке на очередь сообщений интеграционной шины.
		/// </summary>
		/// <param name="subscription">Информация о подписке на очередь сообщений интеграционной шины.</param>
		public void AddSubscription(IntegrationBusSubscription subscription)
		{
			if (subscription == null || string.IsNullOrWhiteSpace(subscription.ConsumerId))
			{
				throw new ArgumentNullException("subscription");
			}

			var subscriptionId = subscription.ConsumerId;
			var subscriptionTime = DateTimeOffset.UtcNow;
			var subscriptionRoutingHeaders = _serializer.Serialize(subscription.RoutingHeaders);

			_dataAdapter.Insert(InsertSubscriptionQuery, new object[]
				                                             {
					                                             subscriptionId,
					                                             subscriptionTime,
					                                             subscription.ExchangeType,
					                                             subscription.ExchangeName,
					                                             subscription.QueueName,
					                                             subscription.RoutingKey,
					                                             subscriptionRoutingHeaders,
					                                             subscription.Address
				                                             });
		}


		private static readonly DeleteQueryStatement DeleteSubscriptionQuery
			= new DeleteQueryStatement
				  {
					  Table = Table,
					  Where = new[] { new KeyFilter(IdColumn.Name) }
				  };

		/// <summary>
		/// Удалить информацию о подписке на очередь сообщений интеграционной шины.
		/// </summary>
		/// <param name="consumerId">Уникальный идентификатор обработчика очереди сообщений.</param>
		public void RemoveSubscription(string consumerId)
		{
			if (consumerId == null)
			{
				throw new ArgumentNullException("consumerId");
			}

			_dataAdapter.Delete(DeleteSubscriptionQuery, new object[] { consumerId });
		}


		private static readonly CreateTableQueryStatement CreateTableQuery
			= new CreateTableQueryStatement
				  {
					  Table = Table,
					  Columns = new[]
						            {
							            IdColumn,
							            TimeColumn,
							            ExchangeTypeColumn,
							            ExchangeNameColumn,
							            QueueNameColumn,
							            RoutingKeyColumn,
							            RoutingHeadersColumn,
							            AddressColumn
						            }
				  };

		/// <summary>
		/// Создать хранилище.
		/// </summary>
		public void CreateStorage()
		{
			_storage.CreateTable(CreateTableQuery);
		}


		private static readonly DropTableQueryStatement DropTableQuery
			= new DropTableQueryStatement
				  {
					  Table = Table
				  };

		/// <summary>
		/// Удалить хранилище.
		/// </summary>
		public void DeleteStorage()
		{
			_storage.DeleteTable(DropTableQuery);
		}
	}
}