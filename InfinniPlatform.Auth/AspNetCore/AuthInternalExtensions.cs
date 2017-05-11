using System;
using InfinniPlatform.Auth;
using InfinniPlatform.Auth.Identity;
using InfinniPlatform.Auth.IoC;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable once CheckNamespace

namespace InfinniPlatform.AspNetCore
{
    public static class AuthInternalExtensions
    {
        public static IServiceCollection AddAuthInternal<TUser, TRole>(this IServiceCollection services) where TUser : AppUser where TRole : AppUserRole
        {
            var options = AuthOptions.Default;

            return AddAuthInternal<TUser, TRole>(services, options);
        }

        public static IServiceCollection AddAuthInternal<TUser, TRole>(this IServiceCollection services, IConfigurationRoot configuration, Action<AuthOptions> callback = null) where TUser : AppUser where TRole : AppUserRole
        {
            var options = configuration.GetSection(AuthOptions.SectionName)
                                       .Get<AuthOptions>();

            callback?.Invoke(options);

            return AddAuthInternal<TUser, TRole>(services, options);
        }

        public static IServiceCollection AddAuthInternal<TUser, TRole>(this IServiceCollection services, AuthOptions options) where TUser : AppUser where TRole : AppUserRole
        {
            services.AddIdentity<TUser, TRole>();

            return services.AddSingleton(provider => new AuthInternalContainerModule(options ?? AuthOptions.Default));
        }
    }
}