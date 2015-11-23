using System;
using System.Web.Http.Dependencies;

using Autofac;

using InfinniPlatform.IoC.Owin.Modules;
using InfinniPlatform.IoC.Properties;
using InfinniPlatform.IoC.WebApi;
using InfinniPlatform.Owin.Modules;
using InfinniPlatform.Sdk.Api;
using InfinniPlatform.Sdk.IoC;

namespace InfinniPlatform.IoC.Owin
{
    public sealed class AutofacOwinHostingContextFactory
    {
        public IOwinHostingContext CreateOwinHostingContext(HostingConfig hostingConfig)
        {
            var autofacContainerBuilder = new ContainerBuilder();

            var containerModuleScanner = new BaseDirectoryContainerModuleScanner();

            // Поиск всех доступных модулей
            var containerModules = containerModuleScanner.FindContainerModules();

            foreach (var moduleInfo in containerModules)
            {
                var containerModule = CreateContainerModule(moduleInfo);

                // Регистрация очередного модуля
                autofacContainerBuilder.RegisterModule(new AutofacContainerModule(containerModule));
            }

            // ReSharper disable AccessToModifiedClosure

            // Регистрация самого Autofac-контейнера
            IContainer autofacContainer = null;
            Func<IContainer> autofacContainerFactory = () => autofacContainer;
            autofacContainerBuilder.RegisterInstance(autofacContainerFactory);

            // Регистрация обертки над контейнером
            IContainerResolver containerResolver = null;
            Func<IContainerResolver> containerResolverFactory = () => containerResolver;
            autofacContainerBuilder.RegisterInstance(containerResolverFactory);

            // Регистрация контейнера для WebApi
            IDependencyResolver webApiDependencyResolver = null;
            Func<IDependencyResolver> webApiDependencyResolverFactory = () => webApiDependencyResolver;
            autofacContainerBuilder.RegisterInstance(webApiDependencyResolverFactory);

            // ReSharper restore AccessToModifiedClosure

            // Построение контейнера зависимостей
            autofacContainer = autofacContainerBuilder.Build();
            containerResolver = new AutofacContainerResolver(autofacContainer);
            webApiDependencyResolver = new AutofacWebApiDependencyResolver(autofacContainer);

            return new OwinHostingContext(hostingConfig, containerResolver, new AutofacOwinMiddlewareResolver());
        }

        private static IContainerModule CreateContainerModule(ContainerModuleInfo moduleInfo)
        {
            try
            {
                // Загрузка типа модуля
                var moduleType = moduleInfo.Type.Value;

                // Создание экземпляра модуля
                var moduleInstance = Activator.CreateInstance(moduleType);

                return (IContainerModule)moduleInstance;
            }
            catch (Exception error)
            {
                var createModuleException = new InvalidOperationException(Resources.CannotCreateContainerModule, error);
                createModuleException.Data.Add("AssemblyPath", moduleInfo.Location.AssemblyPath);
                createModuleException.Data.Add("TypeFullName", moduleInfo.Location.TypeFullName);
                throw createModuleException;
            }
        }
    }
}