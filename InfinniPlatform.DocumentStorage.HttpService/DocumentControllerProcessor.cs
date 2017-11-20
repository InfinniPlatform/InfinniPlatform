using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

using InfinniPlatform.BlobStorage;
using InfinniPlatform.DocumentStorage.Properties;
using InfinniPlatform.Dynamic;
using InfinniPlatform.Http;
using InfinniPlatform.IoC;
using InfinniPlatform.Logging;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;

namespace InfinniPlatform.DocumentStorage
{
    public abstract class DocumentControllerProcessorBase
    {
        private readonly ILogger _logger;
        private readonly IPerformanceLogger _perfLogger;

        protected DocumentControllerProcessorBase(ILogger logger,
                                                  IPerformanceLogger perfLogger)
        {
            _logger = logger;
            _perfLogger = perfLogger;
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
        /// Обрабатывает запрос с использованием указанных правил.
        /// </summary>
        protected async Task<object> ProcessRequestAsync<TQuery, TResult>(
            HttpRequest request,
            RouteData routeData,
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
                var queryResult = TryExecute(() => queryFunc(request, routeData));
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

                var jsonResult = new JsonResult(requestResult);

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

                jsonResult.StatusCode = statusCode;

                response = jsonResult;
            }

            // Запись в журнал

            var method = $"{request.Method}::{request.Path}";

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

        private static string BuildError(Exception error, Func<Exception, string> onError)
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


    public class DocumentControllerProcessorProvider : IDocumentControllerProcessorProvider
    {
        public DocumentControllerProcessorProvider(IEnumerable<IDocumentHttpServiceHandlerBase> httpServiceHandlers,
                                                   IContainerResolver containerResolver)
        {
            _httpServiceHandlers = httpServiceHandlers;
            _containerResolver = containerResolver;
            _processorsCacheLazy = new Lazy<Dictionary<string, IDocumentControllerProcessor>>(CreateProcessorsCache);
        }

        private readonly IEnumerable<IDocumentHttpServiceHandlerBase> _httpServiceHandlers;
        private readonly IContainerResolver _containerResolver;
        private readonly Lazy<Dictionary<string, IDocumentControllerProcessor>> _processorsCacheLazy;

        public Dictionary<string, IDocumentControllerProcessor> ProcessorsCache => _processorsCacheLazy.Value;

        private Dictionary<string, IDocumentControllerProcessor> CreateProcessorsCache()
        {
            var dictionary = new Dictionary<string, IDocumentControllerProcessor>();

            foreach (var httpServiceHandler in _httpServiceHandlers)
            {
                // Создание типизированных сервисов

                var httpServiceHandlerType = httpServiceHandler.GetType();

                var clrDocumentTypes = httpServiceHandlerType
                    .GetTypeInfo()
                    .GetInterfaces()
                    .Where(i => i.GetTypeInfo().IsGenericType && i.GetGenericTypeDefinition() == typeof(IDocumentHttpServiceHandler<>))
                    .Select(i => i.GetTypeInfo().GetGenericArguments()[0]);

                foreach (var clrDocumentType in clrDocumentTypes)
                {
                    var handlerType = typeof(IDocumentHttpServiceHandlerBase);
                    var serviceType = typeof(DocumentControllerProcessor<>).MakeGenericType(clrDocumentType);
                    var serviceFunc = typeof(Func<,>).MakeGenericType(handlerType, serviceType);

                    var serviceFactory = (Delegate)_containerResolver.Resolve(serviceFunc);

                    var processor = (IDocumentControllerProcessor)serviceFactory.FastDynamicInvoke(httpServiceHandler);

                    dictionary.Add(httpServiceHandler.DocumentType, processor);
                }

                // Создание не типизированных сервисов

                if (httpServiceHandler is IDocumentHttpServiceHandler)
                {
                    var serviceFactory = _containerResolver.Resolve<Func<IDocumentHttpServiceHandlerBase, DocumentControllerProcessor>>();

                    var processor = (IDocumentControllerProcessor)serviceFactory.Invoke(httpServiceHandler);

                    dictionary.Add(httpServiceHandler.DocumentType, processor);
                }
            }

            return dictionary;
        }
    }


    public interface IDocumentControllerProcessorProvider
    {
        Dictionary<string, IDocumentControllerProcessor> ProcessorsCache { get; }
    }


    public interface IDocumentControllerProcessor
    {
        Task<object> Get(HttpRequest request, RouteData routeData);

        Task<object> Post(HttpRequest request, RouteData routeData);

        Task<object> Delete(HttpRequest request, RouteData routeData);
    }


    /// <summary>
    /// Сервис по работе с документами на базе <see cref="IDocumentStorage{TDocument}" />.
    /// </summary>
    public class DocumentControllerProcessor : DocumentControllerProcessorBase, IDocumentControllerProcessor
    {
        public DocumentControllerProcessor(IDocumentHttpServiceHandlerBase serviceHandler,
                                           IDocumentQueryFactory queryFactory,
                                           IDocumentStorageFactory storageFactory,
                                           ISystemDocumentStorageFactory systemStorageFactory,
                                           IBlobStorage blobStorage,
                                           ILogger<DocumentControllerProcessorBase> logger,
                                           IPerformanceLogger<DocumentControllerProcessorBase> perfLogger)
            : base(logger, perfLogger)
        {
            var storage = serviceHandler.AsSystem
                              ? systemStorageFactory.GetStorage(serviceHandler.DocumentType)
                              : storageFactory.GetStorage(serviceHandler.DocumentType);

            DocumentType = storage.DocumentType;
            CanGet = serviceHandler.CanGet;
            CanPost = serviceHandler.CanPost;
            CanDelete = serviceHandler.CanDelete;

            _serviceHandler = (IDocumentHttpServiceHandler)serviceHandler;
            _queryFactory = queryFactory;
            _blobStorage = blobStorage;
            _storage = storage;
        }


        private readonly IDocumentHttpServiceHandler _serviceHandler;
        private readonly IDocumentQueryFactory _queryFactory;
        private readonly IDocumentStorage _storage;
        private readonly IBlobStorage _blobStorage;


        public Task<object> Get(HttpRequest request, RouteData routeData)
        {
            if (!CanGet)
            {
                return Task.FromResult<object>(new ForbidResult());
            }

            return ProcessRequestAsync(request,
                                       routeData,
                                       (r, rd) => _queryFactory.CreateGetQuery(r, rd),
                                       async query =>
                                       {
                                           // Установка фильтра
                                           var cursor = string.IsNullOrWhiteSpace(query.Search)
                                                            ? _storage.Find(query.Filter)
                                                            : _storage.FindText(query.Search, filter: query.Filter);

                                           long? count = null;

                                           if (query.Count)
                                           {
                                               // Подсчет общего количества документов по фильтру
                                               count = await cursor.CountAsync();
                                           }

                                           // Установка правил сортировки
                                           if (query.Order != null)
                                           {
                                               IDocumentFindSortedCursor sortedCursor = null;

                                               foreach (var order in query.Order)
                                               {
                                                   switch (order.Value)
                                                   {
                                                       case DocumentSortOrder.Asc:
                                                           cursor = sortedCursor = sortedCursor == null
                                                                                       ? cursor.SortBy(order.Key)
                                                                                       : sortedCursor.ThenBy(order.Key);
                                                           break;
                                                       case DocumentSortOrder.Desc:
                                                           cursor = sortedCursor = sortedCursor == null
                                                                                       ? cursor.SortByDescending(order.Key)
                                                                                       : sortedCursor.ThenByDescending(order.Key);
                                                           break;
                                                       case DocumentSortOrder.TextScore:
                                                           cursor = sortedCursor = sortedCursor == null
                                                                                       ? cursor.SortByTextScore(order.Key)
                                                                                       : sortedCursor.ThenByTextScore(order.Key);
                                                           break;
                                                   }
                                               }
                                           }

                                           // Установки диапазона выборки
                                           cursor = cursor.Skip(query.Skip).Limit(query.Take);

                                           IEnumerable<object> items;

                                           if (query.Select == null)
                                           {
                                               // Выборка документов
                                               items = await cursor.ToListAsync();
                                           }
                                           else
                                           {
                                               // Выборка проекции документов
                                               items = await cursor.Project(query.Select).ToListAsync();
                                           }

                                           return new DocumentGetQueryResult { Items = items, Count = count };
                                       },
                                       _serviceHandler.OnBeforeGet,
                                       _serviceHandler.OnAfterGet,
                                       _serviceHandler.OnError);
        }

        public Task<object> Post(HttpRequest request, RouteData routeData)
        {
            if (!CanPost)
            {
                return Task.FromResult<object>(new ForbidResult());
            }

            return ProcessRequestAsync(request,
                                       routeData, 
                                       (r, rd) => _queryFactory.CreatePostQuery(r, rd),
                                       async query =>
                                       {
                                           IDictionary<string, BlobInfo> fileInfos = null;

                                           if (query.Files != null)
                                           {
                                               fileInfos = new Dictionary<string, BlobInfo>();

                                               // Сохранение списка файлов
                                               foreach (var file in query.Files)
                                               {
                                                   // Сохранение файла в хранилище
                                                   var blobInfo = await _blobStorage.CreateBlobAsync(file.Name, file.ContentType, file.Value);

                                                   // Включение информации о файле в ответ
                                                   fileInfos[file.Key] = blobInfo;

                                                   // Установка ссылки на файл в документе
                                                   query.Document.TrySetPropertyValueByPath(file.Key, blobInfo);
                                               }
                                           }

                                           DocumentUpdateResult status = null;
                                           DocumentValidationResult validationResult = null;

                                           try
                                           {
                                               // Сохранение документа в хранилище
                                               status = await _storage.SaveOneAsync(query.Document);
                                           }
                                           catch (DocumentStorageWriteException exception)
                                           {
                                               validationResult = exception.WriteResult?.ValidationResult;
                                           }

                                           return new DocumentPostQueryResult
                                                  {
                                                      DocumentId = query.Document["_id"],
                                                      FileInfos = fileInfos,
                                                      Status = status,
                                                      ValidationResult = validationResult
                                                  };
                                       },
                                       _serviceHandler.OnBeforePost,
                                       _serviceHandler.OnAfterPost,
                                       _serviceHandler.OnError);
        }

        public Task<object> Delete(HttpRequest request, RouteData routeData)
        {
            if (!CanDelete)
            {
                return Task.FromResult<object>(new ForbidResult());
            }

            return ProcessRequestAsync(request,
                                       routeData,
                                       (r, rd) => _queryFactory.CreateDeleteQuery(r, rd),
                                       async query =>
                                       {
                                           long? deletedCount = null;
                                           DocumentValidationResult validationResult = null;

                                           try
                                           {
                                               // Удаление документа из хранилища
                                               deletedCount = await _storage.DeleteManyAsync(query.Filter);
                                           }
                                           catch (DocumentStorageWriteException exception)
                                           {
                                               validationResult = exception.WriteResult?.ValidationResult;
                                           }

                                           return new DocumentDeleteQueryResult
                                                  {
                                                      DeletedCount = deletedCount,
                                                      ValidationResult = validationResult
                                                  };
                                       },
                                       _serviceHandler.OnBeforeDelete,
                                       _serviceHandler.OnAfterDelete,
                                       _serviceHandler.OnError);
        }
    }


    /// <summary>
    /// Сервис по работе с документами на базе <see cref="IDocumentStorage{TDocument}" />.
    /// </summary>
    public class DocumentControllerProcessor<TDocument> : DocumentControllerProcessorBase, IDocumentControllerProcessor where TDocument : Document
    {
        public DocumentControllerProcessor(IDocumentHttpServiceHandlerBase serviceHandler,
                                           IDocumentQueryFactory<TDocument> queryFactory,
                                           IDocumentStorageFactory storageFactory,
                                           ISystemDocumentStorageFactory systemStorageFactory,
                                           IBlobStorage blobStorage,
                                           IPerformanceLogger<DocumentControllerProcessor<TDocument>> perfLogger,
                                           ILogger<DocumentControllerProcessor<TDocument>> logger)
            : base(logger, perfLogger)
        {
            var storage = serviceHandler.AsSystem
                              ? systemStorageFactory.GetStorage<TDocument>(serviceHandler.DocumentType)
                              : storageFactory.GetStorage<TDocument>(serviceHandler.DocumentType);

            DocumentType = storage.DocumentType;
            CanGet = serviceHandler.CanGet;
            CanPost = serviceHandler.CanPost;
            CanDelete = serviceHandler.CanDelete;

            _serviceHandler = (IDocumentHttpServiceHandler<TDocument>)serviceHandler;
            _queryFactory = queryFactory;
            _blobStorage = blobStorage;
            _storage = storage;
        }


        private readonly IDocumentHttpServiceHandler<TDocument> _serviceHandler;
        private readonly IDocumentQueryFactory<TDocument> _queryFactory;
        private readonly IDocumentStorage<TDocument> _storage;
        private readonly IBlobStorage _blobStorage;


        public Task<object> Get(HttpRequest request, RouteData routeData)
        {
            if (!CanGet)
            {
                return Task.FromResult<object>(new ForbidResult());
            }

            return ProcessRequestAsync(request,
                                       routeData, 
                                       (r, rd) => _queryFactory.CreateGetQuery(r, rd),
                                       async query =>
                                       {
                                           // Установка фильтра
                                           var cursor = string.IsNullOrWhiteSpace(query.Search)
                                                            ? _storage.Find(query.Filter)
                                                            : _storage.FindText(query.Search, filter: query.Filter);

                                           long? count = null;

                                           if (query.Count)
                                           {
                                               // Подсчет общего количества документов по фильтру
                                               count = await cursor.CountAsync();
                                           }

                                           // Установка правил сортировки
                                           if (query.Order != null)
                                           {
                                               IDocumentFindSortedCursor<TDocument, TDocument> sortedCursor = null;

                                               foreach (var order in query.Order)
                                               {
                                                   switch (order.Value)
                                                   {
                                                       case DocumentSortOrder.Asc:
                                                           cursor = sortedCursor = sortedCursor == null
                                                                                       ? cursor.SortBy(order.Key)
                                                                                       : sortedCursor.ThenBy(order.Key);
                                                           break;
                                                       case DocumentSortOrder.Desc:
                                                           cursor = sortedCursor = sortedCursor == null
                                                                                       ? cursor.SortByDescending(order.Key)
                                                                                       : sortedCursor.ThenByDescending(order.Key);
                                                           break;
                                                   }
                                               }
                                           }

                                           // Установки диапазона выборки
                                           cursor = cursor.Skip(query.Skip).Limit(query.Take);

                                           IEnumerable<object> items;

                                           if (query.Select == null)
                                           {
                                               // Выборка документов
                                               items = await cursor.ToListAsync();
                                           }
                                           else
                                           {
                                               // Выборка проекции документов
                                               items = await cursor.Project(query.Select).ToListAsync();
                                           }

                                           return new DocumentGetQueryResult { Items = items, Count = count };
                                       },
                                       _serviceHandler.OnBeforeGet,
                                       _serviceHandler.OnAfterGet,
                                       _serviceHandler.OnError);
        }

        public Task<object> Post(HttpRequest request, RouteData routeData)
        {
            if (!CanPost)
            {
                return Task.FromResult<object>(new ForbidResult());
            }

            return ProcessRequestAsync(request,
                                       routeData,
                                       (r, rd) => _queryFactory.CreatePostQuery(r, rd),
                                       async query =>
                                       {
                                           IDictionary<string, BlobInfo> fileInfos = null;

                                           if (query.Files != null)
                                           {
                                               fileInfos = new Dictionary<string, BlobInfo>();

                                               // Сохранение списка файлов
                                               foreach (var file in query.Files)
                                               {
                                                   // Сохранение файла в хранилище
                                                   var blobInfo = await _blobStorage.CreateBlobAsync(file.Name, file.ContentType, file.Value);

                                                   // Включение информации о файле в ответ
                                                   fileInfos[file.Key] = blobInfo;

                                                   // Установка ссылки на файл в документе
                                                   query.Document.TrySetPropertyValueByPath(file.Key, blobInfo);
                                               }
                                           }

                                           DocumentUpdateResult status = null;
                                           DocumentValidationResult validationResult = null;

                                           try
                                           {
                                               // Сохранение документа в хранилище
                                               status = await _storage.SaveOneAsync(query.Document);
                                           }
                                           catch (DocumentStorageWriteException exception)
                                           {
                                               validationResult = exception.WriteResult?.ValidationResult;
                                           }

                                           return new DocumentPostQueryResult
                                                  {
                                                      DocumentId = query.Document._id,
                                                      FileInfos = fileInfos,
                                                      Status = status,
                                                      ValidationResult = validationResult
                                                  };
                                       },
                                       _serviceHandler.OnBeforePost,
                                       _serviceHandler.OnAfterPost,
                                       _serviceHandler.OnError);
        }

        public Task<object> Delete(HttpRequest request, RouteData routeData)
        {
            if (!CanDelete)
            {
                return Task.FromResult<object>(new ForbidResult());
            }

            return ProcessRequestAsync(request,
                                       routeData,
                                       (r, rd) => _queryFactory.CreateDeleteQuery(r, rd),
                                       async query =>
                                       {
                                           long? deletedCount = null;
                                           DocumentValidationResult validationResult = null;

                                           try
                                           {
                                               // Удаление документа из хранилища
                                               deletedCount = await _storage.DeleteManyAsync(query.Filter);
                                           }
                                           catch (DocumentStorageWriteException exception)
                                           {
                                               validationResult = exception.WriteResult?.ValidationResult;
                                           }

                                           return new DocumentDeleteQueryResult
                                                  {
                                                      DeletedCount = deletedCount,
                                                      ValidationResult = validationResult
                                                  };
                                       },
                                       _serviceHandler.OnBeforeDelete,
                                       _serviceHandler.OnAfterDelete,
                                       _serviceHandler.OnError);
        }
    }
}