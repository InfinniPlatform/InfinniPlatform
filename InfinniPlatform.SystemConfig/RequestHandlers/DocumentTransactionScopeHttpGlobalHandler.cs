using System;

using InfinniPlatform.Core.Transactions;
using InfinniPlatform.Sdk.Services;

namespace InfinniPlatform.SystemConfig.RequestHandlers
{
    internal sealed class DocumentTransactionScopeHttpGlobalHandler : IHttpGlobalHandler
    {
        public DocumentTransactionScopeHttpGlobalHandler(IDocumentTransactionScopeProvider transactionScopeProvider)
        {
            _transactionScopeProvider = transactionScopeProvider;

            OnAfter = (request, result) => OnCompleteRequest(result, null);
            OnError = (request, error) => OnCompleteRequest(null, error);
        }


        private readonly IDocumentTransactionScopeProvider _transactionScopeProvider;


        public Func<IHttpRequest, object> OnBefore { get; set; }

        public Func<IHttpRequest, object, object> OnAfter { get; set; }

        public Func<IHttpRequest, Exception, object> OnError { get; set; }

        public Func<object, IHttpResponse> ResultConverter { get; set; }


        private object OnCompleteRequest(object result, Exception error)
        {
            var transactionScope = _transactionScopeProvider.GetTransactionScope();

            if (transactionScope != null)
            {
                var isSuccessStatus = (error == null) && IsSuccessResult(result);

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

            return result ?? error;
        }

        private static bool IsSuccessResult(object result)
        {
            var response = result as IHttpResponse;

            return (response == null) || (response.StatusCode >= 200 && response.StatusCode <= 299);
        }
    }
}