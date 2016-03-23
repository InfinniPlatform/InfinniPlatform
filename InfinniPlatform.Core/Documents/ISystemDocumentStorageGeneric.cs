using InfinniPlatform.Sdk.Documents;

namespace InfinniPlatform.Core.Documents
{
    public interface ISystemDocumentStorage<TDocument> : IDocumentStorage<TDocument> where TDocument : Document
    {
    }
}