using System;
using System.Collections.Generic;
using System.Linq;

using InfinniPlatform.Api.Dynamic;
using InfinniPlatform.Api.Metadata.ConfigurationManagers.Standard.SchemaReaders;
using Newtonsoft.Json.Linq;

namespace InfinniPlatform.Api.Metadata.ConfigurationManagers.Standard.MetadataReaders
{
	public sealed class MetadataReaderConfigurationElement : MetadataReader
	{
	    private readonly IMetadataContainerInfo _metadataContainerInfo;

		public MetadataReaderConfigurationElement(string version, string configurationId, IMetadataContainerInfo metadataContainerInfo)
			: base(version, configurationId, metadataContainerInfo.GetMetadataTypeName())
		{
		    _metadataContainerInfo = metadataContainerInfo;
		}

	    /// <summary>
		///   Получить метаданные объекта в кратком виде (ссылки на метаданные объектов конфигурации)
		/// </summary>
		/// <returns>Список описаний метаданных объекта в кратком формате</returns>
		public override IEnumerable<dynamic> GetItems()
		{

			dynamic result = QueryMetadata.QueryConfiguration(Version, QueryMetadata.GetConfigurationMetadataShortListIql(ConfigurationId, _metadataContainerInfo.GetMetadataContainerName())).FirstOrDefault();
			if (result != null)
			{
                var searchResult = result[_metadataContainerInfo.GetMetadataContainerName()];

			    if (searchResult != null)
			    {
			        return DynamicWrapperExtensions.ToEnumerable(searchResult);
			    }

			}
			return new List<dynamic>();

		}

	    /// <summary>
	    ///   Получить метаданные конкретного объекта
	    /// </summary>
	    /// <param name="metadataName">наименование объекта</param>
	    /// <returns>Метаданные объекта конфигурации</returns>
	    public override dynamic GetItem(string metadataName)
		{

            var result = QueryMetadata.QueryConfiguration(Version, QueryMetadata.GetConfigurationMetadataByNameIql(ConfigurationId, metadataName, _metadataContainerInfo.GetMetadataContainerName(), _metadataContainerInfo.GetMetadataTypeName())).FirstOrDefault();


			if (result != null)
			{
				dynamic metadataList = result[_metadataContainerInfo.GetMetadataContainerName()];

				dynamic resultItem = metadataList != null && metadataList.Count > 0
						   ? metadataList[0][string.Format("{0}Full", _metadataContainerInfo.GetMetadataTypeName())]
						   : null;

				if (resultItem != null)
				{
					ConvertStringToJsonProperties(resultItem);
				}

				return resultItem;
			}
			return null;
		}

		private void ConvertStringToJsonProperties(dynamic resultItem)
		{
			SchemaConverter.ConvertStringToJsonProperties(resultItem);

			if (_metadataContainerInfo.GetMetadataTypeName() == MetadataType.Report && resultItem != null)
			{
				dynamic report = resultItem.Content;
				if (report != null && report.StringifiedJson != null && report.StringifiedJson == true)
				{
					try
					{
						dynamic jsonReport = DynamicWrapperExtensions.ToDynamic((string)report.JsonString);
						resultItem.Content = jsonReport;
					}
					catch (Exception e)
					{
						throw new Exception(string.Format("Can't parse report content: {0}", e.Message));
					}

				}
			}
		}
	}
}