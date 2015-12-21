using System;
using System.Collections.Generic;

using InfinniPlatform.Api.ContextTypes.ContextImpl;
using InfinniPlatform.Api.RestApi.CommonApi;
using InfinniPlatform.Api.RestApi.DataApi;
using InfinniPlatform.Api.RestQuery;
using InfinniPlatform.Api.SearchOptions.Builders;
using InfinniPlatform.RestfulApi.ActionUnits;
using InfinniPlatform.RestfulApi.DefaultProcessUnits;

namespace InfinniPlatform.RestfulApi.Executors
{
    public class GetDocumentExecutor : IGetDocumentExecutor
    {
        public GetDocumentExecutor(RestQueryApi restQueryApi,
                                   ActionUnitSetCredentials actionUnitSetCredentials,
                                   ActionUnitComplexAuth actionUnitComplexAuth,
                                   ActionUnitGetDocument actionUnitGetDocument)
        {
            _restQueryApi = restQueryApi;
            _actionUnitSetCredentials = actionUnitSetCredentials;
            _actionUnitComplexAuth = actionUnitComplexAuth;
            _actionUnitGetDocument = actionUnitGetDocument;
        }

        private readonly RestQueryApi _restQueryApi;
        private readonly ActionUnitSetCredentials _actionUnitSetCredentials;
        private readonly ActionUnitComplexAuth _actionUnitComplexAuth;
        private readonly ActionUnitGetDocument _actionUnitGetDocument;

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

        public IEnumerable<dynamic> GetDocumentUnfolded(string configuration, string metadata, dynamic filter, int pageNumber, int pageSize, IEnumerable<dynamic> ignoreResolve = null, dynamic sorting = null)
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

            var applyContext = new ApplyContext();

            var actionUnitSetCredentials = _actionUnitSetCredentials;
            actionUnitSetCredentials.Action(applyContext);

            //var actionUnitDocumentAuth = new ActionUnitDocumentAuth();

            var actionUnitComplexAuth = _actionUnitComplexAuth;
            actionUnitComplexAuth.Action(applyContext);

            var actionUnitGetDocument = _actionUnitGetDocument;
            actionUnitGetDocument.Action(applyContext);

            //var actionUnitFilterAuthDocument = new ActionUnitFilterAuthDocument();
            var result = ExecutePost("getdocument", null, document);

            return result.ToDynamicList();
        }

        private RestQueryResponse ExecutePost(string action, string id, object body)
        {
            var response = _restQueryApi.QueryPostJsonRaw("RestfulApi", "configuration", action, id, body);

            return response;
        }
    }
}