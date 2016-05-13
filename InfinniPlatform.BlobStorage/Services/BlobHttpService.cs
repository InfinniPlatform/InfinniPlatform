using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using InfinniPlatform.BlobStorage.Properties;
using InfinniPlatform.Sdk.BlobStorage;
using InfinniPlatform.Sdk.Logging;
using InfinniPlatform.Sdk.Services;

namespace InfinniPlatform.BlobStorage.Services
{
    /// <summary>
    /// Сервис по работе с BLOB (Binary Large OBject).
    /// </summary>
    [LoggerName("BlobHttpService")]
    internal sealed class BlobHttpService : IHttpService
    {
        public BlobHttpService(IBlobStorage blobStorage, IPerformanceLog performanceLog, ILog log)
        {
            _blobStorage = blobStorage;
            _performanceLog = performanceLog;
            _log = log;
        }


        private readonly IBlobStorage _blobStorage;
        private readonly IPerformanceLog _performanceLog;
        private readonly ILog _log;


        public void Load(IHttpServiceBuilder builder)
        {
            builder.ServicePath = "/blob";

            builder.Get["/{id}"] = GetFileContentAsync;
            builder.Post["/{id}"] = GetFileContentAsync;
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
                    var blobData = _blobStorage.GetBlobData(blobId);

                    if (blobData != null)
                    {
                        var fileResponse = new StreamHttpResponse(blobData.Data, blobData.Info.Type)
                                           {
                                               FileName = blobData.Info.Name,
                                               LastWriteTimeUtc = blobData.Info.Time
                                           };

                        fileResponse.SetContentDispositionAttachment(request.Headers.UserAgent);

                        return Task.FromResult<object>(fileResponse);
                    }
                }

                return Task.FromResult<object>(HttpResponse.NotFound);
            }
            catch (Exception e)
            {
                exception = e;

                _log.Error(Resources.RequestProcessedWithException, new Dictionary<string, object> { { "method", method } }, e);

                throw;
            }
            finally
            {
                _performanceLog.Log(method, startTime, exception);
            }
        }
    }
}