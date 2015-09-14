using System;
using System.Collections;
using System.Linq;
using InfinniPlatform.Sdk.Dynamic;

namespace InfinniPlatform.UserInterface.ViewBuilders.Data.DataProviders
{
    /// <summary>
    ///     Провайдер данных для объектов.
    /// </summary>
    public sealed class ObjectDataProvider : IDataProvider
    {
        private string _configId;
        private string _documentId;
        private string _version;
        private readonly ConcurrentCollection _items;

        public ObjectDataProvider(string idProperty, IEnumerable items)
        {
            _items = new ConcurrentCollection(idProperty);

            if (items != null)
            {
                _items.ReplaceAll(items.Cast<object>());
            }
        }

        public string GetConfigId()
        {
            return _configId;
        }

        public void SetConfigId(string value)
        {
            _configId = value;
        }

        public string GetDocumentId()
        {
            return _documentId;
        }

        public void SetDocumentId(string value)
        {
            _documentId = value;
        }

        public string GetVersion()
        {
            throw new NotImplementedException();
        }

        public object CreateItem()
        {
            return new DynamicWrapper();
        }

        public void ReplaceItem(object item)
        {
            _items.AddOrUpdate(item);
        }

        public void DeleteItem(string itemId)
        {
            _items.RemoveById(itemId);
        }

        public object GetItem(string itemId)
        {
            return _items.GetById(itemId);
        }

        public object CloneItem(string itemId)
        {
            var item = _items.GetById(itemId);

            if (item is ICloneable)
            {
                item = ((ICloneable) item).Clone();
            }

            return item;
        }

        public IEnumerable GetItems(IEnumerable criterias, int pageNumber, int pageSize)
        {
            return _items;
        }

        public void SetVersion(string version)
        {
            _version = version;
        }
    }
}