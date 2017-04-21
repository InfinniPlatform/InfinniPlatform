using InfinniPlatform.DocumentStorage.Attributes;

using MongoDB.Bson.Serialization;

namespace InfinniPlatform.DocumentStorage
{
    internal class MongoPropertyNameConvention : MongoAttributePropertyConvention<DocumentPropertyNameAttribute>
    {
        protected override void ApplyAttribute(BsonClassMap classMap, BsonMemberMap memberMap, DocumentPropertyNameAttribute attribute)
        {
            memberMap.SetElementName(attribute.Name);
        }
    }
}