using System.Collections.Generic;

namespace InfinniPlatform.Api.RestApi.DataApi
{
    /// <summary>
    /// </summary>
    public static class MetadataLoaderHelper
    {
        /// <summary>
        /// Получает элемент конфигурации из словаря.
        /// </summary>
        /// <param name="dictionary">Словарь элементов конфигурации.</param>
        /// <param name="itemId">Идентификатор элемента конфигурации.</param>
        public static object TryGetItem(this object dictionary, string itemId)
        {
            dynamic item;
            return ((dynamic)dictionary).TryGetValue(itemId, out item)
                       ? item
                       : null;
        }

        /// <summary>
        /// Получает метаданные элемента конфигурации из словаря.
        /// </summary>
        /// <param name="dictionary">Словарь элементов конфигурации.</param>
        /// <param name="itemId">Идентификатор элемента конфигурации.</param>
        public static object TryGetItemContent(this object dictionary, string itemId)
        {
            return ((dynamic)TryGetItem(dictionary, itemId))?.Content;
        }

        /// <summary>
        /// Преобразует массив элементов конфигурации в словарь.
        /// </summary>
        /// <param name="enumerable">Массив элементов конфигурации.</param>
        /// <returns>Словарь (имя элемента, элемент).</returns>
        public static Dictionary<string, object> ConvertToDictionary(this IEnumerable<dynamic> enumerable)
        {
            var dictionary = new Dictionary<string, object>();

            foreach (var item in enumerable)
            {
                var key = (string)item.Content.Name;

                if (dictionary.ContainsKey(key))
                {
                    dictionary.Remove(key);
                }

                dictionary.Add(key, item);
            }

            return dictionary;
        }
    }
}