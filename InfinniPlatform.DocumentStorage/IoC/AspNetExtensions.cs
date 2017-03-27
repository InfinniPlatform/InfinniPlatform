using InfinniPlatform.DocumentStorage.IoC;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable once CheckNamespace

namespace InfinniPlatform.Extensions
{
    public static class AspNetExtensions
    {
        /// <summary>
        /// Регистрирует сервисы хранилища документов.
        /// </summary>
        /// <param name="serviceCollection">Коллекция зарегистрированных сервисов.</param>
        public static IServiceCollection AddDocumentStorage(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton(provider => new DocumentStorageContainerModule());
            return serviceCollection;
        }
    }
}