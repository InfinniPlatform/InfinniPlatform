using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyModel;

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
            //InitializeAssemblyLoadContext();

            // Запускает хостинг приложения
            RunServiceHost(args);
        }

//        private static void InitializeAssemblyLoadContext()
//        {
//            var context = new DirectoryAssemblyLoadContext();
//            DirectoryAssemblyLoadContext.InitializeDefaultContext(context);
//        }

        private static void RunServiceHost(string[] args)
        {
            try
            {
                // Поиск компонента для хостинга приложения
                var serviceHost = CreateComponent("InfinniPlatformServiceHost");
                //var serviceHost = new Core.ServiceHost.ServiceHost();

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

        private static dynamic CreateComponent(string contractName)
        {
            var depsReader = new DependencyContextJsonReader();

            var dlls = Directory.GetFiles(Directory.GetCurrentDirectory(), "*.dll", SearchOption.AllDirectories);
            var deps = Directory.GetFiles(Directory.GetCurrentDirectory(), "*.deps.json", SearchOption.AllDirectories)
                                .ToDictionary(dep => Path.GetFileNameWithoutExtension(dep).Replace(".deps", string.Empty), dep => depsReader.Read(new FileStream(dep, FileMode.Open)));

            Log.Clear();
            //Log.Add($"After merge: RuntimeLibraries: {DependencyContext.Default.RuntimeLibraries.Count}, CompileLibraries: {DependencyContext.Default.CompileLibraries.Count}, RuntimeGraph: {DependencyContext.Default.RuntimeGraph.Count}");

            var commonDependencyContext = DependencyContext.Default;

            foreach (var dep in deps)
            {
                //Log.Add($"{dep.Key}: RuntimeLibraries: {dep.Value.RuntimeLibraries.Count}, CompileLibraries: {dep.Value.CompileLibraries.Count}, RuntimeGraph: {dep.Value.RuntimeGraph.Count}");

                commonDependencyContext = commonDependencyContext.Merge(dep.Value);
            }

            //Log.Add($"After merge: RuntimeLibraries: {commonDependencyContext.RuntimeLibraries.Count}, CompileLibraries: {commonDependencyContext.CompileLibraries.Count}, RuntimeGraph: {commonDependencyContext.RuntimeGraph.Count}");

            Log.Add(commonDependencyContext.RuntimeLibraries.Count(s => !string.IsNullOrEmpty(s.Path)));
            Log.Add(commonDependencyContext.CompileLibraries.Count(s => !string.IsNullOrEmpty(s.Path)));

            var assemblyLoadContext = new CustomAssemblyLoadContext(commonDependencyContext);

            foreach (var dll in dlls)
            {
                var assembly = assemblyLoadContext.LoadFromAssemblyPath(dll);

                try
                {
                    Log.Add(dll);
                    var types = assembly.GetTypes();
                    Log.Add(types.Length);
                }
                catch (ReflectionTypeLoadException e)
                {
                    foreach (var loaderException in e.LoaderExceptions)
                    {
                        if (loaderException is FileNotFoundException)
                        {
                            Log.Add(((FileNotFoundException) loaderException).FileName);
                        }
                        else
                        {
                            Log.Add(loaderException.Message);
                        }
                    }
                }
            }

            return depsReader;
        }

        public class CustomAssemblyLoadContext : AssemblyLoadContext
        {
            private readonly DependencyContext _dependencyContext;


            public CustomAssemblyLoadContext(DependencyContext dependencyContext)
            {
                _dependencyContext = dependencyContext;
            }

            // Not exactly sure about this
            protected override Assembly Load(AssemblyName assemblyName)
            {
                var runtimeLib = _dependencyContext.RuntimeLibraries.FirstOrDefault(d => d.Name.Contains(assemblyName.Name));

                if (runtimeLib == null)
                {
                    return null;
                }

                try
                {
                    AssemblyName name;

                    if (runtimeLib.Name.StartsWith("Infinni"))
                    {
                        var path = $"{runtimeLib.Name}.dll";
                        var exists = File.Exists(path);
                        var assembly = LoadFromAssemblyPath(path);
                        name = GetAssemblyName(path);
                    }
                    else
                    {
                        name = GetAssemblyName(runtimeLib.Path);
                    }

                    return Assembly.Load(name);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
        }
    }
}