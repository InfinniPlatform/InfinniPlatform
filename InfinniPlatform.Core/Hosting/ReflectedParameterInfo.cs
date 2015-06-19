using System;
using System.Collections;
using System.Dynamic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using InfinniPlatform.Sdk.Application.Dynamic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace InfinniPlatform.Hosting
{
    public sealed class ReflectedParameterInfo
    {
        // Helpers

        private static readonly Type ObjectType = typeof (object);
        private static readonly Type DynamicType = typeof (IDynamicMetaObjectProvider);
        private readonly object _defaultValue;
        private readonly bool _hasDefaultValue;
        private readonly bool _isDynamicCollection;
        private readonly bool _isDynamicObject;
        private readonly bool _isIEnumerable;
        private readonly bool _isStream;
        private readonly bool _isString;
        private readonly bool _isValueType;
        private readonly string _name;
        private readonly Type _parameterType;

        public ReflectedParameterInfo(ParameterInfo parameterInfo)
        {
            _parameterType = parameterInfo.ParameterType;
            _name = parameterInfo.Name;
            _isString = TypeIsString(_parameterType);
            _isValueType = _parameterType.IsValueType;
            _isIEnumerable = TypeIsEnumerable(_parameterType);
            _isDynamicCollection = TypeIsDynamicCollection(_parameterType);
            _isDynamicObject = TypeIsDynamicObject(_parameterType);
            _isStream = TypeIsStream(_parameterType);

            if (parameterInfo.HasDefaultValue)
            {
                _hasDefaultValue = true;
                _defaultValue = parameterInfo.DefaultValue;
            }
        }

        public bool IsString
        {
            get { return _isString; }
        }

        public bool IsValueType
        {
            get { return _isValueType; }
        }

        public bool IsSimpleType
        {
            get { return IsString || IsValueType; }
        }

        public object DefaultValue
        {
            get { return _defaultValue; }
        }

        public bool HasDefaultValue
        {
            get { return _hasDefaultValue; }
        }

        public string Name
        {
            get { return _name; }
        }

        public bool IsStream
        {
            get { return _isStream; }
        }

        public bool Named(string paramName)
        {
            return Name.ToLowerInvariant() == paramName.ToLowerInvariant();
        }

        /// <summary>
        ///     Получить значение аргумента запроса
        /// </summary>
        /// <remarks>
        ///     Очень неудачный код, необходимо в ближайшее время
        ///     провести рефакторинг этого метода, разложив его по возможным вариантам использования
        /// </remarks>
        public object GetArgumentValue(object value)
        {
            if (IsStream)
            {
                return value;
            }
            if (IsString)
            {
                return value != null ? value.ToString() : null;
            }
            if (IsValueType)
            {
                return ValueToType(value);
            }
            if (_isIEnumerable && !_isDynamicObject)
            {
                if (value == null || string.IsNullOrEmpty(value.ToString()))
                {
                    return null;
                }
                JArray obj = null;

                if (value is string)
                {
                    if (!string.IsNullOrEmpty((string) value))
                    {
                        obj = JArray.Parse(value as string);
                    }
                }
                else
                {
                    obj = JArray.FromObject(value);
                }

                if (_isDynamicCollection)
                {
                    return obj.ToString().ToDynamicList();
                }
                return obj.ToObject(_parameterType);
            }
            if (_isDynamicObject && value != null && !value.GetType().IsValueType && value.GetType() != typeof (string))
            {
                if (value == null || string.IsNullOrEmpty(value.ToString()))
                {
                    return null;
                }
                if (value is string && !string.IsNullOrEmpty((string) value))
                {
                    return value.ToDynamic();
                }
                return value.ToDynamic();
            }
            if (value is JObject && _isDynamicObject)
            {
                return value.ToDynamic();
            }
            return value != null ? JsonConvert.DeserializeObject(value.ToString(), _parameterType) : null;
        }

        private object ValueToType(object value)
        {
            if (value == null)
                return null;
            if (_parameterType == typeof (string))
                value = value.ToString();
            else if (_parameterType == typeof (int))
                value = Convert.ToInt32(value, CultureInfo.CurrentCulture);
            else if (_parameterType == typeof (Int32))
                value = Convert.ToInt32(value, CultureInfo.CurrentCulture);
            else if (_parameterType == typeof (Int64))
                value = Convert.ToInt64(value, CultureInfo.CurrentCulture);
            else if (_parameterType == typeof (decimal))
                value = Convert.ToDecimal(value, CultureInfo.CurrentCulture);
            else if (_parameterType == typeof (double))
                value = Convert.ToDouble(value, CultureInfo.CurrentCulture);
            else if (_parameterType == typeof (DateTime))
                value = Convert.ToDateTime(value, CultureInfo.CurrentCulture);
            else if (_parameterType == typeof (bool))
                value = Convert.ToBoolean(value, CultureInfo.CurrentCulture);
            else if (_parameterType == typeof (Guid))
                value = new Guid(value.ToString());
            else if (_parameterType.IsEnum)
                value = Enum.ToObject(_parameterType, value);
            else if (_parameterType == typeof (DateTime?))
                value = string.IsNullOrEmpty(value.ToString())
                    ? new DateTime?()
                    : Convert.ToDateTime(value, CultureInfo.CurrentCulture);

            return value;
        }

        private static bool TypeIsDynamicObject(Type target)
        {
            return (target == ObjectType) || (target.IsGenericType && target.GetInterfaces().Any(t => t == DynamicType));
        }

        private static bool TypeIsDynamicCollection(Type target)
        {
            return target.IsGenericType
                   && typeof (IEnumerable).IsAssignableFrom(target)
                   &&
                   (DynamicType.IsAssignableFrom(target.GetGenericArguments().First()) ||
                    target.GetGenericArguments().First() == ObjectType);
        }

        private static bool TypeIsEnumerable(Type target)
        {
            return typeof (IEnumerable).IsAssignableFrom(target);
        }

        private static bool TypeIsString(Type target)
        {
            return target == typeof (string);
        }

        private static bool TypeIsStream(Type target)
        {
            return typeof (Stream).IsAssignableFrom(target);
        }
    }
}