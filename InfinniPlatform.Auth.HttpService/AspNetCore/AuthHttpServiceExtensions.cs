using InfinniPlatform.Auth;
using InfinniPlatform.Auth.HttpService;
using InfinniPlatform.Auth.HttpService.IoC;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable once CheckNamespace

namespace InfinniPlatform.AspNetCore
{
    public static class AuthHttpServiceExtensions
    {
        public static IServiceCollection AddAuthHttpService(this IServiceCollection services)
        {
            var options = AuthHttpServiceOptions.Default;

            return services.AddSingleton(provider => new AuthHttpServiceContainerModule<AppUser>(options));
        }

        public static IServiceCollection AddAuthHttpService(this IServiceCollection services, IConfigurationRoot configuration)
        {
            var options = configuration.GetSection(AuthHttpServiceOptions.SectionName).Get<AuthHttpServiceOptions>();

            return services.AddSingleton(provider => new AuthHttpServiceContainerModule<AppUser>(options));
        }

        public static IServiceCollection AddAuthHttpService(this IServiceCollection services, AuthHttpServiceOptions options)
        {
            return services.AddSingleton(provider => new AuthHttpServiceContainerModule<AppUser>(options));
        }

        public static IServiceCollection AddAuthHttpService<TUser>(this IServiceCollection services) where TUser : AppUser
        {
            var options = AuthHttpServiceOptions.Default;

            return services.AddSingleton(provider => new AuthHttpServiceContainerModule<TUser>(options));
        }

        public static IServiceCollection AddAuthHttpService<TUser>(this IServiceCollection services, IConfigurationRoot configuration) where TUser : AppUser
        {
            var options = configuration.GetSection(AuthHttpServiceOptions.SectionName).Get<AuthHttpServiceOptions>();

            return services.AddSingleton(provider => new AuthHttpServiceContainerModule<TUser>(options));
        }

        public static IServiceCollection AddAuthHttpService<TUser>(this IServiceCollection services, AuthHttpServiceOptions options) where TUser : AppUser
        {
            return services.AddSingleton(provider => new AuthHttpServiceContainerModule<TUser>(options));
        }
    }
}