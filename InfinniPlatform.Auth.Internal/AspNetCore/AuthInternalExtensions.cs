using InfinniPlatform.Auth;
using InfinniPlatform.Auth.Identity.MongoDb;
using InfinniPlatform.Auth.IoC;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable once CheckNamespace

namespace InfinniPlatform.AspNetCore
{
    public static class AuthInternalExtensions
    {
        public static IServiceCollection AddAuthInternal(this IServiceCollection services)
        {
            var options = AuthInternalOptions.Default;

            return AddAuthInternal(services, options);
        }

        public static IServiceCollection AddAuthInternal(this IServiceCollection services, IConfigurationRoot configuration)
        {
            var options = configuration.GetSection(AuthInternalOptions.SectionName).Get<AuthInternalOptions>();

            return AddAuthInternal(services, options);
        }

        public static IServiceCollection AddAuthInternal(this IServiceCollection services, AuthInternalOptions options)
        {
            services.AddIdentity<IdentityUser, IdentityRole>();

            return services.AddSingleton(provider => new AuthInternalContainerModule(options ?? AuthInternalOptions.Default));
        }
    }
}