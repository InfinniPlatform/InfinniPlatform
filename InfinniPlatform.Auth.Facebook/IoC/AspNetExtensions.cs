using InfinniPlatform.Auth.Facebook.IoC;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable once CheckNamespace

namespace InfinniPlatform.Extensions
{
    public static class AspNetExtensions
    {
        /// <summary>
        /// Регистрирует сервисы провайдера аутентификации Facebook.
        /// </summary>
        /// <param name="serviceCollection">Коллекция зарегистрированных сервисов.</param>
        public static IServiceCollection AddAuthFacebook(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton(provider => new AuthFacebookContainerModule());
            return serviceCollection;
        }
    }
}