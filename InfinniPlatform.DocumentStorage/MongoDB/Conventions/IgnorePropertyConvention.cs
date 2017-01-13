using InfinniPlatform.DocumentStorage.Contract.Attributes;

using MongoDB.Bson.Serialization;

namespace InfinniPlatform.DocumentStorage.MongoDB.Conventions
{
    internal class IgnorePropertyConvention : AttributePropertyConvention<DocumentIgnoreAttribute>
    {
        protected override void ApplyAttribute(BsonClassMap classMap, BsonMemberMap memberMap, DocumentIgnoreAttribute attribute)
        {
            classMap.UnmapMember(memberMap.MemberInfo);
        }
    }
}