using InfinniPlatform.Auth.Identity.MongoDb;
using InfinniPlatform.Auth.IoC;

using Microsoft.Extensions.DependencyInjection;

// ReSharper disable once CheckNamespace

namespace InfinniPlatform.AspNetCore
{
    public static class AuthInternalExtensions
    {
        public static IServiceCollection AddAuth(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddIdentity<IdentityUser, IdentityRole>();
            serviceCollection.AddSingleton(provider => new AuthInternalContainerModule());

            return serviceCollection;
        }
    }
}