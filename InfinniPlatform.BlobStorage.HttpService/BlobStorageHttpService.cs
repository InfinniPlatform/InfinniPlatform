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
    /// Provides HTTP service to get data from <see cref="IBlobStorage" />.
    /// </summary>
    [LoggerName(nameof(BlobStorageHttpService))]
    public class BlobStorageHttpService : IHttpService
    {
        public const string DefaultServicePath = "/blob";


        private readonly IBlobStorage _blobStorage;
        private readonly IBlobStorageService _blobStorageService;
        private readonly ILogger _logger;
        private readonly IPerformanceLogger _perfLogger;


        public BlobStorageHttpService(IBlobStorage blobStorage,
                                      IPerformanceLogger<BlobStorageHttpService> perfLogger,
                                      ILogger<BlobStorageHttpService> logger,
                                      IBlobStorageService blobStorageService)
        {
            _blobStorage = blobStorage;
            _perfLogger = perfLogger;
            _logger = logger;
            _blobStorageService = blobStorageService;
        }


        public virtual void Load(IHttpServiceBuilder builder)
        {
            builder.ServicePath = DefaultServicePath;

            builder.Get["/{id}"] = GetFileContentAsync;
            builder.Post["/upload"] = PostFileContentAsync;
        }

        private Task<object> GetFileContentAsync(IHttpRequest request)
        {
            var startTime = DateTime.Now;

            var method = $"{request.Method}::{request.Path}";

            Exception exception = null;

            try
            {
                string blobId = request.Parameters.id;

                if (!string.IsNullOrEmpty(blobId))
                {
                    var fileResponse = _blobStorageService.GetFileResponse(blobId);

                    if (fileResponse != null)
                    {
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

        private async Task<object> PostFileContentAsync(IHttpRequest request)
        {
            var startTime = DateTime.Now;

            var method = $"{request.Method}::{request.Path}";

            var result = new ServiceResult<List<BlobInfo>>();

            Exception exception = null;

            try
            {
                var blobInfos = new List<BlobInfo>();

                foreach (var file in request.Files)
                {
                    var blobInfo = await _blobStorage.CreateBlobAsync(file.Name, file.ContentType, file.Value);

                    blobInfos.Add(blobInfo);
                }

                result.Result = blobInfos;
                result.Success = true;
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

            return result;
        }
    }
}