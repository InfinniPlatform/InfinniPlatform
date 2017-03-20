using System;
using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using InfinniPlatform.Core.Properties;
using InfinniPlatform.Sdk.IoC;
using Microsoft.Extensions.DependencyInjection;

namespace InfinniPlatform.Core.IoC.Http
{
    public static class AutofacHttpContainerResolverFactory
    {
        public static IServiceProvider BuildInfinniServiceProvider(this IServiceCollection serviceCollection)
        {
            //AssemblyLoadContext.Default.Resolving += CustomAssemblyLoader.DefaultOnResolving;

            var containerBuilder = new ContainerBuilder();
            containerBuilder.Populate(serviceCollection);

            var containerModuleScanner = new BaseDirectoryContainerModuleScanner();

            // Поиск всех доступных модулей
            var containerModules = containerModuleScanner.FindContainerModules();

            foreach (var moduleType in containerModules)
            {
                var containerModule = CreateContainerModule(moduleType);

                // Регистрация очередного модуля
                containerBuilder.RegisterModule(new AutofacContainerModule(containerModule));
            }

            // ReSharper disable AccessToModifiedClosure

            // Регистрация самого Autofac-контейнера
            IContainer autofacRootContainer = null;
            Func<IContainer> autofacRootContainerFactory = () => autofacRootContainer;
            containerBuilder.RegisterInstance(autofacRootContainerFactory);
            containerBuilder.Register(r => r.Resolve<Func<IContainer>>()()).As<IContainer>().SingleInstance();

            // Регистрация обертки над контейнером
            IContainerResolver containerResolver = null;
            Func<IContainerResolver> containerResolverFactory = () => containerResolver;
            containerBuilder.RegisterInstance(containerResolverFactory);
            containerBuilder.Register(r => r.Resolve<Func<IContainerResolver>>()()).As<IContainerResolver>().SingleInstance();

            // Регистрация контейнера для Asp.Net
            IServiceProvider serviceProvider = null;
            Func<IServiceProvider> serviceProviderFactory = () => serviceProvider;
            containerBuilder.RegisterInstance(serviceProviderFactory);
            containerBuilder.Register(r => r.Resolve<Func<IServiceProvider>>()()).As<IServiceProvider>().SingleInstance();

            // ReSharper restore AccessToModifiedClosure

            // Построение контейнера зависимостей
            autofacRootContainer = containerBuilder.Build();
            containerResolver = new AutofacContainerResolver(autofacRootContainer, AutofacRequestLifetimeScopeOwinMiddleware.TryGetRequestContainer);
            serviceProvider = new AutofacServiceProvider(autofacRootContainer);

            return serviceProvider;
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