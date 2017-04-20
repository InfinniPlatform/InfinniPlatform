using InfinniPlatform.Core.Dynamic;

namespace InfinniPlatform.DocumentStorage
{
    internal interface IDocumentStorageIdProvider
    {
        void SetDocumentId(DynamicWrapper document);

        void SetDocumentId<TDocument>(TDocument document) where TDocument : Document;
    }
}