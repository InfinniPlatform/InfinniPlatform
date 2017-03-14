using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Threading;
using System.Threading.Tasks;

namespace InfinniPlatform.ServiceHost
{
    public class Program
    {
        private static string NugetProbingPath => Path.Combine(Environment.GetEnvironmentVariable("USERPROFILE"), ".nuget", "packages");
        private static readonly MergedDependencyContext MergedDependencyContext = new MergedDependencyContext();
        private static readonly Lazy<Dictionary<string, string>> LocalAssembliesCache = new Lazy<Dictionary<string, string>>(BuildLocalAssembliesCache);

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
            AssemblyLoadContext.Default.Resolving += DefaultOnResolving;
        }

        private static void RunServiceHost(string[] args)
        {
            try
            {
                // Поиск компонента для хостинга приложения
                var serviceHost = CreateComponent("netstandard1.6\\InfinniPlatform.Core.dll");

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

        private static dynamic CreateComponent(string path)
        {
            var assemblyPath = Path.GetFullPath(path);

            try
            {
                var classLibraryAssembly = AssemblyLoadContext.Default.LoadFromAssemblyPath(assemblyPath);
                var type = classLibraryAssembly.GetType("InfinniPlatform.Core.ServiceHost.ServiceHost");
                return Activator.CreateInstance(type);
            }
            catch (Exception e)
            {
                var exception = e as ReflectionTypeLoadException;

                if (exception != null)
                {
                    foreach (var loaderException in exception.LoaderExceptions)
                    {
                        Console.WriteLine(loaderException);
                    }
                }
                else
                {
                    Console.WriteLine(e);
                }
            }

            return null;
        }

        private static Assembly DefaultOnResolving(AssemblyLoadContext assemblyLoadContext, AssemblyName assemblyName)
        {
            var runtimeInformation = System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription;

            if (LocalAssembliesCache.Value.ContainsKey(assemblyName.Name))
            {
                var localAssemblyPath = LocalAssembliesCache.Value[assemblyName.Name];
                return assemblyLoadContext.LoadFromAssemblyPath(localAssemblyPath);
            }

            var runtimeLibrary = MergedDependencyContext.GetRuntimeLibrary(assemblyName);

            var nugetPackagePath = runtimeLibrary?.Path.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
            var nugetAssemblyPath = runtimeLibrary?.RuntimeAssemblyGroups.FirstOrDefault()
                                                  ?.AssetPaths.FirstOrDefault()
                                                  ?.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);

            var fullAssemblyPath = Path.Combine(NugetProbingPath, nugetPackagePath, nugetAssemblyPath ?? string.Empty);

            if (File.Exists(fullAssemblyPath))
            {
                return assemblyLoadContext.LoadFromAssemblyPath(fullAssemblyPath);
            }
            else
            {
                if (Directory.Exists(fullAssemblyPath))
                {
                    var assemblyPathInLib = Directory.EnumerateFiles(Path.Combine(fullAssemblyPath, "lib"), $"*.dll", SearchOption.AllDirectories)
                                              .Where(path=>path.Contains("netstandard"))
                                              .FirstOrDefault();

                    if (assemblyPathInLib!=null)
                    {
                        return assemblyLoadContext.LoadFromAssemblyPath(assemblyPathInLib);
                    }

                    var assemblyPathInRuntimes = Directory.EnumerateFiles(Path.Combine(fullAssemblyPath, "runtimes"), $"*.dll", SearchOption.AllDirectories)
                                                        .Where(path => path.Contains("netstandard") && path.Contains("win"))
                                                        .FirstOrDefault();

                    if (assemblyPathInRuntimes != null)
                    {
                        return assemblyLoadContext.LoadFromAssemblyPath(assemblyPathInRuntimes);
                    }
                }

                return null;
            }

            
        }

        private static Dictionary<string, string> BuildLocalAssembliesCache()
        {
            Dictionary<string, string> localAssemblies = new Dictionary<string, string>();
            var path = Directory.GetFiles(".", "*.dll", SearchOption.AllDirectories);

            foreach (var s in path)
            {
                localAssemblies.Add(Path.GetFileNameWithoutExtension(s), Path.GetFullPath(s));
            }
            return localAssemblies;
        }
    }
}