using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using InfinniPlatform.Api.ContextTypes;
using InfinniPlatform.Api.Dynamic;
using InfinniPlatform.Api.Packages;
using InfinniPlatform.Api.Settings;
using InfinniPlatform.Api.SystemExtensions;
using InfinniPlatform.Hosting.Implementation;

namespace InfinniConfiguration.SystemConfig.Configurator
{
    /// <summary>
    ///   Загружает переданную сборку на сервер, так что она становится доступной 
    ///   для загрузки скриптовых модулей
    /// </summary>
    public sealed class ActionUnitGetAssemblyList
    {
		public void Action(IUploadContext target)
		{

            var zipArchive = target.FileContent.ReadArchive(Encoding.Default);
            var entries = zipArchive.GetZipEntries();
            var folderName = DateTime.Now.ToString("yyyyMMddHHmmss");

            var configurationList = new List<dynamic>();
            foreach (var entry in entries)
            {
                zipArchive.UnzipFile(entry, stream =>
                {
                    var assemblyVersionPath = AppSettings.GetValue("AssemblyVersionPath");

                    var pathToFileAssmbly = Path.Combine(assemblyVersionPath, folderName, entry);

                    if (pathToFileAssmbly.EndsWith(".exe") || pathToFileAssmbly.EndsWith(".dll"))
                    {
                        //распаковываем в текущую директорию сборку 
                        stream.SaveToFile(pathToFileAssmbly);

                        var assembly = Assembly.Load(File.ReadAllBytes(pathToFileAssmbly));
                        IEnumerable<dynamic> installers = assembly.GetInstallers().ToEnumerable();

                        foreach (var installer in installers)
                        {
                            dynamic entryResult = new DynamicInstance();
                            entryResult.ConfigurationName = installer.ConfigurationName;
                            entryResult.AssemblyPath = entry;

                            configurationList.Add(entryResult);
                        }
                    }
                });
            }


            target.Result = configurationList;

		}
    }
}
