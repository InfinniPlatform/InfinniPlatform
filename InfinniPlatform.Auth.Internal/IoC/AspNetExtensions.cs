using InfinniPlatform.Auth.Internal.IoC;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable once CheckNamespace

namespace InfinniPlatform.Extensions
{
    public static class AspNetExtensions
    {
        public static IServiceCollection AddAuth(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton(provider => new AuthInternalContainerModule());
            return serviceCollection;
        }
    }
}