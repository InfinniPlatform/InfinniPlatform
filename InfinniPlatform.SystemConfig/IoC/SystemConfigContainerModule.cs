using InfinniPlatform.Core.Modules;
using InfinniPlatform.Core.Security;
using InfinniPlatform.Sdk.Environment;
using InfinniPlatform.Sdk.Environment.Settings;
using InfinniPlatform.Sdk.IoC;
using InfinniPlatform.SystemConfig.RequestHandlers;
using InfinniPlatform.SystemConfig.StartupInitializers;
using InfinniPlatform.SystemConfig.UserStorage;

namespace InfinniPlatform.SystemConfig.IoC
{
    internal sealed class SystemConfigContainerModule : IContainerModule
    {
        public void Load(IContainerBuilder builder)
        {
            builder.RegisterFactory(r => r.Resolve<IAppConfiguration>().GetSection<UserStorageSettings>(UserStorageSettings.SectionName))
                   .As<UserStorageSettings>()
                   .SingleInstance();

            // Кэш сведений о пользователях системы
            builder.RegisterType<ApplicationUserStoreCache>()
                   .AsSelf()
                   .SingleInstance();

            // Хранилище сведений о пользователях системы
            builder.RegisterType<ApplicationUserStorePersistentStorage>()
                   .As<IApplicationUserStore>()
                   .SingleInstance();

            // Сервисы инициализации для обработки события старта приложения

            builder.RegisterType<PackageJsonConfigurationsInitializer>()
                   .As<IStartupInitializer>()
                   .SingleInstance();

            builder.RegisterType<DocumentIndexTypeInitializer>()
                   .As<IStartupInitializer>()
                   .SingleInstance();

            // Обработчик событий приложения системной конфигурации
            builder.RegisterType<SystemConfigApplicationEventHandler>()
                   .As<IApplicationEventHandler>()
                   .SingleInstance();

            // Обработчики сервисов системной конфигурации
            builder.RegisterType<SystemConfigInstaller>()
                   .As<IModuleInstaller>()
                   .SingleInstance();

            // Прикладные скрипты
            builder.RegisterActionUnits(GetType().Assembly);
        }
    }
}