using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using InfinniPlatform.BlobStorage.Abstractions;
using InfinniPlatform.BlobStorage.HttpService.Properties;
using InfinniPlatform.Sdk.Http.Services;
using InfinniPlatform.Sdk.Logging;

namespace InfinniPlatform.BlobStorage.HttpService
{
    /// <summary>
    /// Provides HTTP serivce to get data from <see cref="IBlobStorage"/>.
    /// </summary>
    /// <example>
    /// <code>
    /// GET /blob/{id}
    /// </code>
    /// </example>
    public class BlobStorageHttpService : IHttpService
    {
        public const string DefaultServicePath = "/blob";


        public BlobStorageHttpService(IBlobStorage blobStorage, IPerformanceLog performanceLog, ILog log)
        {
            _blobStorage = blobStorage;
            _performanceLog = performanceLog;
            _log = log;
        }


        private readonly IBlobStorage _blobStorage;
        private readonly IPerformanceLog _performanceLog;
        private readonly ILog _log;


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

                        fileResponse.SetContentDispositionAttachment(request.Headers.UserAgent);

                        return Task.FromResult<object>(fileResponse);
                    }
                }

                return Task.FromResult<object>(HttpResponse.NotFound);
            }
            catch (Exception e)
            {
                exception = e;

                _log.Error(Resources.RequestProcessedWithException, e, () => new Dictionary<string, object> { { "method", method } });

                throw;
            }
            finally
            {
                _performanceLog.Log(method, startTime, exception);
            }
        }
    }
}