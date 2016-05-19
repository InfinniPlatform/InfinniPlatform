using System;
using System.Collections.Generic;
using System.IO;

using InfinniPlatform.Core.Documents;
using InfinniPlatform.Sdk.BlobStorage;
using InfinniPlatform.Sdk.Documents;
using InfinniPlatform.Sdk.Documents.Obsolete;
using InfinniPlatform.Sdk.Dynamic;

namespace InfinniPlatform.DocumentStorage.Obsolete
{
    internal sealed class MongoDocumentApi : IDocumentApi
    {
        public MongoDocumentApi(IDocumentStorageFactory storageFactory, ISetDocumentExecutor setDocumentExecutor, IBlobStorage blobStorage)
        {
            _storageFactory = storageFactory;
            _setDocumentExecutor = setDocumentExecutor;
            _blobStorage = blobStorage;
        }


        private readonly IDocumentStorageFactory _storageFactory;
        private readonly ISetDocumentExecutor _setDocumentExecutor;
        private readonly IBlobStorage _blobStorage;


        public long GetNumberOfDocuments(string documentType, Action<FilterBuilder> filter)
        {
            var documentStorage = _storageFactory.GetStorage(documentType);
            return documentStorage.Count(filter.ToDocumentStorageFilter());
        }

        public long GetNumberOfDocuments(string documentType, IEnumerable<FilterCriteria> filter)
        {
            var documentStorage = _storageFactory.GetStorage(documentType);
            return documentStorage.Count(filter.ToDocumentStorageFilter());
        }


        public dynamic GetDocumentById(string documentType, string documentId)
        {
            var documentStorage = _storageFactory.GetStorage(documentType);
            var document = documentStorage.Find(f => f.Eq("_id", documentId)).FirstOrDefault();

            if (document != null)
            {
                document["Id"] = document["_id"];
            }

            return document;
        }


        public IEnumerable<dynamic> GetDocument(string documentType, Action<FilterBuilder> filter, int pageNumber, int pageSize, Action<SortingBuilder> sorting = null)
        {
            var documentStorage = _storageFactory.GetStorage(documentType);
            var documents = documentStorage.Find(filter.ToDocumentStorageFilter()).Skip(pageNumber * pageSize).Limit(pageSize).ToSortedCursor(sorting).ToList();
            SetDocumentIds(documents);
            return documents;
        }

        public IEnumerable<dynamic> GetDocuments(string documentType, IEnumerable<FilterCriteria> filter, int pageNumber, int pageSize, IEnumerable<SortingCriteria> sorting = null)
        {
            var documentStorage = _storageFactory.GetStorage(documentType);
            var documents = documentStorage.Find(filter.ToDocumentStorageFilter()).Skip(pageNumber * pageSize).Limit(pageSize).ToSortedCursor(sorting).ToList();
            SetDocumentIds(documents);
            return documents;
        }


        public dynamic SetDocument(string documentType, object document)
        {
            return _setDocumentExecutor.SaveDocument(documentType, document);
        }

        public dynamic SetDocuments(string documentType, IEnumerable<object> documents)
        {
            return _setDocumentExecutor.SaveDocuments(documentType, documents);
        }


        public dynamic DeleteDocument(string documentType, string instanceId)
        {
            return _setDocumentExecutor.DeleteDocument(documentType, instanceId);
        }


        public void AttachFile(string documentType, string documentId, string fileProperty, string fileName, string fileType, Stream fileStream)
        {
            var documentStorage = _storageFactory.GetStorage(documentType);

            // Сохранение файла

            var fileId = _blobStorage.CreateBlob(fileName, fileType, fileStream).Id;

            var blobData = new DynamicWrapper
                           {
                               {
                                   "Info", new DynamicWrapper
                                           {
                                               { "ContentId", fileId },
                                               { "Name", fileName },
                                               { "Type", fileType }
                                           }
                               }
                           };

            // Обновление документа

            documentStorage.UpdateOne(u => u.Set(fileProperty, blobData), f => f.Eq("_id", documentId));
        }


        private static void SetDocumentIds(IEnumerable<DynamicWrapper> documents)
        {
            foreach (var document in documents)
            {
                document["Id"] = document["_id"];
            }
        }
    }
}