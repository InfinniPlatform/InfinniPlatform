using InfinniPlatform.Core.Security;
using InfinniPlatform.Sdk.Environment;
using InfinniPlatform.Sdk.IoC;
using InfinniPlatform.Sdk.PrintView;
using InfinniPlatform.Sdk.Registers;
using InfinniPlatform.Sdk.Services;
using InfinniPlatform.Sdk.Settings;
using InfinniPlatform.SystemConfig.PrintView;
using InfinniPlatform.SystemConfig.Registers;
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

            // Печатные представления

            builder.RegisterType<PrintViewApi>()
                   .As<IPrintViewApi>()
                   .SingleInstance();

            // Регистры

            builder.RegisterType<RegisterApi>()
                   .As<IRegisterApi>()
                   .SingleInstance();

            // Прикладные скрипты
            builder.RegisterActionUnits(GetType().Assembly);

            // Прикладные сервисы
            builder.RegisterType<ChangeHttpRequestHandler>().AsSelf().SingleInstance();
            builder.RegisterType<SearchHttpRequestHandler>().AsSelf().SingleInstance();
            builder.RegisterType<CustomHttpRequestHandler>().AsSelf().SingleInstance();
            builder.RegisterType<AttachHttpRequestHandler>().AsSelf().SingleInstance();
            builder.RegisterType<DownloadHttpRequestHandler>().AsSelf().SingleInstance();
            builder.RegisterType<ReportHttpRequestHandler>().AsSelf().SingleInstance();
            builder.RegisterType<DocumentTransactionScopeOnAfterHandler>().AsSelf().SingleInstance();
            builder.RegisterHttpServices(GetType().Assembly);
        }
    }
}