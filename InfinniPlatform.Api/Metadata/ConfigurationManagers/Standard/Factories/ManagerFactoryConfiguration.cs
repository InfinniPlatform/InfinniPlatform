using System.Collections.Generic;
using InfinniPlatform.Api.Metadata.ConfigurationManagers.Standard.MetadataManagers;
using InfinniPlatform.Api.Metadata.ConfigurationManagers.Standard.MetadataReaders;
using InfinniPlatform.Api.Metadata.MetadataContainers;

namespace InfinniPlatform.Api.Metadata.ConfigurationManagers.Standard.Factories
{
	/// <summary>
	/// Фабрика менеджеров метаданных для работы с метаданными конфигурации.
	/// </summary>
	public sealed class ManagerFactoryConfiguration : IManagerFactoryConfiguration
	{
		private readonly Dictionary<string, IDataManager> _managers = new Dictionary<string, IDataManager>();

		private readonly Dictionary<string, IDataReader> _readers = new Dictionary<string, IDataReader>();

	    /// <summary>
	    /// Конструктор.
	    /// </summary>
	    /// <param name="version">Идентификатор версии конфигурации</param>
	    /// <param name="configId">Идентификатор конфигурации (например, "Integration").</param>
	    public ManagerFactoryConfiguration(string version, string configId)
		{
			_configId = configId;
	        _version = version;

			_managers.Add(MetadataType.Document, BuildDocumentManager());
			_managers.Add(MetadataType.Assembly, BuildAssemblyManager());
			_managers.Add(MetadataType.Register, BuildRegisterManager());
			_managers.Add(MetadataType.Report, BuildReportManager());
			_managers.Add(MetadataType.Menu, BuildMenuManager());

			_readers.Add(MetadataType.Document, BuildDocumentMetadataReader());
			_readers.Add(MetadataType.Assembly, BuildAssemblyMetadataReader());
			_readers.Add(MetadataType.Register, BuildRegisterMetadataReader());
			_readers.Add(MetadataType.Report, BuildReportMetadataReader());
			_readers.Add(MetadataType.Menu, BuildMenuMetadataReader());
		}


		private readonly string _configId;
	    private readonly string _version;


	    public static IDataReader BuildConfigurationMetadataReader(string version, bool doNotCheckVersion = false)
		{
			return new MetadataReaderConfiguration(version,doNotCheckVersion);
		}


		public IDataReader BuildMenuMetadataReader()
		{
			return new MetadataReaderConfigurationElement(_version, _configId, new MetadataContainerMenu());
		}

		public IDataReader BuildDocumentMetadataReader()
		{
			return new MetadataReaderConfigurationElement(_version, _configId, new MetadataContainerDocument());
		}

		public IDataReader BuildRegisterMetadataReader()
		{
            return new MetadataReaderConfigurationElement(_version, _configId, new MetadataContainerRegister());
		}

		public IDataReader BuildAssemblyMetadataReader()
		{
            return new MetadataReaderConfigurationElement(_version, _configId, new MetadataContainerAssembly());
		}

		public IDataReader BuildReportMetadataReader()
		{
            return new MetadataReaderConfigurationElement(_version, _configId, new MetadataContainerReport());
		}

        /// <summary>
        ///   Получить менеджер конфигураций для работы с указанной версией прикладной конфигурации
        /// </summary>
        /// <param name="version">Версия прикладной конфигурации</param>
        /// <returns>Менеджер метаданных конфигураций</returns>
		public static MetadataManagerConfiguration BuildConfigurationManager(string version)
		{
			return new MetadataManagerConfiguration(new MetadataReaderConfiguration(version), version);
		}

        /// <summary>
        ///  Получить менеджер для управления метаданными меню конфигураций
        /// </summary>
        /// <returns>Менеджер метаданных меню</returns>
		public MetadataManagerElement BuildMenuManager()
		{
			return new MetadataManagerElement(_version, _configId, 
				null,
				new MetadataReaderConfigurationElement(_version, _configId, new MetadataContainerMenu()), 				
				new MetadataContainerMenu(), 
				MetadataType.Configuration);
		}

	    /// <summary>
	    ///  Получить менеджер метаданных документов
	    /// </summary>
	    /// <returns>Менеджер метаданных документов</returns>
	    public MetadataManagerDocument BuildDocumentManager()
		{
			return new MetadataManagerDocument(_version, _configId, new MetadataContainerDocument(), MetadataType.Configuration);
		}

		public MetadataManagerElement BuildRegisterManager()
		{
			return new MetadataManagerElement(_version, _configId,
				null,
				new MetadataReaderConfigurationElement(_version, _configId, new MetadataContainerRegister()), 
				new MetadataContainerRegister(), 
				MetadataType.Configuration);
		}

		public MetadataManagerElement BuildAssemblyManager()
		{
			return new MetadataManagerElement(_version, _configId, 
				null,
				new MetadataReaderConfigurationElement(_version, _configId, new MetadataContainerAssembly()), 
				new MetadataContainerAssembly(), 
				MetadataType.Configuration);
		}

		public MetadataManagerElement BuildReportManager()
		{
			return new MetadataManagerElement(_version, _configId, 
				null,
				new MetadataReaderConfigurationElement(_version, _configId, new MetadataContainerReport()), 
				new MetadataContainerReport(), 
				MetadataType.Configuration);
		}

		public IDataManager BuildManagerByType(string configMetadataType)
		{
			if (_managers.ContainsKey(configMetadataType))
			{
				return _managers[configMetadataType];
			}
			return null;
		}

		public IDataReader BuildMetadataReaderByType(string configMetadataType)
		{
			return _readers[configMetadataType];
		}
	}
}