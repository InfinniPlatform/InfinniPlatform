using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace InfinniPlatform.Core.Serialization
{
    internal class MemberValueJsonConverterInitializer : IJsonPropertyInitializer
    {
        public MemberValueJsonConverterInitializer(IEnumerable<IMemberValueConverter> converters)
        {
            _converters = converters?.ToArray() ?? new IMemberValueConverter[] { };
        }


        private readonly IMemberValueConverter[] _converters;


        public void InitializeProperty(JsonProperty property, MemberInfo member, MemberSerialization memberSerialization)
        {
            var converter = _converters.FirstOrDefault(i => i.CanConvert(member));

            if (converter != null)
            {
                // For serialization only
                property.Converter = new MemberValueJsonConverter(false, true, converter);

                // For deserialization only
                property.MemberConverter = new MemberValueJsonConverter(true, false, converter);
            }
        }
    }
}