using System;
using System.Collections.Generic;
using System.Linq;

using InfinniPlatform.Core.RestApi.DataApi;
using InfinniPlatform.Core.SearchOptions.Builders;
using InfinniPlatform.Sdk.Contracts;
using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.Sdk.Environment.Binary;

namespace InfinniPlatform.SystemConfig.ActionUnits.Binary
{
    public sealed class ActionUnitDownloadBinaryContent
    {
        public ActionUnitDownloadBinaryContent(DocumentApi documentApi, IBlobStorage blobStorage)
        {
            _documentApi = documentApi;
            _blobStorage = blobStorage;
        }


        private readonly DocumentApi _documentApi;
        private readonly IBlobStorage _blobStorage;


        public void Action(IUrlEncodedDataContext target)
        {
            if (target.FormData.ContentId != null)
            {
                target.Result = _blobStorage.GetBlobData(target.FormData.ContentId);
            }
            else
            {
                target.Result = FillContentByDocumentId(target.FormData);
            }
        }

        private object FillContentByDocumentId(dynamic formData)
        {
            string configuration = formData.Configuration;
            string documentType = formData.Metadata;
            object documentId = formData.DocumentId;

            Action<FilterBuilder> filter = f => f.AddCriteria(cr => cr.Property("Id").IsEquals(documentId));
            IEnumerable<dynamic> documents = _documentApi.GetDocument(configuration, documentType, filter, 0, 1);

            var document = documents.FirstOrDefault();

            if (document != null)
            {
                string blobFieldName = formData.FieldName;
                dynamic blobFieldValue = ObjectHelper.GetProperty(document, blobFieldName);

                if (blobFieldValue != null)
                {
                    return _blobStorage.GetBlobData(blobFieldValue.Info.ContentId);
                }

                return null;
            }

            return null;
        }
    }
}