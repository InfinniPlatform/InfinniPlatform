using System;
using System.Collections.Generic;

using InfinniPlatform.UserInterface.Dynamic;
using InfinniPlatform.UserInterface.Properties;

namespace InfinniPlatform.UserInterface.Services.Metadata
{
    /// <summary>
    /// Базовый класс сервисов для работы с метаданными системы.
    /// </summary>
    public abstract class BaseMetadataService : IMetadataService
    {
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