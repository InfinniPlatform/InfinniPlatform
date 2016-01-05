using System;
using System.Collections.Generic;

using InfinniPlatform.Core.Transactions;
using InfinniPlatform.Owin.Middleware;
using InfinniPlatform.Sdk.Environment.Log;

using Microsoft.Owin;

namespace InfinniPlatform.WebApi.Middleware
{
    /// <summary>
    /// Модуль хостинга приложений на платформе
    /// </summary>
    internal sealed class ApplicationSdkOwinMiddleware : RoutingOwinMiddleware
    {
        public ApplicationSdkOwinMiddleware(OwinMiddleware next, IDocumentTransactionScopeProvider transactionScopeProvider, IEnumerable<IHandlerRegistration> handlers) : base(next)
        {
            _transactionScopeProvider = transactionScopeProvider;

            foreach (var handler in handlers)
            {
                RegisterHandler(handler);
            }
        }

        private readonly IDocumentTransactionScopeProvider _transactionScopeProvider;

        protected override IRequestHandlerResult OnRequestExecuted(IRequestHandlerResult result)
        {
            var transactionScope = _transactionScopeProvider.GetTransactionScope();

            if (transactionScope != null)
            {
                // Если запрос завершился успешно
                if (result.IsSuccess)
                {
                    try
                    {
                        // Попытка фиксация транзакции
                        transactionScope.Complete();
                    }
                    catch (Exception exception)
                    {
                        // Обработка ошибки фиксации транзакции
                        result = new ErrorRequestHandlerResult(exception.GetMessage());
                    }
                }
                else
                {
                    // Отмена транзакции
                    transactionScope.Rollback();
                }
            }

            return base.OnRequestExecuted(result);
        }
    }
}