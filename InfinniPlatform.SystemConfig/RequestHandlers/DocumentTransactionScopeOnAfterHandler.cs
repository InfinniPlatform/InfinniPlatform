using System;
using System.IO;

using InfinniPlatform.Core.Transactions;
using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.Sdk.Logging;
using InfinniPlatform.Sdk.Serialization;
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

        public void OnAfter(IHttpRequest request, IHttpResponse response)
        {
            var transactionScope = _transactionScopeProvider.GetTransactionScope();

            if (transactionScope != null)
            {
                var isSuccessStatusCode = (response.StatusCode >= 200 && response.StatusCode <= 299);

                // Если запрос завершился успешно
                if (isSuccessStatusCode)
                {
                    try
                    {
                        // Попытка фиксация транзакции
                        transactionScope.Complete();
                    }
                    catch (Exception unexpectedException)
                    {
                        // Обработка ошибки фиксации транзакции
                        response.StatusCode = 500;
                        response.ContentType = HttpConstants.JsonContentType;
                        response.Content = responseStream => CreateErrorContent(responseStream, unexpectedException);
                    }
                }
                else
                {
                    // Отмена транзакции
                    transactionScope.Rollback();
                }
            }
        }

        private static void CreateErrorContent(Stream responseStream, Exception unexpectedException)
        {
            // TODO: Сейчас все обработчики ожидают именно такой формат ответа

            var error = new DynamicWrapper
                        {
                            ["IsValid"] = false,
                            ["IsInternalServerError"] = true,
                            ["ValidationMessage"] = unexpectedException.GetMessage()
                        };

            JsonObjectSerializer.Default.Serialize(responseStream, error);
        }
    }
}