using System.Collections.Generic;
using System.Threading.Tasks;

using InfinniPlatform.BlobStorage;
using InfinniPlatform.Dynamic;
using InfinniPlatform.Logging;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;

namespace InfinniPlatform.DocumentStorage
{
    /// <summary>
    /// Сервис по работе с документами на базе <see cref="IDocumentStorage{TDocument}" />.
    /// </summary>
    public class DocumentRequestExecutor : DocumentRequestExecutorBase, IDocumentRequestExecutor
    {
        public DocumentRequestExecutor(IDocumentHttpServiceHandlerBase serviceHandler,
                                       IDocumentQueryFactory queryFactory,
                                       IDocumentStorageFactory storageFactory,
                                       ISystemDocumentStorageFactory systemStorageFactory,
                                       IBlobStorage blobStorage,
                                       ILogger<DocumentRequestExecutor> logger,
                                       IPerformanceLogger<DocumentRequestExecutor> perfLogger)
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
                                                   var blobInfo = await _blobStorage.CreateBlobAsync(file.Name, file.ContentType, file);

                                                   // Включение информации о файле в ответ
                                                   fileInfos[file.Name] = blobInfo;

                                                   // Установка ссылки на файл в документе
                                                   query.Document.TrySetPropertyValueByPath(file.Name, blobInfo);
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
    public class DocumentRequestExecutor<TDocument> : DocumentRequestExecutorBase, IDocumentRequestExecutor where TDocument : Document
    {
        public DocumentRequestExecutor(IDocumentHttpServiceHandlerBase serviceHandler,
                                       IDocumentQueryFactory<TDocument> queryFactory,
                                       IDocumentStorageFactory storageFactory,
                                       ISystemDocumentStorageFactory systemStorageFactory,
                                       IBlobStorage blobStorage,
                                       IPerformanceLogger<DocumentRequestExecutor<TDocument>> perfLogger,
                                       ILogger<DocumentRequestExecutor<TDocument>> logger)
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
                                                   var blobInfo = await _blobStorage.CreateBlobAsync(file.Name, file.ContentType, file);

                                                   // Включение информации о файле в ответ
                                                   fileInfos[file.Name] = blobInfo;

                                                   // Установка ссылки на файл в документе
                                                   query.Document.TrySetPropertyValueByPath(file.Name, blobInfo);
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