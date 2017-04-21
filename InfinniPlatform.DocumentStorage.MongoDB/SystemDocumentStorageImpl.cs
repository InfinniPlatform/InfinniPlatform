using System.Diagnostics;

namespace InfinniPlatform.DocumentStorage
{
    [DebuggerDisplay("DocumentType = {" + nameof(DocumentType) + "}")]
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