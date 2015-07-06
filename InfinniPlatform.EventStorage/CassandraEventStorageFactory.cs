using System;
using InfinniPlatform.Cassandra;
using InfinniPlatform.Factories;
using InfinniPlatform.Sdk.Environment;
using InfinniPlatform.Sdk.Environment.Binary;

namespace InfinniPlatform.EventStorage
{
	/// <summary>
	/// Фабрика для создания хранилища событий на базе Apache Cassandra.
	/// </summary>
	public sealed class CassandraEventStorageFactory : IEventStorageFactory
	{
		public CassandraEventStorageFactory(IColumnFamilyDatabaseFactory factory)
		{
			if (factory == null)
			{
				throw new ArgumentNullException("factory");
			}

			_eventStorage = new CassandraEventStorage(factory);
		}


		private readonly CassandraEventStorage _eventStorage;


		/// <summary>
		/// Создать хранилище событий.
		/// </summary>
		public IEventStorage CreateEventStorage()
		{
			return _eventStorage;
		}

		/// <summary>
		/// Создать менеджер для управления хранилищем событий.
		/// </summary>
		public IEventStorageManager CreateEventStorageManager()
		{
			return _eventStorage;
		}
	}
}