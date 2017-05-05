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
            var options = AuthInternalOptions.Default;

            return AddAuthInternal<TUser, TRole>(services, options);
        }

        public static IServiceCollection AddAuthInternal<TUser, TRole>(this IServiceCollection services, IConfigurationRoot configuration, Action<AuthInternalOptions> callback = null) where TUser : AppUser where TRole : AppUserRole
        {
            var options = configuration.GetSection(AuthInternalOptions.SectionName)
                                       .Get<AuthInternalOptions>();

            callback?.Invoke(options);

            return AddAuthInternal<TUser, TRole>(services, options);
        }

        public static IServiceCollection AddAuthInternal<TUser, TRole>(this IServiceCollection services, AuthInternalOptions options) where TUser : AppUser where TRole : AppUserRole
        {
            services.AddIdentity<TUser, TRole>();

            return services.AddSingleton(provider => new AuthInternalContainerModule(options ?? AuthInternalOptions.Default));
        }
    }
}