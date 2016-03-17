﻿using System.Collections;
using System.Threading.Tasks;

using InfinniPlatform.Sdk.BlobStorage;
using InfinniPlatform.Sdk.Documents;
using InfinniPlatform.Sdk.Documents.Services;
using InfinniPlatform.Sdk.Logging;
using InfinniPlatform.Sdk.Services;
using System.Collections.Generic;

using InfinniPlatform.Sdk.Dynamic;

namespace InfinniPlatform.DocumentStorage.Services
{
    /// <summary>
    /// Сервис по работе с документами на базе <see cref="IDocumentStorage{TDocument}"/>.
    /// </summary>
    [LoggerName("DocumentHttpService")]
    internal sealed class DocumentHttpService : DocumentHttpServiceBase
    {
        public DocumentHttpService(IDocumentHttpServiceHandler serviceHandler,
                                   IDocumentQueryFactory queryFactory,
                                   IDocumentStorageFactory storageFactory,
                                   IBlobStorage blobStorage,
                                   IPerformanceLog performanceLog,
                                   ILog log)
            : base(performanceLog, log)
        {
            var storage = storageFactory.GetStorage(serviceHandler.DocumentType);

            DocumentType = storage.DocumentType;

            _serviceHandler = serviceHandler;
            _queryFactory = queryFactory;
            _blobStorage = blobStorage;
            _storage = storage;
        }


        private readonly IDocumentHttpServiceHandler _serviceHandler;
        private readonly IDocumentQueryFactory _queryFactory;
        private readonly IDocumentStorage _storage;
        private readonly IBlobStorage _blobStorage;


        public override string DocumentType { get; }


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

                    IEnumerable items;

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

                    long? count = null;

                    if (query.Count)
                    {
                        // Подсчет общего количества документов по фильтру
                        count = await cursor.CountAsync();
                    }

                    return new DocumentGetQueryResult { Items = items, Count = count };
                },
                _serviceHandler.OnBeforeGet,
                _serviceHandler.OnAfterGet);
        }

        protected override Task<object> Post(IHttpRequest request)
        {
            return ProcessRequestAsync(request,
                r => _queryFactory.CreatePostQuery(r, DocumentFormKey),
                async query =>
                {
                    IDictionary<string, object> fileIds = null;

                    if (query.Files != null)
                    {
                        fileIds = new Dictionary<string, object>();

                        // Сохранение списка файлов
                        foreach (var file in query.Files)
                        {
                            // Сохранение файла в хранилище
                            var blobInfo = _blobStorage.CreateBlob(file.Name, file.ContentType, file.Value);

                            // Включение информации о файле в ответ
                            fileIds[file.Key] = blobInfo;

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
                        FileIds = fileIds,
                        Status = status,
                        ValidationResult = validationResult
                    };
                },
                _serviceHandler.OnBeforePost,
                _serviceHandler.OnAfterPost);
        }

        protected override Task<object> Delete(IHttpRequest request)
        {
            return ProcessRequestAsync(request,
                r => _queryFactory.CreateDeleteQuery(r, DocumentIdKey),
                async query =>
                {
                    long? deletedCount = null;
                    DocumentValidationResult validationResult = null;

                    try
                    {
                        // Удаление документа из хранилища
                        deletedCount = await _storage.DeleteOneAsync(f => f.Eq("_id", query.DocumentId));
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
                _serviceHandler.OnAfterDelete);
        }
    }
}