using InfinniPlatform.Sdk.Documents;

namespace InfinniPlatform.DocumentStorage.Storage
{
    internal class SystemDocumentStorageImpl : DocumentStorageImpl, ISystemDocumentStorage
    {
        public SystemDocumentStorageImpl(string documentType,
                                         IDocumentStorageProviderFactory storageProviderFactory,
                                         IDocumentStorageIdProvider storageIdProvider,
                                         ISystemDocumentStorageHeaderProvider storageHeaderProvider,
                                         ISystemDocumentStorageFilterProvider systemDocumentStorageFilterProvider,
                                         IDocumentStorageInterceptorProvider storageInterceptorProvider)
            : base(documentType, storageProviderFactory, storageIdProvider, storageHeaderProvider, systemDocumentStorageFilterProvider, storageInterceptorProvider)
        {
        }
    }
}