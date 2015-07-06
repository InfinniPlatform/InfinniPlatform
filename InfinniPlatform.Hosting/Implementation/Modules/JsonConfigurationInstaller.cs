using System;
using System.Collections.Generic;
using System.Linq;
using InfinniPlatform.Api.Context;
using InfinniPlatform.Api.Hosting;
using InfinniPlatform.Api.Index;
using InfinniPlatform.Api.Metadata;
using InfinniPlatform.Api.Metadata.ConfigurationManagers.Standard.SchemaReaders;
using InfinniPlatform.Api.RestQuery;
using InfinniPlatform.Hosting.Implementation.ExtensionPointHandling;
using InfinniPlatform.Logging;
using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.Sdk.Environment;
using InfinniPlatform.Sdk.Environment.Hosting;
using InfinniPlatform.Sdk.Environment.Index;
using InfinniPlatform.Sdk.Environment.Metadata;

namespace InfinniPlatform.Hosting.Implementation.Modules
{
    /// <summary>
    ///     Установщик конфигурации метаданных
    /// </summary>
    public sealed class JsonConfigurationInstaller
    {
        private readonly IEnumerable<dynamic> _documents;
        private readonly IEnumerable<dynamic> _generators;
        private readonly IEnumerable<dynamic> _menu;
        private readonly IEnumerable<dynamic> _printViews;
        private readonly IEnumerable<dynamic> _processes;
        private readonly IEnumerable<dynamic> _registers;
        private readonly IEnumerable<dynamic> _scenario;
        private readonly IEnumerable<dynamic> _services;
        private readonly IEnumerable<dynamic> _validationErrors;
        private readonly IEnumerable<dynamic> _validationWarnings;
        private readonly IEnumerable<dynamic> _views;

        public JsonConfigurationInstaller(
            IEnumerable<dynamic> documents,
            IEnumerable<dynamic> menu,
            IEnumerable<dynamic> scenario,
            IEnumerable<dynamic> processes,
            IEnumerable<dynamic> services,
            IEnumerable<dynamic> generators,
            IEnumerable<dynamic> views,
            IEnumerable<dynamic> printViews,
            IEnumerable<dynamic> validationErrors,
            IEnumerable<dynamic> validationWarnings,
            IEnumerable<dynamic> registers
            )
        {
            _documents = documents;
            _menu = menu;
            _scenario = scenario;
            _processes = processes;
            _services = services;
            _generators = generators;
            _views = views;
            _printViews = printViews;
            _validationErrors = validationErrors;
            _validationWarnings = validationWarnings;
            _registers = registers;
        }

        /// <summary>
        ///     Установить конфигурацию из JSON-файла
        /// </summary>
        /// <param name="metadataConfiguration">Конфигурация метаданных</param>
        public void InstallConfiguration(IMetadataConfiguration metadataConfiguration)
        {
            var scriptConfiguration = metadataConfiguration.ScriptConfiguration;

            scriptConfiguration.ModuleName = metadataConfiguration.ConfigurationId;


            foreach (
                var document in
                    _documents.Where(
                        d =>
                            d.ConfigId == metadataConfiguration.ConfigurationId &&
                            d.Version == metadataConfiguration.Version).ToList())
            {
                try
                {
                    RegisterSchema(metadataConfiguration, document);

                    RegisterScenario(
                        _scenario.Where(
                            sc =>
                                sc.DocumentId == document.Name && sc.ConfigId == metadataConfiguration.ConfigurationId &&
                                sc.Version == metadataConfiguration.Version).ToList(),
                        metadataConfiguration, document.Name);

                    SetCommonOptions(metadataConfiguration, document);

                    RegisterProcesses(
                        _processes.Where(
                            pr =>
                                pr.DocumentId == document.Name && pr.ConfigId == metadataConfiguration.ConfigurationId &&
                                pr.Version == metadataConfiguration.Version).ToList(),
                        metadataConfiguration, document.Name);

                    RegisterServices(
                        _services.Where(
                            pr =>
                                pr.DocumentId == document.Name && pr.ConfigId == metadataConfiguration.ConfigurationId &&
                                pr.Version == metadataConfiguration.Version).ToList(),
                        metadataConfiguration, document.Name);

                    RegisterGenerators(
                        _generators.Where(
                            pr =>
                                pr.DocumentId == document.Name && pr.ConfigId == metadataConfiguration.ConfigurationId &&
                                pr.Version == metadataConfiguration.Version).ToList(),
                        metadataConfiguration, document.Name);

                    RegisterViews(
                        _views.Where(
                            pr =>
                                pr.DocumentId == document.Name && pr.ConfigId == metadataConfiguration.ConfigurationId &&
                                pr.Version == metadataConfiguration.Version).ToList(),
                        metadataConfiguration, document.Name);

                    RegisterPrintViews(
                        _printViews.Where(
                            pr =>
                                pr.DocumentId == document.Name && pr.ConfigId == metadataConfiguration.ConfigurationId &&
                                pr.Version == metadataConfiguration.Version).ToList(),
                        metadataConfiguration, document.Name);

                    RegisterValidators(
                        _validationErrors.Where(
                            pr =>
                                pr.DocumentId == document.Name && pr.ConfigId == metadataConfiguration.ConfigurationId &&
                                pr.Version == metadataConfiguration.Version).ToList(),
                        metadataConfiguration, document.Name, true);

                    RegisterValidators(
                        _validationWarnings.Where(
                            pr => pr.DocumentId == document.Name && pr.ConfigId == metadataConfiguration.ConfigurationId)
                            .ToList(),
                        metadataConfiguration, document.Name, false);
                }
                catch (Exception e)
                {
                    Logger.Log.Error("Error construct document metadata \"{0}\"", document.Name);
                }
            }

            var registerList = _registers.Where(r => r.ConfigId == metadataConfiguration.ConfigurationId);

            foreach (var register in registerList)
            {
                metadataConfiguration.RegisterRegister(register);
            }

            metadataConfiguration.RegisterMenu(_menu.Where(m => m.ConfigId == metadataConfiguration.ConfigurationId));
        }

