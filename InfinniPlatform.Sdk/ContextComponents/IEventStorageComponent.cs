using InfinniPlatform.Sdk.Environment;
using InfinniPlatform.Sdk.Environment.Binary;

namespace InfinniPlatform.Sdk.ContextComponents
{
    /// <summary>
    ///     Компонент для сохранения событий из контекста
    /// </summary>
    public interface IEventStorageComponent
    {
        IEventStorage GetEventStorage();
    }
}