using InfinniPlatform.Cassandra.DataAdapter;
using InfinniPlatform.Cassandra.Repository;
using InfinniPlatform.Cassandra.Storage;

namespace InfinniPlatform.Cassandra
{
	/// <summary>
	/// Фабрика для получения точек доступа к хранилищу типа ColumnFamily.
	/// </summary>
	public interface IColumnFamilyDatabaseFactory
	{
		/// <summary>
		/// Создать объект для модификации данных хранилища.
		/// </summary>
		IColumnFamilyDataAdapter CreateDataAdapter();

		/// <summary>
		/// Создать объект для выборки данных из хранилища.
		/// </summary>
		IColumnFamilyRepository CreateRepository();

		/// <summary>
		/// Создать объект для управления хранилищем.
		/// </summary>
		IColumnFamilyStorage CreateStorage();
	}
}