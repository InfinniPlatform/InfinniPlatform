namespace InfinniPlatform.Http.Middlewares
{
    /// <summary>
    /// Уровень для обработки прикладных запросов.
    /// </summary>
    /// <remarks>
    /// Позволяет осуществлять любую логику после прохождения всех остальных уровней обработки запросов.
    /// </remarks>
    public interface IApplicationMiddleware : IMiddleware
    {
    }
}