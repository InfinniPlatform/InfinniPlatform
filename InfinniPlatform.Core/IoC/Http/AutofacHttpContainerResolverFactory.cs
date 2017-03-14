using System;
using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using InfinniPlatform.Core.Properties;
using InfinniPlatform.Sdk.IoC;

namespace InfinniPlatform.Core.IoC.Http
{
    public class AutofacHttpContainerResolverFactory
    {
        public IContainerResolver CreateContainerResolver(ContainerBuilder autofacContainerBuilder)
        {
            var containerModuleScanner = new BaseDirectoryContainerModuleScanner();

            // Поиск всех доступных модулей
            var containerModules = containerModuleScanner.FindContainerModules();

            foreach (var moduleType in containerModules)
            {
                var containerModule = CreateContainerModule(moduleType);

                // Регистрация очередного модуля
                autofacContainerBuilder.RegisterModule(new AutofacContainerModule(containerModule));
            }

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

            // Регистрация контейнера для Asp.Net
            IServiceProvider serviceProvider = null;
            Func<IServiceProvider> serviceProviderFactory = () => serviceProvider;
            autofacContainerBuilder.RegisterInstance(serviceProviderFactory);
            autofacContainerBuilder.Register(r => r.Resolve<Func<IServiceProvider>>()()).As<IServiceProvider>().SingleInstance();

            // ReSharper restore AccessToModifiedClosure

            // Построение контейнера зависимостей
            autofacRootContainer = autofacContainerBuilder.Build();
            containerResolver = new AutofacContainerResolver(autofacRootContainer, AutofacRequestLifetimeScopeOwinMiddleware.TryGetRequestContainer);
            serviceProvider = new AutofacServiceProvider(autofacRootContainer);

            return containerResolver;
        }


        private static IContainerModule CreateContainerModule(Type moduleType)
        {
            try
            {
                // Создание экземпляра модуля
                var moduleInstance = Activator.CreateInstance(moduleType);

                return (IContainerModule)moduleInstance;
            }
            catch (Exception error)
            {
                var createModuleException = new InvalidOperationException(Resources.CannotCreateContainerModule, error);
                createModuleException.Data.Add("AssemblyPath", moduleType.GetTypeInfo().Assembly.Location);
                createModuleException.Data.Add("TypeFullName", moduleType.FullName);
                throw createModuleException;
            }
        }
    }
}