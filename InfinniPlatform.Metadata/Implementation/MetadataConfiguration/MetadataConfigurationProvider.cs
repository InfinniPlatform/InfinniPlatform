using System.Collections.Generic;
using System.Linq;
using InfinniPlatform.Api.Actions;
using InfinniPlatform.Api.Hosting;
using InfinniPlatform.Api.Metadata;
using InfinniPlatform.Factories;
using InfinniPlatform.Hosting;

namespace InfinniPlatform.Metadata.Implementation.MetadataConfiguration
{
	/// <summary>
	///   Провайдер для работы с конфигурациями метаданных
	/// </summary>
	public class MetadataConfigurationProvider : IMetadataConfigurationProvider
	{
		private readonly IServiceRegistrationContainerFactory _serviceRegistrationContainerFactory;
		private readonly IServiceTemplateConfiguration _serviceTemplateConfiguration;

		public MetadataConfigurationProvider(IServiceRegistrationContainerFactory serviceRegistrationContainerFactory, IServiceTemplateConfiguration serviceTemplateConfiguration)
		{
			_serviceRegistrationContainerFactory = serviceRegistrationContainerFactory;
			_serviceTemplateConfiguration = serviceTemplateConfiguration;
		}

		private List<IMetadataConfiguration> _configurations = new List<IMetadataConfiguration>();

        /// <summary>
        ///   Список конфигураций метаданных
        /// </summary>
	    public IEnumerable<IMetadataConfiguration> Configurations
	    {
	        get { return _configurations; }
	    }

		public void RemoveConfiguration(string metadataConfigurationId)
		{
			_configurations = _configurations.Where(c => c.ConfigurationId != metadataConfigurationId).ToList();
		}

		/// <summary>
	    ///   Получить метаданные конфигурации
	    /// </summary>
	    /// <param name="metadataConfigurationId">Идентификатор метаданных конфигурации</param>
	    /// <returns>Метаданные конфигурации</returns>
	    public IMetadataConfiguration GetMetadataConfiguration(string metadataConfigurationId) {
			return Configurations.FirstOrDefault(c => c.ConfigurationId.ToLowerInvariant() == metadataConfigurationId.ToLowerInvariant());
		}

		/// <summary>
		///   Добавить конфигурацию метаданных
		/// </summary>
		/// <param name="metadataConfigurationId">Идентификатор конфигурации метаданных</param>
		/// <param name="actionConfiguration">Конфигурация скриптовых модулей</param>
		/// <param name="isEmbeddedConfiguration">Признак встроенной в код конфигурации C#</param>
		/// <returns>Конфигурация метаданных</returns>
		public IMetadataConfiguration AddConfiguration(string metadataConfigurationId, IScriptConfiguration actionConfiguration, bool isEmbeddedConfiguration)
		{
            var metadataConfiguration = new MetadataConfiguration(actionConfiguration,_serviceRegistrationContainerFactory.BuildServiceRegistrationContainer(metadataConfigurationId), _serviceTemplateConfiguration, isEmbeddedConfiguration) { ConfigurationId = metadataConfigurationId };
			_configurations.Add(metadataConfiguration);
			return metadataConfiguration;
		}

	}

}