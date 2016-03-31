using System.Collections.Generic;
using System.Linq;

using InfinniPlatform.Sdk.Serialization;

using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;

namespace InfinniPlatform.DocumentStorage.MongoDB
{
    /// <summary>
    /// Определяет правила преобразования объекта для MongoDB на основе списка <see cref="IMemberValueConverter" />.
    /// </summary>
    internal sealed class MongoMemberValueConverterResolver : ConventionBase, IMemberMapConvention
    {
        public MongoMemberValueConverterResolver(IEnumerable<IMemberValueConverter> converters)
        {
            _converters = converters?.ToArray() ?? new IMemberValueConverter[] { };
        }


        private readonly IMemberValueConverter[] _converters;


        public void Apply(BsonMemberMap memberMap)
        {
            var member = memberMap.MemberInfo;
            var converter = _converters.FirstOrDefault(i => i.CanConvert(member));

            if (converter != null)
            {
                memberMap.SetSerializer(new MongoMemberValueConverter(member, converter));
            }
        }
    }
}