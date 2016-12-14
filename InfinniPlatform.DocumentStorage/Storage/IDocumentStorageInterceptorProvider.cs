using InfinniPlatform.DocumentStorage.Contract;
using InfinniPlatform.DocumentStorage.Contract.Interceptors;

namespace InfinniPlatform.DocumentStorage.Storage
{
    internal interface IDocumentStorageInterceptorProvider
    {
        IDocumentStorageInterceptor GetInterceptor(string documentType);

        IDocumentStorageInterceptor<TDocument> GetInterceptor<TDocument>(string documentType) where TDocument : Document;
    }
}