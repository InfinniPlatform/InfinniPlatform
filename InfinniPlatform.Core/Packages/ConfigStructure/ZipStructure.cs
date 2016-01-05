using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;

using InfinniPlatform.Core.Properties;
using InfinniPlatform.Core.SystemExtensions;
using InfinniPlatform.Sdk.Dynamic;

namespace InfinniPlatform.Core.Packages.ConfigStructure
{
    public class ZipStructure : IExportStructure
    {
        private FileStream _fileStream;
        private ZipArchive _zipArchive;
        private readonly string _archivePath;
        private readonly string _assemblyVersionPath;
        private readonly bool _doNotReadArchiveFromFile;
        private readonly string _folderToUnzip;
        private readonly string _rootPath;
        private readonly bool _toWrite;

        public ZipStructure(string archivePath)
        {
            _archivePath = archivePath;
            if (File.Exists(_archivePath))
            {
                File.Delete(_archivePath);
            }
            _assemblyVersionPath = "AssemblyVersion";
            _toWrite = true;
        }

        public ZipStructure(ZipArchive zipArchive, string folderToUnzip, string rootPath)
        {
            _zipArchive = zipArchive;
            _folderToUnzip = folderToUnzip;
            _rootPath = rootPath;
            _doNotReadArchiveFromFile = true;
            _assemblyVersionPath = "AssemblyVersion";
        }

        public ZipStructure(string archivePath, string folderToUnzip)
        {
            _archivePath = archivePath;
            _folderToUnzip = folderToUnzip;
            _assemblyVersionPath = "AssemblyVersion";
        }

        public void Start()
        {
            if (!_doNotReadArchiveFromFile)
            {
                _fileStream = new FileStream(_archivePath, _toWrite ? FileMode.CreateNew : FileMode.Open);
                _zipArchive = new ZipArchive(_fileStream, _toWrite ? ZipArchiveMode.Create : ZipArchiveMode.Read, true,
                    Encoding.UTF8);
            }
        }

        public void End()
        {
            if (!_doNotReadArchiveFromFile)
            {
                _zipArchive.Dispose();
                _fileStream.Dispose();
            }
        }

        public void AddSolution(IEnumerable<string> solution)
        {
            _zipArchive.AddFile(ConfigurationFixtureNames.GetSolutionFileName(), solution);
        }

        public void AddConfiguration(IEnumerable<string> configuration)
        {
            _zipArchive.AddFile(GetBasePath() + ConfigurationFixtureNames.GetConfigurationFileName(), configuration);
        }

        private string GetBasePath()
        {
            return string.IsNullOrEmpty(_rootPath) ? string.Empty : _rootPath + "\\";
        }

        public dynamic GetConfiguration()
        {
            return UnzipFile(GetBasePath() + ConfigurationFixtureNames.GetConfigurationFileName());
        }

        public dynamic GetSolution()
        {
            return UnzipFile(ConfigurationFixtureNames.GetSolutionFileName()); 
        }

        public void AddDocument(string documentName, IEnumerable<string> document)
        {
            var currentDocumentFolder = ConfigurationFixtureNames.GetDocumentFolderName(GetBasePath() + documentName);

            var documentFileName = ConfigurationFixtureNames.GetDocumentFileName(currentDocumentFolder, documentName);

            _zipArchive.AddFile(documentFileName, document);
        }

        public void AddRegister(string registerName, IEnumerable<string> register)
        {
            var currentRegisterFolder = ConfigurationFixtureNames.GetRegisterFolderName(GetBasePath() + registerName);

            var registerFileName = ConfigurationFixtureNames.GetRegisterFileName(currentRegisterFolder, registerName);

            _zipArchive.AddFile(registerFileName, register);
        }

        public dynamic GetDocument(string documentName)
        {
            var documentFolder = GetBasePath() + ConfigurationFixtureNames.GetDocumentFolderName(documentName);

            return UnzipFile(ConfigurationFixtureNames.GetDocumentFileName(documentFolder, documentName));
        }

