using InfinniPlatform.Sdk.Dynamic;

namespace InfinniPlatform.DocumentStorage.Storage
{
    internal interface IDocumentStorageIdProvider
    {
        void SetDocumentId(DynamicWrapper document);
    }
}