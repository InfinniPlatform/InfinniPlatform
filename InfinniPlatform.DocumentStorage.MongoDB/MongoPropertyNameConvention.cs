using InfinniPlatform.DocumentStorage.Abstractions.Attributes;

using MongoDB.Bson.Serialization;

namespace InfinniPlatform.DocumentStorage.MongoDB
{
    internal class MongoPropertyNameConvention : MongoAttributePropertyConvention<DocumentPropertyNameAttribute>
    {
        protected override void ApplyAttribute(BsonClassMap classMap, BsonMemberMap memberMap, DocumentPropertyNameAttribute attribute)
        {
            memberMap.SetElementName(attribute.Name);
        }
    }
}