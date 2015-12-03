using System.Collections.Generic;

namespace InfinniPlatform.Api.RestApi.DataApi
{
    public static class MetadataLoaderHelper
    {
        /// <summary>
        /// Получает элемент конфигурации из словаря.
        /// </summary>
        /// <param name="dictionary">Словарь элементов конфигурации.</param>
        /// <param name="itemId">Идентификатор элемента конфигурации.</param>
        public static object TryGetItem(this IDictionary<string, object> dictionary, string itemId)
        {
            object item;

            dictionary.TryGetValue(itemId, out item);

            return item;
        }

        /// <summary>
        /// Преобразует массив элементов конфигурации в словарь.
        /// </summary>
        /// <param name="enumerable">Массив элементов конфигурации.</param>
        /// <returns>Словарь (имя элемента, элемент).</returns>
        public static IDictionary<string, object> ConvertToDictionary(this IEnumerable<object> enumerable)
        {
            var dictionary = new Dictionary<string, object>();

            foreach (dynamic item in enumerable)
            {
                var key = (string)item.Content.Name;

                dictionary[key] = item;
            }

            return dictionary;
        }
    }
}