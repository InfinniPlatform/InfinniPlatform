using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

using InfinniPlatform.Api.Metadata;
using InfinniPlatform.Api.SearchOptions;
using InfinniPlatform.Logging;
using InfinniPlatform.Metadata;
using InfinniPlatform.Runtime.Properties;
using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.Sdk.Environment;
using InfinniPlatform.Sdk.Environment.Index;
using InfinniPlatform.Sdk.Environment.Metadata;
using InfinniPlatform.Sdk.Environment.Settings;

namespace InfinniPlatform.Runtime.Implementation.AssemblyDispatch
{
	/// <summary>
	/// Загрузчик версий конфигурационных объектов.
	/// </summary>
	public class AssemblyVersionLoader : IVersionLoader
	{
		private readonly IConfigurationObjectBuilder _configurationObjectBuilder;

		public AssemblyVersionLoader(IConfigurationObjectBuilder configurationObjectBuilder)
		{
			_configurationObjectBuilder = configurationObjectBuilder;
		}


		private static bool IsSystemConfig(string configurationId)
		{
			if (string.Equals(configurationId, "systemconfig", StringComparison.OrdinalIgnoreCase)
				|| string.Equals(configurationId, "restfulapi", StringComparison.OrdinalIgnoreCase)
				|| string.Equals(configurationId, "update", StringComparison.OrdinalIgnoreCase))
			{
				return true;
			}

			return false;
		}

		private IEnumerable<dynamic> LoadVersions(string version, string configurationId)
		{
		    //функционал всех версий конфигурации 'update' одинаков, можно не указывать версию
            var configurationObject = _configurationObjectBuilder.GetConfigurationObject(null, "update");
			var packageProvider = configurationObject.GetDocumentProvider("package",version, null);

			// если индекс не существует, конфигурация еще не развернута
			if (packageProvider == null)
			{
				return new List<dynamic>();
			}

			dynamic criteria = new DynamicWrapper();
			criteria.Property = "ConfigurationName";
			criteria.Value = configurationId;
			criteria.CriteriaType = CriteriaType.IsEquals;

		    dynamic criteriaVersion = new DynamicWrapper();
            criteriaVersion.Property = "Version";
            criteriaVersion.Value = version;
            criteriaVersion.CriteriaType = CriteriaType.IsEquals;


			// получаем актуальные версии сохраненных сборок конфигурации
			IEnumerable<dynamic> searchResult = DynamicWrapperExtensions.ToEnumerable(packageProvider.GetDocument(new[] { criteria, criteriaVersion }, 0, 10000));

			var blobStorage = configurationObject.GetBlobStorage();

			foreach (var document in searchResult)
			{
				// если системная конфигурация, загружаем ее из текущей папки сервера
				if (IsSystemConfig(configurationId))
				{
					var assemblyFile = Directory.GetFiles(Directory.GetCurrentDirectory()).FirstOrDefault(f => string.Equals(Path.GetFileName(f), document.ModuleId, StringComparison.OrdinalIgnoreCase));

					if (assemblyFile != null)
					{
						document.Assembly = File.ReadAllBytes(assemblyFile);
						continue;
					}

					throw new ArgumentException(string.Format("System metadata assembly: {0} not found. Current location: {1}", configurationId, Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)));
				}

				if (document.ContentId == null)
				{
					throw new ArgumentException(string.Format(Resources.AssemblyMetadataNotFound, document.ModuleId));
				}

				var documentAssembly = blobStorage.GetBlobData(new Guid(document.ContentId));

				if (documentAssembly != null)
				{
					document.Assembly = documentAssembly.Data;
				}

				if (document.Assembly == null)
				{
					Logger.Log.Info("Assembly content not found: {0}", document.ModuleId);
				}

				if (document.PdbId != null)
				{
					var documentPdb = blobStorage.GetBlobData(new Guid(document.PdbId));

					if (documentPdb != null)
					{
						document.Pdb = documentPdb.Data;
					}
				}
			}

			return searchResult;
		}

		public MethodInvokationCacheList RefreshInvokationCache(string version, string metadataConfigurationId, MethodInvokationCacheList invokationCacheList)
		{
			var assemblyVersionPath = AppSettings.GetValue("AssemblyVersionPath");

			if (string.IsNullOrEmpty(assemblyVersionPath))
			{
				throw new ArgumentException(Resources.AssemblyVersionRepositoryNotSpecified);
			}

            var versionAssemblies = LoadVersions(version, metadataConfigurationId).ToList();

			foreach (var configurationVersion in versionAssemblies)
			{
				var assemblyResult = new List<Assembly>();

				var directory = Directory.CreateDirectory(Path.Combine(assemblyVersionPath, string.Format("{0}_{1}", configurationVersion.Version, DateTime.Now.ToString("yyyyMMddHHmmss")))).FullName;

				if (configurationVersion.Assembly != null)
				{
					LoadAppliedAssemblies(directory, configurationVersion, assemblyResult);

				    MethodInvokationCache cacheExisting = invokationCacheList.GetCache(configurationVersion.Version, false);

					if (cacheExisting == null)
					{
						invokationCacheList.AddCache(new MethodInvokationCache(configurationVersion.Version,
																				 configurationVersion.TimeStamp,
																				 assemblyResult));
					}
					else
					{                        
						cacheExisting.AddVersionAssembly(assemblyResult);
					}
				}
			}

			return invokationCacheList;
		}

	    public MethodInvokationCacheList ConstructInvokationCache(string version, string metadataConfigurationId)
	    {
	        return RefreshInvokationCache(version, metadataConfigurationId,new MethodInvokationCacheList());
	    }

        public void UpdateInvokationCache(string version, string metadataConfigurationId, MethodInvokationCacheList versionCacheList)
        {
            RefreshInvokationCache(version, metadataConfigurationId, versionCacheList);
        }

	    private static void LoadAppliedAssemblies(dynamic directory, dynamic configurationVersion, List<Assembly> assemblyResult)
	    {
	        var assemblyLocation = Path.Combine(directory, configurationVersion.ModuleId);
	        var pdbLocation = Path.Combine(directory, Path.GetFileNameWithoutExtension(configurationVersion.ModuleId) + ".pdb");

	        try
	        {
	            File.WriteAllBytes(assemblyLocation, configurationVersion.Assembly);

	            if (configurationVersion.Pdb != null)
	            {
	                File.WriteAllBytes(pdbLocation, configurationVersion.Pdb);
	            }
	        }
	        catch (Exception)
	        {
	            throw new ArgumentException(string.Format("Cannot write file in location: {0}.", assemblyLocation));
	        }

	        // Желательно загрузить еще pdb file, чтобы в случае возникновения исключения получить информацию о строке,
	        // в которой это исключение произошло
	        assemblyResult.Add(configurationVersion.Pdb == null
	            ? Assembly.Load(File.ReadAllBytes(assemblyLocation))
	            : Assembly.Load(File.ReadAllBytes(assemblyLocation), File.ReadAllBytes(pdbLocation)));
	    }


	}
}