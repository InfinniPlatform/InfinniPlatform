using InfinniPlatform.DocumentStorage.Interceptors;

namespace InfinniPlatform.DocumentStorage
{
    internal interface IDocumentStorageInterceptorProvider
    {
        IDocumentStorageInterceptor GetInterceptor(string documentType);

        IDocumentStorageInterceptor<TDocument> GetInterceptor<TDocument>(string documentType) where TDocument : Document;
    }
}