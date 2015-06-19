using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using InfinniPlatform.Sdk.Application.Dynamic;
using InfinniPlatform.Sdk.Application.Events;

namespace InfinniPlatform.Api.RestQuery.EventObjects
{
    /// <summary>
    ///     Класс для выполнения преобразований объектов в список событий
    /// </summary>
    public class EventList
    {
        private List<EventDefinition> _eventList;
        private readonly ObjectMetadataHandler _handler;

        public EventList(dynamic root, string objectName)
        {
            _handler = new ObjectMetadataHandler();
            _eventList = new List<EventDefinition>();
            _eventList.AddRange(ToEventList(root, objectName));
        }

        public EventList(dynamic root, string collectionName, int itemIndex)
        {
            _handler = new ObjectMetadataHandler();

            _eventList = new List<EventDefinition>
            {
                _handler.AddItemToCollection(collectionName)
            };

            foreach (var property in root)
            {
                var itemName = string.IsNullOrEmpty(collectionName)
                    ? property.Key
                    : String.Join(".",
                        collectionName,
                        itemIndex == -1 ? "@" : itemIndex.ToString(CultureInfo.InvariantCulture),
                        property.Key);

                HandleProperty(property.Value, _eventList, itemName);
            }
        }

        /// <summary>
        ///     Исключает заданное событие из списка событий
        /// </summary>
        public EventList Exclude(string propertyExclusion)
        {
            var updatedEventList = new List<EventDefinition>();

            updatedEventList.AddRange(
                _eventList.Where(x => !(new Regex(propertyExclusion).IsMatch(x.Property))));

            _eventList = updatedEventList;

            return this;
        }

        /// <summary>
        ///     Получить представление объекта в виде списка событий
        /// </summary>
        public IEnumerable<EventDefinition> GetEvents(bool isUpdateEvents = false)
        {
            return isUpdateEvents ? _eventList.Skip(1) : _eventList;
        }

        /// <summary>
        ///     Получить представление объекта в виде сериализованного списка событий
        /// </summary>
        public IEnumerable<string> GetSerializedEvents(bool isUpdateEvents = false)
        {
            var result = new List<string>();
            var isFirstEvent = true;

            foreach (var eventDefinition in _eventList)
            {
                if (isUpdateEvents && isFirstEvent)
                {
                    isFirstEvent = false;
                }
                else
                {
                    result.Add(eventDefinition.ToDynamic().ToString());
                }
            }

            return result;
        }

        private IEnumerable<EventDefinition> ToEventList(dynamic targetInstance, string instanceName)
        {
            var result = new List<EventDefinition>();

            if (targetInstance is IEnumerable<object>)
            {
                result.Add(_handler.CreateContainerCollection(instanceName));

                var index = 0;

                foreach (var item in targetInstance)
                {
                    if (item is IEnumerable<object>)
                    {
                        result.Add(_handler.AddItemToCollection(instanceName));

                        result.AddRange(ToEventList(item, String.Join(".", instanceName, index)));
                    }
                    if (item is DynamicWrapper)
                    {
                        result.Add(_handler.AddItemToCollection(instanceName));

                        foreach (var property in item)
                        {
                            HandleProperty(property.Value, result, String.Join(".", instanceName, index, property.Key));
                        }
                    }
                    else
                    {
                        result.Add(_handler.AddItemToCollection(instanceName, item));
                    }

                    index++;
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(instanceName))
                {
                    result.Add(_handler.CreateContainer(instanceName));
                }

                foreach (var property in targetInstance)
                {
                    var itemName = string.IsNullOrEmpty(instanceName)
                        ? property.Key
                        : String.Join(".", instanceName, property.Key);

                    HandleProperty(property.Value, result, itemName);
                }
            }

            return result;
        }

        private void HandleProperty(object propertyValue, List<EventDefinition> result, string itemName)
        {
            if (propertyValue is DynamicWrapper ||
                propertyValue is IEnumerable<dynamic>)
            {
                result.AddRange(ToEventList(propertyValue, itemName));
            }
            else
            {
                result.Add(_handler.CreateProperty(itemName, propertyValue));
            }
        }
    }

    /// <summary>
    ///     Расширение для преобразования объектов в список событий
    /// </summary>
    public static class EventListExtensions
    {
        /// <summary>
        ///     Возвращает <see cref="EventList" /> для объекта
        /// </summary>
        /// <param name="target">Объект, для которого необходимо составить список событий</param>
        /// <param name="rootName">Имя корневого узла</param>
        /// <returns></returns>
        public static EventList ToEventListAsObject(this object target, string rootName = "FormMetadata")
        {
            return new EventList(target.ToDynamic(), rootName);
        }

        /// <summary>
        ///     Возвращает <see cref="EventList" /> для элемента коллекции
        /// </summary>
        /// <param name="target">Объект, для которого необходимо составить список событий</param>
        /// <param name="collectionName">Наименование коллекции</param>
        /// <param name="indexItem">Индекс элемента в коллекции</param>
        /// <returns>Конструктор событий для апдейта элемента коллекции</returns>
        public static EventList ToEventListCollectionItem(this object target, string collectionName, int indexItem = -1)
        {
            return new EventList(target.ToDynamic(), collectionName, indexItem);
        }
    }
}