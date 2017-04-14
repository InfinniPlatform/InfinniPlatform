using InfinniPlatform.DocumentStorage.Abstractions;
using InfinniPlatform.Sdk.Dynamic;

namespace InfinniPlatform.DocumentStorage.MongoDB
{
    internal interface IDocumentStorageIdProvider
    {
        void SetDocumentId(DynamicWrapper document);

        void SetDocumentId<TDocument>(TDocument document) where TDocument : Document;
    }
}