namespace InfinniPlatform.Sdk.Documents
{
    public interface ISystemDocumentStorage<TDocument> : IDocumentStorage<TDocument> where TDocument : Document
    {
    }
}