        private void RegisterGenerators(IEnumerable<dynamic> generators, IMetadataConfiguration metadataConfiguration,
            string metadataName)
        {
            foreach (var generator in generators)
            {
                try
                {
                    metadataConfiguration.RegisterGenerator(metadataName, generator);

                    Logger.Log.Info("Config:{0}, Document: {1}, Generator: {2} registered",
                        metadataConfiguration.ConfigurationId, metadataName, generator.Name);
                }
                catch (Exception e)
                {
                    Logger.Log.Error("Fail to register generator: {0}. Error: {1}", generator.Name, e.Message);
                }
            }
        }

        private void RegisterValidators(IEnumerable<dynamic> validators, IMetadataConfiguration metadataConfiguration,
            string metadataName, bool isError)
        {
            foreach (var validator in validators)
            {
                try
                {
                    if (isError)
                    {
                        metadataConfiguration.RegisterValidationError(metadataName, validator);
                    }
                    else
                    {
                        metadataConfiguration.RegisterValidationWarning(metadataName, validator);
                    }

                    Logger.Log.Info("Config:{0}, Document: {1}, Validator: {2} registered",
                        metadataConfiguration.ConfigurationId, metadataName, validator.Name);
                }
                catch (Exception e)
                {
                    Logger.Log.Error("Fail to register validator: {0}. Error: {1}", validator.Name, e.Message);
                }
            }
        }

        private void RegisterViews(IEnumerable<dynamic> views, IMetadataConfiguration metadataConfiguration,
            string metadataName)
        {
            foreach (var view in views)
            {
                try
                {
                    metadataConfiguration.RegisterView(metadataName, view);

                    Logger.Log.Info("Config: {0}, Document: {1}, View: {2} registered",
                        metadataConfiguration.ConfigurationId, metadataName, view.Name);
                }
                catch (Exception e)
                {
                    Logger.Log.Error("Fail to register View: {0}. Error: {1}", view.Name, e.Message);
                }
            }
        }

        private void RegisterPrintViews(IEnumerable<dynamic> printViews, IMetadataConfiguration metadataConfiguration,
            string metadataName)
        {
            foreach (var printView in printViews)
            {
                try
                {
                    metadataConfiguration.RegisterPrintView(metadataName, printView);

                    Logger.Log.Info("Config: {0}, Document: {1}, PrintView: {2} registered",
                        metadataConfiguration.ConfigurationId, metadataName, printView.Name);
                }
                catch (Exception e)
                {
                    Logger.Log.Error("Fail to register PrintView: {0}. Error: {1}", printView.Name, e.Message);
                }
            }
        }

        private void RegisterSchema(IMetadataConfiguration metadataConfiguration, dynamic document)
        {
            try
            {
                SchemaConverter.ConvertStringToJsonProperties(document);

                metadataConfiguration.SetSchemaVersion(document.Name, document.Schema);

                Logger.Log.Info("Config:{0}, Document: {1}, document schema registered",
                    metadataConfiguration.ConfigurationId, document.Name);
            }
            catch (Exception e)
            {
                Logger.Log.Error("Fail to parse document schema: {0}. Error: {1}", document.Name, e.Message);
            }
        }

