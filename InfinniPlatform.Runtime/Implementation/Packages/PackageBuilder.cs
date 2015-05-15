using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using InfinniPlatform.Api.Dynamic;
using InfinniPlatform.Api.Events;
using InfinniPlatform.Api.Packages;
using InfinniPlatform.Api.Properties;
using InfinniPlatform.Compression;
using InfinniPlatform.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace InfinniPlatform.Runtime.Implementation.Packages
{
    /// <summary>
    ///   Конструктор пакетов обновления
    /// </summary>
    public sealed class PackageBuilder 
    {

        public dynamic BuildPackage(string configurationName, string versionIdentifier,
                                          string fileLocation)
        {
	        dynamic package = new DynamicInstance(new
		                      {

					                ModuleId = Path.GetFileName(fileLocation),
					                ConfigurationName = configurationName,										
									Version = versionIdentifier,
									Assembly = Convert.ToBase64String(File.ReadAllBytes(fileLocation))									
		                      });
	        var pdbFileName = Path.GetFileNameWithoutExtension(fileLocation) + ".pdb";
	        var pathToPdb = Path.Combine(Path.GetDirectoryName(fileLocation), pdbFileName);
			if (File.Exists(pathToPdb))
			{
				package.PdbFile = Convert.ToBase64String(File.ReadAllBytes(fileLocation));
			}

	        return package;
        }



    }




}
