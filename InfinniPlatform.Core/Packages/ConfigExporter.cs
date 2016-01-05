using System;
using System.Collections.Generic;

using InfinniPlatform.Core.Metadata;
using InfinniPlatform.Core.Metadata.ConfigurationManagers.Standard.Factories;

namespace InfinniPlatform.Core.Packages
{
    public sealed class ConfigExporter
    {
        private readonly IExportStructure _exportStructure;

        public ConfigExporter(IExportStructure exportStructure)
        {
            _exportStructure = exportStructure;
        }

        /// <summary>
        ///   Экспортировать конфигурацию
        /// </summary>
        /// <param name="version">Текущая версия конфигурации</param>
        /// <param name="newVersion">Новая версия конфигурации после экспорта</param>
        /// <param name="configurationId">Наименование экспортируемой конфигурации</param>
        public void ExportHeaderToStructure(string version, string newVersion, string configurationId)
        {
            var managerConfig = ManagerFactoryConfiguration.BuildConfigurationMetadataReader();

            dynamic config = managerConfig.GetItem(configurationId);

            _exportStructure.Start();

            try
            {
                config.Version = newVersion;

                var result = ((string) config.ToString()).Split("\n\r".ToCharArray(),
                    StringSplitOptions.RemoveEmptyEntries);

                _exportStructure.AddConfiguration(result);

                var menuReader = new ManagerFactoryConfiguration(configurationId).BuildMenuMetadataReader();
                var menuList = menuReader.GetItems();

                foreach (var menu in menuList)
                {
                    object fullMenu = menuReader.GetItem(menu.Name);
                    //текст для экспорта в файл
                    result = fullMenu.ToString().Split("\n\r".ToCharArray(),
                        StringSplitOptions.RemoveEmptyEntries);
                    menu.Version = newVersion;

                    _exportStructure.AddMenu(menu.Name, result);
                }

                var reportReader = new ManagerFactoryConfiguration(configurationId).BuildReportMetadataReader();
                var reportList = reportReader.GetItems();

                foreach (var report in reportList)
                {
                    dynamic fullReport = reportReader.GetItem(report.Name);
                    fullReport.Version = newVersion;

                    result = fullReport.ToString().Split("\n\r".ToCharArray(),
                        StringSplitOptions.RemoveEmptyEntries);
                    _exportStructure.AddReport(report.Name, result);
                }

                var documentReader =
                    new ManagerFactoryConfiguration(configurationId).BuildDocumentMetadataReader();
                var documents = documentReader.GetItems();

                foreach (var document in documents)
                {
                    dynamic fullDocument = documentReader.GetItem(document.Name);
                    fullDocument.Version = newVersion;

                    result = fullDocument.ToString().Split("\n\r".ToCharArray(),
                        StringSplitOptions.RemoveEmptyEntries);

                    _exportStructure.AddDocument(document.Name, result);

                    foreach (var containedMetadataType in MetadataType.GetDocumentMetadataTypes())
                    {
                        try
                        {
                            ProcessMetadataType(version, newVersion, configurationId, document.Name, containedMetadataType);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("Can't export metadataType: {0}", containedMetadataType);
                        }
                    }
                }

                var registerReader =
                    new ManagerFactoryConfiguration(configurationId).BuildRegisterMetadataReader();
                var registers = registerReader.GetItems();

                foreach (var register in registers)
                {
                    dynamic fullRegister = registerReader.GetItem(register.Name);
                    register.Version = newVersion;
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

        private void ProcessMetadataType(string version, string newVersion, string configId, string documentId, string metadataType)
        {
            var metadataReader =
                new ManagerFactoryDocument(configId, documentId).BuildManagerByType(metadataType)
                    .MetadataReader;
            foreach (var item in metadataReader.GetItems())
            {
                dynamic fullItemMetadata = metadataReader.GetItem(item.Name);
                fullItemMetadata.Version = newVersion;
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
        public dynamic ImportHeaderFromStructure( string version)
        {
            _exportStructure.Start();

            var factoryContainer = new MetadataContainerInfoFactory();

            dynamic config = _exportStructure.GetConfiguration();

            foreach (var assembly in config.Assemblies)
            {
            }

            IEnumerable<dynamic> menuList = config.Menu;
            config.Menu = new List<dynamic>();
            foreach (var menu in menuList)
            {
                dynamic menuFull = _exportStructure.GetMenu(menu.Name);

                config.Menu.Add(menuFull);
            }

            IEnumerable<dynamic> reportList = config.Reports;
            config.Reports = new List<dynamic>();
            if (reportList != null)
            {
                foreach (var report in reportList)
                {
                    dynamic reportFull = _exportStructure.GetReport(report.Name);

                    config.Reports.Add(reportFull);
                }
            }

            IEnumerable<dynamic> documents = config.Documents;
            config.Documents = new List<dynamic>();
            foreach (var document in documents)
            {
                dynamic documentFull = _exportStructure.GetDocument(document.Name);

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

                    config.Registers.Add(registerFull);
                }
            }


            return config;
        }
    }
}