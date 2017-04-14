using InfinniPlatform.DocumentStorage.Abstractions;
using InfinniPlatform.DocumentStorage.Abstractions.Interceptors;

namespace InfinniPlatform.DocumentStorage.MongoDB
{
    internal interface IDocumentStorageInterceptorProvider
    {
        IDocumentStorageInterceptor GetInterceptor(string documentType);

        IDocumentStorageInterceptor<TDocument> GetInterceptor<TDocument>(string documentType) where TDocument : Document;
    }
}