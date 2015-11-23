using InfinniPlatform.Api.Security;
using InfinniPlatform.Modules;
using InfinniPlatform.Sdk.Environment;
using InfinniPlatform.Sdk.IoC;
using InfinniPlatform.SystemConfig.Initializers;
using InfinniPlatform.SystemConfig.Installers;
using InfinniPlatform.SystemConfig.UserStorage;

namespace InfinniPlatform.SystemConfig.IoC
{
    internal class SystemConfigContainerModule : IContainerModule
    {
        public void Load(IContainerBuilder builder)
        {
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

            // Хранилище сведений о пользователях системы
            builder.RegisterType<ApplicationUserStorePersistentStorage>()
                   .As<IApplicationUserStore>()
                   .SingleInstance();

            // Обработчики сервисов системной конфигурации
            builder.RegisterType<SystemConfigInstaller>()
                   .As<IModuleInstaller>()
                   .SingleInstance();
        }
    }
}