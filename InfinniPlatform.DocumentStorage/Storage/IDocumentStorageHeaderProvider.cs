using System;

using InfinniPlatform.Sdk.Documents;
using InfinniPlatform.Sdk.Dynamic;

namespace InfinniPlatform.DocumentStorage.Storage
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