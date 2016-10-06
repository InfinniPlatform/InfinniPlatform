using System;
using System.Collections;
using System.Linq;

using InfinniPlatform.Sdk.Dynamic;

namespace InfinniPlatform.PrintView.Factories
{
    internal static class ConvertHelper
    {
        public static bool ObjectIsNullOrEmpty(object value)
        {
            return (value == null)
                   || (ObjectIsCollection(value)
                       && !((IEnumerable)value).Cast<object>().Any());
        }

        public static bool ObjectIsCollection(object value)
        {
            return (value is IEnumerable)
                   && !(value is string)
                   && !(value is DynamicWrapper);
        }

        public static bool TryToDateTime(object value, out DateTime result)
        {
            return TryConvert(value, Convert.ToDateTime, out result);
        }

        public static bool TryToDouble(object value, out double result)
        {
            return TryConvert(value, Convert.ToDouble, out result);
        }

        public static bool TryToBool(object value, out bool result)
        {
            return TryConvert(value, Convert.ToBoolean, out result);
        }

        private static bool TryConvert<T>(object value, Func<object, T> convert, out T result)
        {
            result = default(T);

            if (value != null)
            {
                try
                {
                    result = convert(value);
                    return true;
                }
                catch
                {
                }
            }

            return false;
        }
    }
}