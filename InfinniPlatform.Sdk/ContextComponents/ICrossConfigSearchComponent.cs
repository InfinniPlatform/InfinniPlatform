using InfinniPlatform.Sdk.Environment;

namespace InfinniPlatform.Sdk.ContextComponents
{
    /// <summary>
    ///     Компонент для получения документов из различных конфигураций внутри глобального контекста
    /// </summary>
    public interface ICrossConfigSearchComponent
    {
        ICrossConfigSearcher GetCrossConfigSearcher();
    }
}