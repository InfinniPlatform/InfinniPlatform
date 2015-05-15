﻿using System.Collections.Generic;
using InfinniPlatform.Api.Actions;
using InfinniPlatform.Api.Metadata;

namespace InfinniPlatform.Metadata
{
    /// <summary>
    ///   Провайдер метаданных конфигураций
    /// </summary>
    public interface IMetadataConfigurationProvider {

        /// <summary>
        ///   Получить метаданные конфигурации
        /// </summary>
        /// <param name="metadataConfigurationId">Идентификатор метаданных конфигурации</param>
        /// <returns>Метаданные конфигурации</returns>
        IMetadataConfiguration GetMetadataConfiguration(string metadataConfigurationId);


	    /// <summary>
	    ///   Добавить конфигурацию метаданных
	    /// </summary>
	    /// <param name="metadataConfigurationId">Идентификатор конфигурации метаданных</param>
	    /// <param name="actionConfiguration">Конфигурация скриптовых модулей</param>
		/// <param name="isEmbeddedConfiguration">Признак встроенной в код конфигурации C#</param>
	    /// <returns>Конфигурация метаданных</returns>
	    IMetadataConfiguration AddConfiguration(string metadataConfigurationId, IScriptConfiguration actionConfiguration, bool isEmbeddedConfiguration);

        /// <summary>
        ///   Список конфигураций метаданных
        /// </summary>
        IEnumerable<IMetadataConfiguration> Configurations { get; }

		/// <summary>
		///   Удалить указанную конфигурацию метаданных из списка загруженных конфигурации
		/// </summary>
		/// <param name="metadataConfigurationId">Идентификатор конфигурации</param>
		void RemoveConfiguration(string metadataConfigurationId);
    }


}
