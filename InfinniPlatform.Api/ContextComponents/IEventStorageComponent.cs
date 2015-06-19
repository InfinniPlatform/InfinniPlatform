using InfinniPlatform.Api.Factories;

namespace InfinniPlatform.Api.ContextComponents
{
    /// <summary>
    ///     Компонент для сохранения событий из контекста
    /// </summary>
    public interface IEventStorageComponent
    {
        IEventStorage GetEventStorage();
    }
}