using System.Linq;
using System.Reflection;
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
                // TODO Check if it's work.
                var attributes = memberMap.MemberInfo.GetCustomAttributes(typeof(TAttribute), false).ToArray();

                if (attributes.Length > 0)
                {
                    var attribute = (TAttribute) (attributes[0] as object);

                    ApplyAttribute(classMap, memberMap, attribute);
                }
            }
        }

        protected abstract void ApplyAttribute(BsonClassMap classMap, BsonMemberMap memberMap, TAttribute attribute);
    }
}