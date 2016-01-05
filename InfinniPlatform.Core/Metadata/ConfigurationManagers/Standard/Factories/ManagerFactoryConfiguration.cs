using System.Collections.Generic;

using InfinniPlatform.Core.Metadata.ConfigurationManagers.Standard.MetadataManagers;
using InfinniPlatform.Core.Metadata.ConfigurationManagers.Standard.MetadataReaders;
using InfinniPlatform.Core.Metadata.MetadataContainers;

namespace InfinniPlatform.Core.Metadata.ConfigurationManagers.Standard.Factories
{
    /// <summary>
    ///     Фабрика менеджеров метаданных для работы с метаданными конфигурации.
    /// </summary>
    public sealed class ManagerFactoryConfiguration : IManagerFactoryConfiguration
    {
        private readonly string _configId;
        private readonly Dictionary<string, IDataManager> _managers = new Dictionary<string, IDataManager>();
        private readonly Dictionary<string, IDataReader> _readers = new Dictionary<string, IDataReader>();

        /// <summary>
        ///     Конструктор.
        /// </summary>
        /// <param name="configId">Идентификатор конфигурации (например, "Integration").</param>
        public ManagerFactoryConfiguration(string configId)
        {
            _configId = configId;

            _managers.Add(MetadataType.Document.ToLowerInvariant(), BuildDocumentManager());
            _managers.Add(MetadataType.Assembly.ToLowerInvariant(), BuildAssemblyManager());
            _managers.Add(MetadataType.Register.ToLowerInvariant(), BuildRegisterManager());
            _managers.Add(MetadataType.Report.ToLowerInvariant(), BuildReportManager());
            _managers.Add(MetadataType.Menu.ToLowerInvariant(), BuildMenuManager());

            _readers.Add(MetadataType.Document.ToLowerInvariant(), BuildDocumentMetadataReader());
            _readers.Add(MetadataType.Assembly.ToLowerInvariant(), BuildAssemblyMetadataReader());
            _readers.Add(MetadataType.Register.ToLowerInvariant(), BuildRegisterMetadataReader());
            _readers.Add(MetadataType.Report.ToLowerInvariant(), BuildReportMetadataReader());
            _readers.Add(MetadataType.Menu.ToLowerInvariant(), BuildMenuMetadataReader());
        }

        public IDataReader BuildMenuMetadataReader()
        {
            return new MetadataReaderConfigurationElement(_configId, new MetadataContainerMenu());
        }

        public IDataReader BuildDocumentMetadataReader()
        {
            return new MetadataReaderConfigurationElement(_configId, new MetadataContainerDocument());
        }

        public IDataReader BuildAssemblyMetadataReader()
        {
            return new MetadataReaderConfigurationElement(_configId, new MetadataContainerAssembly());
        }

        public IDataReader BuildReportMetadataReader()
        {
            return new MetadataReaderConfigurationElement(_configId, new MetadataContainerReport());
        }

        /// <summary>
        ///     Получить менеджер для управления метаданными меню конфигураций
        /// </summary>
        /// <returns>Менеджер метаданных меню</returns>
        public MetadataManagerElement BuildMenuManager()
        {
            return new MetadataManagerElement(_configId,
                null,
                new MetadataReaderConfigurationElement(_configId, new MetadataContainerMenu()),
                new MetadataContainerMenu(),
                MetadataType.Configuration);
        }

        /// <summary>
        ///     Получить менеджер метаданных документов
        /// </summary>
        /// <returns>Менеджер метаданных документов</returns>
        public MetadataManagerDocument BuildDocumentManager()
        {
            return new MetadataManagerDocument(_configId, new MetadataContainerDocument(),
                MetadataType.Configuration);
        }

        public MetadataManagerElement BuildAssemblyManager()
        {
            return new MetadataManagerElement(_configId,
                null,
                new MetadataReaderConfigurationElement(_configId, new MetadataContainerAssembly()),
                new MetadataContainerAssembly(),
                MetadataType.Configuration);
        }

        public MetadataManagerElement BuildReportManager()
        {
            return new MetadataManagerElement(_configId,
                null,
                new MetadataReaderConfigurationElement(_configId, new MetadataContainerReport()),
                new MetadataContainerReport(),
                MetadataType.Configuration);
        }

        public static IDataReader BuildConfigurationMetadataReader(bool doNotCheckVersion = false)
        {
            return new MetadataReaderConfiguration(doNotCheckVersion);
        }

        public IDataReader BuildRegisterMetadataReader()
        {
            return new MetadataReaderConfigurationElement(_configId, new MetadataContainerRegister());
        }

        /// <summary>
        ///     Получить менеджер конфигураций для работы с указанной версией прикладной конфигурации
        /// </summary>
        /// <returns>Менеджер метаданных конфигураций</returns>
        public static MetadataManagerConfiguration BuildConfigurationManager()
        {
            return new MetadataManagerConfiguration(new MetadataReaderConfiguration());
        }

        public MetadataManagerElement BuildRegisterManager()
        {
            return new MetadataManagerElement(_configId,
                null,
                new MetadataReaderConfigurationElement(_configId, new MetadataContainerRegister()),
                new MetadataContainerRegister(),
                MetadataType.Configuration);
        }

        public IDataManager BuildManagerByType(string configMetadataType)
        {
            if (_managers.ContainsKey(configMetadataType.ToLowerInvariant()))
            {
                return _managers[configMetadataType.ToLowerInvariant()];
            }
            return null;
        }

        public IDataReader BuildMetadataReaderByType(string configMetadataType)
        {
            return _readers[configMetadataType.ToLowerInvariant()];
        }
    }
}