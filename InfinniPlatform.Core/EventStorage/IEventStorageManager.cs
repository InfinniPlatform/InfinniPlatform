namespace InfinniPlatform.EventStorage
{
	/// <summary>
	/// Represents manager of the event storage.
	/// </summary>
	public interface IEventStorageManager
	{
		/// <summary>
		/// Creates storage.
		/// </summary>
		void CreateStorage();

		/// <summary>
		///  Delete storage
		/// </summary>
		void DeleteStorage();
	}
}