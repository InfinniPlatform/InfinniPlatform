using System.Collections;
using System.Linq;

using InfinniPlatform.Api.Metadata;
using InfinniPlatform.Api.Metadata.ConfigurationManagers.Standard.Factories;

namespace InfinniPlatform.UserInterface.Services.Metadata
{
	/// <summary>
	/// Сервис для работы с метаданными конфигурации.
	/// </summary>
	sealed class ConfigurationMetadataService : BaseMetadataService
	{
	    private readonly string _version;

	    public ConfigurationMetadataService(string version)
	    {
	        _version = version;
            
	    }

	    public override IEnumerable GetItems()
		{
			var items = base.GetItems();

			if (items != null)
			{
				items = items.Cast<dynamic>().OrderBy(i => i.Name).ToArray();
			}

			return items;
		}

		protected override IDataReader CreateDataReader()
		{
			return ManagerFactoryConfiguration.BuildConfigurationMetadataReader(_version,true);
		}

		protected override IDataManager CreateDataManager()
		{
			return ManagerFactoryConfiguration.BuildConfigurationManager(_version);
		}
	}
}