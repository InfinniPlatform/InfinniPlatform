using InfinniPlatform.Core.Serialization;

namespace InfinniPlatform.Core.TestEntities
{
    public class EnityWithCustomPropertyName
    {
        [SerializerPropertyName("forename")]
        public string FirstName { get; set; }

        [SerializerPropertyName("surname")]
        public string LastName { get; set; }
    }
}