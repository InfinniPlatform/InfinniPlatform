using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;

namespace InfinniPlatform.DocumentStorage.MongoDB
{
    internal class MongoIgnoreDiscriminatorConvention : ConventionBase, IClassMapConvention
    {
        public void Apply(BsonClassMap classMap)
        {
            classMap.SetDiscriminatorIsRequired(false);
        }
    }
}