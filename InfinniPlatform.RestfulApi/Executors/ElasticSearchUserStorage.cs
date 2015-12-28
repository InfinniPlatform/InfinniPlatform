using System.Linq;

using InfinniPlatform.Api.RestApi.Auth;
using InfinniPlatform.Index.ElasticSearch.Implementation.ElasticProviders;
using InfinniPlatform.Transactions;

namespace InfinniPlatform.RestfulApi.Executors
{
    public sealed class ElasticSearchUserStorage
    {
        private readonly ElasticConnection _elasticConnection;
        private readonly IDocumentTransactionScopeProvider _transactionScopeProvider;

        public ElasticSearchUserStorage(ElasticConnection elasticConnection, IDocumentTransactionScopeProvider transactionScopeProvider)
        {
            _elasticConnection = elasticConnection;
            _transactionScopeProvider = transactionScopeProvider;
        }

        public object Find(string property, object value)
        {
            var indexName = _elasticConnection.GetIndexName(AuthorizationStorageExtensions.AuthorizationConfigId);
            var indexType = _elasticConnection.GetActualTypeName(AuthorizationStorageExtensions.AuthorizationConfigId, AuthorizationStorageExtensions.UserStore);

            //TODO Partial get.
            var searchResponse = _elasticConnection
                .Client.Search<dynamic>(d => d.Index(indexName)
                                                  .Type(indexType)
                                                  .Filter(f => f.Term(ElasticConstants.IndexObjectPath + property, value))
                                                  .Size(1));

            return (searchResponse.Total > 0)
                       ? searchResponse.Documents?.FirstOrDefault()?.Values
                       : null;
        }

        public void Save(object userId, object userInfo)
        {
            var transactionScope = _transactionScopeProvider.GetTransactionScope();
            transactionScope.SaveDocument(AuthorizationStorageExtensions.AuthorizationConfigId, AuthorizationStorageExtensions.UserStore, userId, userInfo);
        }

        public void Delete(object userId)
        {
            var transactionScope = _transactionScopeProvider.GetTransactionScope();
            transactionScope.DeleteDocument(AuthorizationStorageExtensions.AuthorizationConfigId, AuthorizationStorageExtensions.UserStore, userId);
        }
    }
}