using InfinniPlatform.Core.Abstractions.Serialization;

namespace InfinniPlatform.Core.Tests.TestEntities
{
    public class EnityWithCustomPropertyName
    {
        [SerializerPropertyName("forename")]
        public string FirstName { get; set; }

        [SerializerPropertyName("surname")]
        public string LastName { get; set; }
    }
}