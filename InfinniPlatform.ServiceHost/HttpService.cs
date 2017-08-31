using System.Threading.Tasks;
using InfinniPlatform.DocumentStorage;
using InfinniPlatform.Dynamic;
using InfinniPlatform.Http;

namespace InfinniPlatform.ServiceHost
{
    public class HttpService : IHttpService
    {
        private readonly IDocumentStorageProvider _documentStorageProvider;

        public HttpService(IDocumentStorageProviderFactory storageFactory)
        {
            _documentStorageProvider = storageFactory.GetStorageProvider("Documents");
        }

        public void Load(IHttpServiceBuilder builder)
        {
            builder.Post["/save"] = Save;
        }

        public async Task<object> Save(IHttpRequest httpRequest)
        {
            var doc = new DynamicDocument {{"Field1", "Value1"}};

            await _documentStorageProvider.InsertOneAsync(doc);

            return doc;
        }
    }
}