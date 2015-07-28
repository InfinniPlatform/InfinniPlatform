using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfinniPlatform.Api.Hosting;
using InfinniPlatform.Sdk.Global;

namespace InfinniPlatform.Metadata.Implementation.Handlers
{
    /// <summary>
    ///   Обработчик запроса пользовательского сервиса SDK API
    /// </summary>
    public sealed class CustomServiceHandler
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
