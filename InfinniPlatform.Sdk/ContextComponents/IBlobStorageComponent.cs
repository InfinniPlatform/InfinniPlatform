using InfinniPlatform.Sdk.Environment;
using InfinniPlatform.Sdk.Environment.Binary;

namespace InfinniPlatform.Sdk.ContextComponents
{
    /// <summary>
    ///     Компонент для хранилища, работающего с бинарными данными
    /// </summary>
    public interface IBlobStorageComponent
    {
        /// <summary>
        ///     Получить хранилище бинарных данных
        /// </summary>
        /// <returns>Хранилище бинарных данных</returns>
        IBlobStorage GetBlobStorage();
    }
}