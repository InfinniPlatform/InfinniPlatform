using System;
using System.Collections.Generic;

using InfinniPlatform.Api.Dynamic;
using InfinniPlatform.Api.Metadata;
using InfinniPlatform.Api.Metadata.ConfigurationManagers.Standard.Factories;
using InfinniPlatform.Api.RestApi.CommonApi;

namespace InfinniPlatform.Api.Packages
{
    public sealed class ConfigExporter
    {
        private readonly IConfigStructure _configStructure;

        public ConfigExporter(IConfigStructure configStructure)
        {
            _configStructure = configStructure;
        }

        public void ExportHeaderToStructure(string version, string configurationId)
        {
            var managerConfig = ManagerFactoryConfiguration.BuildConfigurationMetadataReader(version,false);

            dynamic config = managerConfig.GetItem(configurationId);

            _configStructure.Start();

            try
            {
                string[] result = ((string) config.ToString()).Split("\n\r".ToCharArray(),
                    StringSplitOptions.RemoveEmptyEntries);

                _configStructure.AddConfiguration(result);

                var menuReader = new ManagerFactoryConfiguration(version, configurationId).BuildMenuMetadataReader();
                var menuList = menuReader.GetItems();

                foreach (dynamic menu in menuList)
                {
                    object fullMenu = menuReader.GetItem(menu.Name);
                    //текст для экспорта в файл
                    result = fullMenu.ToString().Split("\n\r".ToCharArray(),
                        StringSplitOptions.RemoveEmptyEntries);

                    _configStructure.AddMenu(menu.Name, result);
                }

                var reportReader = new ManagerFactoryConfiguration(version, configurationId).BuildReportMetadataReader();
                var reportList = reportReader.GetItems();

                foreach (dynamic report in reportList)
                {
                    object fullReport = reportReader.GetItem(report.Name);

                    result = fullReport.ToString().Split("\n\r".ToCharArray(),
                        StringSplitOptions.RemoveEmptyEntries);
                    _configStructure.AddReport(report.Name, result);
                }

                var documentReader =
                    new ManagerFactoryConfiguration(version, configurationId).BuildDocumentMetadataReader();
                var documents = documentReader.GetItems();

                foreach (dynamic document in documents)
                {
                    object fullDocument = documentReader.GetItem(document.Name);
                    result = fullDocument.ToString().Split("\n\r".ToCharArray(),
                        StringSplitOptions.RemoveEmptyEntries);

                    _configStructure.AddDocument(document.Name, result);

                    foreach (string containedMetadataType in MetadataType.GetDocumentMetadataTypes())
                    {
                        try
                        {
                            ProcessMetadataType(version, configurationId, document.Name, containedMetadataType);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("Can't export metadataType: {0}", containedMetadataType);
                        }
                    }
                }

                var registerReader =
                    new ManagerFactoryConfiguration(version, configurationId).BuildRegisterMetadataReader();
                var registers = registerReader.GetItems();

                foreach (dynamic register in registers)
                {
                    object fullRegister = registerReader.GetItem(register.Name);
                    result = fullRegister.ToString().Split("\n\r".ToCharArray(),
                        StringSplitOptions.RemoveEmptyEntries);

                    _configStructure.AddRegister(register.Name, result);
                }
            }

            finally
            {
                _configStructure.End();
            }
        }

        private void ProcessMetadataType(string version, string configId, string documentId, string metadataType)
        {
            IDataReader metadataReader =
                new ManagerFactoryDocument(version, configId, documentId).BuildManagerByType(metadataType).MetadataReader;
            foreach (dynamic item in metadataReader.GetItems())
            {
                dynamic fullItemMetadata = metadataReader.GetItem(item.Name);
                string[] result = ((string) fullItemMetadata.ToString()).Split("\n\r".ToCharArray(),
                    StringSplitOptions.RemoveEmptyEntries);

                _configStructure.AddDocumentMetadataType(documentId, item.Name, metadataType, result);
            }
        }

        /// <summary>
        ///     Загрузить конфигурацию из архива
        /// </summary>
        /// <param name="version">Наименование версии конфигурации</param>
        /// <returns>Объект конфигурации</returns>
        public dynamic ImportHeaderFromStructure(string version)
        {
            _configStructure.Start();

            var factoryContainer = new MetadataContainerInfoFactory();

            dynamic config = _configStructure.GetConfiguration();

            UpdateApi.UpdateMetadataObject(config.Name, null, config, MetadataType.Configuration);

            foreach (dynamic assembly in config.Assemblies)
            {
                UpdateApi.UpdateMetadataObject(config.Name, null, assembly, MetadataType.Assembly);
            }

            IEnumerable<dynamic> menuList = config.Menu;
            config.Menu = new List<dynamic>();
            foreach (dynamic menu in menuList)
            {
                dynamic menuFull = _configStructure.GetMenu(menu.Name);
                UpdateApi.UpdateMetadataObject(config.Name, null, menuFull, MetadataType.Menu);

                config.Menu.Add(menuFull);
            }

            IEnumerable<dynamic> reportList = config.Reports;
            config.Reports = new List<dynamic>();
            if (reportList != null)
            {
                foreach (dynamic report in reportList)
                {
                    dynamic reportFull = _configStructure.GetReport(report.Name);
                    UpdateApi.UpdateMetadataObject(config.Name, null, reportFull, MetadataType.Report);

                    config.Reports.Add(reportFull);
                }
            }

            IEnumerable<dynamic> documents = config.Documents;
            config.Documents = new List<dynamic>();
            foreach (dynamic document in documents)
            {
                dynamic documentFull = _configStructure.GetDocument(document.Name);
                UpdateApi.UpdateMetadataObject(config.Name, document.Name, documentFull, MetadataType.Document);

                config.Documents.Add(documentFull);

                foreach (string documentMetadataType in MetadataType.GetDocumentMetadataTypes())
                {
                    try
                    {
                        string metadataContainer =
                            factoryContainer.BuildMetadataContainerInfo(documentMetadataType).GetMetadataContainerName();
                        
                        IEnumerable<dynamic> items = documentFull[metadataContainer];
                        documentFull[metadataContainer] = new List<dynamic>();
                        if (items != null)
                        {
                            foreach (dynamic metadataType in items)
                            {
                                dynamic metadataTypeObject = _configStructure.GetDocumentMetadataType(document.Name,
                                    metadataType.Name,
                                    documentMetadataType);

                                UpdateApi.UpdateMetadataObject(config.Name, document.Name, metadataTypeObject,
                                    documentMetadataType);

                                documentFull[metadataContainer].Add(metadataTypeObject);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("can't import metadata type {0}. Error message: {1}", documentMetadataType, e.Message);
                    }
                }
            }

            IEnumerable<dynamic> registers = config.Registers;
            config.Registers = new List<dynamic>();
            if (registers != null)
            {
                foreach (dynamic register in registers)
                {
                    dynamic registerFull = _configStructure.GetRegister(register.Name);
                    UpdateApi.UpdateMetadataObject(config.Name, register.Name, registerFull, MetadataType.Register);

                    config.Registers.Add(registerFull);
                }
            }


            return config;
        }
    }
}