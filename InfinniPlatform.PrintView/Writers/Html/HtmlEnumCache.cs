using System;
using System.Collections.Generic;

namespace InfinniPlatform.PrintView.Writers.Html
{
    internal class HtmlEnumCache
    {
        private readonly Dictionary<Type, Dictionary<object, string>> _enums
            = new Dictionary<Type, Dictionary<object, string>>();


        public HtmlEnumCache AddValue<TEnum>(TEnum key, string value)
        {
            Dictionary<object, string> enumValues;

            if (!_enums.TryGetValue(typeof(TEnum), out enumValues))
            {
                enumValues = new Dictionary<object, string>();

                _enums.Add(typeof(TEnum), enumValues);
            }

            enumValues.Add(key, value);

            return this;
        }


        public string GetValue(object key, string defaultValue = null)
        {
            string value;

            Dictionary<object, string> values;

            return key != null
                   && _enums.TryGetValue(key.GetType(), out values)
                   && values.TryGetValue(key, out value)
                ? value
                : defaultValue;
        }
    }
}