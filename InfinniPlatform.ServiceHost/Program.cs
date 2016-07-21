using System;
using System.ComponentModel.Composition.Hosting;
using System.Threading;

using InfinniPlatform.ServiceHost.Properties;

namespace InfinniPlatform.ServiceHost
{
    public class Program
    {
        public static void Main()
        {
            /* Вся логика метода Main() находится в отдельных методах, чтобы JIT-компиляция Main()
             * прошла без загрузки дополнительных сборок, поскольку до этого момента нужно успеть
             * установить свою собственную логику загрузки сборок.
             */

            // Устанавливает для приложения контекст загрузки сборок по умолчанию
            InitializeAssemblyLoadContext();

            // Запускает хостинг приложения
            RunServiceHost();
        }


        private static void InitializeAssemblyLoadContext()
        {
            var context = new DirectoryAssemblyLoadContext();
            DirectoryAssemblyLoadContext.InitializeDefaultContext(context);
        }


        private static void RunServiceHost()
        {
            try
            {
                // Поиск компонента для хостинга приложения
                var serviceHost = CreateComponent<dynamic>("InfinniPlatformServiceHost");

                // Запуск хостинга приложения
                serviceHost.Start(Timeout.InfiniteTimeSpan);

                Console.WriteLine(Resources.ServerStarted);
                Console.ReadLine();
            }
            catch (Exception error)
            {
                Console.WriteLine(error);
            }
        }

        private static T CreateComponent<T>(string contractName) where T : class
        {
            var aggregateCatalog = new DirectoryAssemblyCatalog();
            var compositionContainer = new CompositionContainer(aggregateCatalog);
            var lazyInstance = compositionContainer.GetExport<T>(contractName);
            return lazyInstance?.Value;
        }
    }
}