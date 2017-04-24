using InfinniPlatform.Dynamic;

namespace InfinniPlatform.DocumentStorage
{
    internal interface IDocumentStorageIdProvider
    {
        void SetDocumentId(DynamicDocument document);

        void SetDocumentId<TDocument>(TDocument document) where TDocument : Document;
    }
}