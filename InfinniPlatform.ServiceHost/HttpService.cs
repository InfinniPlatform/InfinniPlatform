using System;
using System.Threading.Tasks;
using InfinniPlatform.DocumentStorage;
using InfinniPlatform.Http;
using InfinniPlatform.Logging;

namespace InfinniPlatform.ServiceHost
{
    public class HttpService : IHttpService
    {
        private readonly IDocumentStorageProvider<Entity> _documentStorageProvider;
        private readonly DocExecutor<Entity> _executor;

        public HttpService(IDocumentStorageProviderFactory storageFactory,
                           DocExecutor<Entity> executor)
        {
            _executor = executor;
            _documentStorageProvider = storageFactory.GetStorageProvider<Entity>();
        }

        public void Load(IHttpServiceBuilder builder)
        {
            builder.Post["/save"] = Save;
            builder.Get["/get"] = Get;
        }

        public async Task<object> Save(IHttpRequest httpRequest)
        {
            var doc = new Entity {Digit = 5, Name = "Five"};

            await _documentStorageProvider.InsertOneAsync(doc);

            return doc;
        }

        public Task<object> Get(IHttpRequest httpRequest)
        {
            return Task.FromResult<object>(_executor.Get());
        }
    }

    [LoggerName("CustomComponentName")]
    public class DocExecutor<TDoc> where TDoc : new()
    {
        private readonly IPerformanceLogger<DocExecutor<TDoc>> _logger;

        public DocExecutor(IPerformanceLogger<DocExecutor<TDoc>> logger)
        {
            _logger = logger;
        }

        public TDoc Get()
        {
            var next = new Random().Next(1, 10);

            _logger.Log("Get", TimeSpan.FromMilliseconds(next));
            return new TDoc();
        }
    }

    public class Entity : Document
    {
        public Entity()
        {
            Name = "Default";
            Digit = 1;
        }

        public string Name { get; set; }
        public int Digit { get; set; }
    }
}