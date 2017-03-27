using InfinniPlatform.Auth.Google.IoC;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable once CheckNamespace

namespace InfinniPlatform.Extensions
{
    public static class AspNetExtensions
    {
        /// <summary>
        /// Регистрирует сервисы провайдера аутентификации Google.
        /// </summary>
        /// <param name="serviceCollection">Коллекция зарегистрированных сервисов.</param>
        public static IServiceCollection AddAuthGoogle(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton(provider => new AuthGoogleContainerModule());
            return serviceCollection;
        }
    }
}