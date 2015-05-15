using InfinniPlatform.Cassandra.DataAdapter;
using InfinniPlatform.Cassandra.Repository;
using InfinniPlatform.Cassandra.Storage;

namespace InfinniPlatform.Cassandra.Client
{
	/// <summary>
	/// Фабрика для получения точек доступа к хранилищу типа ColumnFamily на базе Apache Cassandra.
	/// </summary>
	public sealed class CassandraDatabaseFactory : IColumnFamilyDatabaseFactory
	{
		public CassandraDatabaseFactory()
		{
			var sessionFactory = new CassandraSessionFactory();
			var queryExecutor = new QueryExecutor(sessionFactory);

			_dataAdapter = new ColumnFamilyDataAdapter(queryExecutor);
			_repository = new ColumnFamilyRepository(queryExecutor);
			_storage = new ColumnFamilyStorage(queryExecutor);
		}


		private readonly ColumnFamilyDataAdapter _dataAdapter;
		private readonly ColumnFamilyRepository _repository;
		private readonly ColumnFamilyStorage _storage;


		public IColumnFamilyDataAdapter CreateDataAdapter()
		{
			return _dataAdapter;
		}

		public IColumnFamilyRepository CreateRepository()
		{
			return _repository;
		}

		public IColumnFamilyStorage CreateStorage()
		{
			return _storage;
		}
	}
}