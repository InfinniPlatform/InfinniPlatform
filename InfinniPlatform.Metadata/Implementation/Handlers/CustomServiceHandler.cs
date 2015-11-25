using InfinniPlatform.Api.Hosting;
using InfinniPlatform.Hosting;
using InfinniPlatform.Sdk.Global;

namespace InfinniPlatform.Metadata.Implementation.Handlers
{
    /// <summary>
    ///   Обработчик запроса пользовательского сервиса SDK API
    /// </summary>
    public sealed class CustomServiceHandler : IWebRoutingHandler
    {
        private readonly ICustomServiceGlobalContext _context;

        public CustomServiceHandler(ICustomServiceGlobalContext context)
        {
            _context = context;
        }

        public IConfigRequestProvider ConfigRequestProvider { get; set; }

        public dynamic ApplyJsonObject(string id, dynamic changesObject)
        {
            //делегируем выполнение обработчику ApplyChangesHandler, но с другим контекстом исполнения, содержащим только объект доступа к компонентам SDK
            var applyChangesHandler = new ApplyChangesHandler(_context) {ConfigRequestProvider = ConfigRequestProvider};


            return applyChangesHandler.ApplyJsonObject(id, changesObject);
        }

    }
}
