using System.Threading.Tasks;
using InfinniPlatform.DocumentStorage;
using InfinniPlatform.Dynamic;
using InfinniPlatform.Http;
using Microsoft.Extensions.Logging;

namespace InfinniPlatform.ServiceHost
{
    public class HttpService : IHttpService
    {
        private readonly IDocumentStorageProvider _documentStorageProvider;
        private readonly ILogger<HttpService> _logger;

        public HttpService(IDocumentStorageProviderFactory storageFactory,
                           ILogger<HttpService> logger)
        {
            _logger = logger;
            _documentStorageProvider = storageFactory.GetStorageProvider("Documents");
        }

        public void Load(IHttpServiceBuilder builder)
        {
            builder.Post["/save"] = Save;
        }

        private async Task<object> Save(IHttpRequest httpRequest)
        {
            _logger.LogInformation($"Starting saving doc. IMMA {httpRequest.User?.Name}.");

            var doc = new DynamicDocument {{"Field1", "Value1"}};

            await _documentStorageProvider.InsertOneAsync(doc);

            _logger.LogInformation($"Finished saving doc. IMMA {httpRequest.User?.Name}.");

            return "Ok.";
        }
    }
}