        private void RegisterProcesses(IEnumerable<dynamic> processes, IMetadataConfiguration metadataConfiguration,
            string metadataName)
        {
            //настройка бизнес-процессов
            foreach (var process in processes)
            {
                metadataConfiguration.RegisterProcess(metadataName, process);
            }
        }

        /// <summary>
        ///     Регистрация сервисов конфигурации
        /// </summary>
        /// <param name="services">Список сервисов</param>
        /// <param name="metadataConfiguration">Конфигурация метаданных</param>
        /// <param name="metadataName">Наименование объекта метаданных</param>
        private static void RegisterServices(IEnumerable<dynamic> services, IMetadataConfiguration metadataConfiguration,
            string metadataName)
        {
            foreach (var service in services)
            {
                try
                {
                    metadataConfiguration.RegisterService(metadataName, service);

                    dynamic localService = service;
                    Action<IExtensionPointHandlerInstance> instanceAction = ia =>
                    {
                        foreach (
                            var extensionPoint in DynamicWrapperExtensions.ToEnumerable(localService.ExtensionPoints))
                        {
                            ia.RegisterExtensionPoint(extensionPoint.TypeName.Name, extensionPoint.ScenarioId);
                        }
                    };

                    Action<IServiceRegistration> registrationAction =
                        reg =>
                            ExtensionPointHandlerExtensions.RegisterHandlerInstance(reg, localService.Name,
                                instanceAction)
                                .SetResultHandler(HttpResultHandlerType.BadRequest);

                    metadataConfiguration.ServiceRegistrationContainer.AddRegistration(metadataName, service.Type.Name,
                        registrationAction);

                    Logger.Log.Info("Config:{0}, Document{1}, Service: {2} registered",
                        metadataConfiguration.ConfigurationId, metadataName, service.Name);
                }
                catch (Exception e)
                {
                    Logger.Log.Error("Fail to register service: {0}. Error: {1}", service.Name, e.Message);
                }
            }
        }

        /// <summary>
        ///     Установить общие свойства метаданных
        /// </summary>
        /// <param name="metadataConfiguration">Конфигурация метаданных</param>
        /// <param name="fullDocument">Объект метаданных</param>
        private static void SetCommonOptions(IMetadataConfiguration metadataConfiguration, dynamic fullDocument)
        {
            try
            {
                //настройка общих свойств метаданных

                if (fullDocument != null && fullDocument.SearchAbility != null)
                {
                    metadataConfiguration.SetSearchAbilityType(fullDocument.Name,
                        (SearchAbilityType) fullDocument.SearchAbility);
                }
                else
                {
                    metadataConfiguration.SetSearchAbilityType(fullDocument.Name, SearchAbilityType.KeywordBasedSearch);
                }

                if (fullDocument != null && fullDocument.MetadataIndex != null)
                {
                    metadataConfiguration.SetMetadataIndexType(fullDocument.Name, fullDocument.MetadataIndex);
                }

                Logger.Log.Info("Config:{0}, Common document registered", metadataConfiguration.ConfigurationId);
            }
            catch (Exception e)
            {
                Logger.Log.Error("Fail to register common metadata options: {0}. Error: {1}", fullDocument.Name,
                    e.Message);
            }
        }

        /// <summary>
        ///     Регистрация сценариев
        /// </summary>
        /// <param name="metadataConfiguration">Метаданные конфигурации</param>
        /// <param name="metadataName"></param>
        /// <param name="scenarios">Список регистрируемых сценариев</param>
        private static void RegisterScenario(IEnumerable<dynamic> scenarios,
            IMetadataConfiguration metadataConfiguration, string metadataName)
        {
            //регистрируем скрипты
            foreach (var scenario in scenarios)
            {
                try
                {
                    metadataConfiguration.RegisterScenario(metadataName, scenario);


                    if ((ScriptUnitType) scenario.ScriptUnitType == ScriptUnitType.Action)
                    {
                        metadataConfiguration.ScriptConfiguration.RegisterActionUnitDistributedStorage(scenario.Id,
                            scenario.ScenarioId, metadataConfiguration.Version);
                    }
                    else
                    {
                        metadataConfiguration.ScriptConfiguration.RegisterValidationUnitDistributedStorage(scenario.Id,
                            scenario.ScenarioId,
                            metadataConfiguration.Version);
                    }
                    Logger.Log.Info("Config:{0}, Document{1}, Scenario: {2} registered",
                        metadataConfiguration.ConfigurationId, metadataName, scenario.Name);
                }
                catch (Exception e)
                {
                    Logger.Log.Error("Fail to register scenario: {0}. Error: {1}", scenario.Name, e.Message);
                }
            }
        }
    }
}