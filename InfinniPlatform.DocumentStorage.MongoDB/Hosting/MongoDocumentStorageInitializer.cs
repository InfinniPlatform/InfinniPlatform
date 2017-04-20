using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using InfinniPlatform.Core.Hosting;
using InfinniPlatform.Core.Logging;
using InfinniPlatform.Core.Threading;
using InfinniPlatform.DocumentStorage.Metadata;
using InfinniPlatform.DocumentStorage.Properties;

namespace InfinniPlatform.DocumentStorage.Hosting
{
    internal class MongoDocumentStorageInitializer : IAppInitHandler
    {
        public MongoDocumentStorageInitializer(IDocumentStorageManager documentStorageManager,
                                               IEnumerable<IDocumentMetadataSource> documentMetadataSources,
                                               ILog log)
        {
            _documentStorageManager = documentStorageManager;
            _documentMetadataSources = documentMetadataSources;
            _log = log;
        }


        private readonly IEnumerable<IDocumentMetadataSource> _documentMetadataSources;
        private readonly IDocumentStorageManager _documentStorageManager;
        private readonly ILog _log;


        public void Handle()
        {
            _log.Info(Resources.CreatingDocumentStorageStarted);

            var documentMetadataSources = _documentMetadataSources.SelectMany(documentMetadataSource => documentMetadataSource.GetDocumentsMetadata()).ToList();

            foreach (var metadata in documentMetadataSources)
            {
                // Специально для Mono пришлось выполнять создание коллекций в последовательном режиме
                AsyncHelper.RunSync(() => CreateStorageAsync(metadata));
            }

            _log.Info(Resources.CreatingDocumentStorageSuccessfullyCompleted, () => new Dictionary<string, object> { { "documentTypeCount", documentMetadataSources.Count } });
        }

        private async Task CreateStorageAsync(DocumentMetadata documentMetadata)
        {
            Func<Dictionary<string, object>> logContext = () => new Dictionary<string, object> { { "documentType", documentMetadata.Type } };

            _log.Debug(Resources.CreatingDocumentStorageForTypeStarted, logContext);

            try
            {
                await _documentStorageManager.CreateStorageAsync(documentMetadata);

                _log.Debug(Resources.CreatingDocumentStorageForTypeSuccessfullyCompleted, logContext);
            }
            catch (Exception exception)
            {
                _log.Error(Resources.CreatingDocumentStorageForTypeCompletedWithException, exception, logContext);
            }
        }
    }
}