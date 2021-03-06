﻿using System.Collections.Generic;
using InfinniPlatform.Auth.DocumentStorage;
using InfinniPlatform.Auth.UserCache;
using InfinniPlatform.DocumentStorage.Metadata;
using InfinniPlatform.IoC;
using InfinniPlatform.MessageQueue;
using Microsoft.AspNetCore.Identity;

namespace InfinniPlatform.Auth.IoC
{
    /// <summary>
    /// Container module for authentication services.
    /// </summary>
    /// <typeparam name="TUser">User type.</typeparam>
    public class AuthContainerModule<TUser> : IContainerModule where TUser : AppUser
    {
        private readonly AuthOptions _options;

        /// <summary>
        /// Initializes a new instance of <see cref="AuthContainerModule{TUser}" />.
        /// </summary>
        /// <param name="options">Auth configuration options.</param>
        public AuthContainerModule(AuthOptions options)
        {
            _options = options;
        }

        /// <inheritdoc />
        public void Load(IContainerBuilder builder)
        {
            builder.RegisterInstance(_options).AsSelf().SingleInstance();

            // User storage

            builder.RegisterType<UserStore<TUser>>()
                   .As<IUserPhoneNumberStoreExtended<TUser>>()
                   .AsSelf()
                   .SingleInstance();

            builder.RegisterFactory(CreateUserStore)
                   .As<IUserStore<TUser>>()
                   .SingleInstance();

            builder.RegisterType<UserCache<AppUser>>()
                   .As<IUserCacheObserver>()
                   .AsSelf()
                   .SingleInstance();

            // Role storage

            builder.RegisterType<RoleStore<AppUserRole>>()
                   .As<IRoleStore<AppUserRole>>()
                   .SingleInstance();

            // User manager

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

            builder.RegisterType<UserCacheConsumer>()
                   .AsSelf()
                   .SingleInstance();

            builder.RegisterType<AuthConsumerSource>()
                   .As<IConsumerSource>()
                   .SingleInstance();

            builder.RegisterType<AuthDocumentMetadataSource>()
                   .As<IDocumentMetadataSource>()
                   .SingleInstance();
        }


        private IUserStore<TUser> CreateUserStore(IContainerResolver resolver)
        {
            return _options.UserStoreFactory?.Get<TUser>(resolver) ?? resolver.Resolve<UserStore<TUser>>();
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