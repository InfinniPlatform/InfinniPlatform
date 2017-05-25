namespace InfinniPlatform.Http.Middlewares
{
    /// <summary>
    /// Уровень для обработки ошибок выполнения запросов.
    /// </summary>
    /// <remarks>
    /// Позволяет осуществлять обработку ошибок выполнения запросов. Например, вести журнал со статистикой обработки запросов.
    /// </remarks>
    public interface IErrorHandlingAppLayer : IAppLayer
    {
    }
}