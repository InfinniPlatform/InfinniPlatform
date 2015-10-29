using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using InfinniPlatform.Api.RestApi.DataApi;
using InfinniPlatform.Api.Serialization;
using InfinniPlatform.Sdk.Dynamic;

namespace InfinniPlatform.UserInterface.Services.Metadata
{
    /// <summary>
    ///     Сервис для работы с метаданными сборок.
    /// </summary>
    internal sealed class AssemblyMetadataService : BaseMetadataService
    {
        private readonly string _configId;
        
        public AssemblyMetadataService(string version, string configId, string server, int port, string route)
            : base(version, server, port, route)
        {
            _configId = configId;
        }

        public string ConfigId
        {
            get { return _configId; }
        }


        public override object CreateItem()
        {
			dynamic assembly = new DynamicWrapper();

			assembly.Id = Guid.NewGuid().ToString();
			assembly.Name = string.Empty;
			assembly.Caption = string.Empty;
			assembly.Description = string.Empty;

	        return assembly;
        }

        public override void ReplaceItem(dynamic item)
        {
			dynamic configuration = PackageMetadataLoader.Configurations[_configId];
			IEnumerable<object> assemblies = configuration.Content.Assemblies;

	        var newAssembliesList = new List<object>(assemblies) { item };

	        configuration.Content.Assemblies = newAssembliesList;

			var filePath = configuration.FilePath;

			var serializedItem = JsonObjectSerializer.Formated.Serialize(configuration.Content);

			File.WriteAllBytes(filePath, serializedItem);

			PackageMetadataLoader.UpdateCache();
        }

	    public override void DeleteItem(string itemId)
	    {
		    dynamic configuration = PackageMetadataLoader.Configurations[_configId];
		    IEnumerable<dynamic> assemblies = configuration.Content.Assemblies;

		    var newAssembliesList = assemblies.Where(assembly => assembly.Name != itemId)
											  .ToArray();

		    configuration.Content.Assemblies = newAssembliesList;

		    var filePath = configuration.FilePath;

		    var serializedItem = JsonObjectSerializer.Formated.Serialize(configuration.Content);

		    File.WriteAllBytes(filePath, serializedItem);

		    PackageMetadataLoader.UpdateCache();
	    }

        public override object GetItem(string itemId)
        {
	        dynamic configuration = PackageMetadataLoader.Configurations[_configId];
	        IEnumerable<dynamic> assemblies = configuration.Content.Assemblies;
	        return assemblies.FirstOrDefault(assembly => assembly.Name == itemId);
        }

        public override IEnumerable<object> GetItems()
        {
			dynamic configuration = PackageMetadataLoader.Configurations[_configId];
			return configuration.Content.Assemblies;
        }
    }
}