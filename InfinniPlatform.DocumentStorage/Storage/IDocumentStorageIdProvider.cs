using InfinniPlatform.DocumentStorage.Contract;
using InfinniPlatform.Sdk.Dynamic;

namespace InfinniPlatform.DocumentStorage.Storage
{
    internal interface IDocumentStorageIdProvider
    {
        void SetDocumentId(DynamicWrapper document);

        void SetDocumentId<TDocument>(TDocument document) where TDocument : Document;
    }
}