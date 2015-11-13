using System.Collections.Generic;

namespace InfinniPlatform.UserInterface.Services.Metadata
{
    /// <summary>
    /// Интерфейс сервиса для работы с метаданными системы.
    /// </summary>
    internal interface IMetadataService
    {
        object CreateItem();

        /// <summary>
        /// Заменить объект метаданных.
        /// </summary>
        void ReplaceItem(dynamic item);

        /// <summary>
        /// Удалить объект метаданных.
        /// </summary>
        void DeleteItem(string itemId);

        /// <summary>
        /// Получить объект метаданных.
        /// </summary>
        object GetItem(string itemId);

        /// <summary>
        /// Получить объект метаданных.
        /// </summary>
        object CloneItem(string itemId);

        /// <summary>
        /// Получить список объектов метаданных.
        /// </summary>
        IEnumerable<object> GetItems();
    }
}