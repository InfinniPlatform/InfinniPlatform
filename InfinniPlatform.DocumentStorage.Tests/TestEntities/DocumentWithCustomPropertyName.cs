using InfinniPlatform.DocumentStorage.Attributes;

namespace InfinniPlatform.DocumentStorage.TestEntities
{
    public class DocumentWithCustomPropertyName
    {
        public object _id { get; set; }

        [DocumentPropertyName("forename")]
        public string FirstName { get; set; }

        [DocumentPropertyName("surname")]
        public string LastName { get; set; }
    }
}