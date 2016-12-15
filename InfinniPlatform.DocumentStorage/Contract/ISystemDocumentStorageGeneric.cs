namespace InfinniPlatform.DocumentStorage.Contract
{
    public interface ISystemDocumentStorage<TDocument> : IDocumentStorage<TDocument> where TDocument : Document
    {
    }
}