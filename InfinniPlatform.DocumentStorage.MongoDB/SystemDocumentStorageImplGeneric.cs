using System.Diagnostics;

namespace InfinniPlatform.DocumentStorage
{
    [DebuggerDisplay("DocumentType = {" + nameof(DocumentType) + "}")]
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