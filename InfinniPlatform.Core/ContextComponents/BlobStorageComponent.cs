using InfinniPlatform.Sdk.ContextComponents;
using InfinniPlatform.Sdk.Environment.Binary;

namespace InfinniPlatform.Core.ContextComponents
{
    public sealed class BlobStorageComponent : IBlobStorageComponent
    {
        public BlobStorageComponent(IBlobStorage blobStorage)
        {
            _blobStorage = blobStorage;
        }

        private readonly IBlobStorage _blobStorage;

        public IBlobStorage GetBlobStorage()
        {
            return _blobStorage;
        }
    }
}