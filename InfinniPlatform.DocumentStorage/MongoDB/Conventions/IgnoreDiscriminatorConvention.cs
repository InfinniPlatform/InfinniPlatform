using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;

namespace InfinniPlatform.DocumentStorage.MongoDB.Conventions
{
    internal sealed class IgnoreDiscriminatorConvention : IClassMapConvention
    {
        public string Name => nameof(IgnoreDiscriminatorConvention);

        public void Apply(BsonClassMap classMap)
        {
            classMap.SetDiscriminatorIsRequired(false);
        }
    }
}