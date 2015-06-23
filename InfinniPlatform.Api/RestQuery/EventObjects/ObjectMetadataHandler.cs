using InfinniPlatform.Sdk.Events;

namespace InfinniPlatform.Api.RestQuery.EventObjects
{
    /// <summary>
    ///     Конструктор событий
    /// </summary>
    public class ObjectMetadataHandler
    {
        private EventDefinition CreateEvent(string property, object value, EventType action, int index = -1)
        {
            dynamic eventObject = new EventDefinition();
            eventObject.Property = property;
            if (value != null)
            {
                eventObject.Value = value;
            }
            eventObject.Action = action;
            if (index != -1)
            {
                eventObject.Index = index;
            }
            return eventObject;
        }

        /// <summary>
        ///     Сформировать событие изменения свойства объекта
        /// </summary>
        /// <param name="property">Путь к свойству</param>
        /// <param name="value">Значение свойства</param>
        /// <returns>Cобытие</returns>
        public EventDefinition CreateProperty(string property, object value)
        {
            return CreateEvent(property, value, EventType.CreateProperty);
        }

        /// <summary>
        ///     Сформировать событие создания контейнера
        /// </summary>
        /// <param name="property">Путь к контейнеру</param>
        /// <returns>Cобытие</returns>
        public EventDefinition CreateContainer(string property)
        {
            return CreateEvent(property, null, EventType.CreateContainer);
        }

        /// <summary>
        ///     Сформировать событие создания коллекции объектов
        /// </summary>
        /// <param name="collectionProperty">Путь к коллекции объектов</param>
        /// <returns>Cобытие</returns>
        public EventDefinition CreateContainerCollection(string collectionProperty)
        {
            return CreateEvent(collectionProperty, null, EventType.CreateCollection);
        }

        /// <summary>
        ///     Сформировать событие добавления объекта в коллекцию
        /// </summary>
        /// <param name="collectionProperty">Путь к коллекции объектов</param>
        /// <param name="value">Добавляемый элемент коллекции</param>
        /// <param name="index">Индекс в коллекции для вставки элемента (-1 для добавления в конец)</param>
        /// <returns>Cобытие добавления объекта в коллекцию</returns>
        public EventDefinition AddItemToCollection(string collectionProperty, object value = null, int index = -1)
        {
            return CreateEvent(collectionProperty, value, EventType.AddItemToCollection, index);
        }
    }
}