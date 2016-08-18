using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;

namespace InfinniPlatform.DocumentStorage.MongoDB.Conventions
{
    internal class IgnoreDiscriminatorConvention : ConventionBase, IClassMapConvention
    {
        public void Apply(BsonClassMap classMap)
        {
            classMap.SetDiscriminatorIsRequired(false);
        }
    }
}