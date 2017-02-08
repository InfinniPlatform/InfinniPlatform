using System;
using System.IO;
using System.Linq;
using System.Runtime.Loader;
using System.Threading;
using System.Threading.Tasks;

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
            var assemblyLoadContext = InitializeAssemblyLoadContext();

            // Запускает хостинг приложения
            RunServiceHost(args, assemblyLoadContext);
        }

        private static AssemblyLoadContext InitializeAssemblyLoadContext()
        {
            var context = new DirectoryAssemblyLoadContext();
            return context;
        }

        private static void RunServiceHost(string[] args, AssemblyLoadContext assemblyLoadContext)
        {
            try
            {
                // Поиск компонента для хостинга приложения
                var serviceHost = CreateComponent("InfinniPlatform.Core", assemblyLoadContext);

                // Запуск хостинга приложения

                if (args.Any(s => s == "-i" || s == "--init"))
                {
                    serviceHost.Init(Timeout.InfiniteTimeSpan);
                    Console.WriteLine("Resources.ServerInitialized");
                }

                if (!args.Any()
                    || args.Any(s => s == "-s" || s == "--start"))
                {
                    serviceHost.Start(Timeout.InfiniteTimeSpan);
                    Console.WriteLine("Resources.ServerStarted");
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

        private static dynamic CreateComponent(string assemblyName, AssemblyLoadContext assemblyLoadContext)
        {
            var assemblyPath = Directory.EnumerateFiles(Directory.GetCurrentDirectory(), $"{assemblyName}.dll", SearchOption.AllDirectories).FirstOrDefault();

            try
            {
                if (assemblyPath != null)
                {
                    var assembly = assemblyLoadContext.LoadFromAssemblyPath(assemblyPath);
                    var type = assembly.GetType("InfinniPlatform.Core.ServiceHost.ServiceHost");
                    return Activator.CreateInstance(type);
                }
            }
            catch (Exception error)
            {
                Console.WriteLine(error);
            }

            throw new DllNotFoundException($"{assemblyName}.dll not found.");
        }
    }
}