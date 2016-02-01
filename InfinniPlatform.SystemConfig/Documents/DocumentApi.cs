using System;
using System.Collections.Generic;
using System.IO;

using InfinniPlatform.Core.Documents;
using InfinniPlatform.Sdk.BlobStorage;
using InfinniPlatform.Sdk.Documents;
using InfinniPlatform.Sdk.Dynamic;

namespace InfinniPlatform.SystemConfig.Documents
{
    internal sealed class DocumentApi : IDocumentApi
    {
        public DocumentApi(ISetDocumentExecutor setDocumentExecutor, IGetDocumentExecutor getDocumentExecutor, IBlobStorage blobStorage)
        {
            _setDocumentExecutor = setDocumentExecutor;
            _getDocumentExecutor = getDocumentExecutor;
            _blobStorage = blobStorage;
        }

        private readonly ISetDocumentExecutor _setDocumentExecutor;
        private readonly IGetDocumentExecutor _getDocumentExecutor;
        private readonly IBlobStorage _blobStorage;
        
        public dynamic GetDocumentById(string configuration, string documentType, string documentId)
        {
            return _getDocumentExecutor.GetDocumentById(configuration, documentType, documentId);
        }

        public dynamic DeleteDocument(string configuration, string documentType, string documentId)
        {
            return _setDocumentExecutor.DeleteDocument(configuration, documentType, documentId);
        }

        long IDocumentApi.GetNumberOfDocuments(string configuration, string documentType, Action<FilterBuilder> filter)
        {
            return GetNumberOfDocuments(configuration, documentType, filter);
        }

        long IDocumentApi.GetNumberOfDocuments(string configuration, string documentType, IEnumerable<FilterCriteria> filter)
        {
            return _getDocumentExecutor.GetNumberOfDocuments(configuration, documentType, filter);
        }

        public IEnumerable<dynamic> GetDocuments(string configurationName, string documentType, IEnumerable<FilterCriteria> filter, int pageNumber, int pageSize, IEnumerable<SortingCriteria> sorting = null)
        {
            return _getDocumentExecutor.GetDocument(configurationName, documentType, filter, pageNumber, pageSize, sorting);
        }

        public dynamic SetDocument(string configuration, string documentType, object documentInstance)
        {
            return _setDocumentExecutor.SaveDocument(configuration, documentType, documentInstance);
        }

        public dynamic SetDocuments(string configuration, string documentType, IEnumerable<object> documentInstances)
        {
            return _setDocumentExecutor.SaveDocuments(configuration, documentType, documentInstances);
        }

        public int GetNumberOfDocuments(string configurationName, string documentType, dynamic filter)
        {
            return _getDocumentExecutor.GetNumberOfDocuments(configurationName, documentType, filter);
        }

        public int GetNumberOfDocuments(string configurationName, string documentType, Action<FilterBuilder> filter)
        {
            return _getDocumentExecutor.GetNumberOfDocuments(configurationName, documentType, filter);
        }

        public IEnumerable<dynamic> GetDocument(string configurationName, string documentType, dynamic filter, int pageNumber, int pageSize, IEnumerable<dynamic> ignoreResolve = null, dynamic sorting = null)
        {
            return _getDocumentExecutor.GetDocument(configurationName, documentType, filter, pageNumber, pageSize, sorting: sorting);
        }

        public IEnumerable<dynamic> GetDocument(string configurationName, string documentType, Action<FilterBuilder> filter, int pageNumber, int pageSize, Action<SortingBuilder> sorting = null)
        {
            return _getDocumentExecutor.GetDocument(configurationName, documentType, filter, pageNumber, pageSize, sorting, null);
        }

        IEnumerable<dynamic> IDocumentApi.GetDocument(string configurationName, string documentType, Action<FilterBuilder> filter, int pageNumber, int pageSize, Action<SortingBuilder> sorting)
        {
            return _getDocumentExecutor.GetDocument(configurationName, documentType, filter, pageNumber, pageSize, sorting, null);
        }

        /// <summary>
        /// Прикрепляет файл к свойству документа.
        /// </summary>
        public void AttachFile(string configuration, string documentType, string documentId, string fileProperty, Stream fileStream)
        {
            // Получение документа

            object document = GetDocumentById(configuration, documentType, documentId);

            // Получение свойства

            dynamic filePropertyValue = document.GetProperty(fileProperty);

            if (filePropertyValue == null)
            {
                filePropertyValue = new DynamicWrapper();
                filePropertyValue.Info = new DynamicWrapper();
                ObjectHelper.SetProperty(document, fileProperty, filePropertyValue);
            }

            if (filePropertyValue.Info == null)
            {
                filePropertyValue.Info = new DynamicWrapper();
            }

            string fileId = filePropertyValue.Info.ContentId;

            // Сохранение файла

            if (string.IsNullOrEmpty(fileId))
            {
                fileId = _blobStorage.CreateBlob(fileProperty, string.Empty, fileStream);

                filePropertyValue.Info.ContentId = fileId;
            }
            else
            {
                _blobStorage.UpdateBlob(fileId, fileProperty, string.Empty, fileStream);
            }

            // Сохранение документа

            SetDocument(configuration, documentType, document);
        }
    }
}