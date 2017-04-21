using System;

using InfinniPlatform.Dynamic;

namespace InfinniPlatform.DocumentStorage
{
    internal interface IDocumentStorageHeaderProvider
    {
        void SetInsertHeader(DynamicWrapper document);

        void SetInsertHeader<TDocument>(TDocument document) where TDocument : Document;


        void SetReplaceHeader(DynamicWrapper document);

        void SetReplaceHeader<TDocument>(TDocument document) where TDocument : Document;


        Action<IDocumentUpdateBuilder> SetUpdateHeader(Action<IDocumentUpdateBuilder> update);

        Action<IDocumentUpdateBuilder<TDocument>> SetUpdateHeader<TDocument>(Action<IDocumentUpdateBuilder<TDocument>> update) where TDocument : Document;


        Action<IDocumentUpdateBuilder> SetDeleteHeader();

        Action<IDocumentUpdateBuilder<TDocument>> SetDeleteHeader<TDocument>() where TDocument : Document;
    }
}