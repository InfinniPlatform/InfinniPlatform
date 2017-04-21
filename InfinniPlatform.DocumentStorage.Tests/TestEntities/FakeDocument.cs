namespace InfinniPlatform.DocumentStorage.TestEntities
{
    [DocumentType("FakeDocumentCollection")]
    public class FakeDocument : Document
    {
        public int prop1 { get; set; }

        public string prop2 { get; set; }

        public int? prop3 { get; set; }
    }
}