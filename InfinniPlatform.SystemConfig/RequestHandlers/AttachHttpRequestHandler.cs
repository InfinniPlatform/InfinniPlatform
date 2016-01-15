using System;
using System.Linq;

using InfinniPlatform.Sdk.Documents;
using InfinniPlatform.Sdk.Serialization;
using InfinniPlatform.Sdk.Services;

namespace InfinniPlatform.SystemConfig.RequestHandlers
{
    internal sealed class AttachHttpRequestHandler : SimpleHttpRequestHandler
    {
        public AttachHttpRequestHandler(IDocumentApi documentApi)
        {
            _documentApi = documentApi;
        }

        private readonly IDocumentApi _documentApi;

        protected override object ActionResult(IHttpRequest request)
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
            var fileStream = request.Files?.FirstOrDefault()?.Value;

            _documentApi.AttachFile(configuration, documentType, documentId, fileProperty, fileStream);

            return null;
        }
    }
}