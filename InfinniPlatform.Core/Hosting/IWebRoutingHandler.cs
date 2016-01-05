namespace InfinniPlatform.Core.Hosting
{
    /// <summary>
    ///     Интерфейс обработчика web-роутинга
    /// </summary>
    public interface IWebRoutingHandler
    {
        IConfigRequestProvider ConfigRequestProvider { get; set; }
    }
}