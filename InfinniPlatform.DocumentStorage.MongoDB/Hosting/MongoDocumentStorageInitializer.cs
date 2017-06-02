using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using InfinniPlatform.DocumentStorage.Metadata;
using InfinniPlatform.DocumentStorage.Properties;
using InfinniPlatform.Hosting;
using InfinniPlatform.Threading;

using Microsoft.Extensions.Logging;

namespace InfinniPlatform.DocumentStorage.Hosting
{
    internal class MongoDocumentStorageInitializer : IAppInitHandler
    {
        public MongoDocumentStorageInitializer(IDocumentStorageManager documentStorageManager,
                                               IEnumerable<IDocumentMetadataSource> documentMetadataSources,
                                               ILogger<MongoDocumentStorageInitializer> logger)
        {
            _documentStorageManager = documentStorageManager;
            _documentMetadataSources = documentMetadataSources;
            _logger = logger;
        }


        private readonly IEnumerable<IDocumentMetadataSource> _documentMetadataSources;
        private readonly IDocumentStorageManager _documentStorageManager;
        private readonly ILogger _logger;


        public void Handle()
        {
            _logger.LogInformation(Resources.CreatingDocumentStorageStarted);

            var documentMetadataSources = _documentMetadataSources.SelectMany(documentMetadataSource => documentMetadataSource.GetDocumentsMetadata()).ToList();

            foreach (var metadata in documentMetadataSources)
            {
                // Специально для Mono пришлось выполнять создание коллекций в последовательном режиме
                AsyncHelper.RunSync(() => CreateStorageAsync(metadata));
            }

            _logger.LogInformation(Resources.CreatingDocumentStorageSuccessfullyCompleted, () => new Dictionary<string, object> { { "documentTypeCount", documentMetadataSources.Count } });
        }

        private async Task CreateStorageAsync(DocumentMetadata documentMetadata)
        {
            Func<Dictionary<string, object>> logContext = () => new Dictionary<string, object> { { "documentType", documentMetadata.Type } };

            _logger.LogDebug(Resources.CreatingDocumentStorageForTypeStarted, logContext);

            try
            {
                await _documentStorageManager.CreateStorageAsync(documentMetadata);

                _logger.LogDebug(Resources.CreatingDocumentStorageForTypeSuccessfullyCompleted, logContext);
            }
            catch (Exception exception)
            {
                _logger.LogError(Resources.CreatingDocumentStorageForTypeCompletedWithException, exception, logContext);
            }
        }
    }
}