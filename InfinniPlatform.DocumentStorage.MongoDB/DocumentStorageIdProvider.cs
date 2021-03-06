﻿using InfinniPlatform.Dynamic;

namespace InfinniPlatform.DocumentStorage
{
    internal class DocumentStorageIdProvider : IDocumentStorageIdProvider
    {
        public DocumentStorageIdProvider(IDocumentIdGenerator idGenerator)
        {
            _idGenerator = idGenerator;
        }


        private readonly IDocumentIdGenerator _idGenerator;


        public void SetDocumentId(DynamicDocument document)
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