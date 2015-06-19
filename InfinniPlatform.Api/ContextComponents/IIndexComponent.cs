using InfinniPlatform.Api.Index;

namespace InfinniPlatform.Api.ContextComponents
{
    /// <summary>
    ///     Компонент для работы с индексами в глобальном контексте
    /// </summary>
    public interface IIndexComponent
    {
        IIndexFactory IndexFactory { get; }
    }
}