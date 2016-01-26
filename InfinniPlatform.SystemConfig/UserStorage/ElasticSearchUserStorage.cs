using System.Linq;

using InfinniPlatform.Core.RestApi.Auth;
using InfinniPlatform.Core.Transactions;
using InfinniPlatform.ElasticSearch.ElasticProviders;

namespace InfinniPlatform.SystemConfig.UserStorage
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

        public object Find(string property, string value)
        {
            var indexName = _elasticConnection.GetIndexName(AuthorizationStorageExtensions.AuthorizationConfigId);

            // TODO: Load only UserInfo (without header).
            // TODO: Set specific types, not AllTypes().
            var searchResponse = _elasticConnection
                .Client.Search<dynamic>(d => d.Index(indexName)
                                              .AllTypes()
                                              .Filter(f => f.Term(ElasticConstants.IndexObjectPath + property, value.ToLower()))
                                              .Size(1));

            object userInfo = null;

            if (searchResponse.Total > 0 && searchResponse.Documents != null)
            {
                var document = searchResponse.Documents.FirstOrDefault();

                if (document != null)
                {
                    userInfo = document.Values;
                }
            }

            if (userInfo != null)
            {
                var transactionScope = _transactionScopeProvider.GetTransactionScope();

                userInfo = transactionScope.GetDocuments(AuthorizationStorageExtensions.AuthorizationConfigId, AuthorizationStorageExtensions.UserStore, new[] { userInfo }).FirstOrDefault();
            }

            return userInfo;
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