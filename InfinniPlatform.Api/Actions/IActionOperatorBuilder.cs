namespace InfinniPlatform.Api.Actions
{
    /// <summary>
    ///     Конструктор исполнителей скриптов
    /// </summary>
    public interface IActionOperatorBuilder
    {
        IActionOperator BuildActionOperator();
    }
}