using InfinniPlatform.Api.Factories;

namespace InfinniPlatform.Api.ContextComponents
{
    /// <summary>
    ///     Компонент для получения документов из различных конфигураций внутри глобального контекста
    /// </summary>
    public interface ICrossConfigSearchComponent
    {
        ICrossConfigSearcher GetCrossConfigSearcher();
    }
}