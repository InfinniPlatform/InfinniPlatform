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

            // Регистрация IoC-модуля для OWIN 
            autofacContainerBuilder.RegisterType<AutofacOwinHostingModule>()
                                   .As<IOwinHostingModule>()
                                   .SingleInstance();

            // ReSharper disable AccessToModifiedClosure

            // Регистрация самого Autofac-контейнера
            IContainer autofacRootContainer = null;
            Func<IContainer> autofacRootContainerFactory = () => autofacRootContainer;
            autofacContainerBuilder.RegisterInstance(autofacRootContainerFactory);
            autofacContainerBuilder.Register(r => r.Resolve<Func<IContainer>>()()).As<IContainer>().SingleInstance();

            // Регистрация обертки над контейнером
            IContainerResolver containerResolver = null;
            Func<IContainerResolver> containerResolverFactory = () => containerResolver;
            autofacContainerBuilder.RegisterInstance(containerResolverFactory);
            autofacContainerBuilder.Register(r => r.Resolve<Func<IContainerResolver>>()()).As<IContainerResolver>().SingleInstance();

            // Регистрация контейнера для WebApi
            IDependencyResolver webApiDependencyResolver = null;
            Func<IDependencyResolver> webApiDependencyResolverFactory = () => webApiDependencyResolver;
            autofacContainerBuilder.RegisterInstance(webApiDependencyResolverFactory);
            autofacContainerBuilder.Register(r => r.Resolve<Func<IDependencyResolver>>()()).As<IDependencyResolver>().SingleInstance();

            // ReSharper restore AccessToModifiedClosure

            // Построение контейнера зависимостей
            autofacRootContainer = autofacContainerBuilder.Build();
            containerResolver = new AutofacContainerResolver(autofacRootContainer, Middleware.AutofacRequestLifetimeScopeOwinMiddleware.TryGetRequestContainer);
            webApiDependencyResolver = new AutofacWebApiDependencyResolver(autofacRootContainer);

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