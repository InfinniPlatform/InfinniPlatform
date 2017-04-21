using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using InfinniPlatform.DocumentStorage.Properties;
using InfinniPlatform.Http;
using InfinniPlatform.Logging;

namespace InfinniPlatform.DocumentStorage
{
    /// <summary>
    /// Базовый класс сервисов по работе с документами.
    /// </summary>
    public abstract class DocumentHttpServiceBase : IHttpService
    {
        protected DocumentHttpServiceBase(IPerformanceLog performanceLog, ILog log)
        {
            _performanceLog = performanceLog;
            _log = log;
        }


        private readonly IPerformanceLog _performanceLog;
        private readonly ILog _log;


        void IHttpService.Load(IHttpServiceBuilder builder)
        {
            builder.ServicePath = DocumentHttpServiceConstants.DefaultServicePath;

            if (CanGet)
            {
                builder.Get[$"/{DocumentType}/{{{DocumentHttpServiceConstants.DocumentIdKey}?}}"] = Get;
            }

            if (CanPost)
            {
                builder.Post[$"/{DocumentType}"] = Post;
            }

            if (CanDelete)
            {
                builder.Delete[$"/{DocumentType}/{{{DocumentHttpServiceConstants.DocumentIdKey}?}}"] = Delete;
            }

            Load(builder);
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
        /// Загружает настройки сервиса по работе с документами.
        /// </summary>
        protected abstract void Load(IHttpServiceBuilder builder);

        /// <summary>
        /// Обрабатывает запрос на получение документов.
        /// </summary>
        protected abstract Task<object> Get(IHttpRequest request);

        /// <summary>
        /// Обрабатывает запрос на сохранение документа.
        /// </summary>
        protected abstract Task<object> Post(IHttpRequest request);

        /// <summary>
        /// Обрабатывает запрос на удаление документа.
        /// </summary>
        protected abstract Task<object> Delete(IHttpRequest request);


        /// <summary>
        /// Обрабатывает запрос с использованием указанных правил.
        /// </summary>
        protected async Task<object> ProcessRequestAsync<TQuery, TResult>(
            IHttpRequest request,
            Func<IHttpRequest, TQuery> queryFunc,
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
                var queryResult = TryExecute(() => queryFunc(request));
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

            var method = $"{request.Method}::{request.Path}";

            if (error != null)
            {
                _log.Error(Resources.RequestProcessedWithException, error, () => new Dictionary<string, object> { { "method", method } });
            }

            _performanceLog.Log(method, startTime, error);

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
            }

            if (string.IsNullOrEmpty(errorMessage))
            {
                try
                {
                    errorMessage = error.GetFullMessage();
                }
                catch
                {
                }
            }

            return errorMessage;
        }
    }
}