using InfinniPlatform.DocumentStorage.Contract;

using MongoDB.Bson.Serialization;

namespace InfinniPlatform.DocumentStorage.MongoDB.Conventions
{
    internal class PropertyNameConvention : AttributePropertyConvention<DocumentPropertyNameAttribute>
    {
        protected override void ApplyAttribute(BsonClassMap classMap, BsonMemberMap memberMap, DocumentPropertyNameAttribute attribute)
        {
            memberMap.SetElementName(attribute.Name);
        }
    }
}