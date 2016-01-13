using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web.Http.Filters;

using InfinniPlatform.Core.Transactions;
using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.Sdk.Logging;
using InfinniPlatform.Sdk.Serialization;

namespace InfinniPlatform.WebApi.Filters
{
    /// <summary>
    /// Обработчик окончания обработки запроса для завершения транзакции.
    /// </summary>
    internal sealed class DocumentTransactionScopeFilterAttribute : ActionFilterAttribute
    {
        public DocumentTransactionScopeFilterAttribute(IDocumentTransactionScopeProvider transactionScopeProvider)
        {
            _transactionScopeProvider = transactionScopeProvider;
        }

        private readonly IDocumentTransactionScopeProvider _transactionScopeProvider;

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            var transactionScope = _transactionScopeProvider.GetTransactionScope();

            if (transactionScope != null)
            {
                // Если запрос завершился успешно
                if (actionExecutedContext.Response.IsSuccessStatusCode)
                {
                    try
                    {
                        // Попытка фиксация транзакции
                        transactionScope.Complete();
                    }
                    catch (Exception exception)
                    {
                        // Обработка ошибки фиксации транзакции
                        actionExecutedContext.Response.StatusCode = HttpStatusCode.InternalServerError;
                        actionExecutedContext.Response.Content = CreateErrorContent(exception);
                    }
                }
                else
                {
                    // Отмена транзакции
                    transactionScope.Rollback();
                }
            }

            base.OnActionExecuted(actionExecutedContext);
        }

        private static HttpContent CreateErrorContent(Exception exception)
        {
            // TODO: Сейчас все обработчики ожидают именно такой формат ответа

            var error = new DynamicWrapper
                        {
                            ["IsValid"] = false,
                            ["IsInternalServerError"] = true,
                            ["ValidationMessage"] = exception.GetMessage()
                        };

            var errorContent = JsonObjectSerializer.Default.Serialize(error);

            return new ByteArrayContent(errorContent)
                   {
                       Headers =
                       {
                           ContentType = new MediaTypeHeaderValue("application/json")
                                         {
                                             CharSet = Encoding.UTF8.WebName
                                         }
                       }
                   };
        }
    }
}