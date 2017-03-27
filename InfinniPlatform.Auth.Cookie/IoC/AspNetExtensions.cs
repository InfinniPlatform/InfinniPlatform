using InfinniPlatform.Auth.Cookie.IoC;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable once CheckNamespace

namespace InfinniPlatform.Extensions
{
    public static class AspNetExtensions
    {
        public static IServiceCollection AddAuthCookie(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton(provider => new AuthCookieContainerModule());
            return serviceCollection;
        }
    }
}