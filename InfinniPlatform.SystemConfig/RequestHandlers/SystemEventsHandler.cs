using InfinniPlatform.Core.Hosting;

namespace InfinniPlatform.SystemConfig.RequestHandlers
{
    /// <summary>
    /// Обработчик системных изменений, выполняющий нотификацию слушателей изменений
    /// </summary>
    public sealed class SystemEventsHandler : IWebRoutingHandler
    {
        public IConfigRequestProvider ConfigRequestProvider { get; set; }
    }
}