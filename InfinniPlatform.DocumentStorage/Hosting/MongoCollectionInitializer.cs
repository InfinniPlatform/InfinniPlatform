using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using InfinniPlatform.Core.Metadata;
using InfinniPlatform.Sdk.Documents;
using InfinniPlatform.Sdk.Hosting;
using InfinniPlatform.Sdk.Logging;
using InfinniPlatform.Sdk.Metadata.Documents;
using InfinniPlatform.Sdk.Serialization;

namespace InfinniPlatform.DocumentStorage.Hosting
{
    internal class MongoCollectionInitializer : ApplicationEventHandler
    {
        private const int CreateStorageTimeout = 5 * 60 * 1000;


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

            var tasks = new List<Task>();

            foreach (var documentType in documentTypes)
            {
                var documentMetadata = new DocumentMetadata { Type = documentType };

                var documentIndexes = _metadataApi.GetDocumentIndexes(documentType);

                if (documentIndexes != null)
                {
                    documentMetadata.Indexes = documentIndexes.Select(i => _jsonObjectSerializer.ConvertFromDynamic<DocumentIndex>(i)).ToArray();
                }

                var logContext = new Dictionary<string, object> { { "documentType", documentType } };

                _log.Info("Creating storage for type started.", logContext);

                var createStorageTask = _documentStorageManager.CreateStorageAsync(documentMetadata);

                createStorageTask.ContinueWith(t =>
                                               {
                                                   if (t.IsFaulted)
                                                   {
                                                       _log.Error("Creating storage for type completed with exception.", logContext, t.Exception);
                                                   }
                                                   else
                                                   {
                                                       _log.Info("Creating storage for type successfully completed.", logContext);
                                                   }
                                               });

                tasks.Add(createStorageTask);
            }

            if (Task.WaitAll(tasks.ToArray(), CreateStorageTimeout))
            {
                _log.Info("Creating the document storage successfully completed.");
            }
            else
            {
                _log.Error("Creating the document storage completed with a timeout.");
            }
        }
    }
}