namespace InfinniPlatform.DocumentStorage.Abstractions
{
    public interface ISystemDocumentStorage<TDocument> : IDocumentStorage<TDocument> where TDocument : Document
    {
    }
}