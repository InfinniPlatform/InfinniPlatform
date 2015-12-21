using System;
using System.Collections.Generic;
using System.Threading;

using InfinniPlatform.Api.ContextTypes.ContextImpl;
using InfinniPlatform.Api.Metadata;
using InfinniPlatform.Api.RestApi.Auth;
using InfinniPlatform.Api.RestApi.CommonApi;
using InfinniPlatform.Api.RestApi.DataApi;
using InfinniPlatform.Api.RestQuery;
using InfinniPlatform.Api.SearchOptions.Builders;
using InfinniPlatform.ContextComponents;
using InfinniPlatform.RestfulApi.Utils;
using InfinniPlatform.Sdk.ContextComponents;

namespace InfinniPlatform.RestfulApi.Executors
{
    public class GetDocumentExecutor : IGetDocumentExecutor
    {
        public GetDocumentExecutor(RestQueryApi restQueryApi,
                                   IMetadataComponent metadataComponent,
                                   IScriptRunnerComponent scriptRunnerComponent,
                                   IConfigurationMediatorComponent configurationMediatorComponent,
                                   InprocessDocumentComponent inprocessDocumentComponent,
                                   IReferenceResolver referenceResolver)
        {
            _restQueryApi = restQueryApi;
            _metadataComponent = metadataComponent;
            _scriptRunnerComponent = scriptRunnerComponent;
            _configurationMediatorComponent = configurationMediatorComponent;
            _inprocessDocumentComponent = inprocessDocumentComponent;
            _referenceResolver = referenceResolver;
        }

        private readonly RestQueryApi _restQueryApi;
        private readonly IMetadataComponent _metadataComponent;
        private readonly IScriptRunnerComponent _scriptRunnerComponent;
        private readonly IConfigurationMediatorComponent _configurationMediatorComponent;
        private readonly InprocessDocumentComponent _inprocessDocumentComponent;
        private readonly IReferenceResolver _referenceResolver;

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

            var target = new ApplyContext();
            target.UserName = string.IsNullOrEmpty(Thread.CurrentPrincipal.Identity.Name) ? AuthorizationStorageExtensions.UnknownUser : Thread.CurrentPrincipal.Identity.Name;
            target.Item = document;

            //ActionUnitSetCredentials
            if (document.Configuration != null)
            {
                if (target.Item.Configuration.ToLowerInvariant() != "systemconfig" &&
                target.Item.Configuration.ToLowerInvariant() != "update" &&
                target.Item.Configuration.ToLowerInvariant() != "restfulapi")
                {
                    //ищем метаданные бизнес-процесса по умолчанию документа 
                    dynamic defaultBusinessProcess = _metadataComponent.GetMetadata(target.Item.Configuration, target.Item.Metadata, MetadataType.Process, "Default");

                    if (defaultBusinessProcess != null && defaultBusinessProcess.Transitions[0].CredentialsType != null)
                    {
                        dynamic credentialsType = defaultBusinessProcess.Transitions[0].CredentialsType;
                        if (credentialsType != null)
                        {
                            if (credentialsType == AuthorizationStorageExtensions.AnonimousUserCredentials)
                            {
                                target.UserName = AuthorizationStorageExtensions.AnonymousUser;
                            }

                            if (credentialsType == AuthorizationStorageExtensions.CustomCredentials)
                            {
                                var scriptArguments = new ApplyContext();
                                scriptArguments.CopyPropertiesFrom(target);
                                scriptArguments.Item = target.Item.Document;
                                scriptArguments.Item.Configuration = target.Item.Configuration;
                                scriptArguments.Item.Metadata = target.Item.Metadata;

                                _scriptRunnerComponent.InvokeScript(defaultBusinessProcess.Transitions[0].CredentialsPoint.ScenarioId, scriptArguments);
                            }
                        }
                    }

                    //ActionUnitComplexAuth
                    //ищем метаданные бизнес-процесса по умолчанию документа 
                    if (defaultBusinessProcess != null && defaultBusinessProcess.Transitions[0].ComplexAuthorizationPoint != null)
                    {
                        var scriptArguments = new ApplyContext();
                        scriptArguments.CopyPropertiesFrom(target);
                        scriptArguments.Item = target.Item.Document;
                        scriptArguments.Item.Configuration = target.Item.Configuration;
                        scriptArguments.Item.Metadata = target.Item.Metadata;

                        _scriptRunnerComponent.InvokeScript(defaultBusinessProcess.Transitions[0].ComplexAuthorizationPoint.ScenarioId, scriptArguments);
                    }
                }
            }

            //ActionUnitGetDocument
            var executor = new DocumentExecutor(_configurationMediatorComponent,
                                                _metadataComponent,
                                                _inprocessDocumentComponent,
                                                _referenceResolver);

            target.Result = executor.GetCompleteDocuments(null,
                                                          document.Configuration,
                                                          document.Metadata,
                                                          target.UserName,
                                                          Convert.ToInt32(document.PageNumber),
                                                          Convert.ToInt32(document.PageSize),
                                                          document.Filter,
                                                          document.Sorting,
                                                          document.IgnoreResolve);

            return target.Result;
        }

        private RestQueryResponse ExecutePost(string action, string id, object body)
        {
            var response = _restQueryApi.QueryPostJsonRaw("RestfulApi", "configuration", action, id, body);

            return response;
        }
    }
}