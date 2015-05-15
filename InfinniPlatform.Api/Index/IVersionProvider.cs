using System.Collections.Generic;

namespace InfinniPlatform.Api.Index
{
    /// <summary>
	///   Стратегия хранения, получения и обновления истории объектов индекса
	/// </summary>
	public interface IVersionProvider : IDocumentProvider
    {
        /// <summary>
		///   Получить версию по уникальному идентификатору
		/// </summary>
		/// <param name="id">Уникальный идентификатор версии</param>
		/// <returns>Версия объекта</returns>
		dynamic GetDocument(string id);

	    /// <summary>
	    ///   Получить список версий по уникальному идентификатору
	    /// </summary>
	    /// <param name="ids">Список идентификаторов версий</param>
	    /// <returns>Список версий</returns>
	    IEnumerable<dynamic> GetDocuments(IEnumerable<string> ids);

		/// <summary>
		///   Записать версию объекта в индекс
		/// </summary>
		/// <param name="version">Обновляемая версия объекта</param>
		void SetDocument(dynamic version);

        /// <summary>
        ///   Вставить список версий в индекс
        /// </summary>
        /// <param name="versions">Список версий</param>
	    void SetDocuments(IEnumerable<dynamic> versions);


		/// <summary>
		///   Удалить документ
		/// </summary>
		/// <param name="id">Идентификатор версии</param>
		void DeleteDocument(string id);
	}
}
