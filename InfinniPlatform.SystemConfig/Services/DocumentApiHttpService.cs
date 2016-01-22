using System;
using System.Collections.Generic;
using System.Linq;

using InfinniPlatform.Sdk.Contracts;
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
        public DocumentApiHttpService(IDocumentApi documentApi)
        {
            _documentApi = documentApi;
        }

        private readonly IDocumentApi _documentApi;

        public void Load(IHttpServiceBuilder builder)
        {
            builder.Post["/RestfulApi/StandardApi/configuration/GetDocumentById"] = CreateHandler(GetDocumentById);
            builder.Post["/RestfulApi/StandardApi/configuration/GetDocument"] = CreateHandler(GetDocuments);
            builder.Post["/RestfulApi/StandardApi/configuration/GetNumberOfDocuments"] = CreateHandler(GetNumberOfDocuments);
            builder.Post["/RestfulApi/StandardApi/configuration/SetDocument"] = CreateHandler(SaveDocuments);
            builder.Post["/RestfulApi/StandardApi/configuration/DeleteDocument"] = CreateHandler(DeleteDocument);
            builder.Post["/RestfulApi/Upload/configuration/UploadBinaryContent"] = AttachFile;
        }

        private static Func<IHttpRequest, object> CreateHandler(Action<IActionContext> action)
        {
            return new ChangeHttpRequestHandler(action).Action;
        }

        private void GetDocumentById(IActionContext target)
        {
            string configuration = target.Item.ConfigId;
            string documentType = target.Item.DocumentId;
            string documentId = target.Item.Id;

            target.Result = _documentApi.GetDocumentById(configuration, documentType, documentId);
        }

        private void GetDocuments(IActionContext target)
        {
            string configuration = target.Item.Configuration;
            string documentType = target.Item.Metadata;

            IEnumerable<dynamic> dynamicFilters = target.Item.Filter;
            var filter = (dynamicFilters != null) ? dynamicFilters.Select(o => new FilterCriteria(o.Property, o.Value, (CriteriaType)Enum.Parse(typeof(CriteriaType), o.CriteriaType.ToString()))) : null;

            int pageNumber = Math.Max((int)(target.Item.PageNumber ?? 0), 0);
            int pageSize = Math.Min((int)(target.Item.PageSize ?? 0), 1000);

            IEnumerable<dynamic> dynamicSorting = target.Item.Sorting;
            var sorting = (dynamicSorting != null) ? dynamicSorting.Select(o => new SortingCriteria(o.PropertyName, o.SortingOrder)) : null;

            target.Result = _documentApi.GetDocuments(configuration, documentType, filter, pageNumber, pageSize, sorting);
        }

        private void GetNumberOfDocuments(IActionContext target)
        {
            string configuration = target.Item.Configuration;
            string documentType = target.Item.Metadata;
            object filter = target.Item.Filter;

            var filterCriterias = JsonObjectSerializer.Default.ConvertFromDynamic<FilterCriteria[]>(filter);

            var numberOfDocuments = _documentApi.GetNumberOfDocuments(configuration, documentType, filterCriterias);

            target.Result = new DynamicWrapper();
            target.Result.IsValid = true;
            target.Result.NumberOfDocuments = numberOfDocuments;
        }

        private void SaveDocuments(IActionContext target)
        {
            string configuration = target.Item.Configuration;
            string documentType = target.Item.Metadata;
            IEnumerable<dynamic> documents = target.Item.Documents;

            var result = _documentApi.SetDocuments(configuration, documentType, documents);

            target.IsValid = result.IsValid;
            target.ValidationMessage = result.ValidationMessage;
            target.Result = result;
        }

        private void DeleteDocument(IActionContext target)
        {
            string configuration = target.Item.Configuration;
            string documentType = target.Item.Metadata;
            string documentId = target.Item.Id;

            var result = _documentApi.DeleteDocument(configuration, documentType, documentId);

            target.IsValid = result.IsValid;
            target.ValidationMessage = result.ValidationMessage;
            target.Result = result;
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
                    _documentApi.AttachFile(configuration, documentType, documentId, fileProperty, firstFile.Value);
                }
            }

            return null;
        }
    }
}