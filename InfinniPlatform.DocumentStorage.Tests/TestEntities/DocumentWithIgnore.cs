using InfinniPlatform.DocumentStorage.Abstractions.Attributes;

namespace InfinniPlatform.DocumentStorage.Tests.TestEntities
{
    public class DocumentWithIgnore
    {
        public object _id { get; set; }

        public int Property1 { get; set; }

        [DocumentIgnore]
        public int Property2Ignore { get; set; }

        public int Property3 { get; set; }

        [DocumentIgnore]
        public int Property4Ignore { get; set; }
    }
}