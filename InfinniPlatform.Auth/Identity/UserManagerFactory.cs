using System;
using System.Collections.Generic;
using System.Linq;
using InfinniPlatform.IoC;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace InfinniPlatform.Auth.Identity
{
    internal class UserManagerFactory : IUserManagerFactory
    {
        private readonly IContainerResolver _containerResolver;

        public UserManagerFactory(IContainerResolver containerResolver)
        {
            _containerResolver = containerResolver;
        }

        public UserManager<TUser> GetUserManager<TUser>() where TUser : AppUser
        {
            // ��������� �������������
            var appUserStoreFactory = _containerResolver.Resolve<IUserStoreFactory>();
            var appUserStore = appUserStoreFactory.GetUserStore<TUser>();

            // ��������� �������� AspNet.Identity
            var optionsAccessor = new OptionsWrapper<IdentityOptions>(new IdentityOptions());

            // ������ ����������� �������
            var identityPasswordHasher = new DefaultAppUserPasswordHasher<TUser>();

            // ���������� ������ � �������������
            var userValidators = new List<IUserValidator<TUser>> {new AppUserValidator<TUser>(appUserStore)};

            // ��������� ������� �������������
            var passwordValidators = Enumerable.Empty<IPasswordValidator<TUser>>();

            // ������������
            var keyNormalizer = new UpperInvariantLookupNormalizer();

            // ������ ��������� ������ AspNet.Identity
            var identityErrorDescriber = new IdentityErrorDescriber();

            // ��������� ������������������ � IoC ��������
            var serviceProvider = _containerResolver.Resolve<IServiceProvider>();

            // ������
            var logger = _containerResolver.Resolve<ILogger<UserManager<TUser>>>();

            var userManager = new UserManager<TUser>(appUserStore,
                                                     optionsAccessor,
                                                     identityPasswordHasher,
                                                     userValidators,
                                                     passwordValidators,
                                                     keyNormalizer,
                                                     identityErrorDescriber,
                                                     serviceProvider,
                                                     logger);

            return userManager;
        }
    }
}