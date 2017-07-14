using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using InfinniPlatform.BlobStorage.Properties;
using InfinniPlatform.Http;
using InfinniPlatform.Logging;

using Microsoft.Extensions.Logging;

namespace InfinniPlatform.BlobStorage
{
    /// <summary>
    /// Provides HTTP service to get data from <see cref="IBlobStorage"/>.
    /// </summary>
    /// <example>
    /// <code>
    /// GET /blob/{id}
    /// </code>
    /// </example>
    [LoggerName(nameof(BlobStorageHttpService))]
    public class BlobStorageHttpService : IHttpService
    {
        public const string DefaultServicePath = "/blob";


        public BlobStorageHttpService(IBlobStorage blobStorage, IPerformanceLogger<BlobStorageHttpService> perfLogger, ILogger<BlobStorageHttpService> logger)
        {
            _blobStorage = blobStorage;
            _perfLogger = perfLogger;
            _logger = logger;
        }


        private readonly IBlobStorage _blobStorage;
        private readonly IPerformanceLogger _perfLogger;
        private readonly ILogger _logger;


        public virtual void Load(IHttpServiceBuilder builder)
        {
            builder.ServicePath = DefaultServicePath;

            builder.Get["/{id}"] = GetFileContentAsync;
        }


        protected virtual Task<object> GetFileContentAsync(IHttpRequest request)
        {
            var startTime = DateTime.Now;

            var method = $"{request.Method}::{request.Path}";

            Exception exception = null;

            try
            {
                string blobId = request.Parameters.id;

                if (!string.IsNullOrEmpty(blobId))
                {
                    var blobData = _blobStorage.GetBlobData(blobId);

                    if (blobData != null)
                    {
                        var fileResponse = new StreamHttpResponse(blobData.Data, blobData.Info.Type)
                                           {
                                               FileName = blobData.Info.Name,
                                               LastWriteTimeUtc = blobData.Info.Time
                                           };

                        fileResponse.SetContentDispositionAttachment();

                        return Task.FromResult<object>(fileResponse);
                    }
                }

                return Task.FromResult<object>(HttpResponse.NotFound);
            }
            catch (Exception e)
            {
                exception = e;

                _logger.LogError(Resources.RequestProcessedWithException, e, () => new Dictionary<string, object> { { "method", method } });

                throw;
            }
            finally
            {
                _perfLogger.Log(method, startTime, exception);
            }
        }
    }
}