using System.Collections.Generic;
using System.IO;
using InfinniPlatform.Sdk.Dynamic;

namespace InfinniPlatform.Api.Packages.ConfigStructure
{
    public sealed class DirectoryStructure : IConfigStructure
    {
        private readonly string _folderToStructure;

        public DirectoryStructure(string folderToStructure)
        {
            _folderToStructure = folderToStructure;
        }

        public void AddConfiguration(IEnumerable<string> configuration)
        {
            if (!Directory.Exists(Path.Combine(_folderToStructure, ConfigurationFixtureNames.GetConfigurationFolder())))
            {
                Directory.CreateDirectory(Path.Combine(_folderToStructure,
                    ConfigurationFixtureNames.GetConfigurationFolder()));
            }

            File.WriteAllLines(Path.Combine(_folderToStructure, ConfigurationFixtureNames.GetConfigurationFileName()),
                configuration);
        }

        public void AddDocument(string documentName, IEnumerable<string> document)
        {
            if (
                !Directory.Exists(Path.Combine(_folderToStructure,
                    ConfigurationFixtureNames.GetDocumentFolderName(documentName))))
            {
                Directory.CreateDirectory(Path.Combine(_folderToStructure,
                    ConfigurationFixtureNames.GetDocumentFolderName(documentName)));
            }


            var currentDocumentFolder = ConfigurationFixtureNames.GetDocumentFolderName(documentName);

            var documentFileName = ConfigurationFixtureNames.GetDocumentFileName(currentDocumentFolder, documentName);

            File.WriteAllLines(Path.Combine(_folderToStructure, documentFileName), document);
        }

        public void AddRegister(string registerName, IEnumerable<string> register)
        {
            if (
                !Directory.Exists(Path.Combine(_folderToStructure,
                    ConfigurationFixtureNames.GetRegisterFolderName(registerName))))
            {
                Directory.CreateDirectory(Path.Combine(_folderToStructure,
                    ConfigurationFixtureNames.GetRegisterFolderName(registerName)));
            }


            var currentRegisterFolder = ConfigurationFixtureNames.GetRegisterFolderName(registerName);

            var registerFileName = ConfigurationFixtureNames.GetRegisterFileName(currentRegisterFolder, registerName);

            File.WriteAllLines(Path.Combine(_folderToStructure, registerFileName), register);
        }

        public void AddMenu(string menuName, IEnumerable<string> menu)
        {
            if (
                !Directory.Exists(Path.Combine(_folderToStructure, ConfigurationFixtureNames.GetMenuFolderName(menuName))))
            {
                Directory.CreateDirectory(Path.Combine(_folderToStructure,
                    ConfigurationFixtureNames.GetMenuFolderName(menuName)));
            }


            var currentMenuFolder = ConfigurationFixtureNames.GetMenuFolderName(menuName);

            var menuFileName = ConfigurationFixtureNames.GetDocumentFileName(currentMenuFolder, menuName);

            File.WriteAllLines(Path.Combine(_folderToStructure, menuFileName), menu);
        }

        public void AddAssembly(string assemblyName, IEnumerable<string> assembly)
        {
            if (
                !Directory.Exists(Path.Combine(_folderToStructure,
                    ConfigurationFixtureNames.GetAssemblyFolderName(assemblyName))))
            {
                Directory.CreateDirectory(Path.Combine(_folderToStructure,
                    ConfigurationFixtureNames.GetDocumentFolderName(assemblyName)));
            }


            var currentAssemblyFolder = ConfigurationFixtureNames.GetAssemblyFolderName(assemblyName);

            var currentAssemblyFileName = ConfigurationFixtureNames.GetAssemblyFileName(currentAssemblyFolder,
                assemblyName);

            File.WriteAllLines(Path.Combine(_folderToStructure, currentAssemblyFileName), assembly);
        }

        public void AddReport(string reportName, IEnumerable<string> report)
        {
            if (
                !Directory.Exists(Path.Combine(_folderToStructure,
                    ConfigurationFixtureNames.GetReportFolderName(reportName))))
            {
                Directory.CreateDirectory(Path.Combine(_folderToStructure,
                    ConfigurationFixtureNames.GetReportFolderName(reportName)));
            }


            var currentReportFolder = ConfigurationFixtureNames.GetReportFolderName(reportName);

            var currentReportFileName = ConfigurationFixtureNames.GetReportFileName(currentReportFolder, reportName);

            File.WriteAllLines(Path.Combine(_folderToStructure, currentReportFileName), report);
        }

        public void AddDocumentMetadataType(string document, string metadataName, string metadataType,
            IEnumerable<string> metadata)
        {
            if (
                !Directory.Exists(Path.Combine(_folderToStructure,
                    ConfigurationFixtureNames.GetMetadataTypeFolder(document, metadataType, metadataName))))
            {
                Directory.CreateDirectory(Path.Combine(_folderToStructure,
                    ConfigurationFixtureNames.GetMetadataTypeFolder(document, metadataType,
                        metadataName)));
            }


            var currentMetadataTypeFolder = ConfigurationFixtureNames.GetMetadataTypeFolder(document, metadataType,
                metadataName);

            var currentMetadatatypeFilename =
                ConfigurationFixtureNames.GetMetadataTypeFileName(currentMetadataTypeFolder, metadataName);

            File.WriteAllLines(Path.Combine(_folderToStructure, currentMetadatatypeFilename), metadata);
        }

        public void Start()
        {
        }

        public void End()
        {
        }

        public dynamic GetConfiguration()
        {
            return
                File.ReadAllText(Path.Combine(_folderToStructure, ConfigurationFixtureNames.GetConfigurationFileName()))
                    .ToDynamic();
        }

        public dynamic GetDocument(string documentName)
        {
            var documentFolder = ConfigurationFixtureNames.GetDocumentFolderName(documentName);

            return
                File.ReadAllText(Path.Combine(_folderToStructure,
                    ConfigurationFixtureNames.GetDocumentFileName(documentFolder, documentName))).ToDynamic();
        }

        public dynamic GetRegister(string registerName)
        {
            var registerFolder = ConfigurationFixtureNames.GetRegisterFolderName(registerName);

            return
                File.ReadAllText(Path.Combine(_folderToStructure,
                    ConfigurationFixtureNames.GetRegisterFileName(registerFolder, registerName))).ToDynamic();
        }

        public dynamic GetDocumentMetadataType(string document, string metadataName, string metadataType)
        {
            var currentMetadataTypeFolder = ConfigurationFixtureNames.GetMetadataTypeFolder(document, metadataType,
                metadataName);

            return
                File.ReadAllText(Path.Combine(_folderToStructure,
                    ConfigurationFixtureNames.GetMetadataTypeFileName(currentMetadataTypeFolder, metadataName)))
                    .ToDynamic();
        }

        public dynamic GetMenu(string menuName)
        {
            var menuFolder = ConfigurationFixtureNames.GetMenuFolderName(menuName);

            return
                File.ReadAllText(Path.Combine(_folderToStructure,
                    ConfigurationFixtureNames.GetMenuFileName(menuFolder, menuName))).ToDynamic();
        }

        public dynamic GetReport(string reportName)
        {
            var reportFolder = ConfigurationFixtureNames.GetReportFolderName(reportName);

            return
                File.ReadAllText(Path.Combine(_folderToStructure,
                    ConfigurationFixtureNames.GetReportFileName(reportFolder, reportName))).ToDynamic();
        }
    }
}