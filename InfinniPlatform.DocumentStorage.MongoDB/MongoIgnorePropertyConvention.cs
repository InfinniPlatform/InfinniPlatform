using InfinniPlatform.DocumentStorage.Attributes;

using MongoDB.Bson.Serialization;

namespace InfinniPlatform.DocumentStorage
{
    internal class MongoIgnorePropertyConvention : MongoAttributePropertyConvention<DocumentIgnoreAttribute>
    {
        protected override void ApplyAttribute(BsonClassMap classMap, BsonMemberMap memberMap, DocumentIgnoreAttribute attribute)
        {
            classMap.UnmapMember(memberMap.MemberInfo);
        }
    }
}