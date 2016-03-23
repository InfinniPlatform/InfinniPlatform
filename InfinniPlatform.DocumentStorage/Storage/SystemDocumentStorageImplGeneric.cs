using InfinniPlatform.Core.Documents;
using InfinniPlatform.Sdk.Documents;

namespace InfinniPlatform.DocumentStorage.Storage
{
    internal class SystemDocumentStorageImpl<TDocument> : DocumentStorageImpl<TDocument>, ISystemDocumentStorage<TDocument> where TDocument : Document
    {
        public SystemDocumentStorageImpl(IDocumentStorageProviderFactory storageProviderFactory,
                                         IDocumentStorageIdProvider storageIdProvider,
                                         ISystemDocumentStorageHeaderProvider storageHeaderProvider,
                                         ISystemDocumentStorageFilterProvider systemStorageFilterProvider,
                                         IDocumentStorageInterceptorProvider storageInterceptorProvider,
                                         string documentType = null)
            : base(storageProviderFactory, storageIdProvider, storageHeaderProvider, systemStorageFilterProvider, storageInterceptorProvider, documentType)
        {
        }
    }
}