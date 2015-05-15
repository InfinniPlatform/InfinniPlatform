using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace InfinniPlatform.Api.Dynamic
{
    /// <summary>
    /// Необходимость добавления данных расширений возникла вследствие того,
    /// что в платформе написано много кода, завязанного на использование методов класса
    /// DynamicInstance. В настоящее время DynamicInstance заменен на DynamicWrapper,
    /// но для совместимости некоторые методы DynamicInstance перенесены сюда в виде методов расширений
    /// </summary>
    public static class DynamicWrapperExtensions
    {
        public static dynamic ToDynamic(this string instance)
        {
            return ConvertFromJsonToken(JToken.Parse(instance));
        }

        public static IEnumerable<dynamic> ToDynamicList(this string instance)
        {
            var jarray = JArray.Parse(instance);

            return jarray.Select(ConvertFromJsonToken).ToList();
        }

        public static dynamic ToDynamic(this object objectInstance)
        {
            if (objectInstance is DynamicWrapper)
            {
                return objectInstance;
            }

            return ConvertFromJsonToken(JToken.FromObject(objectInstance));
        }

        public static IEnumerable<dynamic> ToEnumerable(this object sourceObject)
        {
            if (sourceObject == null)
            {
                return new List<dynamic>();
            }

            var objectInstance = ToDynamic(sourceObject);

            var dynamicArray = objectInstance as IEnumerable<dynamic>;
            if (dynamicArray != null)
            {
                return dynamicArray;
            }

            return new List<dynamic> { ToDynamic(objectInstance) };
        }

        public static string DynamicEnumerableToString(this IEnumerable<dynamic> sourceObject)
        {
            return JArray.FromObject(sourceObject).ToString();
        }

        public static object ConvertFromJsonObject(JObject value)
        {
            DynamicWrapper result = null;

            if (value != null)
            {
                result = new DynamicWrapper();

                foreach (var property in value)
                {
                    if (property.Value != null)
                    {
                        result[property.Key] = ConvertFromJsonToken(property.Value);
                    }
                }
            }

            return result;
        }

        public static object ConvertFromJsonToken(JToken value)
        {
            object result = null;

            if (value != null)
            {
                if (value is JValue)
                {
                    result = ((JValue)value).Value;
                }
                else if (value is JObject)
                {
                    result = ConvertFromJsonObject((JObject)value).ToDynamic();
                }
                else if (value is JArray)
                {
                    var array = new List<dynamic>();

                    foreach (var item in (JArray)value)
                    {
                        array.Add(ConvertFromJsonToken(item));
                    }

                    result = array;
                }

            }

            return result;
        }
    }
}
