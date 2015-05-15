using System;
using System.Collections.Generic;
using System.Linq;
using InfinniPlatform.Api.Context;
using InfinniPlatform.Api.ContextComponents;
using InfinniPlatform.Api.ContextTypes.ContextImpl;
using InfinniPlatform.Api.Factories;
using InfinniPlatform.Api.Hosting;
using InfinniPlatform.Api.RestApi.AuthApi;
using InfinniPlatform.Api.SearchOptions;
using InfinniPlatform.Hosting;
using InfinniPlatform.Metadata.Properties;
using IConfigurationObjectBuilder = InfinniPlatform.Api.Metadata.IConfigurationObjectBuilder;


namespace InfinniPlatform.Metadata.Implementation.Handlers
{
	public sealed class SearchHandler : IWebRoutingHandler
	{

        private readonly IGlobalContext _globalContext;
	    public IConfigRequestProvider ConfigRequestProvider { get; set; }

		public SearchHandler(
            IGlobalContext globalContext)
		{
		    _globalContext = globalContext;
		}

	    /// <summary>
	    ///   Найти список объектов, удовлетворяющих указанным критериям
	    /// </summary>
	    /// <param name="filterObject">Фильтр поиска объектов</param>
	    /// <param name="pageNumber">Номер страницы результатов поиска</param>
	    /// <param name="pageSize">Размер страницы результатов</param>
	    /// <param name="version">Версия конфигурации, для которой выполняется поиск объектов</param>
	    /// <param name="searchType">Искать только актуальную версию объектов</param>
	    /// <returns>Список результатов поиска</returns>
	    public object GetSearchResult(IEnumerable<dynamic> filterObject, int pageNumber, int pageSize, SearchType searchType = SearchType.All, string version = null)
	    {
            
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

			var idType = ConfigRequestProvider.GetMetadataIdentifier();
			var config = _globalContext.GetComponent<IConfigurationMediatorComponent>().ConfigurationBuilder.GetConfigurationObject(ConfigRequestProvider.GetConfiguration()).MetadataConfiguration;

			//устанавливаем контекст прикладной конфигурации. В ходе рефакторинга необходимо обдумать, как вынести это на более высокий уровень абстракции
		    var appliedConfig =
			    _globalContext.GetComponent<IConfigurationMediatorComponent>().GetConfiguration(ConfigRequestProvider); 
	        

			if (string.IsNullOrEmpty(idType))
			{
				throw new ArgumentException("index type undefined");
			}

			var target = new SearchContext
			    {
			        Filter = filters,
			        IsValid = true,
			        Index = ConfigRequestProvider.GetConfiguration(),
                    IndexType = ConfigRequestProvider.GetMetadataIdentifier(),
			        Context = _globalContext,
					Configuration = ConfigRequestProvider.GetConfiguration(),
					Metadata = ConfigRequestProvider.GetMetadataIdentifier(),
					Action = ConfigRequestProvider.GetServiceName(),

					PageNumber = pageNumber,
					PageSize = pageSize
			    };
            //1. Выполняем валидацию фильтров перед фильтрацией
            config.MoveWorkflow(idType, config.GetExtensionPointValue(ConfigRequestProvider, "validatefilter"), target);


			if (target.IsValid)
			{
			    string configVersion = null;
			    if (searchType != SearchType.All)
			    {
			        configVersion = version ?? appliedConfig.GetConfigurationVersion();
			    }


			    var documentProvider = appliedConfig
			        .GetDocumentProvider(ConfigRequestProvider.GetMetadataIdentifier(), configVersion, 
					target.Context.GetComponent<ISecurityComponent>().GetClaim(AuthorizationStorageExtensions.OrganizationClaim,target.UserName) ?? AuthorizationStorageExtensions.AnonimousUser);

			    dynamic sr = null;
			    if (documentProvider != null)
			    {
			        //2. Выполняем поиск объектов
			        sr = documentProvider
			            .GetDocument(filters, pageNumber, pageSize);
			    }

			    //3. Получаем проекцию данных
				if (sr != null)
				{
					var context = target;
					context.SearchResult = sr;
                    config.MoveWorkflow(idType,config.GetExtensionPointValue(ConfigRequestProvider, "searchmodel"), context);
					return context.SearchResult;
				}

			    target.ValidationMessage = string.Format(Resources.DocumentProviderNotRegisteredError,
			                                             ConfigRequestProvider.GetMetadataIdentifier());
			}


			return AggregateExtensions.PrepareInvalidFilterAggregate(target);
		}


	}
}
