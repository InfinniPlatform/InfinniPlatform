using System.Collections;

namespace InfinniPlatform.UserInterface.ViewBuilders.Data
{
	/// <summary>
	/// Провайдер данных.
	/// </summary>
	public interface IDataProvider
	{
		/// <summary>
		/// Возвращает идентификатор конфигурации.
		/// </summary>
		string GetConfigId();

		/// <summary>
		/// Устанавливает идентификатор конфигурации.
		/// </summary>
		void SetConfigId(string value);


		/// <summary>
		/// Возвращает идентфикатор документа.
		/// </summary>
		string GetDocumentId();

		/// <summary>
		/// Устанавливает идентфикатор документа.
		/// </summary>
		void SetDocumentId(string value);


		/// <summary>
		/// Создать объект по умолчанию.
		/// </summary>
		object CreateItem();

		/// <summary>
		/// Заменяет объект в хранилище.
		/// </summary>
		/// <param name="item">Структура данных объекта.</param>
		void ReplaceItem(object item);

		/// <summary>
		/// Удаляет объект из хранилища.
		/// </summary>
		/// <param name="itemId">Идентификатор объекта.</param>
		void DeleteItem(string itemId);

		/// <summary>
		/// Возвращает объект из хранилища.
		/// </summary>
		/// <param name="itemId">Идентификатор объекта.</param>
		object GetItem(string itemId);

		/// <summary>
		/// Возвращает копию объекта из хранилища.
		/// </summary>
		/// <param name="itemId">Идентификатор объекта.</param>
		object CloneItem(string itemId);

		/// <summary>
		/// Возвращает список объектов из хранилища.
		/// </summary>
		/// <param name="criterias">Критерии поиска.</param>
		/// <param name="pageNumber">Номер страницы.</param>
		/// <param name="pageSize">Размер страницы.</param>
		IEnumerable GetItems(IEnumerable criterias, int pageNumber, int pageSize);
	}
}