using InfinniPlatform.Api.Factories;
using InfinniPlatform.Api.Index;
using InfinniPlatform.Factories;
using System.Collections.Generic;

namespace InfinniPlatform.Metadata.Implementation.MetadataConfiguration
{
    public sealed class CrossConfigSearcher : ICrossConfigSearcher
    {
        private readonly IIndexFactory _indexFactory;

        public CrossConfigSearcher(IIndexFactory indexFactory)
        {
            _indexFactory = indexFactory;
        }

        /// <summary>
        /// Выполнить запрос для поиска документа по определенным критериям 
        /// по всем имеющимся конфигурациям
        /// </summary>
        /// <param name="filterObject">Фильтр</param>
        /// <param name="pageNumber">Номер страницы</param>
        /// <param name="pageSize">Размер страницы</param>
        /// <param name="tenantId"></param>
        /// <param name="sorting">Сортировка</param>
        /// <param name="configs">Наименования конфигураций, по которым будет производиться поиск</param>
        /// <param name="documents">Типы документов, по которым будет производиться поиск</param>
        /// <returns>Найденные документы</returns>
        public dynamic GetDocuments(IEnumerable<object> filterObject, int pageNumber, int pageSize, string tenantId, IEnumerable<object> sorting, IEnumerable<string> configs = null, IEnumerable<string> documents = null)
        {
            var elasticProvider = _indexFactory.BuildMultiIndexDocumentProvider(tenantId, configs, documents);
            return elasticProvider.GetDocument(filterObject, pageNumber, pageSize, sorting);
        }
    }
}