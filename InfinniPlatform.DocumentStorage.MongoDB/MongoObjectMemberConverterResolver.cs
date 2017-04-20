using System;
using System.Reflection;

using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;

namespace InfinniPlatform.DocumentStorage
{
    /// <summary>
    /// Определяет правила преобразования объекта для MongoDB в случаях, когда тип объекта заранее не известен <see cref="object"/>.
    /// </summary>
    internal class MongoObjectMemberConverterResolver : ConventionBase, IMemberMapConvention
    {
        public void Apply(BsonMemberMap memberMap)
        {
            var member = memberMap.MemberInfo;
            var memberType = GetMemberType(member);

            // Если тип свойства не определен
            if (memberType == typeof(object))
            {
                // Используется дополнительная логика при десериализации значения свойства
                memberMap.SetSerializer(new MongoObjectMemberConverter(memberType));
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