        public dynamic GetRegister(string registerName)
        {
            var registerFolder = GetBasePath() + ConfigurationFixtureNames.GetRegisterFolderName(registerName);

            return UnzipFile(ConfigurationFixtureNames.GetRegisterFileName(registerFolder, registerName));
        }

        public void AddReport(string reportName, IEnumerable<string> report)
        {
            var currentReportFolder = GetBasePath() + ConfigurationFixtureNames.GetReportFolderName(reportName);

            var reportFileName = ConfigurationFixtureNames.GetReportFileName(currentReportFolder, reportName);

            _zipArchive.AddFile(reportFileName, report);
        }


        public void AddDocumentMetadataType(string document, string metadataName, string metadataType,
            IEnumerable<string> metadata)
        {
            var currentMetadataTypeFolder = GetBasePath() + ConfigurationFixtureNames.GetMetadataTypeFolder(document, metadataType,
                metadataName);

            var currentMetadatatypeFilename =
                ConfigurationFixtureNames.GetMetadataTypeFileName(currentMetadataTypeFolder, metadataName);

            _zipArchive.AddFile(currentMetadatatypeFilename, metadata);
        }

        public dynamic GetDocumentMetadataType(string document, string metadataName, string metadataType)
        {
            var currentMetadataTypeFolder = GetBasePath() + ConfigurationFixtureNames.GetMetadataTypeFolder(document, metadataType,
                metadataName);

            return UnzipFile(ConfigurationFixtureNames.GetMetadataTypeFileName(currentMetadataTypeFolder, metadataName));
        }

        public dynamic GetMenu(string menuName)
        {
            var menuFolder = GetBasePath() + ConfigurationFixtureNames.GetMenuFolderName(menuName);

            return UnzipFile(ConfigurationFixtureNames.GetMenuFileName(menuFolder, menuName));
        }

        public dynamic GetReport(string reportName)
        {
            var reportFolder = GetBasePath() + ConfigurationFixtureNames.GetReportFolderName(reportName);

            return UnzipFile(ConfigurationFixtureNames.GetDocumentFileName(reportFolder, reportName));
        }

        public void AddMenu(string menuName, IEnumerable<string> menu)
        {
            var currentFolderMenu = GetBasePath() + ConfigurationFixtureNames.GetMenuFolderName(menuName);

            var menuFileName = ConfigurationFixtureNames.GetMenuFileName(currentFolderMenu, menuName);

            _zipArchive.AddFile(menuFileName, menu);
        }

        public void AddAssembly(string assemblyName, IEnumerable<string> assembly)
        {
            var currentAssemblyFolder = GetBasePath() + ConfigurationFixtureNames.GetAssemblyFolderName(assemblyName);

            var currentAssemblyFileName = ConfigurationFixtureNames.GetAssemblyFileName(currentAssemblyFolder,
                assemblyName);

            _zipArchive.AddFile(currentAssemblyFileName, assembly);
        }

        private dynamic UnzipFile(string entryName)
        {
            dynamic result = null;
            _zipArchive.UnzipFile( entryName, stream =>
            {
                //формируем путь к JSON конфигурации
                //\AssemblyVersions\here_version_name_1B655209-E262-4FDF-9C24-E41BCFA432D3\Document_Patient.json
                var pathToJsonConfigFolder = Path.Combine(_assemblyVersionPath, _folderToUnzip,
                    entryName);
                //распаковываем в текущую директорию архив с конфигурацией документа в виде JSON
                stream.SaveToFile(pathToJsonConfigFolder);

                //получаем объект конфигурации
                //configObject представляет собой JSON описание документа конфигурации
                try
                {
                    result = File.ReadAllText(pathToJsonConfigFolder).ToDynamic();
                }
                catch
                {
                    throw new ArgumentException(Resources.ErrorParsingJsonConfiguration);
                }
            });
            return result;
        }
    }
}