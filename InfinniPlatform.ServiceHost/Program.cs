using System;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using InfinniPlatform.ServiceHost.Properties;

namespace InfinniPlatform.ServiceHost
{
    public class Program
    {
        public static void Main(string[] args)
        {
            /* Вся логика метода Main() находится в отдельных методах, чтобы JIT-компиляция Main()
             * прошла без загрузки дополнительных сборок, поскольку до этого момента нужно успеть
             * установить свою собственную логику загрузки сборок.
             */

            // Устанавливает для приложения контекст загрузки сборок по умолчанию
            InitializeAssemblyLoadContext();

            // Запускает хостинг приложения
            RunServiceHost(args);
        }

        private static void InitializeAssemblyLoadContext()
        {
            var context = new DirectoryAssemblyLoadContext();
            DirectoryAssemblyLoadContext.InitializeDefaultContext(context);
        }


        private static void RunServiceHost(string[] args)
        {
            try
            {
                // Поиск компонента для хостинга приложения
                var serviceHost = CreateComponent<dynamic>("InfinniPlatformServiceHost");

                // Запуск хостинга приложения

                if (args.Any(s => (s == "-i") || (s == "--init")))
                {
                    serviceHost.Init(Timeout.InfiniteTimeSpan);
                    Console.WriteLine(Resources.ServerInitialized);
                }

                if (args.Any(s => (s == "-s") || (s == "--start")))
                {
                    serviceHost.Start(Timeout.InfiniteTimeSpan);
                    Console.WriteLine(Resources.ServerStarted);
                }

                var stopEvent = new TaskCompletionSource<bool>();

                Console.CancelKeyPress += (s, e) =>
                                          {
                                              // Остановка хостинга приложения
                                              serviceHost.Stop(Timeout.InfiniteTimeSpan);
                                              stopEvent.SetResult(true);
                                          };

                // Ожидание остановки приложения (Ctrl+C)
                stopEvent.Task.Wait(Timeout.Infinite);
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