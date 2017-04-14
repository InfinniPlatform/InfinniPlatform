using System.Diagnostics;

using InfinniPlatform.DocumentStorage.Abstractions;

namespace InfinniPlatform.DocumentStorage.MongoDB
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