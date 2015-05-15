using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfinniPlatform.Metadata.Implementation.MetadataConfiguration
{
	/// <summary>
	///   Базовый провайдер наименований индексов метаданных объектов
	/// </summary>
	public sealed class MetadataIndexNameProvider : IMetadataIndexNameProvider
	{
		private readonly string _configurationId;

		public MetadataIndexNameProvider(string configurationId)
		{
			_configurationId = ToElasticIndexNameString(configurationId);
		}

		/// <summary>
		///   Получить наименование индекса с указанным идентификатором метаданных
		/// </summary>
		/// <param name="baseName">Идентификатор метаданных</param>
		/// <returns>Наименование индекса метаданных</returns>
		public string GetMetadataIndexName(string baseName)
		{
			return string.Format("{0}_{1}", _configurationId, baseName);
		    
		}

		private static string ToElasticIndexNameString(string configurationName)
		{
			return configurationName.Replace(".", "").ToLowerInvariant();
		}
	}
}
