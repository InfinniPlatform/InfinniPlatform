﻿using InfinniPlatform.Auth;
using InfinniPlatform.Auth.HttpService;
using InfinniPlatform.Auth.HttpService.IoC;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable once CheckNamespace

namespace InfinniPlatform.AspNetCore
{
    public static class AuthHttpServiceExtensions
    {
        public static IServiceCollection AddAuthHttpService(this IServiceCollection services, IMvcBuilder builder)
        {
            var options = AuthHttpServiceOptions.Default;

            builder.ConfigureApplicationPartManager(manager => manager.FeatureProviders.Add(new AuthControllersFeatureProvider<AppUser>()));

            return services.AddSingleton(provider => new AuthHttpServiceContainerModule<AppUser>(options));
        }

        public static IServiceCollection AddAuthHttpService(IServiceCollection services, IMvcBuilder builder, IConfiguration configuration)
        {
            var options = configuration.GetSection(AuthHttpServiceOptions.SectionName).Get<AuthHttpServiceOptions>();

            builder.ConfigureApplicationPartManager(manager => manager.FeatureProviders.Add(new AuthControllersFeatureProvider<AppUser>()));

            return services.AddSingleton(provider => new AuthHttpServiceContainerModule<AppUser>(options));
        }

        public static IServiceCollection AddAuthHttpService(IServiceCollection services, IMvcBuilder builder, AuthHttpServiceOptions options)
        {
            builder.ConfigureApplicationPartManager(manager => manager.FeatureProviders.Add(new AuthControllersFeatureProvider<AppUser>()));

            return services.AddSingleton(provider => new AuthHttpServiceContainerModule<AppUser>(options));
        }

        public static IServiceCollection AddAuthHttpService<TUser>(this IServiceCollection services, IMvcBuilder builder) where TUser : AppUser
        {
            var options = AuthHttpServiceOptions.Default;

            builder.ConfigureApplicationPartManager(manager => manager.FeatureProviders.Add(new AuthControllersFeatureProvider<TUser>()));

            return services.AddSingleton(provider => new AuthHttpServiceContainerModule<TUser>(options));
        }

        public static IServiceCollection AddAuthHttpService<TUser>(IServiceCollection services, IMvcBuilder builder, IConfiguration configuration) where TUser : AppUser
        {
            var options = configuration.GetSection(AuthHttpServiceOptions.SectionName).Get<AuthHttpServiceOptions>();

            builder.ConfigureApplicationPartManager(manager => manager.FeatureProviders.Add(new AuthControllersFeatureProvider<TUser>()));

            return services.AddSingleton(provider => new AuthHttpServiceContainerModule<TUser>(options));
        }

        public static IServiceCollection AddAuthHttpService<TUser>(IServiceCollection services, IMvcBuilder builder, AuthHttpServiceOptions options) where TUser : AppUser
        {
            builder.ConfigureApplicationPartManager(manager => manager.FeatureProviders.Add(new AuthControllersFeatureProvider<TUser>()));

            return services.AddSingleton(provider => new AuthHttpServiceContainerModule<TUser>(options));
        }
    }
}