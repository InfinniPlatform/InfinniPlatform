using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using InfinniPlatform.Sdk.IoC;

namespace InfinniPlatform.Core.IoC
{
    /// <summary>
    /// Поисковик модулей регистрации зависимостей <see cref="IContainerModule"/> в базовом каталоге приложения.
    /// </summary>
    internal sealed class BaseDirectoryContainerModuleScanner
    {
        public BaseDirectoryContainerModuleScanner()
        {
            _containerModules = new Lazy<IEnumerable<Type>>(LoadContainerModules);
        }


        private readonly Lazy<IEnumerable<Type>> _containerModules;


        /// <summary>
        /// Возвращает список найденных модулей регистрации зависимостей.
        /// </summary>
        public IEnumerable<Type> FindContainerModules()
        {
            return _containerModules.Value;
        }


        private static IEnumerable<Type> LoadContainerModules()
        {
            var result = new List<Type>();

            // Кэширование имен сборок во избежания повторной загрузки в текущий домен приложения
            var assemblies = new Dictionary<AssemblyName, AssemblyName>(AssemblyNameComparer.Default);

            // Поиск всех исполняемых модулей в каталоге текущего домена приложения
            var assemblyFiles = Directory.EnumerateFiles(".", "*.dll", SearchOption.AllDirectories)
                                         .Concat(Directory.EnumerateFiles(".", "*.exe", SearchOption.AllDirectories))
                                         .Select(Path.GetFullPath);

            foreach (var assemblyFile in assemblyFiles)
            {
                try
                {
                    // Попытка загрузки сборки из найденного файла
                    var assemblyFullPath = Path.GetFullPath(assemblyFile);
                    var assembly = AssemblyLoadContext.Default.LoadFromAssemblyPath(assemblyFullPath);

                    var name = assembly.GetName();

                    // При совпадении имен сборки, наибольший приоритет у сборки в корне проекта
                    if (!assemblies.ContainsKey(name))
                    {
                        var realAssembly = AssemblyLoadContext.Default.LoadFromAssemblyPath(assemblyFullPath);

                        assemblies.Add(name, name);

                        var types = realAssembly.GetTypes();

                        foreach (var type in types)
                        {
                            if (type.GetTypeInfo().IsClass && !type.GetTypeInfo().IsAbstract && !type.GetTypeInfo().IsGenericType && typeof(IContainerModule).IsAssignableFrom(type))
                            {
                                result.Add(type);
                            }
                        }
                    }
                }
                catch(Exception e)
                {
                    // ReflectionTypeLoadException
                }
            }

            return result;
        }
    }
}