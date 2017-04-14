using InfinniPlatform.Sdk.Serialization;

namespace InfinniPlatform.Sdk.Tests.TestEntities
{
    public class EnityWithCustomPropertyName
    {
        [SerializerPropertyName("forename")]
        public string FirstName { get; set; }

        [SerializerPropertyName("surname")]
        public string LastName { get; set; }
    }
}