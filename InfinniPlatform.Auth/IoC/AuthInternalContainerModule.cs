﻿using System;
using System.Collections.Generic;
using System.Linq;
using InfinniPlatform.Auth.DocumentStorage;
using InfinniPlatform.Auth.Middlewares;
using InfinniPlatform.Auth.UserCache;
using InfinniPlatform.Auth.Validators;
using InfinniPlatform.DocumentStorage.Metadata;
using InfinniPlatform.Http.Middlewares;
using InfinniPlatform.IoC;
using InfinniPlatform.MessageQueue;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace InfinniPlatform.Auth.IoC
{
    public class AuthInternalContainerModule<TUser> : IContainerModule where TUser : AppUser
    {
        private readonly AuthOptions _options;

        public AuthInternalContainerModule(AuthOptions options)
        {
            _options = options;
        }

        public void Load(IContainerBuilder builder)
        {
            builder.RegisterInstance(_options).AsSelf().SingleInstance();

            // User storage

            builder.RegisterType<UserStore<TUser>>()
                   .AsSelf()
                   .SingleInstance();

            builder.RegisterFactory(CreateUserStore)
                   .As<IUserStore<TUser>>()
                   .SingleInstance();

            // Role storage

            builder.RegisterType<RoleStore<AppUserRole>>()
                   .As<IRoleStore<AppUserRole>>()
                   .SingleInstance();

            // User manager

            builder.RegisterFactory(CreateOptionsAccessor)
                   .As<IOptions<IdentityOptions>>()
                   .SingleInstance();

            builder.RegisterFactory(CreatePasswordHasher)
                   .As<IPasswordHasher<TUser>>()
                   .SingleInstance();

            builder.RegisterFactory(CreateLookupNormalizer)
                   .As<ILookupNormalizer>()
                   .SingleInstance();

            builder.RegisterFactory(CreateIdentityErrorDescriber)
                   .As<IdentityErrorDescriber>()
                   .SingleInstance();

            if (_options.UserValidatorsFactory != null)
            {
                builder.RegisterFactory(_options.UserValidatorsFactory.Get<TUser>)
                       .As<IEnumerable<IUserValidator<TUser>>>()
                       .SingleInstance();
            }

            if (_options.PasswordValidatorsFactory != null)
            {
                builder.RegisterFactory(_options.PasswordValidatorsFactory.Get<TUser>)
                       .As<IEnumerable<IUserValidator<TUser>>>()
                       .SingleInstance();
            }

            // Middlewares

            builder.RegisterType<AuthInternalHttpMiddleware>()
                   .As<IHttpMiddleware>()
                   .SingleInstance();

            builder.RegisterType<AuthCookieHttpMiddleware>()
                   .As<IHttpMiddleware>()
                   .SingleInstance();

            // UserStorage

            builder.RegisterType<UserCache<AppUser>>()
                   .As<IUserCacheObserver>()
                   .AsSelf()
                   .SingleInstance();

            builder.RegisterType<AppUserStoreCacheConsumer>()
                   .AsSelf()
                   .SingleInstance();

            builder.RegisterType<AuthInternalConsumerSource>()
                   .As<IConsumerSource>()
                   .SingleInstance();

            builder.RegisterType<AuthInternalDocumentMetadataSource>()
                   .As<IDocumentMetadataSource>()
                   .SingleInstance();
        }


        private IUserStore<TUser> CreateUserStore(IContainerResolver resolver)
        {
            return _options.UserStoreFactory?.Get<TUser>(resolver) ?? resolver.Resolve<UserStore<TUser>>();
        }


        private IOptions<IdentityOptions> CreateOptionsAccessor(IContainerResolver resolver)
        {
            return _options.IdentityOptions ?? new OptionsWrapper<IdentityOptions>(new IdentityOptions());
        }

        private IPasswordHasher<TUser> CreatePasswordHasher(IContainerResolver resolver)
        {
            return _options.PasswordHasherFactory?.Get<TUser>(resolver) ?? new DefaultAppUserPasswordHasher<TUser>();
        }

        private ILookupNormalizer CreateLookupNormalizer(IContainerResolver resolver)
        {
            return _options.LookupNormalizerFactory?.Get(resolver) ?? new UpperInvariantLookupNormalizer();
        }

        private IdentityErrorDescriber CreateIdentityErrorDescriber(IContainerResolver resolver)
        {
            return _options.IdentityErrorDescriber?.Get(resolver) ?? new IdentityErrorDescriber();
        }
    }
}