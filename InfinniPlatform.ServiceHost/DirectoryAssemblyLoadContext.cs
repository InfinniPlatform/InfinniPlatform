using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace InfinniPlatform.ServiceHost
{
    /// <summary>
    /// Контекст загрузки сборок из указанного каталога.
    /// </summary>
    public class DirectoryAssemblyLoadContext
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="assemblyDirectory">Путь к каталогу со сборками.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public DirectoryAssemblyLoadContext(string assemblyDirectory = ".")
        {
            if (string.IsNullOrEmpty(assemblyDirectory))
            {
                throw new ArgumentNullException(nameof(assemblyDirectory));
            }

            _assemblyDirectory = assemblyDirectory;
        }


        private readonly string _assemblyDirectory;

        private readonly ConcurrentDictionary<AssemblyName, Assembly> _assemblyResolutions
            = new ConcurrentDictionary<AssemblyName, Assembly>(AssemblyNameComparer.Default);

        private readonly ConcurrentDictionary<string, SortedDictionary<AssemblyName, Assembly>> _loadedAssemblies
            = new ConcurrentDictionary<string, SortedDictionary<AssemblyName, Assembly>>(StringComparer.OrdinalIgnoreCase);


        /// <summary>
        /// Загружает сборку с указанным именем.
        /// </summary>
        /// <param name="assemblyName">Имя сборки.</param>
        /// <exception cref="ArgumentNullException"></exception>
        protected virtual Assembly Load(AssemblyName assemblyName)
        {
            if (assemblyName == null)
            {
                throw new ArgumentNullException(nameof(assemblyName));
            }

            Assembly result;

            // Если сборка с указанным именем уже разрешена
            if (_assemblyResolutions.TryGetValue(assemblyName, out result))
            {
                return result;
            }

            SortedDictionary<AssemblyName, Assembly> assemblies;

            // Если сборки с указанным именем еще не загружены
            if (!_loadedAssemblies.TryGetValue(assemblyName.Name, out assemblies))
            {
                assemblies = new SortedDictionary<AssemblyName, Assembly>(AssemblyNameComparer.Default);
                assemblies = _loadedAssemblies.GetOrAdd(assemblyName.Name, assemblies);

                try
                {
                    // Поиск всех файлов с указанным именем сборки
                    var assemblyFiles = Directory.EnumerateFiles(_assemblyDirectory, assemblyName.Name + ".dll", SearchOption.AllDirectories);

                    foreach (var assemblyFile in assemblyFiles)
                    {
                        try
                        {
                            // Попытка загрузки сборки из найденного файла
                            var assembly = Assembly.LoadFile(Path.GetFullPath(assemblyFile));
                            assemblies[assembly.GetName()] = assembly;
                        }
                        catch
                        {
                            // Игнорирование исключений загрузки сборки
                        }
                    }
                }
                catch
                {
                    // Игнорирование исключений файловой системы
                }
            }

            // Поиск совместимой сборки с указанным именем
            var lowestAssembly = assemblies.FirstOrDefault(i => AssemblyNameComparer.Default.Compare(i.Key, assemblyName) >= 0);

            // Добавление сборки в список разрешенных
            result = _assemblyResolutions.GetOrAdd(assemblyName, lowestAssembly.Value);

            return result;
        }


        /// <summary>
        /// Устанавливает для приложения контекст загрузки сборок по умолчанию.
        /// </summary>
        /// <param name="context">Контекст загрузки сборок.</param>
        public static void InitializeDefaultContext(DirectoryAssemblyLoadContext context)
        {
            AppDomain.CurrentDomain.AssemblyResolve += (s, e) => context.Load(new AssemblyName(e.Name));
        }
    }
}