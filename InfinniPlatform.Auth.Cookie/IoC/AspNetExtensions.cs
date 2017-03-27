using InfinniPlatform.Auth.Cookie.IoC;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable once CheckNamespace

namespace InfinniPlatform.Extensions
{
    public static class AspNetExtensions
    {
        /// <summary>
        /// Регистрирует сервисы провайдера cookie-аутентификации.
        /// </summary>
        /// <param name="serviceCollection">Коллекция зарегистрированных сервисов.</param>
        public static IServiceCollection AddAuthCookie(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton(provider => new AuthCookieContainerModule());
            return serviceCollection;
        }
    }
}