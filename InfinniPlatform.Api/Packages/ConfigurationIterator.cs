using InfinniPlatform.Api.Dynamic;
using InfinniPlatform.Api.Metadata;
using InfinniPlatform.Api.Metadata.ConfigurationManagers.Standard.Factories;

using System;
using System.Collections.Generic;

namespace InfinniPlatform.Api.Packages
{
    public sealed class ConfigurationIterator
    {
        private readonly IConfigStructure _configStructure;

        public ConfigurationIterator(IConfigStructure configStructure)
        {
            _configStructure = configStructure;
        }

        public Action<dynamic> OnImportConfigStructure { get; set; }

        public Action<string, dynamic> OnImportAssembly { get; set; }

        public Action<string, dynamic> OnImportMenu { get; set; }

        public Action<string, dynamic> OnImportReport { get; set; }

        public Action<string, dynamic> OnImportDocument { get; set; }

        public Action<string, dynamic> OnImportRegister { get; set; }

        public Action<string, string, dynamic, string> OnImportDocumentMetadataType { get; set; }

        /// <summary>
        ///   Загрузить конфигурацию из архива
        /// </summary>
        /// <returns>Объект конфигурации</returns>
        public dynamic ImportToConfigurationObject()
        {
            _configStructure.Start();

            var factoryContainer = new MetadataContainerInfoFactory();

            dynamic config = _configStructure.GetConfiguration();

            if (OnImportConfigStructure != null)
            {
                OnImportConfigStructure(config);
            }

            //UpdateApi.UpdateMetadataObject(config.Name, null, config, MetadataType.Configuration);

            foreach (dynamic assembly in config.Assemblies)
            {
                if (OnImportAssembly != null)
                {
                    OnImportAssembly(config.Name, assembly);
                }


                //UpdateApi.UpdateMetadataObject(config.Name, null, assembly, MetadataType.Assembly);
            }

            IEnumerable<dynamic> menuList = config.Menu;
            config.Menu = new List<dynamic>();
            config.Menu = new List<dynamic>();
            foreach (var menu in menuList)
            {
                dynamic menuFull = _configStructure.GetMenu(menu.Name);
                // UpdateApi.UpdateMetadataObject(config.Name, null, menuFull, MetadataType.Menu);
                if (OnImportMenu != null)
                {
                    OnImportMenu(config.Name, menuFull);
                }

                config.Menu.Add(menuFull);
            }

            IEnumerable<dynamic> reportList = config.Reports;
            config.Reports = new List<dynamic>();
            if (reportList != null)
            {
                foreach (var report in reportList)
                {
                    dynamic reportFull = _configStructure.GetReport(report.Name);
                    //   UpdateApi.UpdateMetadataObject(config.Name, null, reportFull, MetadataType.Report);

                    if (OnImportReport != null)
                    {
                        OnImportReport(config.Name, report);
                    }


                    config.Reports.Add(reportFull);
                }
            }

            IEnumerable<dynamic> documents = config.Documents;
            config.Documents = new List<dynamic>();
            foreach (var document in documents)
            {

                dynamic documentFull = _configStructure.GetDocument(document.Name);

                if (OnImportDocument != null)
                {
                    OnImportDocument(config.Name, documentFull);
                }

                // UpdateApi.UpdateMetadataObject(config.Name, document.Name, documentFull, MetadataType.Document);

                config.Documents.Add(documentFull);

                foreach (var documentMetadataType in MetadataType.GetDocumentMetadataTypes())
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

                                // UpdateApi.UpdateMetadataObject(config.Name, document.Name, metadataTypeObject,
                                //                                documentMetadataType);

                                if (OnImportDocumentMetadataType != null)
                                {
                                    OnImportDocumentMetadataType(config.Name, document.Name, metadataTypeObject,
                                        documentMetadataType);
                                }

                                documentFull[metadataContainer].Add(metadataTypeObject);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(string.Format("can't import metadata type {0}", documentMetadataType));
                    }
                }
            }

            IEnumerable<dynamic> registers = config.Registers;
            config.Registers = new List<dynamic>();
            if (registers != null)
            {
                foreach (var register in registers)
                {
                    dynamic registerFull = _configStructure.GetRegister(register.Name);

                    if (OnImportRegister != null)
                    {
                        OnImportRegister(config.Name, registerFull);
                    }

                    config.Registers.Add(registerFull);
                }
            }

            return config;
        }
    }
}
