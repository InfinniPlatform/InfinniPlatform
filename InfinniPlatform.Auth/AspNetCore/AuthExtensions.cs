﻿using System;

using InfinniPlatform.Auth;
using InfinniPlatform.Auth.IoC;

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable once CheckNamespace
namespace InfinniPlatform.AspNetCore
{
    /// <summary>
    /// Extension methods for authentication dependencies registration.
    /// </summary>
    public static class AuthExtensions
    {
        /// <summary>
        /// Register authentication services for <see cref="AppUser"/> and <see cref="AppUserRole"/>.
        /// </summary>
        /// <param name="services">Collection of registered services.</param>
        /// <returns>Service collection for further services registration.</returns>
        public static IServiceCollection AddAuthInternal(this IServiceCollection services)
        {
            var options = AuthOptions.Default;

            return AddAuthInternal<AppUser, AppUserRole>(services, options);
        }

        /// <summary>
        /// Register authentication services for <see cref="AppUser"/> and <see cref="AppUserRole"/>.
        /// </summary>
        /// <param name="services">Collection of registered services.</param>
        /// <param name="configuration">Configuration properties set.</param>
        /// <param name="callback">Function for options customization.</param>
        /// <returns>Service collection for further services registration.</returns>
        public static IServiceCollection AddAuthInternal(this IServiceCollection services, IConfiguration configuration, Action<AuthOptions> callback = null)
        {
            var options = AuthOptions.Default;
            
            configuration.GetSection(options.SectionName).Bind(options);

            callback?.Invoke(options);

            return AddAuthInternal<AppUser, AppUserRole>(services, options);
        }

        /// <summary>
        /// Register authentication services for <see cref="AppUser"/> and <see cref="AppUserRole"/>.
        /// </summary>
        /// <param name="services">Collection of registered services.</param>
        /// <param name="options">Authentication options.</param>
        /// <returns>Service collection for further services registration.</returns>
        public static IServiceCollection AddAuthInternal(this IServiceCollection services, AuthOptions options)
        {
            return AddAuthInternal<AppUser, AppUserRole>(services, options);
        }

        /// <summary>
        /// Register authentication services for custom user and role types.
        /// </summary>
        /// <typeparam name="TUser">User type.</typeparam>
        /// <typeparam name="TRole">Role type.</typeparam>
        /// <param name="services">Collection of registered services.</param>
        /// <returns>Service collection for further services registration.</returns>
        public static IServiceCollection AddAuthInternal<TUser, TRole>(this IServiceCollection services) where TUser : AppUser where TRole : AppUserRole
        {
            var options = AuthOptions.Default;

            return AddAuthInternal<TUser, TRole>(services, options);
        }

        /// <summary>
        /// Register authentication services for custom user and role types.
        /// </summary>
        /// <typeparam name="TUser">User type.</typeparam>
        /// <typeparam name="TRole">Role type.</typeparam>
        /// <param name="services">Collection of registered services.</param>
        /// <param name="configuration">Configuration properties set.</param>
        /// <param name="callback"></param>
        /// <returns>Service collection for further services registration.</returns>
        public static IServiceCollection AddAuthInternal<TUser, TRole>(this IServiceCollection services, IConfiguration configuration, Action<AuthOptions> callback = null) where TUser : AppUser where TRole : AppUserRole
        {
            var options = AuthOptions.Default;

            configuration.GetSection(options.SectionName).Bind(options);

            callback?.Invoke(options);

            return AddAuthInternal<TUser, TRole>(services, options);
        }

        /// <summary>
        /// Register authentication services for custom user and role types.
        /// </summary>
        /// <typeparam name="TUser">User type.</typeparam>
        /// <typeparam name="TRole">Role type.</typeparam>
        /// <param name="services">Collection of registered services.</param>
        /// <param name="options">Authentication options.</param>
        /// <returns>Service collection for further services registration.</returns>
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

        /// <summary>
        /// Adds authentication middleware to application pipeline.
        /// </summary>
        /// <param name="app">Application builder.</param>
        /// <returns>Application builder for further usage.</returns>
        public static IApplicationBuilder UseAuthInternal(this IApplicationBuilder app)
        {
            return app.UseAuthentication();
        }
    }
}