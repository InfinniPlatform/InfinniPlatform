using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using InfinniPlatform.Api.Index;
using InfinniPlatform.Api.Metadata;
using InfinniPlatform.Api.Metadata.ConfigurationManagers.Standard.Factories;
using InfinniPlatform.ContextComponents;
using InfinniPlatform.Hosting;
using InfinniPlatform.Hosting.Implementation.Modules;
using InfinniPlatform.Metadata;
using InfinniPlatform.Runtime;
using InfinniPlatform.Sdk.ContextComponents;
using InfinniPlatform.SystemConfig.Multitenancy;
using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.Sdk.Environment;
using InfinniPlatform.Sdk.Environment.Index;
using InfinniPlatform.Sdk.Environment.Metadata;
using InfinniPlatform.WebApi.Factories;
using Newtonsoft.Json.Linq;

namespace InfinniPlatform.SystemConfig.Initializers
{
    public sealed class JsonConfigurationsInitializer : IStartupInitializer
    {
        private readonly IChangeListener _changeListener;
        private readonly IIndexFactory _indexFactory;
        private readonly ISecurityComponent _securityComponent;
        private readonly IMetadataConfigurationProvider _metadataConfigurationProvider;

        public JsonConfigurationsInitializer(IChangeListener changeListener, IIndexFactory indexFactory, ISecurityComponent securityComponent,
                                             IMetadataConfigurationProvider metadataConfigurationProvider)
        {
            _changeListener = changeListener;
            _indexFactory = indexFactory;
            _securityComponent = securityComponent;
            _metadataConfigurationProvider = metadataConfigurationProvider;
        }


        public void OnStart(HostingContextBuilder contextBuilder)
        {
            IDataReader managerApi = ManagerFactoryConfiguration.BuildConfigurationMetadataReader(null, true);
            IEnumerable<dynamic> configurations = managerApi.GetItems();

            _changeListener.RegisterOnChange("JsonConfig",
                                             (version, configurationId) => OnChangeModules(version, configurationId));
            foreach (dynamic configuration in configurations)
            {
                InstallConfiguration(configuration.Version, configuration.Name);
            }
        }

        private void InstallConfiguration(string version, string configurationId)
        {
            IMetadataConfiguration metadataConfig =
                InfinniPlatformHostServer.Instance.CreateConfiguration(version, configurationId, false);


            JsonConfigurationInstaller installer = CreateJsonConfigurationInstaller(version);

            installer.InstallConfiguration(metadataConfig);
            metadataConfig.ScriptConfiguration.InitActionUnitStorage(version);
            InfinniPlatformHostServer.Instance.InstallServices(version, metadataConfig.ServiceRegistrationContainer);

            if (configurationId.ToLowerInvariant() == "authorization")
            {
                _securityComponent.WarmUpAcl();
            }
        }

