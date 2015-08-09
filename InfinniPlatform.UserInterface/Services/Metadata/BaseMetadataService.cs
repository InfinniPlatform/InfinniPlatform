using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using InfinniPlatform.Api.Metadata;
using InfinniPlatform.UserInterface.Dynamic;
using InfinniPlatform.UserInterface.Properties;

namespace InfinniPlatform.UserInterface.Services.Metadata
{
    /// <summary>
    ///     Базовый класс сервисов для работы с метаданными системы.
    /// </summary>
    public abstract class BaseMetadataService : IMetadataService 
    {
        private readonly string _version;
        private readonly string _server;
        private readonly int _port;

        protected BaseMetadataService(string version, string server, int port)
        {
            _version = version;
            _server = server;
            _port = port;
        }

        public string Version
        {
            get { return _version; }
        }

        public abstract object CreateItem();

        public abstract void ReplaceItem(dynamic item);

        public abstract void DeleteItem(string itemId);

        public abstract object GetItem(string itemId);

        public object CloneItem(string itemId)
        {
            // Todo: Избавиться от этой конвертации после того, как в системе будет одна реализация Dynamic

            dynamic item = GetItem(itemId);

            if (item != null)
            {
                item.Id = Guid.NewGuid().ToString();
                item.Name = string.Format(Resources.CloneElementName, item.Name);
            }

            return DynamicExtensions.JsonToObject(item);
        }

        public abstract IEnumerable<object> GetItems();

    }
}