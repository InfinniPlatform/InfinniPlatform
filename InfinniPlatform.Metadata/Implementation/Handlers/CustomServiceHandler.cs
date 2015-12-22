using System;

using InfinniPlatform.Api.Hosting;
using InfinniPlatform.Hosting;
using InfinniPlatform.Sdk.Contracts;
using InfinniPlatform.Sdk.Global;

namespace InfinniPlatform.Metadata.Implementation.Handlers
{
    /// <summary>
    /// Обработчик запроса пользовательского сервиса SDK API
    /// </summary>
    public sealed class CustomServiceHandler : IWebRoutingHandler
    {
        public CustomServiceHandler(ICustomServiceGlobalContext context, Func<IGlobalContext, ApplyChangesHandler> applyChangesHandlerFactory)
        {
            _context = context;
            _applyChangesHandlerFactory = applyChangesHandlerFactory;
        }


        private readonly ICustomServiceGlobalContext _context;
        private readonly Func<IGlobalContext, ApplyChangesHandler> _applyChangesHandlerFactory;


        public IConfigRequestProvider ConfigRequestProvider { get; set; }

        public dynamic ApplyJsonObject(string id, dynamic changesObject)
        {
            // делегируем выполнение обработчику ApplyChangesHandler, но с другим контекстом исполнения, содержащим только объект доступа к компонентам SDK
            var applyChangesHandler = _applyChangesHandlerFactory(_context);
            applyChangesHandler.ConfigRequestProvider = ConfigRequestProvider;

            return applyChangesHandler.ApplyJsonObject(id, changesObject);
        }
    }
}