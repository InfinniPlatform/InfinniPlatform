using InfinniPlatform.Auth.Facebook.IoC;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable once CheckNamespace

namespace InfinniPlatform.Extensions
{
    public static class AspNetExtensions
    {
        public static IServiceCollection AddInfFacebookAuthentication(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton(provider => new AuthFacebookContainerModule());
            return serviceCollection;
        }
    }
}