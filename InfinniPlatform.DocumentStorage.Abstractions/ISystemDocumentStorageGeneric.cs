namespace InfinniPlatform.DocumentStorage
{
    /// <summary>
    /// Provides methods for system documents storage.
    /// </summary>
    public interface ISystemDocumentStorage<TDocument> : IDocumentStorage<TDocument> where TDocument : Document
    {
    }
}