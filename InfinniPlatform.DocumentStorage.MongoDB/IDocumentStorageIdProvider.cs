using InfinniPlatform.Core.Dynamic;
using InfinniPlatform.DocumentStorage.Abstractions;

namespace InfinniPlatform.DocumentStorage.MongoDB
{
    internal interface IDocumentStorageIdProvider
    {
        void SetDocumentId(DynamicWrapper document);

        void SetDocumentId<TDocument>(TDocument document) where TDocument : Document;
    }
}