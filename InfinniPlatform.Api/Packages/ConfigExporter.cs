using System;
using System.Collections.Generic;
using InfinniPlatform.Api.Metadata;
using InfinniPlatform.Api.Metadata.ConfigurationManagers.Standard.Factories;
using InfinniPlatform.Api.RestApi.CommonApi;

namespace InfinniPlatform.Api.Packages
{
    public sealed class ConfigExporter
    {
        private readonly IExportStructure _exportStructure;

        public ConfigExporter(IExportStructure exportStructure)
        {
            _exportStructure = exportStructure;
        }

        public void ExportHeaderToStructure(string version, string configurationId)
        {
            var managerConfig = ManagerFactoryConfiguration.BuildConfigurationMetadataReader(version);

            dynamic config = managerConfig.GetItem(configurationId);

            _exportStructure.Start();

            try
            {
                var result = ((string) config.ToString()).Split("\n\r".ToCharArray(),
                    StringSplitOptions.RemoveEmptyEntries);

                _exportStructure.AddConfiguration(result);

                var menuReader = new ManagerFactoryConfiguration(version, configurationId).BuildMenuMetadataReader();
                var menuList = menuReader.GetItems();

                foreach (var menu in menuList)
                {
                    object fullMenu = menuReader.GetItem(menu.Name);
                    //текст для экспорта в файл
                    result = fullMenu.ToString().Split("\n\r".ToCharArray(),
                        StringSplitOptions.RemoveEmptyEntries);

                    _exportStructure.AddMenu(menu.Name, result);
                }

                var reportReader = new ManagerFactoryConfiguration(version, configurationId).BuildReportMetadataReader();
                var reportList = reportReader.GetItems();

                foreach (var report in reportList)
                {
                    object fullReport = reportReader.GetItem(report.Name);

                    result = fullReport.ToString().Split("\n\r".ToCharArray(),
                        StringSplitOptions.RemoveEmptyEntries);
                    _exportStructure.AddReport(report.Name, result);
                }

                var documentReader =
                    new ManagerFactoryConfiguration(version, configurationId).BuildDocumentMetadataReader();
                var documents = documentReader.GetItems();

                foreach (var document in documents)
                {
                    object fullDocument = documentReader.GetItem(document.Name);
                    result = fullDocument.ToString().Split("\n\r".ToCharArray(),
                        StringSplitOptions.RemoveEmptyEntries);

                    _exportStructure.AddDocument(document.Name, result);

                    foreach (var containedMetadataType in MetadataType.GetDocumentMetadataTypes())
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

                foreach (var register in registers)
                {
                    object fullRegister = registerReader.GetItem(register.Name);
                    result = fullRegister.ToString().Split("\n\r".ToCharArray(),
                        StringSplitOptions.RemoveEmptyEntries);

                    _exportStructure.AddRegister(register.Name, result);
                }
            }

            finally
            {
                _exportStructure.End();
            }
        }

        private void ProcessMetadataType(string version, string configId, string documentId, string metadataType)
        {
            var metadataReader =
                new ManagerFactoryDocument(version, configId, documentId).BuildManagerByType(metadataType)
                    .MetadataReader;
            foreach (var item in metadataReader.GetItems())
            {
                dynamic fullItemMetadata = metadataReader.GetItem(item.Name);
                var result = ((string) fullItemMetadata.ToString()).Split("\n\r".ToCharArray(),
                    StringSplitOptions.RemoveEmptyEntries);

                _exportStructure.AddDocumentMetadataType(documentId, item.Name, metadataType, result);
            }
        }

        /// <summary>
        ///     Загрузить конфигурацию из архива
        /// </summary>
        /// <param name="version">Наименование версии конфигурации</param>
        /// <returns>Объект конфигурации</returns>
        public dynamic ImportHeaderFromStructure(string version)
        {
            _exportStructure.Start();

            var factoryContainer = new MetadataContainerInfoFactory();

            dynamic config = _exportStructure.GetConfiguration();

            new UpdateApi(version).UpdateMetadataObject(config.Name, null, config, MetadataType.Configuration);

            foreach (var assembly in config.Assemblies)
            {
                new UpdateApi(version).UpdateMetadataObject(config.Name, null, assembly, MetadataType.Assembly);
            }

            IEnumerable<dynamic> menuList = config.Menu;
            config.Menu = new List<dynamic>();
            foreach (var menu in menuList)
            {
                dynamic menuFull = _exportStructure.GetMenu(menu.Name);
                new UpdateApi(version).UpdateMetadataObject(config.Name, null, menuFull, MetadataType.Menu);

                config.Menu.Add(menuFull);
            }

            IEnumerable<dynamic> reportList = config.Reports;
            config.Reports = new List<dynamic>();
            if (reportList != null)
            {
                foreach (var report in reportList)
                {
                    dynamic reportFull = _exportStructure.GetReport(report.Name);
                    new UpdateApi(version).UpdateMetadataObject(config.Name, null, reportFull, MetadataType.Report);

                    config.Reports.Add(reportFull);
                }
            }

            IEnumerable<dynamic> documents = config.Documents;
            config.Documents = new List<dynamic>();
            foreach (var document in documents)
            {
                dynamic documentFull = _exportStructure.GetDocument(document.Name);
                new UpdateApi(version).UpdateMetadataObject(config.Name, document.Name, documentFull,
                    MetadataType.Document);

                config.Documents.Add(documentFull);

                foreach (var documentMetadataType in MetadataType.GetDocumentMetadataTypes())
                {
                    try
                    {
                        var metadataContainer =
                            factoryContainer.BuildMetadataContainerInfo(documentMetadataType).GetMetadataContainerName();

                        IEnumerable<dynamic> items = documentFull[metadataContainer];
                        documentFull[metadataContainer] = new List<dynamic>();
                        if (items != null)
                        {
                            foreach (var metadataType in items)
                            {
                                dynamic metadataTypeObject = _exportStructure.GetDocumentMetadataType(document.Name,
                                    metadataType.Name,
                                    documentMetadataType);

                                new UpdateApi(version).UpdateMetadataObject(config.Name, document.Name,
                                    metadataTypeObject,
                                    documentMetadataType);

                                documentFull[metadataContainer].Add(metadataTypeObject);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("can't import metadata type {0}. Error message: {1}", documentMetadataType,
                            e.Message);
                    }
                }
            }

            IEnumerable<dynamic> registers = config.Registers;
            config.Registers = new List<dynamic>();
            if (registers != null)
            {
                foreach (var register in registers)
                {
                    dynamic registerFull = _exportStructure.GetRegister(register.Name);
                    new UpdateApi(version).UpdateMetadataObject(config.Name, register.Name, registerFull,
                        MetadataType.Register);

                    config.Registers.Add(registerFull);
                }
            }


            return config;
        }
    }
}