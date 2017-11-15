using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using InfinniPlatform.DocumentStorage.Properties;
using InfinniPlatform.Http;
using InfinniPlatform.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;

namespace InfinniPlatform.DocumentStorage
{
    /// <summary>
    /// Базовый класс сервисов по работе с документами.
    /// </summary>
    public abstract class DocumentControllerBase : Controller
    {
        protected DocumentControllerBase(IPerformanceLogger perfLogger, ILogger logger)
        {
            _perfLogger = perfLogger;
            _logger = logger;
        }


        private readonly IPerformanceLogger _perfLogger;
        private readonly ILogger _logger;

        [HttpGet("documents/{documentType}/{id?}")]
        public object ProcessGet(string documentType, string id)
        {
            if (CanGet)
            {
                Get();
            }

            return new object();
        }

        [HttpPost("documents/{documentType}")]
        public object ProcessPost(string documentType)
        {
            if (CanPost)
            {
                Post();
            }

            return new object();
        }

        [HttpDelete("documents/{documentType}/{id?}")]
        public object ProcessDelete(string documentType, string id)
        {
            if (CanDelete)
            {
                Delete();
            }

            return new object();
        }


        /// <summary>
        /// Имя типа документа.
        /// </summary>
        public string DocumentType { get; protected set; }

        /// <summary>
        /// Разрешено ли получение документов.
        /// </summary>
        public bool CanGet { get; protected set; }

        /// <summary>
        /// Разрешено ли сохранение документов.
        /// </summary>
        public bool CanPost { get; protected set; }

        /// <summary>
        /// Разрешено ли удаление документов.
        /// </summary>
        public bool CanDelete { get; protected set; }

        /// <summary>
        /// Обрабатывает запрос на получение документов.
        /// </summary>
        protected abstract Task<object> Get();

        /// <summary>
        /// Обрабатывает запрос на сохранение документа.
        /// </summary>
        protected abstract Task<object> Post();

        /// <summary>
        /// Обрабатывает запрос на удаление документа.
        /// </summary>
        protected abstract Task<object> Delete();


        /// <summary>
        /// Обрабатывает запрос с использованием указанных правил.
        /// </summary>
        protected async Task<object> ProcessRequestAsync<TQuery, TResult>(
            Func<HttpRequest, RouteData, TQuery> queryFunc,
            Func<TQuery, Task<TResult>> handlerFunc,
            Func<TQuery, Task<TResult>> onBefore,
            Func<TQuery, TResult, Exception, Task> onAfter,
            Func<Exception, string> onError) where TResult : DocumentQueryResult
        {
            var startTime = DateTime.Now;

            object response;
            TResult result = null;
            Exception error = null;

            try
            {
                // Разбор входящего запроса
                var queryResult = TryExecute(() => queryFunc(Request, RouteData));
                var query = queryResult.Item1;
                error = queryResult.Item2;

                if (error == null)
                {
                    // Выполнение предобработчика
                    var onBeforeResult = await TryExecuteAsync(() => onBefore(query));
                    result = onBeforeResult.Item1;
                    error = onBeforeResult.Item2;

                    if (error == null)
                    {
                        // Если нет результата
                        if (result == null)
                        {
                            // Выполнение обработчика
                            var handlerResult = await TryExecuteAsync(() => handlerFunc(query));
                            result = handlerResult.Item1;
                            error = handlerResult.Item2;
                        }

                        var resultClosure = result;
                        var errorClosure = error;

                        // Выполнение постобработчика
                        var onAfterError = await TryExecuteAsync(() => onAfter(query, resultClosure, errorClosure));

                        if (onAfterError != null)
                        {
                            error = (error == null) ? onAfterError : new AggregateException(error, onAfterError);
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                error = exception;
            }
            finally
            {
                var success = (error == null && (result?.ValidationResult == null || result.ValidationResult.IsValid));

                var serverError = error != null;
                var validationError = result?.ValidationResult != null && result.ValidationResult.IsValid == false;

                var requestResult = new ServiceResult<TResult>
                {
                    Success = success,
                    Result = result,
                    Error = BuildError(error, onError)
                };

                var statusCode = 200;

                if (!success)
                {
                    if (serverError)
                    {
                        statusCode = 500;
                    }
                    else
                    {
                        if (validationError)
                        {
                            statusCode = 400;
                        }
                    }
                }

                response = new JsonHttpResponse(requestResult)
                {
                    StatusCode = statusCode
                };
            }

            // Запись в журнал

            var method = $"{Request.Method}::{Request.Path}";

            if (error != null)
            {
                _logger.LogError(Resources.RequestProcessedWithException, error, () => new Dictionary<string, object> { { "method", method } });
            }

            _perfLogger.Log(method, startTime, error);

            return response;
        }


        private static Tuple<T, Exception> TryExecute<T>(Func<T> action)
        {
            T result;
            Exception error;

            try
            {
                result = action();
                error = null;
            }
            catch (Exception exception)
            {
                result = default(T);
                error = exception;
            }

            return new Tuple<T, Exception>(result, error);
        }

        private static async Task<Tuple<T, Exception>> TryExecuteAsync<T>(Func<Task<T>> action)
        {
            T result;
            Exception error;

            try
            {
                result = await action();
                error = null;
            }
            catch (Exception exception)
            {
                result = default(T);
                error = exception;
            }

            return new Tuple<T, Exception>(result, error);
        }

        private static async Task<Exception> TryExecuteAsync(Func<Task> action)
        {
            Exception error;

            try
            {
                await action();
                error = null;
            }
            catch (Exception exception)
            {
                error = exception;
            }

            return error;
        }

        private string BuildError(Exception error, Func<Exception, string> onError)
        {
            if (error == null)
            {
                return null;
            }

            string errorMessage = null;

            try
            {
                errorMessage = onError(error);
            }
            catch
            {
                // ignored
            }

            if (string.IsNullOrEmpty(errorMessage))
            {
                try
                {
                    errorMessage = error.GetFullMessage();
                }
                catch
                {
                    // ignored
                }
            }

            return errorMessage;
        }
    }
}