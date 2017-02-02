using System;

using Autofac;

using InfinniPlatform.Core.Properties;
using InfinniPlatform.Sdk.IoC;
using Nancy.Extensions;

namespace InfinniPlatform.Core.IoC.Http
{
    public class AutofacHttpContainerResolverFactory
    {
        public IContainerResolver CreateContainerResolver()
        {
            var autofacContainerBuilder = new ContainerBuilder();

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

            // ReSharper restore AccessToModifiedClosure

            // Построение контейнера зависимостей
            autofacRootContainer = autofacContainerBuilder.Build();
            containerResolver = new AutofacContainerResolver(autofacRootContainer, AutofacRequestLifetimeScopeOwinMiddleware.TryGetRequestContainer);

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
                createModuleException.Data.Add("AssemblyPath", moduleType.GetAssembly().Location);
                createModuleException.Data.Add("TypeFullName", moduleType.FullName);
                throw createModuleException;
            }
        }
    }
}