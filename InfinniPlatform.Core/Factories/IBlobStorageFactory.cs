using InfinniPlatform.Api.Factories;
using InfinniPlatform.BlobStorage;

namespace InfinniPlatform.Factories
{
	/// <summary>
	/// Фабрика для создания сервисов по работе с BLOB (Binary Large OBject).
	/// </summary>
	public interface IBlobStorageFactory
	{
		/// <summary>
		/// Создает сервис для работы хранилищем BLOB.
		/// </summary>
		IBlobStorage CreateBlobStorage();

		/// <summary>
		/// Создает сервис для администрирования хранилища BLOB.
		/// </summary>
		IBlobStorageManager CreateBlobStorageManager();
	}
}