using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

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
                                          ILog log) : base(1)
        {
            _documentStorageManager = documentStorageManager;
            _log = log;
        }

        private readonly IDocumentStorageManager _documentStorageManager;
        private readonly ILog _log;

        public override void OnBeforeStart()
        {
            _log.Info("Creating the document storage started.");

            //TODO Add feature to create storages in runtime. Remove hardcoded values.
            var strings = Directory.GetFiles("content\\metadata\\Documents");
            var documentTypes = strings.Select(s =>
                                               {
                                                   var bytes = File.ReadAllBytes(s);

                                                   var documentMetadata = JsonObjectSerializer.Default.Deserialize<DocumentMetadata>(bytes);
                                                   documentMetadata.Type = Path.GetFileNameWithoutExtension(s);
                                                   return documentMetadata;
                                               });

            foreach (var documentType in documentTypes)
            {
                // Специально для Mono пришлось выполнять создание коллекций в последовательном режиме
                AsyncHelper.RunSync(() => CreateStorageAsync(documentType));
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