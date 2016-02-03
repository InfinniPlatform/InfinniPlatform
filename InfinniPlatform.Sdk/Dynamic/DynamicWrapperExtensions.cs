using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using InfinniPlatform.Sdk.Serialization;

namespace InfinniPlatform.Sdk.Dynamic
{
    /// <summary>
    /// Не используйте эти методы!
    /// </summary>
    [Obsolete]
    public static class DynamicWrapperExtensions
    {
        [Obsolete]
        public static dynamic ToDynamic(this string value)
        {
            return JsonObjectSerializer.Default.Deserialize<DynamicWrapper>(value);
        }

        [Obsolete]
        public static IEnumerable<dynamic> ToDynamicList(this string value)
        {
            return JsonObjectSerializer.Default.Deserialize<DynamicWrapper[]>(value);
        }

        [Obsolete]
        public static dynamic ToDynamic(this object value)
        {
            return JsonObjectSerializer.Default.ConvertFromDynamic<DynamicWrapper>(value);
        }

        [Obsolete]
        public static IEnumerable<dynamic> ToEnumerable(this object value)
        {
            var enumerable = value as IEnumerable;
            return (enumerable != null) ? enumerable.Cast<object>().Select(ToDynamic) : Enumerable.Empty<dynamic>();
        }
    }
}