using System;
using InfinniPlatform.Api.Factories;
using InfinniPlatform.Cassandra;
using InfinniPlatform.Factories;

namespace InfinniPlatform.BlobStorage
{
	/// <summary>
	/// Фабрика для создания сервисов по работе с BLOB на базе Apache Cassandra.
	/// </summary>
	public sealed class CassandraBlobStorageFactory : IBlobStorageFactory
	{
		public CassandraBlobStorageFactory(IColumnFamilyDatabaseFactory factory)
		{
			if (factory == null)
			{
				throw new ArgumentNullException("factory");
			}

			_blobStorage = new CassandraBlobStorage(factory);
		}


		private readonly CassandraBlobStorage _blobStorage;


		/// <summary>
		/// Создает сервис для работы хранилищем BLOB.
		/// </summary>
		public IBlobStorage CreateBlobStorage()
		{
			return _blobStorage;
		}

		/// <summary>
		/// Создает сервис для администрирования хранилища BLOB.
		/// </summary>
		public IBlobStorageManager CreateBlobStorageManager()
		{
			return _blobStorage;
		}
	}
}