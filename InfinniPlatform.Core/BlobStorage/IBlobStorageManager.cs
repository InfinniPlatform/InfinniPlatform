namespace InfinniPlatform.BlobStorage
{
	/// <summary>
	/// Сервис для администрирования хранилища BLOB (Binary Large OBject).
	/// </summary>
	public interface IBlobStorageManager
	{
		/// <summary>
		/// Создает хранилище BLOB.
		/// </summary>
		void CreateStorage();

		/// <summary>
		/// Удаляет хранилище BLOB.
		/// </summary>
		void DeleteStorage();
	}
}