using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using InfinniPlatform.Serialization;

using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;

namespace InfinniPlatform.DocumentStorage
{
    /// <summary>
    /// Определяет правила преобразования объекта для MongoDB на основе списка <see cref="IMemberValueConverter" />.
    /// </summary>
    internal class MongoMemberValueConverterResolver : ConventionBase, IMemberMapConvention
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
                var memberType = GetMemberType(member);
                memberMap.SetSerializer(new MongoMemberValueConverter(memberType, converter));
            }
        }


        private static Type GetMemberType(MemberInfo memberInfo)
        {
            if (memberInfo.MemberType == MemberTypes.Property)
            {
                return ((PropertyInfo)memberInfo).PropertyType;
            }

            if (memberInfo.MemberType == MemberTypes.Field)
            {
                return ((FieldInfo)memberInfo).FieldType;
            }

            return null;
        }
    }
}