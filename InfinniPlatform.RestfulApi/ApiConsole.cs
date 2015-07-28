using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using InfinniPlatform.Api.Hosting;
using InfinniPlatform.Api.Metadata.ConfigurationManagers.File;
using InfinniPlatform.Api.Packages;
using InfinniPlatform.Api.RestApi.CommonApi;
using InfinniPlatform.Api.Settings;
using InfinniPlatform.Api.TestEnvironment;
using InfinniPlatform.Hosting;
using InfinniPlatform.Hosting.Implementation;
using InfinniPlatform.Modules;
using InfinniPlatform.RestfulApi.Extensions;
using InfinniPlatform.RestfulApi.Properties;
using InfinniPlatform.Sdk.Api;
using InfinniPlatform.SystemConfig.Initializers;

namespace InfinniPlatform.RestfulApi
{
    public sealed class ApiConsole
    {
        private IHostingService _service;

        private IHostingService ConstructServer(string configurationList, HostingConfig hostingConfig)
        {
            string modules = string.Join(",",
                                         "InfinniPlatform.SystemConfig,InfinniPlatform.Metadata,InfinniPlatform.Update,InfinniPlatform.RestfulApi",
                                         configurationList);
           

            var factory = new OwinHostingServiceFactory(ModuleExtension.LoadModulesAssemblies(modules), null,
                                                        hostingConfig);

            _service = factory.CreateHostingService();

            //заполняем зависимости глобального контекста исполнения пользовательских скриптов
            factory.InfinniPlatformHostServer.RegisterServerInitializer<GlobalContextInitializer>();


            //устанавливаем системные конфигурации
            factory.InfinniPlatformHostServer.RegisterServerInitializer<SystemConfigurationsInitializer>();
            //устанавливаем конфигурации из JSON-описаний

            factory.InfinniPlatformHostServer.RegisterServerInitializer<JsonConfigurationsInitializer>();

            //пользовательские обработчики бизнес-логики старта сервера
            factory.InfinniPlatformHostServer.RegisterServerInitializer<UserLogicInitializer>();

            //обработчик для создания хранилища пользователей
            factory.InfinniPlatformHostServer.RegisterServerInitializer<UserStorageInitializer>();

            return _service;
        }

        public void Run(TestServerParameters parameters)
        {
            FastStorageExtension.CreateBlobStorage();
            FastStorageExtension.CreateEventStorage();

            string configList = string.Join(",", parameters.ConfigurationAssemblies);


            if (parameters.WaitForDebugAttach)
            {
                Console.WriteLine(@"Wait for attach debug");
                Console.ReadKey();
            }


            _service = ConstructServer(string.Join(",", configList),
                                       parameters.HostingConfig);

            _service.Start();


            Console.WriteLine(@"Server started at {0}", parameters.GetServerBaseAddress());


            if (parameters.RedirectConsoleToFileOutput)
            {
                var standardOutput = new StreamWriter(Path.GetFullPath("Console.log"), true, Encoding.UTF8)
                    {
                        AutoFlush = true
                    };
                Console.SetOut(standardOutput);
            }

            var packageBuilder = new PackageBuilder();

            IEnumerable<AssemblyInfo> assemblies = ModuleExtension.LoadModules(configList);
            foreach (AssemblyInfo assembly in assemblies)
            {
                IEnumerable<string> installResult = assembly.InstallFromAssembly(packageBuilder, null);
                foreach (string result in installResult)
                {
                    Console.WriteLine(@"Assembly configuration installed: {0}", result);
                }
            }

            if (parameters.Configurations.Any())
            {
                foreach (ConfigurationInfo configurationInfo in parameters.Configurations)
                {
                    InstallJsonPackage(configurationInfo, packageBuilder, parameters.RealConfigNeeds);
                }
            }
        }

        public static void InstallJsonPackage(ConfigurationInfo configurationInfo, PackageBuilder packageBuilder,
                                              bool realConfigNeeds)
        {
            string configurationId = ExtractConfigurationId(configurationInfo.ConfigurationFilePath);

            if (realConfigNeeds)
            {
                //системные конфигурации в любой момент времени существуют в единственном экземпляре, поэтому указываем версию null
                dynamic result =
                    new UpdateApi(null).UpdateConfigFromJson(
                        GetFullPathToConfiguration(configurationInfo.ConfigurationFilePath));
                Console.WriteLine(@"------Install configuration log------------");

                IEnumerable<string> log = result.Result.InstallLog.ToArray();
                foreach (string logString in log)
                {
                    Console.WriteLine(logString);
                }

                Console.WriteLine(string.Format("-----JSON Config \"{0}\" installed----------------",
                                                result.Result.ConfigurationId));
            }

            if (!configurationInfo.AppliedAssemblyList.Any())
            {
                RestQueryApi.QueryPostNotify(null, configurationId);
            }

            foreach (string appliedAssembly in configurationInfo.AppliedAssemblyList)
            {
                dynamic package = packageBuilder.BuildPackage(configurationId, null, appliedAssembly);

                new UpdateApi(null).InstallPackages(new[] {package});
                Console.WriteLine(@"Assembly ""{0}"" installed", appliedAssembly);
            }
        }

        /// <summary>
        ///     Получить идентификатор конфигурации из указанного файла архива
        /// </summary>
        /// <param name="configurationFileName">Наименование файла архива конфигурации</param>
        /// <returns>Идентификатор конфигурации</returns>
        private static string ExtractConfigurationId(string configurationFileName)
        {
            string pathToConfigFiles = GetPathToConfigFiles();
            var jsonFileConfigManager = new JsonFileConfigManager(pathToConfigFiles);

            // HOTFIX: если не сделать чтение конфигураций, метод GetJsonFileConfigByFileName вернет null
            // До конца не уверен, что чтение конфигураций нужно делать на этом этапе.
            jsonFileConfigManager.ReadConfigurations();

            dynamic jsonConfig = jsonFileConfigManager.GetJsonFileConfigByFileName(configurationFileName);
            if (jsonConfig != null)
            {
                return jsonConfig.Name;
            }
            throw new ArgumentException(string.Format(Resources.ConfigurationFileNotFound, configurationFileName,
                                                      pathToConfigFiles));
        }

        private static string GetPathToConfigFiles()
        {
            string appSettingsPath = AppSettings.GetValue("ConfigurationPath");
            string pathToConfigFiles = appSettingsPath != null
                                           ? Path.GetFullPath(appSettingsPath)
                                           : Directory.GetCurrentDirectory();
            return pathToConfigFiles;
        }

        private static string GetFullPathToConfiguration(string configurationFileName)
        {
            return Path.Combine(GetPathToConfigFiles(), configurationFileName);
        }
    }
}