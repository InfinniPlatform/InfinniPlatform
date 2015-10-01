namespace InfinniPlatform.Sdk.Environment.Actions
{
    /// <summary>
    ///     Конструктор исполнителей скриптов
    /// </summary>
    public interface IActionOperatorBuilder
    {
        IActionOperator BuildActionOperator();
    }
}