using InfinniPlatform.Serialization;

namespace InfinniPlatform.TestEntities
{
    public class EnityWithCustomPropertyName
    {
        [SerializerPropertyName("forename")]
        public string FirstName { get; set; }

        [SerializerPropertyName("surname")]
        public string LastName { get; set; }
    }
}