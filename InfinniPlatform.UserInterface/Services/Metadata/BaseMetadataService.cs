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
        private readonly string _route;

        protected BaseMetadataService(string version, string server, int port, string route)
        {
            _version = version;
            _server = server;
            _port = port;
            _route = route;
        }

        public string Version
        {
            get { return _version; }
        }

        /// <summary>
        ///   Роутинг-селектор, на который будет замаплен Nginx (например "1.5")
        /// </summary>
        public string Route
        {
            get { return _route; }
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