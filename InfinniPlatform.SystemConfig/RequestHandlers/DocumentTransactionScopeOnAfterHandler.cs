using System;

using InfinniPlatform.Core.Transactions;
using InfinniPlatform.Sdk.Services;

namespace InfinniPlatform.SystemConfig.RequestHandlers
{
    internal sealed class DocumentTransactionScopeOnAfterHandler
    {
        public DocumentTransactionScopeOnAfterHandler(IDocumentTransactionScopeProvider transactionScopeProvider)
        {
            _transactionScopeProvider = transactionScopeProvider;
        }

        private readonly IDocumentTransactionScopeProvider _transactionScopeProvider;

        public void OnAfter(IHttpRequest request, IHttpResponse response, Exception exception)
        {
            var transactionScope = _transactionScopeProvider.GetTransactionScope();

            if (transactionScope != null)
            {
                var isSuccessStatus = (exception == null) && ((response == null) || (response.StatusCode >= 200 && response.StatusCode <= 299));

                // Если запрос завершился успешно
                if (isSuccessStatus)
                {
                    // Попытка фиксация транзакции
                    transactionScope.Complete();
                }
                else
                {
                    // Отмена транзакции
                    transactionScope.Rollback();
                }
            }
        }
    }
}