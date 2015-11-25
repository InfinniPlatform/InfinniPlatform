using InfinniPlatform.Api.Hosting;
using InfinniPlatform.Hosting;

namespace InfinniPlatform.Metadata.Implementation.Handlers
{
    /// <summary>
    /// Обработчик системных изменений, выполняющий нотификацию слушателей изменений
    /// </summary>
    public sealed class SystemEventsHandler : IWebRoutingHandler
    {
        public IConfigRequestProvider ConfigRequestProvider { get; set; }
    }
}