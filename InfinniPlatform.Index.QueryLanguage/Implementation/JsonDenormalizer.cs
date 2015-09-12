using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace InfinniPlatform.Index.QueryLanguage.Implementation
{
    /// <summary>
    ///     Выполняет однонаправленную денормализацию данных JSON, полученных с использованием IQL
    /// </summary>
    /// <remarks>
    ///     Пример денормализации JSON следующего вида:
    ///     [
    ///     {
    ///     "Result": {
    ///     "Id": "e33bb5b4-c09d-4af8-b73d-91e2f48c7503",
    ///     "Name": "Ivanov",
    ///     "Address": {
    ///     "Id": "304b9398-7247-43c8-bad1-7efb34aff2a8",
    ///     "DisplayName": "г. Челябинск",
    ///     "addr": {
    ///     "Id": "304b9398-7247-43c8-bad1-7efb34aff2a8",
    ///     "Street": "г. Челябинск, пр. Ленина"
    ///     }
    ///     },
    ///     "Policies": [
    ///     {
    ///     "Id": "0770661b-ee6a-4af5-9b19-60395ee97432",
    ///     "Number": "7070"
    ///     }
    ///     ],
    ///     "Phones": [
    ///     "89226567890",
    ///     "89121234567"
    ///     ]
    ///     }
    ///     }
    ///     ]
    ///     преобразуется в следующий плоский объект:
    ///     [
    ///     [
    ///     {
    ///     "Id": "e33bb5b4-c09d-4af8-b73d-91e2f48c7503",
    ///     "Name": "Ivanov",
    ///     "AddressId": "304b9398-7247-43c8-bad1-7efb34aff2a8",
    ///     "AddressDisplayName": "г. Челябинск",
    ///     "AddressaddrId": "304b9398-7247-43c8-bad1-7efb34aff2a8",
    ///     "AddressaddrStreet": "г. Челябинск, пр. Ленина",
    ///     "PoliciesID": "0770661b-ee6a-4af5-9b19-60395ee97432",
    ///     "PoliciesNumber": "7070",
    ///     "Phones": "89226567890"
    ///     },
    ///     {
    ///     "Id": "e33bb5b4-c09d-4af8-b73d-91e2f48c7503",
    ///     "Name": "Ivanov",
    ///     "AddressId": "304b9398-7247-43c8-bad1-7efb34aff2a8",
    ///     "AddressDisplayName": "г. Челябинск",
    ///     "AddressaddrId": "304b9398-7247-43c8-bad1-7efb34aff2a8",
    ///     "AddressaddrStreet": "г. Челябинск, пр. Ленина",
    ///     "PoliciesID": "0770661b-ee6a-4af5-9b19-60395ee97432",
    ///     "PoliciesNumber": "7070",
    ///     "Phones": "89121234567"
    ///     }
    ///     ]
    ///     ]
    /// </remarks>
    public sealed class JsonDenormalizer
    {
        private readonly string _rootItem;

        public JsonDenormalizer(string rootItem = "Result")
        {
            _rootItem = rootItem;
        }

        /// <summary>
        ///     Выполняет денормализацию результата IQL запроса
        /// </summary>
        /// <param name="source">Сериализованный JSON массив для денормализации</param>
        /// <returns>
        ///     Количество элементов в массиве соответсвует количеству элементов
        ///     в исходном массиве source
        /// </returns>
        public JArray ProcessIqlResult(string source)
        {
            return ProcessIqlResult(JArray.Parse(source));
        }

        /// <summary>
        ///     Выполняет денормализацию результата IQL запроса
        /// </summary>
        /// <param name="source">Исходный массив для денормализации</param>
        /// <returns>
        ///     Количество элементов в массиве соответсвует количеству элементов
        ///     в исходном массиве source
        /// </returns>
        public JArray ProcessIqlResult(IEnumerable<JToken> source)
        {
            var result = new JArray();

            foreach (var jToken in source)
            {
                var singleObjectResult = new JArray();

                // Избавляемся от массивов в объекте
                var objectsWithoutArrays = TransformArrays(jToken[_rootItem]);

                foreach (var objectWithoutArrays in objectsWithoutArrays)
                {
                    // Преобразуем вложенные объекты в плоский список свойств
                    singleObjectResult.Add(new JObject
                    {
                        TransformObjects(objectWithoutArrays)
                    });
                }

                result.Add(singleObjectResult);
            }

            return result;
        }

        /// <summary>
        ///     Метод устраняет массивы в объекте
        /// </summary>
        private IEnumerable<JObject> TransformArrays(JToken source)
        {
            // Конструируем новый объект, который вначале будет
            // состоять только из JValues

            var flatObject = new JObject();

            foreach (var jProp in ((JObject) source).Properties())
            {
                var jValue = jProp.Value as JValue;
                if (jValue != null)
                {
                    flatObject.Add(jProp.Name, jProp.Value);
                }
            }

            var flatObjects = new List<JObject>
            {
                flatObject
            };

            foreach (var jProp in ((JObject) source).Properties())
            {
                if (jProp.Value is JValue)
                {
                    // пропускаем простые значения,
                    // так как они уже добавлены во flatObject
                    continue;
                }

                var tempflatObjects = new List<JObject>();

                var objectsToEnumerate = jProp.Value is JObject
                    ? TransformArrays(jProp.Value)
                    : TransformChildArrays(jProp.Value);

                foreach (var handledObject in objectsToEnumerate)
                {
                    foreach (var flatObjectsElement in flatObjects)
                    {
                        var flatObjectClone = (JObject) flatObjectsElement.DeepClone();
                        flatObjectClone.Add(jProp.Name, handledObject);

                        tempflatObjects.Add(flatObjectClone);
                    }
                }

                flatObjects = tempflatObjects;
            }

            return flatObjects;
        }

        /// <summary>
        ///     Метод устраняет массивы в массиве
        /// </summary>
        private IEnumerable<JToken> TransformChildArrays(IEnumerable<JToken> source)
        {
            var flatObjects = new List<JToken>();

            foreach (var arrayElement in source)
            {
                if (arrayElement is JValue)
                {
                    flatObjects.Add(arrayElement);
                }
                else if (arrayElement is JObject)
                {
                    flatObjects.AddRange(TransformArrays(arrayElement));
                }
                else if (arrayElement is JArray)
                {
                    flatObjects.AddRange(TransformChildArrays(arrayElement));
                }
            }

            return flatObjects;
        }

        /// <summary>
        ///     Метод преобразует вложенные объекты в плоские свойства
        /// </summary>
        /// <param name="source">Исходный объект, не содержащий массивы</param>
        /// <returns>Список свойств объекта</returns>
        private static IEnumerable<JProperty> TransformObjects(JObject source)
        {
            var result = new List<JProperty>();

            foreach (var property in source.Properties())
            {
                if (property.Value is JValue)
                {
                    result.Add(property);
                }
                else
                {
                    var propertyValue = property.Value as JObject;
                    if (propertyValue != null)
                    {
                        result.AddRange(
                            TransformObjects(propertyValue).Select(
                                handledProperty =>
                                    new JProperty(
                                        property.Name + handledProperty.Name,
                                        handledProperty.Value)));
                    }
                }
            }

            return result;
        }
    }
}