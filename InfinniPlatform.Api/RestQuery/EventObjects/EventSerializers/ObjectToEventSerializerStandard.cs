using System.Collections.Generic;
using InfinniPlatform.Sdk.Events;

namespace InfinniPlatform.Api.RestQuery.EventObjects.EventSerializers
{
    /// <summary>
    ///     Стандартный преобразователь объектов в события
    /// </summary>
    public sealed class ObjectToEventSerializerStandard : IObjectToEventSerializer
    {
        private readonly object _eventsObject;

        /// <summary>
        ///     Конструктор
        /// </summary>
        /// <param name="eventsObject">Объект, представляемый списком событий</param>
        public ObjectToEventSerializerStandard(object eventsObject)
        {
            _eventsObject = eventsObject;
        }

        /// <summary>
        ///     Получить список событий по созданию указанного в аргументах объекта
        /// </summary>
        /// <returns>Список событий изменения/создания объекта</returns>
        public IEnumerable<EventDefinition> GetEvents()
        {
            return _eventsObject.ToEventListAsObject("").GetEvents();
        }
    }
}