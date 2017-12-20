using System;
using InfinniPlatform.Auth;
using InfinniPlatform.Auth.IoC;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable once CheckNamespace

namespace InfinniPlatform.AspNetCore
{
    public static class AuthExtensions
    {
        public static IServiceCollection AddAuthInternal(this IServiceCollection services)
        {
            var options = AuthOptions.Default;

            return AddAuthInternal<AppUser, AppUserRole>(services, options);
        }

        public static IServiceCollection AddAuthInternal(this IServiceCollection services, IConfiguration configuration, Action<AuthOptions> callback = null)
        {
            var options = configuration.GetSection(AuthOptions.SectionName)
                                       .Get<AuthOptions>();

            callback?.Invoke(options);

            return AddAuthInternal<AppUser, AppUserRole>(services, options);
        }

        public static IServiceCollection AddAuthInternal(this IServiceCollection services, AuthOptions options)
        {
            return AddAuthInternal<AppUser, AppUserRole>(services, options);
        }

        public static IServiceCollection AddAuthInternal<TUser, TRole>(this IServiceCollection services) where TUser : AppUser where TRole : AppUserRole
        {
            var options = AuthOptions.Default;

            return AddAuthInternal<TUser, TRole>(services, options);
        }

        public static IServiceCollection AddAuthInternal<TUser, TRole>(this IServiceCollection services, IConfiguration configuration, Action<AuthOptions> callback = null) where TUser : AppUser where TRole : AppUserRole
        {
            var options = configuration.GetSection(AuthOptions.SectionName)
                                       .Get<AuthOptions>();

            callback?.Invoke(options);

            return AddAuthInternal<TUser, TRole>(services, options);
        }

        public static IServiceCollection AddAuthInternal<TUser, TRole>(this IServiceCollection services, AuthOptions options) where TUser : AppUser where TRole : AppUserRole
        {
            if (options == null)
            {
                options = AuthOptions.Default;
            }

            services.AddIdentity<TUser, TRole>(options.IdentityOptions);
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                    // TODO Move configuration action to parameters?
                    .AddCookie(opt => opt.Cookie.Domain = options.CookieDomain);

            return services.AddSingleton(provider => new AuthContainerModule<TUser>(options));
        }
    }
}