using System;
using System.Collections;
using System.Linq;
using InfinniPlatform.Sdk.Application.Dynamic;

namespace InfinniPlatform.UserInterface.ViewBuilders.DisplayFormats
{
    /// <summary>
    ///     Предоставляет метод для форматирования данных.
    /// </summary>
    public sealed class DataFormat
    {
        private readonly Func<object, string> _formatFunc;

        public DataFormat(Func<object, string> formatFunc)
        {
            _formatFunc = formatFunc ?? DefaultFormat;
        }

        /// <summary>
        ///     Форматирует указанное значение.
        /// </summary>
        public string Format(object value)
        {
            return ObjectIsCollection(value)
                ? string.Join("; ", ((IEnumerable) value).Cast<object>().Select(_formatFunc))
                : _formatFunc(value);
        }

        private static string DefaultFormat(object value)
        {
            if (value != null)
            {
                return value.ToString();
            }

            return null;
        }

        private static bool ObjectIsCollection(object value)
        {
            if ((value is IEnumerable)
                && !(value is string)
                && !(value is DynamicWrapper))
            {
                var type = value.GetType().FullName;

                return (type == "Newtonsoft.Json.Linq.JArray")
                       || !type.StartsWith("Newtonsoft.Json.Linq.");
            }

            return false;
        }
    }
}