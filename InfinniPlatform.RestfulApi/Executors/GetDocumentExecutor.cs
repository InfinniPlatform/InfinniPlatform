using System;
using System.Collections.Generic;

using InfinniPlatform.Api.ContextTypes.ContextImpl;
using InfinniPlatform.Api.Metadata;
using InfinniPlatform.Api.RestApi.Auth;
using InfinniPlatform.Api.RestApi.CommonApi;
using InfinniPlatform.Api.RestApi.DataApi;
using InfinniPlatform.Api.RestQuery;
using InfinniPlatform.Api.SearchOptions.Builders;
using InfinniPlatform.RestfulApi.Utils;
using InfinniPlatform.Sdk.ContextComponents;

namespace InfinniPlatform.RestfulApi.Executors
{
    public class GetDocumentExecutor : IGetDocumentExecutor
    {
        public GetDocumentExecutor(RestQueryApi restQueryApi,
                                   DocumentExecutor documentExecutor,
                                   IMetadataComponent metadataComponent,
                                   IScriptRunnerComponent scriptRunnerComponent)
        {
            _restQueryApi = restQueryApi;
            _documentExecutor = documentExecutor;
            _metadataComponent = metadataComponent;
            _scriptRunnerComponent = scriptRunnerComponent;
        }

        private readonly DocumentExecutor _documentExecutor;
        private readonly IMetadataComponent _metadataComponent;

        private readonly RestQueryApi _restQueryApi;
        private readonly IScriptRunnerComponent _scriptRunnerComponent;

        public IEnumerable<dynamic> GetDocumentByQuery(string queryText, bool denormalizeResult = false)
        {
            var result = ExecutePost("getbyquery", null, new
                                                         {
                                                             QueryText = queryText,
                                                             DenormalizeResult = denormalizeResult
                                                         });

            return result.ToDynamicList();
        }

        public dynamic GetDocument(string id)
        {
            var result = ExecutePost("getdocumentbyid", null, new
                                                              {
                                                                  Id = id,
                                                                  Secured = false
                                                              });

            return result.ToDynamic();
        }

        public int GetNumberOfDocuments(string configuration, string metadata, dynamic filter)
        {
            var result = ExecutePost("getnumberofdocuments", null, new
                                                                   {
                                                                       Configuration = configuration,
                                                                       Metadata = metadata,
                                                                       Filter = filter,
                                                                       Secured = false
                                                                   });

            return Convert.ToInt32(result.ToDynamic().NumberOfDocuments);
        }

        public int GetNumberOfDocuments(string configuration, string metadata, Action<FilterBuilder> filter)
        {
            var filterBuilder = new FilterBuilder();

            filter?.Invoke(filterBuilder);

            return GetNumberOfDocuments(configuration, metadata, filterBuilder.GetFilter());
        }

        public IEnumerable<dynamic> GetDocument(string configuration, string metadata, dynamic filter, int pageNumber, int pageSize, IEnumerable<dynamic> ignoreResolve = null, dynamic sorting = null)
        {
            var result = ExecutePost("getdocument", null, new
                                                          {
                                                              Configuration = configuration,
                                                              Metadata = metadata,
                                                              Filter = filter,
                                                              PageNumber = pageNumber,
                                                              PageSize = pageSize,
                                                              IgnoreResolve = ignoreResolve,
                                                              Sorting = sorting,
                                                              Secured = false
                                                          });

            return result.ToDynamicList();
        }

        public IEnumerable<dynamic> GetDocument(string configuration, string metadata, Action<FilterBuilder> filter, int pageNumber, int pageSize, Action<SortingBuilder> sorting = null)
        {
            return GetDocument(configuration, metadata, filter, pageNumber, pageSize, null, sorting);
        }

        public IEnumerable<dynamic> GetDocument(string configuration, string metadata, Action<FilterBuilder> filter, int pageNumber, int pageSize, IEnumerable<dynamic> ignoreResolve, Action<SortingBuilder> sorting = null)
        {
            var filterBuilder = new FilterBuilder();

            filter?.Invoke(filterBuilder);

            var sortingBuilder = new SortingBuilder();

            sorting?.Invoke(sortingBuilder);

            return GetDocument(configuration, metadata, filterBuilder.GetFilter(), pageNumber, pageSize, ignoreResolve, sortingBuilder.GetSorting());
        }

        public IEnumerable<object> GetDocumentUnfolded(string configuration, string metadata, dynamic filter, int pageNumber, int pageSize, IEnumerable<dynamic> ignoreResolve = null, dynamic sorting = null)
        {
            var document = new
                           {
                               Configuration = configuration,
                               Metadata = metadata,
                               Filter = filter,
                               PageNumber = pageNumber,
                               PageSize = pageSize,
                               IgnoreResolve = ignoreResolve,
                               Sorting = sorting,
                               Secured = false
                           };

            //ActionUnitSetCredentials
            if (document.Configuration != null)
            {
                if (document.Configuration.ToLowerInvariant() != "systemconfig" &&
                    document.Configuration.ToLowerInvariant() != "update" &&
                    document.Configuration.ToLowerInvariant() != "restfulapi")
                {
                    //ищем метаданные бизнес-процесса по умолчанию документа 
                    dynamic defaultBusinessProcess = _metadataComponent.GetMetadata(document.Configuration, document.Metadata, MetadataType.Process, "Default");

                    if (defaultBusinessProcess != null && defaultBusinessProcess.Transitions[0].CredentialsType != null)
                    {
                        dynamic credentialsType = defaultBusinessProcess.Transitions[0].CredentialsType;
                        if (credentialsType != null)
                        {
                            if (credentialsType == AuthorizationStorageExtensions.CustomCredentials)
                            {
                                var scriptArguments = new ApplyContext
                                                      {
                                                          Configuration = document.Configuration,
                                                          Metadata = document.Metadata,
                                                          Item = document
                                                      };

                                scriptArguments.Item = document;

                                _scriptRunnerComponent.InvokeScript(defaultBusinessProcess.Transitions[0].CredentialsPoint.ScenarioId, scriptArguments);
                            }
                        }
                    }

                    //ActionUnitComplexAuth
                    //ищем метаданные бизнес-процесса по умолчанию документа 
                    if (defaultBusinessProcess != null && defaultBusinessProcess.Transitions[0].ComplexAuthorizationPoint != null)
                    {
                        var scriptArguments = new ApplyContext
                                              {
                                                  Configuration = document.Configuration,
                                                  Metadata = document.Metadata,
                                                  Item = document
                                              };

                        _scriptRunnerComponent.InvokeScript(defaultBusinessProcess.Transitions[0].ComplexAuthorizationPoint.ScenarioId, scriptArguments);
                    }
                }
            }

            //ActionUnitGetDocument
            var result = _documentExecutor.GetCompleteDocuments(document.Configuration,
                                                                document.Metadata,
                                                                null,
                                                                Convert.ToInt32(document.PageNumber),
                                                                Convert.ToInt32(document.PageSize),
                                                                document.Filter,
                                                                document.Sorting,
                                                                document.IgnoreResolve);

            return result;
        }

        private RestQueryResponse ExecutePost(string action, string id, object body)
        {
            var response = _restQueryApi.QueryPostJsonRaw("RestfulApi", "configuration", action, id, body);

            return response;
        }
    }
}