using System.Collections.Generic;
using System.Linq;

using InfinniPlatform.Core.ContextTypes;
using InfinniPlatform.Core.Hosting;
using InfinniPlatform.Core.SearchOptions;
using InfinniPlatform.Sdk.Contracts;
using InfinniPlatform.Sdk.Environment.Index;
using InfinniPlatform.Sdk.Environment.Metadata;
using InfinniPlatform.SystemConfig.Properties;

namespace InfinniPlatform.SystemConfig.RequestHandlers
{
    public sealed class SearchHandler : IWebRoutingHandler
    {
        public SearchHandler(IConfigurationObjectBuilder configurationObjectBuilder)
        {
            _configurationObjectBuilder = configurationObjectBuilder;
        }


        private readonly IConfigurationObjectBuilder _configurationObjectBuilder;


        public IConfigRequestProvider ConfigRequestProvider { get; set; }


        /// <summary>
        /// Найти список объектов, удовлетворяющих указанным критериям
        /// </summary>
        /// <param name="filterObject">Фильтр поиска объектов</param>
        /// <param name="pageNumber">Номер страницы результатов поиска</param>
        /// <param name="pageSize">Размер страницы результатов</param>
        /// <param name="searchType">Искать только актуальную версию объектов</param>
        /// <returns>Список результатов поиска</returns>
        public object GetSearchResult(
            IEnumerable<dynamic> filterObject,
            int pageNumber,
            int pageSize,
            SearchType searchType = SearchType.All)
        {
            var сonfiguration = ConfigRequestProvider.GetConfiguration();
            var documentType = ConfigRequestProvider.GetMetadataIdentifier();
            var serviceName = ConfigRequestProvider.GetServiceName();

            List<dynamic> filters = null;

            if (filterObject != null)
            {
                filters = filterObject.ToList();

                foreach (var filter in filters)
                {
                    if (filter.CriteriaType == null)
                    {
                        filter.CriteriaType = CriteriaType.IsEquals;
                    }
                }
            }

            var metadataConfiguration = _configurationObjectBuilder.GetConfigurationObject(сonfiguration).MetadataConfiguration;

            var target = new SearchContext
                         {
                             Filter = filters,
                             IsValid = true,
                             Index = сonfiguration,
                             IndexType = documentType,
                             Configuration = сonfiguration,
                             Metadata = documentType,
                             Action = serviceName,
                             PageNumber = pageNumber,
                             PageSize = pageSize
                         };

            // 1. Выполняем валидацию фильтров перед фильтрацией
            ExecuteExtensionPoint(metadataConfiguration, documentType, "validatefilter", target);
            
            if (target.IsValid)
            {
                // Устанавливаем контекст прикладной конфигурации
                var appliedConfig = _configurationObjectBuilder.GetConfigurationObject(сonfiguration);

                var documentProvider = appliedConfig.GetDocumentProvider(documentType);

                dynamic searchResult = null;

                if (documentProvider != null)
                {
                    // 2. Выполняем поиск объектов
                    searchResult = documentProvider.GetDocument(filters, pageNumber, pageSize);
                }

                // 3. Получаем проекцию данных
                if (searchResult != null)
                {
                    target.SearchResult = searchResult;

                    ExecuteExtensionPoint(metadataConfiguration, documentType, "searchmodel", target);

                    return target.SearchResult;
                }

                target.ValidationMessage = string.Format(Resources.DocumentProviderNotRegisteredError, documentType);
            }
            
            return AggregateExtensions.PrepareInvalidResult(target);
        }

        private bool ExecuteExtensionPoint(IMetadataConfiguration metadataConfiguration, string documentType, string extensionPointName, ICommonContext extensionPointContext)
        {
            var extensionPoint = metadataConfiguration.GetExtensionPointValue(ConfigRequestProvider, extensionPointName);

            if (!string.IsNullOrEmpty(extensionPoint))
            {
                metadataConfiguration.MoveWorkflow(documentType, extensionPoint, extensionPointContext);
            }

            return extensionPointContext.IsValid;
        }
    }
}