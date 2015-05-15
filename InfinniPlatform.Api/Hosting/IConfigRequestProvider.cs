namespace InfinniPlatform.Api.Hosting
{
	/// <summary>
	///   Вычисляет путь в конфигурации из роутинга 
	/// </summary>
    public interface IConfigRequestProvider
    {
		/// <summary>
		///  Получить идентификатор метаданных конфигурации из роутинга
		/// </summary>
		/// <returns></returns>
        string GetConfiguration();

		/// <summary>
		///   Получить идентификатор метаданных объекта метаданных из роутинга
		/// </summary>
		/// <returns></returns>
        string GetMetadataIdentifier();

		/// <summary>
		///   Получить идентификатор метаданных действия из роутинга
		/// </summary>
		/// <returns></returns>
		string GetServiceName();

		/// <summary>
		///   Получить идентификатор авторизованного в системе пользователя
		/// </summary>
		/// <returns></returns>
		string GetUserName();
    }
}
