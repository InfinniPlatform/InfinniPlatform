using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using InfinniPlatform.Core.Threading;
using InfinniPlatform.Sdk.Documents;
using InfinniPlatform.Sdk.Hosting;
using InfinniPlatform.Sdk.Logging;
using InfinniPlatform.Sdk.Metadata.Documents;

namespace InfinniPlatform.DocumentStorage.Hosting
{
    internal class MongoCollectionInitializer : AppEventHandler
    {
        public MongoCollectionInitializer(IDocumentStorageManager documentStorageManager,
                                          IEnumerable<IDocumentMetadataSource> documentMetadataSources,
                                          ILog log)
            : base(1)
        {
            _documentStorageManager = documentStorageManager;
            _documentMetadataSources = documentMetadataSources;
            _log = log;
        }

        private readonly IEnumerable<IDocumentMetadataSource> _documentMetadataSources;
        private readonly IDocumentStorageManager _documentStorageManager;
        private readonly ILog _log;

        public override void OnBeforeStart()
        {
            _log.Info("Creating the document storage started.");

            var documentMetadataSources = _documentMetadataSources.SelectMany(documentMetadataSource => documentMetadataSource.GetDocumentsMetadata()).ToArray();

            foreach (var metadata in documentMetadataSources)
            {
                // Специально для Mono пришлось выполнять создание коллекций в последовательном режиме
                AsyncHelper.RunSync(() => CreateStorageAsync(metadata));
            }

            _log.Info($"Creating the document storage for {documentMetadataSources.Length} types successfully completed.");
        }

        private async Task CreateStorageAsync(DocumentMetadata documentMetadata)
        {
            var logContext = new Dictionary<string, object> { { "documentType", documentMetadata.Type } };

            _log.Debug("Creating storage for type started.", logContext);

            try
            {
                await _documentStorageManager.CreateStorageAsync(documentMetadata);

                _log.Debug("Creating storage for type successfully completed.", logContext);
            }
            catch (Exception exception)
            {
                _log.Error("Creating storage for type completed with exception.", logContext, exception);
            }
        }
    }
}