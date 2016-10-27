using System;

using Autofac;

using InfinniPlatform.IoC.Properties;
using InfinniPlatform.Sdk.IoC;

namespace InfinniPlatform.IoC.Owin
{
    public class AutofacOwinContainerResolverFactory
    {
        public IContainerResolver CreateContainerResolver()
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