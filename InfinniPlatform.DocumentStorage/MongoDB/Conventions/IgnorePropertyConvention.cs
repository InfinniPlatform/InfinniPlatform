using System;
using System.Linq;

using InfinniPlatform.DocumentStorage.Contract.Attributes;

using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;

namespace InfinniPlatform.DocumentStorage.MongoDB.Conventions
{
    internal class IgnorePropertyConvention : ConventionBase, IClassMapConvention
    {
        public void Apply(BsonClassMap classMap)
        {
            ApplyAttributeConvention<DocumentIgnoreAttribute>(classMap, (c, m, a) => c.UnmapMember(m.MemberInfo));
        }

        private static void ApplyAttributeConvention<TAttribute>(BsonClassMap classMap, Action<BsonClassMap, BsonMemberMap, TAttribute> memberConvention) where TAttribute : Attribute
        {
            foreach (var memberMap in classMap.DeclaredMemberMaps.ToList())
            {
                var attributes = memberMap.MemberInfo.GetCustomAttributes(typeof(TAttribute), false);

                if (attributes.Length > 0)
                {
                    var attribute = (TAttribute)attributes[0];

                    memberConvention(classMap, memberMap, attribute);
                }
            }
        }
    }
}