﻿using System;
using System.Collections.Generic;
using System.Linq;

using InfinniPlatform.Core.Transactions;
using InfinniPlatform.Sdk.Documents;
using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.Sdk.Serialization;
using InfinniPlatform.Sdk.Services;

namespace InfinniPlatform.SystemConfig.Services
{
    /// <summary>
    /// Реализует REST-сервис для DocumentApi.
    /// </summary>
    internal sealed class DocumentApiHttpService : IHttpService
    {
        public DocumentApiHttpService(IDocumentApi documentApi, IDocumentTransactionScopeProvider transactionScopeProvider)
        {
            _documentApi = documentApi;
            _transactionScopeProvider = transactionScopeProvider;
        }


        private readonly IDocumentApi _documentApi;
        private readonly IDocumentTransactionScopeProvider _transactionScopeProvider;


        public void Load(IHttpServiceBuilder builder)
        {
            builder.Post["/RestfulApi/StandardApi/configuration/GetDocumentById"] = GetDocumentById;
            builder.Post["/RestfulApi/StandardApi/configuration/GetDocument"] = GetDocuments;
            builder.Post["/RestfulApi/StandardApi/configuration/GetNumberOfDocuments"] = GetNumberOfDocuments;
            builder.Post["/RestfulApi/StandardApi/configuration/SetDocument"] = SaveDocuments;
            builder.Post["/RestfulApi/StandardApi/configuration/DeleteDocument"] = DeleteDocument;
            builder.Post["/RestfulApi/Upload/configuration/UploadBinaryContent"] = AttachFile;
        }


        private object GetDocumentById(IHttpRequest request)
        {
            dynamic requestForm = request.Form.changesObject;
            string configuration = requestForm.ConfigId;
            string documentType = requestForm.DocumentId;
            string documentId = requestForm.Id;

            var result = _documentApi.GetDocumentById(configuration, documentType, documentId);

            return result;
        }

        private object GetDocuments(IHttpRequest request)
        {
            dynamic requestForm = request.Form.changesObject;
            string configuration = requestForm.Configuration;
            string documentType = requestForm.Metadata;
            object filter = requestForm.Filter;
            object sorting = requestForm.Sorting;

            var filterCriterias = JsonObjectSerializer.Default.ConvertFromDynamic<FilterCriteria[]>(filter);
            var sortingCriterias = JsonObjectSerializer.Default.ConvertFromDynamic<SortingCriteria[]>(sorting);

            int pageNumber = Math.Max((int)(requestForm.PageNumber ?? 0), 0);
            int pageSize = Math.Min((int)(requestForm.PageSize ?? 0), 1000);

            var result = _documentApi.GetDocuments(configuration, documentType, filterCriterias, pageNumber, pageSize, sortingCriterias);

            return result;
        }

        private object GetNumberOfDocuments(IHttpRequest request)
        {
            dynamic requestForm = request.Form.changesObject;
            string configuration = requestForm.Configuration;
            string documentType = requestForm.Metadata;
            object filter = requestForm.Filter;

            var filterCriterias = JsonObjectSerializer.Default.ConvertFromDynamic<FilterCriteria[]>(filter);

            var numberOfDocuments = _documentApi.GetNumberOfDocuments(configuration, documentType, filterCriterias);

            var result = new DynamicWrapper { ["NumberOfDocuments"] = numberOfDocuments };

            return result;
        }

        private object SaveDocuments(IHttpRequest request)
        {
            dynamic requestForm = request.Form.changesObject;
            string configuration = requestForm.Configuration;
            string documentType = requestForm.Metadata;
            IEnumerable<dynamic> documents = requestForm.Documents ?? new object[] { requestForm.Document };

            SetSynchronous(request.Form.Synchronous);

            var result = _documentApi.SetDocuments(configuration, documentType, documents);

            return result;
        }

        private object DeleteDocument(IHttpRequest request)
        {
            dynamic requestForm = request.Form.changesObject;
            string configuration = requestForm.Configuration;
            string documentType = requestForm.Metadata;
            string documentId = requestForm.Id;

            SetSynchronous(request.Form.Synchronous);

            var result = _documentApi.DeleteDocument(configuration, documentType, documentId);

            return result;
        }

        private object AttachFile(IHttpRequest request)
        {
            string linkedDataString = request.Query.LinkedData;

            dynamic linkedData = null;

            if (!string.IsNullOrWhiteSpace(linkedDataString))
            {
                linkedDataString = Uri.UnescapeDataString(linkedDataString);
                linkedData = JsonObjectSerializer.Default.Deserialize(linkedDataString);
            }

            if (linkedData == null)
            {
                throw new ArgumentException(@"LinkedData");
            }

            string configuration = linkedData.Configuration;
            string documentType = linkedData.Metadata;
            string documentId = linkedData.DocumentId;
            string fileProperty = linkedData.FieldName;

            var files = request.Files;

            if (files != null)
            {
                var firstFile = files.FirstOrDefault();

                if (firstFile != null)
                {
                    SetSynchronous(linkedData.Synchronous);

                    _documentApi.AttachFile(configuration, documentType, documentId, fileProperty, firstFile.Value);
                }
            }

            return null;
        }


        private void SetSynchronous(bool? synchronous)
        {
            if (synchronous == true)
            {
                var transactionScope = _transactionScopeProvider.GetTransactionScope();

                if (transactionScope != null)
                {
                    transactionScope.Synchronous();
                }
            }
        }
    }
}