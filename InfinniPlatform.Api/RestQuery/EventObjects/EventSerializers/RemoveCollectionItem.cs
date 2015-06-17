using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfinniPlatform.Api.Events;

namespace InfinniPlatform.Api.RestQuery.EventObjects.EventSerializers
{
    /// <summary>
    ///   Контейнер для удаления элементов коллекции
    /// </summary>
	public sealed class RemoveCollectionItem : IObjectToEventSerializer
	{
		private readonly string _collectionItemPath;
        private readonly string _version;

        public RemoveCollectionItem(string collectionItemPath, string version)
        {
            _collectionItemPath = collectionItemPath;
            _version = version;
        }


        /// <summary>
	    ///   Получить список событий по созданию указанного в аргументах объекта
	    /// </summary>
	    /// <returns>Список событий изменения/создания объекта</returns>
	    public IEnumerable<EventDefinition> GetEvents()
	    {
	        var eventDefinition = new EventDefinition
	            {
	                Action = EventType.RemoveItemFromCollection,
	                Property = _collectionItemPath,
	            };
	        return new List<EventDefinition>() {eventDefinition}.AddVersionDefinition(_version);
	    }
	}
}
