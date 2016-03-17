using System;

using InfinniPlatform.DocumentStorage.Services.QuerySyntax;
using InfinniPlatform.Sdk.Documents.Services;
using InfinniPlatform.Sdk.Serialization;
using InfinniPlatform.Sdk.Services;

namespace InfinniPlatform.DocumentStorage.Services.QueryFactories
{
    public sealed class DocumentQueryFactory : DocumentQueryFactoryBase, IDocumentQueryFactory
    {
        public DocumentQueryFactory(IQuerySyntaxTreeParser syntaxTreeParser, IJsonObjectSerializer objectSerializer) : base(syntaxTreeParser, objectSerializer)
        {
        }

        public DocumentGetQuery CreateGetQuery(IHttpRequest request)
        {
            throw new NotImplementedException();
        }

        public DocumentPostQuery CreatePostQuery(IHttpRequest request, string documentFormKey)
        {
            throw new NotImplementedException();
        }
    }
}