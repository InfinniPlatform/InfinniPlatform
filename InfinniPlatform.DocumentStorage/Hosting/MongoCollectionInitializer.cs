using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using InfinniPlatform.Core.Metadata;
using InfinniPlatform.Core.Threading;
using InfinniPlatform.Sdk.Documents;
using InfinniPlatform.Sdk.Hosting;
using InfinniPlatform.Sdk.Logging;
using InfinniPlatform.Sdk.Metadata.Documents;
using InfinniPlatform.Sdk.Serialization;

namespace InfinniPlatform.DocumentStorage.Hosting
{
    internal class MongoCollectionInitializer : ApplicationEventHandler
    {
        public MongoCollectionInitializer(IDocumentStorageManager documentStorageManager,
                                          IJsonObjectSerializer jsonObjectSerializer,
                                          IMetadataApi metadataApi,
                                          ILog log) : base(1)
        {
            _documentStorageManager = documentStorageManager;
            _jsonObjectSerializer = jsonObjectSerializer;
            _metadataApi = metadataApi;
            _log = log;
        }


        private readonly IDocumentStorageManager _documentStorageManager;
        private readonly IJsonObjectSerializer _jsonObjectSerializer;
        private readonly IMetadataApi _metadataApi;
        private readonly ILog _log;


        public override void OnStart()
        {
            _log.Info("Creating the document storage started.");

            var documentTypes = _metadataApi.GetDocumentNames();

            foreach (var documentType in documentTypes)
            {
                var documentMetadata = new DocumentMetadata { Type = documentType };

                var documentIndexes = _metadataApi.GetDocumentIndexes(documentType);

                if (documentIndexes != null)
                {
                    documentMetadata.Indexes = documentIndexes.Select(i => _jsonObjectSerializer.ConvertFromDynamic<DocumentIndex>(i)).ToArray();
                }

                // Специально для Mono пришлось выполнять создание коллекций в последовательном режиме
                AsyncHelper.RunSync(() => CreateStorageAsync(documentMetadata));
            }

            _log.Info("Creating the document storage successfully completed.");
        }


        private async Task CreateStorageAsync(DocumentMetadata documentMetadata)
        {
            var logContext = new Dictionary<string, object> { { "documentType", documentMetadata.Type } };

            _log.Info("Creating storage for type started.", logContext);

            try
            {
                await _documentStorageManager.CreateStorageAsync(documentMetadata);

                _log.Info("Creating storage for type successfully completed.", logContext);
            }
            catch (Exception exception)
            {
                _log.Error("Creating storage for type completed with exception.", logContext, exception);
            }
        }
    }
}