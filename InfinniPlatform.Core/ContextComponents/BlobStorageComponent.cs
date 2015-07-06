using InfinniPlatform.Sdk.ContextComponents;
using InfinniPlatform.Sdk.Environment;
using InfinniPlatform.Sdk.Environment.Binary;

namespace InfinniPlatform.ContextComponents
{
    public sealed class BlobStorageComponent : IBlobStorageComponent
    {
        private readonly IBlobStorage _blobStorage;

        public BlobStorageComponent(IBlobStorage blobStorage)
        {
            _blobStorage = blobStorage;
        }

        public IBlobStorage GetBlobStorage()
        {
            return _blobStorage;
        }
    }
}