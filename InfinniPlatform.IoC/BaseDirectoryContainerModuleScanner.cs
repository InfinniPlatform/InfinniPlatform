using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

using InfinniPlatform.Sdk.IoC;

namespace InfinniPlatform.IoC
{
    /// <summary>
    /// Поисковик модулей регистрации зависимостей <see cref="IContainerModule"/> в базовом каталоге текущего домена приложения <see cref="AppDomain.BaseDirectory"/>.
    /// </summary>
    internal sealed class BaseDirectoryContainerModuleScanner
    {
        public BaseDirectoryContainerModuleScanner()
        {
            _containerModules = new Lazy<IEnumerable<ContainerModuleInfo>>(LoadContainerModules);
        }


        private readonly Lazy<IEnumerable<ContainerModuleInfo>> _containerModules;


        /// <summary>
        /// Возвращает список найденных модулей регистрации зависимостей.
        /// </summary>
        public IEnumerable<ContainerModuleInfo> FindContainerModules()
        {
            return _containerModules.Value;
        }


        private IEnumerable<ContainerModuleInfo> LoadContainerModules()
        {
            // Возвращается коллекция ленивых объектов, так как ответственностью данного класса является только поиск, а не загрузка

            var result = new List<ContainerModuleInfo>();

            var modules = FindContainerModulesWithoutLoad();

            if (modules != null)
            {
                foreach (var module in modules)
                {
                    result.Add(new ContainerModuleInfo(module, new Lazy<Type>(() => GetOrLoadType(module.AssemblyPath, module.TypeFullName))));
                }
            }

            return result;
        }


        private readonly ConcurrentDictionary<string, Type> _moduleTypes
            = new ConcurrentDictionary<string, Type>(StringComparer.Ordinal);

        private Type GetOrLoadType(string assemblyPath, string typeFullName)
        {
            Type type;

            var typeKey = CombineKey(assemblyPath, typeFullName);

            if (!_moduleTypes.TryGetValue(typeKey, out type))
            {
                var assembly = GetOrLoadAssembly(assemblyPath);

                type = assembly.GetType(typeFullName);

                _moduleTypes.TryAdd(typeKey, type);
            }

            return type;
        }


        private readonly ConcurrentDictionary<string, Assembly> _moduleAssemblies
            = new ConcurrentDictionary<string, Assembly>(StringComparer.OrdinalIgnoreCase);

        private Assembly GetOrLoadAssembly(string assemblyPath)
        {
            Assembly assembly;

            if (!_moduleAssemblies.TryGetValue(assemblyPath, out assembly))
            {
                assembly = Assembly.LoadFrom(assemblyPath);

                _moduleAssemblies.TryAdd(assemblyPath, assembly);
            }

            return assembly;
        }


        private static IEnumerable<ContainerModuleLocation> FindContainerModulesWithoutLoad()
        {
            // Поиск модулей без загрузки сборок, в которых они находятся,
            // чтобы не "засорять" текущий домен приложения раньше времени.

            IEnumerable<ContainerModuleLocation> result;

            var currentDomainInfo = AppDomain.CurrentDomain.SetupInformation;

            var tempDomain = AppDomain.CreateDomain("ContainerModuleScanner", null, new AppDomainSetup
            {
                LoaderOptimization = LoaderOptimization.MultiDomainHost,
                ApplicationBase = currentDomainInfo.ApplicationBase,
                ConfigurationFile = currentDomainInfo.ConfigurationFile
            });

            try
            {
                tempDomain.DoCallBack(() =>
                {
                    AppDomain.CurrentDomain.SetData("Modules", FindBaseDirectoryContainerModules());
                });

                result = tempDomain.GetData("Modules") as IEnumerable<ContainerModuleLocation>;
            }
            finally
            {
                AppDomain.Unload(tempDomain);
            }

            return result;
        }


        private static IEnumerable<ContainerModuleLocation> FindBaseDirectoryContainerModules()
        {
            var result = new List<ContainerModuleLocation>();

            var directory = AppDomain.CurrentDomain.BaseDirectory;

            // Поиск всех исполняемых модулей в каталоге текущего домена приложения
            var assemblyFiles = Directory.EnumerateFiles(directory, "*.dll", SearchOption.TopDirectoryOnly)
                                         .Concat(Directory.EnumerateFiles(directory, "*.exe", SearchOption.TopDirectoryOnly));

            foreach (var assemblyFile in assemblyFiles)
            {
                try
                {
                    // Загрузка сборки для анализа (ReflectionOnlyLoadFrom не подходит по многим причинам)
                    var assembly = Assembly.LoadFrom(assemblyFile);

                    // Сборки со строгим именем не рассматриваются (предполагается, что прикладные сборки его не имеют)
                    if (!HasPublicKeyToken(assembly))
                    {
                        var types = assembly.GetTypes();

                        foreach (var type in types)
                        {
                            if (type.IsClass && !type.IsAbstract && !type.IsGenericType && typeof(IContainerModule).IsAssignableFrom(type))
                            {
                                result.Add(new ContainerModuleLocation(assemblyFile, type.FullName));
                            }
                        }
                    }
                }
                catch
                {
                    // ReflectionTypeLoadException
                }
            }

            return result;
        }

        private static bool HasPublicKeyToken(Assembly assembly)
        {
            var publicKeyToken = assembly.GetName().GetPublicKeyToken();
            return (publicKeyToken != null && publicKeyToken.Length > 0);
        }

        private static string CombineKey(string term1, string term2)
        {
            return term1 + ',' + term2;
        }
    }
}