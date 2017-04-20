namespace InfinniPlatform.DocumentStorage
{
    public interface ISystemDocumentStorage<TDocument> : IDocumentStorage<TDocument> where TDocument : Document
    {
    }
}