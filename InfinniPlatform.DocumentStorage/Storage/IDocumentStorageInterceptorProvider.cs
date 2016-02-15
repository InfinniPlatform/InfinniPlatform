using InfinniPlatform.Sdk.Documents.Interceptors;

namespace InfinniPlatform.DocumentStorage.Storage
{
    internal interface IDocumentStorageInterceptorProvider
    {
        IDocumentStorageInterceptor GetInterceptor(string documentType);

        IDocumentStorageInterceptor<TDocument> GetInterceptor<TDocument>(string documentType);
    }
}