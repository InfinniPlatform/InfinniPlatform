using System;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.IO;
using System.Linq;
using System.Reflection;

namespace InfinniPlatform.ServiceHost
{
    /// <summary>
    /// Каталог сборок для поиска типов.
    /// </summary>
    public class DirectoryAssemblyCatalog : ComposablePartCatalog
    {
        private readonly AggregateCatalog _catalog;

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="assemblyDirectory">Путь к каталогу со сборками.</param>
        /// <param name="searchPattern">Шаблон поиска сборок.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public DirectoryAssemblyCatalog(string assemblyDirectory = ".", string searchPattern = "*.dll")
        {
            if (string.IsNullOrEmpty(assemblyDirectory))
            {
                throw new ArgumentNullException(nameof(assemblyDirectory));
            }

            if (string.IsNullOrEmpty(searchPattern))
            {
                throw new ArgumentNullException(nameof(searchPattern));
            }

            var catalog = new AggregateCatalog();

            var files = Directory.EnumerateFiles(assemblyDirectory, searchPattern, SearchOption.AllDirectories);

            foreach (var file in files)
            {
                try
                {
                    var assemblyCatalog = new AssemblyCatalog(file);

                    if (assemblyCatalog.Parts.Any())
                    {
                        catalog.Catalogs.Add(assemblyCatalog);
                    }
                }
                catch (ReflectionTypeLoadException)
                {
                }
                catch (BadImageFormatException)
                {
                }
            }

            _catalog = catalog;
        }

        public override IQueryable<ComposablePartDefinition> Parts => _catalog.Parts;
    }
}