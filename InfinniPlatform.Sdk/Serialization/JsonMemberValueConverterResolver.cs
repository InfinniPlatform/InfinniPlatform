using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace InfinniPlatform.Sdk.Serialization
{
    /// <summary>
    /// Определяет правила преобразования объекта в JSON-представление и обратно на основе списка <see cref="IMemberValueConverter" />.
    /// </summary>
    internal sealed class JsonMemberValueConverterResolver : JsonDefaultContractResolver
    {
        public JsonMemberValueConverterResolver(IEnumerable<IMemberValueConverter> converters)
        {
            _converters = converters?.ToArray() ?? new IMemberValueConverter[] { };
        }


        private readonly IMemberValueConverter[] _converters;


        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var property = base.CreateProperty(member, memberSerialization);

            var converter = _converters.FirstOrDefault(i => i.CanConvert(member));

            if (converter != null)
            {
                // Используется только для сериализации
                property.Converter = new JsonMemberValueConverter(false, true, converter);

                // Используется только для десериализации
                property.MemberConverter = new JsonMemberValueConverter(true, false, converter);
            }

            return property;
        }
    }
}