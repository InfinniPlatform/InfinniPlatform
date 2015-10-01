using System;
using System.Collections.Generic;
using InfinniPlatform.Api.Index;
using InfinniPlatform.Sdk.Environment.Index;

namespace InfinniPlatform.Index.ElasticSearch.Implementation.Versioning
{
	/// <summary>
	///   Провайдер операций с версионными данными без использования истории
	/// </summary>
	public sealed class VersionProvider : IVersionProvider
	{
        private readonly IDocumentProvider _documentProvider;
        private readonly ICrudOperationProvider _elasticSearchProvider;
		
		public VersionProvider(ICrudOperationProvider elasticSearchProvider, IDocumentProvider documentProvider)
		{
            _elasticSearchProvider = elasticSearchProvider;
            _documentProvider = documentProvider;
        }

		/// <summary>
		///   Получить актуальные версии объектов, отсортированные по дате вставки в индекс по убыванию  
		/// </summary>
		/// <param name="filterObject">Фильтр объектов</param>
		/// <param name="pageNumber">Номер страницы данных</param>
		/// <param name="pageSize">Размер страницы данных</param>
		/// <param name="sortingDescription">Описание правил сортировки</param>
		/// <param name="skipSize"></param>
		/// <returns>Список актуальных версий</returns>
		public dynamic GetDocument(IEnumerable<object> filterObject, int pageNumber, int pageSize, IEnumerable<object> sortingDescription = null, int skipSize = 0)
		{
            return _documentProvider.GetDocument(filterObject, pageNumber, pageSize, sortingDescription, skipSize);
		}

	    /// <summary>
	    ///   Получить общее количество объектов по заданному фильтру
	    /// </summary>
	    /// <param name="filterObject">Фильтр объектов</param>
	    /// <returns>Количество объектов</returns>
	    public int GetNumberOfDocuments(IEnumerable<object> filterObject)
	    {
	        return _documentProvider.GetNumberOfDocuments(filterObject);
	    }

	    /// <summary>
		///   Получить версию по уникальному идентификатору
		/// </summary>
		/// <param name="id">Уникальный идентификатор версии</param>
		/// <returns>Версия объекта</returns>
		public dynamic GetDocument(string id)
		{
            return _elasticSearchProvider.GetItem(id);
		}

		/// <summary>
	    ///   Получить список версий по уникальному идентификатору
	    /// </summary>
	    /// <param name="ids">Список идентификаторов версий</param>
	    /// <returns>Список версий</returns>
	    public IEnumerable<dynamic> GetDocuments(IEnumerable<string> ids)
	    {
            return _elasticSearchProvider.GetItems(ids);
	    }

		/// <summary>
		///   Удалить документ
		/// </summary>
		/// <param name="id">Идентификатор версии</param>
		public void DeleteDocument(string id)
		{
            _elasticSearchProvider.Remove(id);
            _elasticSearchProvider.Refresh();
		}

        /// <summary>
        ///   Удалить документы с идентификаторами из списка
        /// </summary>
        /// <param name="ids">Список идентификаторов</param>
	    public void DeleteDocuments(IEnumerable<string> ids)
	    {
	        _elasticSearchProvider.RemoveItems(ids);
            _elasticSearchProvider.Refresh();
	    }

	    /// <summary>
		///   Записать версию объекта в индекс
		/// </summary>
		/// <param name="version">Обновляемая версия объекта</param>
		public void SetDocument(dynamic version)
		{
			if (version.Id == null)
			{
				version.Id = Guid.NewGuid().ToString();
			}

			_elasticSearchProvider.Set(version);
            _elasticSearchProvider.Refresh();
		}

	    /// <summary>
	    ///   Вставить список версий в индекс
	    /// </summary>
	    /// <param name="versions">Список версий</param>
	    public void SetDocuments(IEnumerable<dynamic> versions)
	    {
            _elasticSearchProvider.SetItems(versions);
            _elasticSearchProvider.Refresh();
	    }
	
	
	}
}