        private JsonConfigurationInstaller CreateJsonConfigurationInstaller(string version)
        {
			IEnumerable<dynamic> configs = _indexFactory.BuildVersionProvider("systemconfig", "metadata",MultitenancyExtensions.SystemTenant,version).GetDocument(null,0,100000);
                            
            Dictionary<string, string> configDictionary = configs.ToDictionary(c => (string) c.Id, c => (string) c.Name);

			IEnumerable<dynamic> documents = _indexFactory.BuildVersionProvider("systemconfig", "documentmetadata", MultitenancyExtensions.SystemTenant,version).GetDocument(null, 0, 1000000);

            Dictionary<string, dynamic> documentDictionary = documents.ToDictionary(c => (string) c.Id, c => c);

			IEnumerable<dynamic> registers = _indexFactory.BuildVersionProvider("systemconfig", "registermetadata", MultitenancyExtensions.SystemTenant,version).GetDocument(null, 0, 1000000);

            foreach (dynamic register in registers)
            {
                register.ConfigId = register.ParentId;
            }

			IEnumerable<dynamic> menu = _indexFactory.BuildVersionProvider("systemconfig", "menumetadata", MultitenancyExtensions.SystemTenant, version).GetDocument(null, 0, 1000000);

            foreach (dynamic menuItem in menu)
            {
                menuItem.ConfigId = menuItem.ParentId;
            }

            GetConfigurationMetadata(documents, configDictionary);
            GetConfigurationMetadata(registers, configDictionary);
            GetConfigurationMetadata(menu, configDictionary);

            IEnumerable<dynamic> scenarios = GetDocumentMetadata("scenariometadata", documentDictionary,
                                                                 configDictionary);
            IEnumerable<dynamic> processes = GetDocumentMetadata("processmetadata", documentDictionary, configDictionary);
            IEnumerable<dynamic> services = GetDocumentMetadata("servicemetadata", documentDictionary, configDictionary);
            IEnumerable<dynamic> generators = GetDocumentMetadata("generatormetadata", documentDictionary,
                                                                  configDictionary);
            IEnumerable<dynamic> views = GetDocumentMetadata("viewmetadata", documentDictionary, configDictionary);
            IEnumerable<dynamic> printViews = GetDocumentMetadata("printviewmetadata", documentDictionary,
                                                                  configDictionary);
            IEnumerable<dynamic> validationErrors = GetDocumentMetadata("validationerrormetadata", documentDictionary,
                                                                        configDictionary);
            IEnumerable<dynamic> validationWarnings = GetDocumentMetadata("validationwarningmetadata",
                                                                          documentDictionary, configDictionary);


            return new JsonConfigurationInstaller(documents, menu, scenarios, processes, services, generators, views,
                                                  printViews, validationErrors, validationWarnings, registers);
        }

        /// <summary>
        ///     Преобразовать свойства JSON, хранимые в виде строк в JSON объекты
        /// </summary>
        /// <param name="resultItem">JSON объект</param>
        private void ConvertStringToJsonProperties(string metadataType, dynamic resultItem)
        {
            if (metadataType == "viewmetadata" && resultItem != null)
            {
                dynamic layoutPanel = resultItem.LayoutPanel;
                if (layoutPanel != null && !(layoutPanel is string))
                {
                    try
                    {
                        dynamic jsonLayoutPanel = DynamicWrapperExtensions.ToDynamic((string) layoutPanel.JsonString);
                        resultItem.LayoutPanel = jsonLayoutPanel;
                    }
                    catch (Exception e)
                    {
                        throw new Exception(string.Format("Can't parse view layout panel: {0}", e.Message));
                    }
                }
                dynamic childViews = resultItem.ChildViews;
                if (childViews != null && !(childViews is string))
                {
                    try
                    {
                        dynamic jsonChildViews = DynamicWrapperExtensions.ToDynamicList((string) childViews.JsonString);
                        resultItem.ChildViews = jsonChildViews;
                    }
                    catch (Exception e)
                    {
                        throw new Exception(string.Format("Can't parse child views layout: {0}", e.Message));
                    }
                }
                dynamic dataSources = resultItem.DataSources;
                if (dataSources != null && !(dataSources is JArray) && dataSources.JsonString != null)
                {
                    try
                    {
                        dynamic jsonDataSources = DynamicWrapperExtensions.ToDynamicList((string) dataSources.JsonString);
                        resultItem.DataSources = jsonDataSources;
                    }
                    catch (Exception e)
                    {
                        throw new Exception(string.Format("Can't parse view datasources: {0}", e.Message));
                    }
                }
                dynamic parameters = resultItem.Parameters;
                if (parameters != null && !(parameters is JArray) && parameters.JsonString != null)
                {
                    try
                    {
                        dynamic jsonParameters = DynamicWrapperExtensions.ToDynamicList((string) parameters.JsonString);
                        resultItem.Parameters = jsonParameters;
                    }
                    catch (Exception e)
                    {
                        throw new Exception(string.Format("Can't parse view parameters: {0}", e.Message));
                    }
                }
            }
            if ((metadataType == "validationerrormetadata" || metadataType == "validationwarningmetadata") &&
                resultItem != null)
            {
                dynamic validationOperator = resultItem.ValidationOperator;
                if (validationOperator != null && validationOperator.StringifiedJson != null &&
                    validationOperator.StringifiedJson == true)
                {
                    try
                    {
                        dynamic jsonValidationOperator =
                            DynamicWrapperExtensions.ToDynamic((string) validationOperator.JsonString);
                        resultItem.ValidationOperator = jsonValidationOperator;
                    }
                    catch (Exception e)
                    {
                        throw new Exception(string.Format("Can't parse validation operator: {0}", e.Message));
                    }
                }
            }
            if (metadataType == "printviewmetadata" && resultItem != null)
            {
                dynamic printView = resultItem.Blocks;
                if (printView != null && printView.StringifiedJson != null && printView.StringifiedJson == true)
                {
                    try
                    {
                        dynamic jsonPrintView = DynamicWrapperExtensions.ToDynamicList((string) printView.JsonString);
                        resultItem.Blocks = jsonPrintView;
                    }
                    catch (Exception e)
                    {
                        throw new Exception(string.Format("Can't parse print view: {0}", e.Message));
                    }
                }
            }

            if (metadataType == "reportmetadata" && resultItem != null)
            {
                dynamic report = resultItem.Content;
                if (report != null && report.StringifiedJson != null && report.StringifiedJson == true)
                {
                    try
                    {
                        dynamic jsonReport = DynamicWrapperExtensions.ToDynamic((string) report.JsonString);
                        resultItem.Content = jsonReport;
                    }
                    catch (Exception e)
                    {
                        throw new Exception(string.Format("Can't parse report content: {0}", e.Message));
                    }
                }
            }
        }


