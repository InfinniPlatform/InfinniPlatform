using System.Collections.Generic;
using System.Threading.Tasks;

using InfinniPlatform.BlobStorage.Abstractions;
using InfinniPlatform.Core.Abstractions.Dynamic;
using InfinniPlatform.Core.Abstractions.Http;
using InfinniPlatform.Core.Abstractions.Logging;
using InfinniPlatform.DocumentStorage.Abstractions;

namespace InfinniPlatform.DocumentStorage.HttpService
{
    /// <summary>
    /// Сервис по работе с документами на базе <see cref="IDocumentStorage{TDocument}"/>.
    /// </summary>
    [LoggerName("DocumentHttpService")]
    public class DocumentHttpService : DocumentHttpServiceBase
    {
        public DocumentHttpService(IDocumentHttpServiceHandlerBase serviceHandler,
                                   IDocumentQueryFactory queryFactory,
                                   IDocumentStorageFactory storageFactory,
                                   ISystemDocumentStorageFactory systemStorageFactory,
                                   IBlobStorage blobStorage,
                                   IPerformanceLog performanceLog,
                                   ILog log)
            : base(performanceLog, log)
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


        protected override void Load(IHttpServiceBuilder builder)
        {
            _serviceHandler.Load(builder);
        }


        protected override Task<object> Get(IHttpRequest request)
        {
            return ProcessRequestAsync(request,
                r => _queryFactory.CreateGetQuery(r),
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
                                    cursor = sortedCursor = (sortedCursor == null)
                                        ? cursor.SortBy(order.Key)
                                        : sortedCursor.ThenBy(order.Key);
                                    break;
                                case DocumentSortOrder.Desc:
                                    cursor = sortedCursor = (sortedCursor == null)
                                        ? cursor.SortByDescending(order.Key)
                                        : sortedCursor.ThenByDescending(order.Key);
                                    break;
                                case DocumentSortOrder.TextScore:
                                    cursor = sortedCursor = (sortedCursor == null)
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

        protected override Task<object> Post(IHttpRequest request)
        {
            return ProcessRequestAsync(request,
                r => _queryFactory.CreatePostQuery(r),
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
                            query.Document.SetProperty(file.Key, blobInfo);
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

        protected override Task<object> Delete(IHttpRequest request)
        {
            return ProcessRequestAsync(request,
                r => _queryFactory.CreateDeleteQuery(r),
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