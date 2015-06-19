using System;
using System.Collections;
using System.Linq;
using InfinniPlatform.Sdk.Application.Dynamic;

namespace InfinniPlatform.FlowDocument.Builders
{
    internal static class ConvertHelper
    {
        public static bool ObjectIsNullOrEmpty(object value)
        {
            return (value == null)
                   || (ObjectIsCollection(value)
                       && !((IEnumerable) value).Cast<object>().Any());
        }

        public static bool ObjectIsCollection(object value)
        {
            if ((value is IEnumerable)
                && !(value is string)
                && !(value is DynamicWrapper))
            {
                return true;
            }
            return false;
        }

        public static bool TryToNormString(object value, out string result)
        {
            if (TryToString(value, out result))
            {
                if (!string.IsNullOrWhiteSpace(result))
                {
                    result = result.Trim().ToLower();
                    return true;
                }
            }

            return false;
        }

        public static bool TryToString(object value, out string result)
        {
            result = default(string);

            if (value != null)
            {
                try
                {
                    result = Convert.ToString(value);

                    if (!string.IsNullOrEmpty(result))
                    {
                        return true;
                    }
                }
                catch
                {
                }
            }

            return false;
        }

        public static bool TryToBytes(object value, out byte[] result)
        {
            result = null;

            if (value is byte[])
            {
                result = (byte[]) value;
                return true;
            }

            if (value is string)
            {
                try
                {
                    result = Convert.FromBase64String((string) value);
                    return true;
                }
                catch
                {
                }
            }

            return false;
        }

        public static bool TryToDateTime(object value, out DateTime result)
        {
            result = default(DateTime);

            if (value != null)
            {
                try
                {
                    result = Convert.ToDateTime(value);
                    return true;
                }
                catch
                {
                }
            }

            return false;
        }

        public static bool TryToDouble(object value, out double result)
        {
            result = default(double);

            if (value != null)
            {
                try
                {
                    result = Convert.ToDouble(value);
                    return true;
                }
                catch
                {
                }
            }

            return false;
        }

        public static bool TryToInt(object value, out int result)
        {
            result = default(int);

            if (value != null)
            {
                try
                {
                    result = Convert.ToInt32(value);
                    return true;
                }
                catch
                {
                }
            }

            return false;
        }

        public static bool TryToBool(object value, out bool result)
        {
            result = default(bool);

            if (value != null)
            {
                try
                {
                    result = Convert.ToBoolean(value);
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