        private IEnumerable<dynamic> GetConfigurationMetadata(IEnumerable<dynamic> items,
                                                              Dictionary<string, string> configDictionary)
        {
            foreach (dynamic item in items)
            {
                if (item.ParentId != null && configDictionary.ContainsKey(item.ParentId))
                {
                    item.ConfigId = configDictionary[item.ParentId];
                }
            }
            return items;
        }

        private IEnumerable<dynamic> GetDocumentMetadata(string metadataType,
                                                         Dictionary<string, dynamic> documentDictionary,
                                                         Dictionary<string, string> configDictionary)
        {
			var items = _indexFactory.BuildVersionProvider("systemconfig", metadataType, MultitenancyExtensions.SystemTenant, null).GetDocument(null,0,1000000);

            foreach (dynamic item in items)
            {
                if (item.ParentId != null && documentDictionary.ContainsKey(item.ParentId))
                {
                    item.DocumentId = documentDictionary[item.ParentId].Name;
                    if (configDictionary.ContainsKey(documentDictionary[item.ParentId].ParentId))
                    {
                        item.ConfigId = configDictionary[documentDictionary[item.ParentId].ParentId];
                    }
                    ConvertStringToJsonProperties(metadataType, item);
                }
            }
            return items;
        }

        /// <summary>
        ///     Обновление конфигурации при получении события обновления сборок
        ///     Пока атомарность обновления не обеспечивается - в момент обновления обращающиеся к серверу запросы получат отлуп
        /// </summary>
        /// <param name="version">Версия измененяемого модуля</param>
        /// <param name="configurationId">Идентификатор конфигурации</param>
        private void OnChangeModules(string version, string configurationId)
        {
            //если конфигурация уже установлена - перезагружаем метаданные конфигурации и прикладные сборки
            IMetadataConfiguration config = _metadataConfigurationProvider.GetMetadataConfiguration(version,
                                                                                                    configurationId);

            //если обновляется несистемная конфигурация (не встроенная конфигурация), то чистим метаданные загруженной конфигурации
            if (config != null && !config.IsEmbeddedConfiguration)
            {
                InfinniPlatformHostServer.Instance.RemoveConfiguration(version, configurationId);
                InfinniPlatformHostServer.Instance.UninstallServices(version, configurationId);
            }

            //если конфигурация еще не загружена или это не системная (встроенная) конфигурация, то устанавливаем конфигурацию
            if (config == null || !config.IsEmbeddedConfiguration)
            {
                InstallConfiguration(version, configurationId);
            }
        }
    }
}