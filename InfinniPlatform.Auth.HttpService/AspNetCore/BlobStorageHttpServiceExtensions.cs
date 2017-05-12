using InfinniPlatform.Auth.HttpService.IoC;
using InfinniPlatform.Auth.Identity;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable once CheckNamespace

namespace InfinniPlatform.AspNetCore
{
    public static class BlobStorageHttpServiceExtensions
    {
        public static IServiceCollection AddAuthHttpService<TUser>(this IServiceCollection services) where TUser : AppUser
        {
            return services.AddSingleton(provider => new AuthHttpServiceContainerModule<TUser>());
        }
    }
}