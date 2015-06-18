using System;
using System.Collections.Generic;
using System.Text;
using InfinniPlatform.Api.ContextTypes;
using InfinniPlatform.Api.Dynamic;
using InfinniPlatform.Api.Packages;
using InfinniPlatform.Api.Packages.ConfigStructure;
using InfinniPlatform.SystemConfig.Properties;

namespace InfinniPlatform.SystemConfig.Configurator
{
	/// <summary>
	///   Обновление конфигурации 
	/// </summary>
	public sealed class ActionUnitUpdateConfigFromJson
	{
		public void Action(IUploadContext target)
		{
			var zipArchive = target.FileContent.ReadArchive(Encoding.UTF8);
			
			target.Result = new DynamicWrapper();
			target.Result.InstallLog = new List<string>();
            var folderName = (string) target.Version + "_" + Guid.NewGuid().ToString();			
		    var configExporter = new ConfigExporter(new ZipStructure(zipArchive, folderName));
		    dynamic config = configExporter.ImportHeaderFromStructure((string) target.Version);
            target.Result.InstallLog.Add(string.Format("Configuration \"{0}\"(version: \"{1}\") sucessfully installed", config.Name, string.IsNullOrEmpty(target.Version) ? "0" : target.Version ));
			target.Result.ConfigurationId = config.Name;					
		}


	}
}
