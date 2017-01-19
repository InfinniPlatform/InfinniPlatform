using System.Linq;

using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;

namespace InfinniPlatform.DocumentStorage.MongoDB.Conventions
{
    internal abstract class AttributePropertyConvention<TAttribute> : ConventionBase, IClassMapConvention
    {
        public virtual void Apply(BsonClassMap classMap)
        {
            foreach (var memberMap in classMap.DeclaredMemberMaps.ToList())
            {
                var attributes = memberMap.MemberInfo.GetCustomAttributes(typeof(TAttribute), false);

                if (attributes.Length > 0)
                {
                    var attribute = (TAttribute)attributes[0];

                    ApplyAttribute(classMap, memberMap, attribute);
                }
            }
        }

        protected abstract void ApplyAttribute(BsonClassMap classMap, BsonMemberMap memberMap, TAttribute attribute);
    }
}