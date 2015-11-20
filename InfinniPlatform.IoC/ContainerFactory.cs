using System;

using Autofac;

using InfinniPlatform.IoC.Properties;
using InfinniPlatform.Sdk.IoC;

namespace InfinniPlatform.IoC
{
    internal class ContainerFactory
    {
        public object CreateContainer(ContainerBuilder builder)
        {
            var containerModuleScanner = new BaseDirectoryContainerModuleScanner();

            // Поиск всех модулей
            var containerModules = containerModuleScanner.FindContainerModules();

            foreach (var moduleInfo in containerModules)
            {
                var containerModule = CreateContainerModule(moduleInfo);

                // Регистрация модуля
                builder.RegisterModule(new AutofacContainerModule(containerModule));
            }

            return builder.Build();
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