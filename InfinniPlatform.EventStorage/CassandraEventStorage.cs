using System;
using System.Collections.Generic;
using System.Linq;
using InfinniPlatform.Cassandra;
using InfinniPlatform.Cassandra.DataAdapter;
using InfinniPlatform.Cassandra.Repository;
using InfinniPlatform.Cassandra.Storage;
using InfinniPlatform.Sdk.Environment;
using InfinniPlatform.Sdk.Environment.Binary;

namespace InfinniPlatform.EventStorage
{
	/// <summary>
	/// Хранилище событий на базе Apache Cassandra.
	/// </summary>
	public sealed class CassandraEventStorage : IEventStorage, IEventStorageManager
	{
		public CassandraEventStorage(IColumnFamilyDatabaseFactory factory)
		{
			if (factory == null)
			{
				throw new ArgumentNullException("factory");
			}

			_dataAdapter = factory.CreateDataAdapter();
			_repository = factory.CreateRepository();
			_storage = factory.CreateStorage();
		}


		private readonly IColumnFamilyDataAdapter _dataAdapter;
		private readonly IColumnFamilyRepository _repository;
		private readonly IColumnFamilyStorage _storage;

		private const string EventStorageTable = "eventstorage";
		private static readonly Column IdColumn = new Column("id", ColumnType.Uuid, true);
		private static readonly Column TimeColumn = new Column("time", ColumnType.DateTime, true);
		private static readonly Column EventColumn = new Column("event", ColumnType.String);

		private static readonly SelectQueryStatement SelectEventQuery = new SelectQueryStatement { Table = EventStorageTable, Columns = new[] { EventColumn.Name }, Where = new[] { new KeyFilter(IdColumn.Name) } };
		private static readonly InsertQueryStatement InsertEventQuery = new InsertQueryStatement { Table = EventStorageTable, Columns = new[] { IdColumn.Name, TimeColumn.Name, EventColumn.Name } };
		private static readonly CreateTableQueryStatement CreateTableQuery = new CreateTableQueryStatement { Table = EventStorageTable, Columns = new[] { IdColumn, TimeColumn, EventColumn } };
		private static readonly DropTableQueryStatement DropTableQuery = new DropTableQueryStatement { Table = EventStorageTable };


		/// <summary>
		/// Returns events.
		/// </summary>
		/// <param name="id">The unique identifier of the aggregate.</param>
		/// <returns>The list events of the aggregate.</returns>
		public IEnumerable<string> GetEvents(Guid id)
		{
			var rows = _repository.Fetch(SelectEventQuery, new object[] { id });

			return (rows != null) ? rows.Select(r => r.GetValue<string>(0)).ToArray() : null;
		}

		/// <summary>
		/// Adds events.
		/// </summary>
		/// <param name="id">The unique identifier of the aggregate.</param>
		/// <param name="events">The list events of the aggregate.</param>
		public void AddEvents(Guid id, IEnumerable<string> events)
		{
			var time = DateTimeOffset.UtcNow;

			foreach (var @event in events)
			{
				_dataAdapter.Insert(InsertEventQuery, new object[] { id, time, @event });
			}
		}

		/// <summary>
		/// Creates storage.
		/// </summary>
		public void CreateStorage()
		{
			_storage.CreateTable(CreateTableQuery);
		}

		/// <summary>
		/// Deletes storage.
		/// </summary>
		public void DeleteStorage()
		{
			_storage.DeleteTable(DropTableQuery);
		}
	}
}