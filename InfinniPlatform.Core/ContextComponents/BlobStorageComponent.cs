using InfinniPlatform.Factories;
using InfinniPlatform.Sdk.ContextComponents;
using InfinniPlatform.Sdk.Environment.Binary;

namespace InfinniPlatform.ContextComponents
{
    public sealed class BlobStorageComponent : IBlobStorageComponent
    {
        public BlobStorageComponent(IBlobStorageFactory blobStorageFactory)
        {
            _blobStorage = blobStorageFactory.CreateBlobStorage();
        }

        private readonly IBlobStorage _blobStorage;

        public IBlobStorage GetBlobStorage()
        {
            return _blobStorage;
        }
    }
}