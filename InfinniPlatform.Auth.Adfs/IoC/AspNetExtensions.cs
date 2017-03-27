using InfinniPlatform.Auth.Adfs.IoC;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable once CheckNamespace

namespace InfinniPlatform.Extensions
{
    public static class AspNetExtensions
    {
        /// <summary>
        /// Регистрирует сервисы провайдера аутентификации Active Directory.
        /// </summary>
        /// <param name="serviceCollection">Коллекция зарегистрированных сервисов.</param>
        public static IServiceCollection AddAuthAdfs(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton(provider => new AuthAdfsContainerModule());
            return serviceCollection;
        }
    }
}