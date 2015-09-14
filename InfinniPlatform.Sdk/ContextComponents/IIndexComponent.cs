using InfinniPlatform.Sdk.Environment.Index;

namespace InfinniPlatform.Sdk.ContextComponents
{
    /// <summary>
    ///     Компонент для работы с индексами в глобальном контексте
    /// </summary>
    public interface IIndexComponent
    {
        IIndexFactory IndexFactory { get; }
    }
}