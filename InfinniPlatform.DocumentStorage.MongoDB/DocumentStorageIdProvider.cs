using InfinniPlatform.DocumentStorage.Abstractions;
using InfinniPlatform.Sdk.Dynamic;

namespace InfinniPlatform.DocumentStorage.MongoDB
{
    internal class DocumentStorageIdProvider : IDocumentStorageIdProvider
    {
        public DocumentStorageIdProvider(IDocumentIdGenerator idGenerator)
        {
            _idGenerator = idGenerator;
        }


        private readonly IDocumentIdGenerator _idGenerator;


        public void SetDocumentId(DynamicWrapper document)
        {
            if (document["_id"] == null)
            {
                document["_id"] = _idGenerator.NewId();
            }
        }

        public void SetDocumentId<TDocument>(TDocument document) where TDocument : Document
        {
            if (document._id == null)
            {
                document._id = _idGenerator.NewId();
            }
        }
    }
}