using System;
using System.Linq;
using InfinniPlatform.Cassandra;
using InfinniPlatform.Cassandra.DataAdapter;
using InfinniPlatform.Cassandra.Repository;
using InfinniPlatform.Cassandra.Storage;
using InfinniPlatform.Sdk.Environment;
using InfinniPlatform.Sdk.Environment.Binary;

namespace InfinniPlatform.BlobStorage
{
	/// <summary>
	/// Сервис для работы хранилищем BLOB на базе Apache Cassandra.
	/// </summary>
	public sealed class CassandraBlobStorage : IBlobStorage, IBlobStorageManager
	{
		public CassandraBlobStorage(IColumnFamilyDatabaseFactory factory)
		{
			if (factory == null)
			{
				throw new ArgumentNullException("factory");
			}

			_typeProvider = new FileExtensionTypeProvider();
			_dataAdapter = factory.CreateDataAdapter();
			_repository = factory.CreateRepository();
			_storage = factory.CreateStorage();
		}


		private readonly FileExtensionTypeProvider _typeProvider;
		private readonly IColumnFamilyDataAdapter _dataAdapter;
		private readonly IColumnFamilyRepository _repository;
		private readonly IColumnFamilyStorage _storage;


		// Метаданные таблицы BLOB
		private const string BlobTableName = "blobstorage";
		private static readonly Column IdColumn = new Column("id", ColumnType.Uuid, true);
		private static readonly Column NameColumn = new Column("name", ColumnType.String);
		private static readonly Column TypeColumn = new Column("type", ColumnType.String);
		private static readonly Column SizeColumn = new Column("size", ColumnType.Int64);
		private static readonly Column TimeColumn = new Column("time", ColumnType.DateTime);
		private static readonly Column DataColumn = new Column("data", ColumnType.Blob);


		private static readonly CreateTableQueryStatement CreateStorageQuery
			= new CreateTableQueryStatement
			  {
				  Table = BlobTableName,
				  Columns = new[] { IdColumn, NameColumn, TypeColumn, SizeColumn, TimeColumn, DataColumn }
			  };

		/// <summary>
		/// Создает хранилище BLOB.
		/// </summary>
		public void CreateStorage()
		{
			_storage.CreateTable(CreateStorageQuery);
		}


		private static readonly DropTableQueryStatement DeleteStorageQuery
			= new DropTableQueryStatement
			  {
				  Table = BlobTableName
			  };

		/// <summary>
		/// Удаляет хранилище BLOB.
		/// </summary>
		public void DeleteStorage()
		{
			_storage.DeleteTable(DeleteStorageQuery);
		}


		private static readonly SelectQueryStatement GetBlobInfoQuery
			= new SelectQueryStatement
			  {
				  Table = BlobTableName,
				  Columns = new[] { NameColumn.Name, TypeColumn.Name, SizeColumn.Name, TimeColumn.Name },
				  Where = new[] { new KeyFilter(IdColumn.Name) },
				  Limit = 1
			  };

		/// <summary>
		/// Возвращает информацию о BLOB.
		/// </summary>
		/// <param name="blobId">Идентификатор BLOB.</param>
		/// <returns>Информация о BLOB.</returns>
		public BlobInfo GetBlobInfo(Guid blobId)
		{
			var rows = _repository.Fetch(GetBlobInfoQuery, new object[] { blobId });

			if (rows != null)
			{
				var dataRow = rows.FirstOrDefault();

				if (dataRow != null)
				{
					return new BlobInfo
						   {
							   Id = blobId,
							   Name = dataRow.GetValue<string>(NameColumn.Name),
							   Type = dataRow.GetValue<string>(TypeColumn.Name),
							   Size = dataRow.GetValue<long>(SizeColumn.Name),
							   Time = dataRow.GetValue<DateTime>(TimeColumn.Name)
						   };
				}
			}

			return null;
		}


		private static readonly SelectQueryStatement GetBlobDataQuery
			= new SelectQueryStatement
			  {
				  Table = BlobTableName,
				  Columns = new[] { NameColumn.Name, TypeColumn.Name, SizeColumn.Name, TimeColumn.Name, DataColumn.Name },
				  Where = new[] { new KeyFilter(IdColumn.Name) },
				  Limit = 1
			  };

		/// <summary>
		/// Возвращает данные BLOB.
		/// </summary>
		/// <param name="blobId">Идентификатор BLOB.</param>
		/// <returns>Данные BLOB.</returns>
		public BlobData GetBlobData(Guid blobId)
		{
			var rows = _repository.Fetch(GetBlobDataQuery, new object[] { blobId });

			if (rows != null)
			{
				var dataRow = rows.FirstOrDefault();

				if (dataRow != null)
				{
					return new BlobData
						   {
							   Info = new BlobInfo
									  {
										  Id = blobId,
										  Name = dataRow.GetValue<string>(NameColumn.Name),
										  Type = dataRow.GetValue<string>(TypeColumn.Name),
										  Size = dataRow.GetValue<long>(SizeColumn.Name),
										  Time = dataRow.GetValue<DateTime>(TimeColumn.Name)
									  },
							   Data = dataRow.GetValue<byte[]>(DataColumn.Name)
						   };
				}
			}

			return null;
		}


		private static readonly InsertQueryStatement SaveBlobQuery
			= new InsertQueryStatement
			  {
				  Table = BlobTableName,
				  Columns = new[] { IdColumn.Name, NameColumn.Name, TypeColumn.Name, SizeColumn.Name, TimeColumn.Name, DataColumn.Name }
			  };

		/// <summary>
		/// Сохраняет BLOB.
		/// </summary>
		/// <param name="blobId">Идентификатор BLOB.</param>
		/// <param name="blobName">Наименование BLOB.</param>
		/// <param name="blobData">Данные BLOB.</param>
		public void SaveBlob(Guid blobId, string blobName, byte[] blobData)
		{
			var blobType = _typeProvider.GetBlobType(blobName);

			SaveBlob(blobId, blobName, blobType, blobData);
		}

		/// <summary>
		/// Сохраняет BLOB.
		/// </summary>
		/// <param name="blobId">Идентификатор BLOB.</param>
		/// <param name="blobName">Наименование BLOB.</param>
		/// <param name="blobType">Формат данных BLOB.</param>
		/// <param name="blobData">Данные BLOB.</param>
		public void SaveBlob(Guid blobId, string blobName, string blobType, byte[] blobData)
		{
			var blobTime = DateTimeOffset.UtcNow;
			var blobSize = (blobData != null) ? blobData.LongLength : 0;

			_dataAdapter.Insert(SaveBlobQuery, new object[] { blobId, blobName, blobType, blobSize, blobTime, blobData });
		}


		private static readonly DeleteQueryStatement DeleteBlobQuery
			= new DeleteQueryStatement
			  {
				  Table = BlobTableName,
				  Where = new[] { new KeyFilter(IdColumn.Name) }
			  };

		/// <summary>
		/// Удаляет BLOB. 
		/// </summary>
		/// <param name="blobId">Идентификатор BLOB.</param>
		public void DeleteBlob(Guid blobId)
		{
			_dataAdapter.Delete(DeleteBlobQuery, new object[] { blobId });
		}
	}
}