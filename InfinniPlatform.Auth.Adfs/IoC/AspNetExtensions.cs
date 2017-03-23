using InfinniPlatform.Auth.Adfs.IoC;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable once CheckNamespace

namespace InfinniPlatform.Extensions
{
    public static class AspNetExtensions
    {
        public static IServiceCollection AddInfAdfsAuthentication(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton(provider => new AuthAdfsContainerModule());
            return serviceCollection;
        }
    }
}