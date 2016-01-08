using System;
using System.Collections.Generic;
using System.Linq;

using InfinniPlatform.Index.ElasticSearch.Implementation.Filters;
using InfinniPlatform.Sdk.Environment.Index;

namespace InfinniPlatform.Index.ElasticSearch.Implementation.Versioning
{
    /// <summary>
    /// Провайдер операций с версионными данными без использования истории
    /// </summary>
    public sealed class VersionProvider : IVersionProvider
    {
        public VersionProvider(ICrudOperationProvider elasticSearchProvider, IIndexQueryExecutor indexQueryExecutor)
        {
            _elasticSearchProvider = elasticSearchProvider;
            _indexQueryExecutor = indexQueryExecutor;
        }

        private readonly ICrudOperationProvider _elasticSearchProvider;
        private readonly IIndexQueryExecutor _indexQueryExecutor;

        /// <summary>
        /// Получить актуальные версии объектов, отсортированные по дате вставки в индекс по убыванию
        /// </summary>
        /// <param name="filterObject">Фильтр объектов</param>
        /// <param name="pageNumber">Номер страницы данных</param>
        /// <param name="pageSize">Размер страницы данных</param>
        /// <param name="sortingDescription">Описание правил сортировки</param>
        /// <param name="skipSize"></param>
        /// <returns>Список актуальных версий</returns>
        public dynamic GetDocument(IEnumerable<object> filterObject, int pageNumber, int pageSize, IEnumerable<dynamic> sortingDescription = null, int skipSize = 0)
        {
            var filterFactory = FilterBuilderFactory.GetInstance();
            var searchModel = filterObject.ExtractSearchModel(filterFactory);
            searchModel.SetPageSize(pageSize);
            searchModel.SetSkip(skipSize);
            searchModel.SetFromPage(pageNumber);

            if (sortingDescription != null)
            {
                foreach (var sorting in sortingDescription)
                {
                    searchModel.AddSort(sorting.PropertyName, (SortOrder)sorting.SortOrder);
                }
            }

            return _indexQueryExecutor.Query(searchModel).Items.ToList();
        }

        /// <summary>
        /// Получить общее количество объектов по заданному фильтру
        /// </summary>
        /// <param name="filterObject">Фильтр объектов</param>
        /// <returns>Количество объектов</returns>
        public int GetNumberOfDocuments(IEnumerable<object> filterObject)
        {
            var queryFactory = QueryBuilderFactory.GetInstance();
            var searchModel = filterObject.ExtractSearchModel(queryFactory);

            // вряд ли документов в одном индексе будет больше чем 2 147 483 647, конвертируем в int
            return Convert.ToInt32(_indexQueryExecutor.CalculateCountQuery(searchModel));
        }

        /// <summary>
        /// Получить версию по уникальному идентификатору
        /// </summary>
        /// <param name="id">Уникальный идентификатор версии</param>
        /// <returns>Версия объекта</returns>
        public dynamic GetDocument(string id)
        {
            return _elasticSearchProvider.GetItem(id);
        }

        /// <summary>
        /// Получить список версий по уникальному идентификатору
        /// </summary>
        /// <param name="ids">Список идентификаторов версий</param>
        /// <returns>Список версий</returns>
        public IEnumerable<dynamic> GetDocuments(IEnumerable<string> ids)
        {
            return _elasticSearchProvider.GetItems(ids);
        }

        /// <summary>
        /// Удалить документ
        /// </summary>
        /// <param name="id">Идентификатор версии</param>
        public void DeleteDocument(string id)
        {
            _elasticSearchProvider.Remove(id);
            _elasticSearchProvider.Refresh();
        }

        /// <summary>
        /// Удалить документы с идентификаторами из списка
        /// </summary>
        /// <param name="ids">Список идентификаторов</param>
        public void DeleteDocuments(IEnumerable<string> ids)
        {
            _elasticSearchProvider.RemoveItems(ids);
            _elasticSearchProvider.Refresh();
        }

        /// <summary>
        /// Записать версию объекта в индекс
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
        /// Вставить список версий в индекс
        /// </summary>
        /// <param name="versions">Список версий</param>
        public void SetDocuments(IEnumerable<dynamic> versions)
        {
            _elasticSearchProvider.SetItems(versions);
            _elasticSearchProvider.Refresh();
        }
    }
}