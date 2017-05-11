using InfinniPlatform.Auth.Identity;
using InfinniPlatform.Auth.Identity.DocumentStorage;
using InfinniPlatform.Auth.Identity.UserCache;
using InfinniPlatform.Auth.Middlewares;
using InfinniPlatform.Auth.Services;
using InfinniPlatform.DocumentStorage.Metadata;
using InfinniPlatform.Http.Middlewares;
using InfinniPlatform.IoC;
using InfinniPlatform.MessageQueue;
using Microsoft.AspNetCore.Identity;

namespace InfinniPlatform.Auth.IoC
{
    public class AuthInternalContainerModule : IContainerModule
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

            builder.RegisterType<UserStoreFactory>()
                   .As<IUserStoreFactory>()
                   .SingleInstance();

            // Role storage

            builder.RegisterType<RoleStore<AppUserRole>>()
                   .As<IRoleStore<AppUserRole>>()
                   .SingleInstance();

            // User manager

            builder.RegisterType<UserManagerFactory>()
                   .As<IUserManagerFactory>()
                   .SingleInstance();

            // Middlewares

            builder.RegisterType<AuthInternalHttpMiddleware>()
                   .As<IHttpMiddleware>()
                   .SingleInstance();

            builder.RegisterType<AuthCookieHttpMiddleware>()
                   .As<IHttpMiddleware>()
                   .SingleInstance();

            // Services

            // TODO Generic AuthInternalHttpService?
            //builder.RegisterType<AuthInternalHttpService<AppCustomUser>>()
            //       .As<IHttpService>()
            //       .SingleInstance();

            builder.RegisterType<UserEventHandlerInvoker>()
                   .AsSelf()
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


        //private object CreateUserStore(IContainerResolver resolver)
        //{
        //    return _options.UserStoreFactory.GetUserStore<AppUser>(resolver) ?? resolver.Resolve<UserStore<AppUser>>();
        //}

        //private static UserManager<AppUser> CreateUserManager(IContainerResolver resolver)
        //{
        //    // Хранилище пользователей
        //    var appUserStore = resolver.Resolve<IUserStore<AppUser>>();

        //    // Провайдер настроек AspNet.Identity
        //    var optionsAccessor = new OptionsWrapper<IdentityOptions>(new IdentityOptions());

        //    // Сервис хэширования паролей
        //    var identityPasswordHasher = new DefaultAppUserPasswordHasher();

        //    // Валидаторы данных о пользователях
        //    var userValidators = new List<IUserValidator<AppUser>> { new AppUserValidator(appUserStore) };

        //    // Валидатор паролей пользователей
        //    var passwordValidators = Enumerable.Empty<IPasswordValidator<AppUser>>();

        //    // Нормализатор
        //    var keyNormalizer = new UpperInvariantLookupNormalizer();

        //    // Сервис обработки ошибок AspNet.Identity
        //    var identityErrorDescriber = new IdentityErrorDescriber();

        //    // Провайдер зарегистрированных в IoC сервисов
        //    var serviceProvider = resolver.Resolve<IServiceProvider>();

        //    // Логгер
        //    var logger = resolver.Resolve<ILogger<UserManager<AppUser>>>();

        //    var userManager = new UserManager<AppUser>(appUserStore,
        //                                               optionsAccessor,
        //                                               identityPasswordHasher,
        //                                               userValidators,
        //                                               passwordValidators,
        //                                               keyNormalizer,
        //                                               identityErrorDescriber,
        //                                               serviceProvider,
        //                                               logger);

        //    return userManager;
        //}
    }
}