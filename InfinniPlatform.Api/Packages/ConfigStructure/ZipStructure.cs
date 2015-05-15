using InfinniPlatform.Api.Dynamic;
using InfinniPlatform.Api.Properties;
using InfinniPlatform.Api.Settings;
using InfinniPlatform.Api.SystemExtensions;

using Newtonsoft.Json.Linq;

using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace InfinniPlatform.Api.Packages.ConfigStructure
{
    public class ZipStructure : IConfigStructure
    {
        private readonly string _archivePath;
        private readonly bool _toWrite;
        private FileStream _fileStream;
        private ZipArchive _zipArchive;
        private readonly string _assemblyVersionPath;
        private readonly string _folderToUnzip;
        private readonly bool _doNotReadArchiveFromFile;

        public ZipStructure(string archivePath)
        {
            _archivePath = archivePath;
            if (File.Exists(_archivePath))
            {
                File.Delete(_archivePath);
            }
            _assemblyVersionPath = AppSettings.GetValue("AssemblyVersionPath") ?? Directory.GetCurrentDirectory();
            _toWrite = true;
        }

        public ZipStructure(ZipArchive zipArchive, string folderToUnzip)
        {
            _zipArchive = zipArchive;
            _folderToUnzip = folderToUnzip;
            _doNotReadArchiveFromFile = true;
			_assemblyVersionPath = AppSettings.GetValue("AssemblyVersionPath") ?? Directory.GetCurrentDirectory();
        }

        public ZipStructure(string archivePath, string folderToUnzip)
        {
            _archivePath = archivePath;
            _folderToUnzip = folderToUnzip;
			_assemblyVersionPath = AppSettings.GetValue("AssemblyVersionPath") ?? Directory.GetCurrentDirectory();
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

        public void AddConfiguration(IEnumerable<string> configuration)
        {
            _zipArchive.AddFile(ConfigurationFixtureNames.GetConfigurationFileName(), configuration);
        }

        public dynamic GetConfiguration()
        {
            return UnzipFile(ConfigurationFixtureNames.GetConfigurationFileName());
        }

        public void AddDocument(string documentName, IEnumerable<string> document)
        {
            var currentDocumentFolder = ConfigurationFixtureNames.GetDocumentFolderName(documentName);

            string documentFileName = ConfigurationFixtureNames.GetDocumentFileName(currentDocumentFolder,documentName);

            _zipArchive.AddFile(documentFileName, document);
        }

        public void AddRegister(string registerName, IEnumerable<string> register)
        {
            var currentRegisterFolder = ConfigurationFixtureNames.GetRegisterFolderName(registerName);

            string registerFileName = ConfigurationFixtureNames.GetRegisterFileName(currentRegisterFolder, registerName);

            _zipArchive.AddFile(registerFileName, register);
        }

        public dynamic GetDocument(string documentName)
        {
            var documentFolder = ConfigurationFixtureNames.GetDocumentFolderName(documentName);

            return UnzipFile(ConfigurationFixtureNames.GetDocumentFileName(documentFolder,documentName));
        }

        public dynamic GetRegister(string registerName)
        {
            var registerFolder = ConfigurationFixtureNames.GetRegisterFolderName(registerName);

            return UnzipFile(ConfigurationFixtureNames.GetRegisterFileName(registerFolder, registerName));
        }

        public void AddReport(string reportName, IEnumerable<string> report)
	    {

		    var currentReportFolder = ConfigurationFixtureNames.GetReportFolderName(reportName);

		    string reportFileName = ConfigurationFixtureNames.GetReportFileName(currentReportFolder, reportName);

			_zipArchive.AddFile(reportFileName,report);
	    }

	    public void AddDocumentMetadataType(string document, string metadataName, string metadataType, IEnumerable<string> metadata)
        {
            

            var currentMetadataTypeFolder = ConfigurationFixtureNames.GetMetadataTypeFolder(document, metadataType, metadataName);

            string currentMetadatatypeFilename = ConfigurationFixtureNames.GetMetadataTypeFileName(currentMetadataTypeFolder,metadataName);

            _zipArchive.AddFile(currentMetadatatypeFilename, metadata);
        }

        public dynamic GetDocumentMetadataType(string document, string metadataName, string metadataType)
        {
            var currentMetadataTypeFolder = ConfigurationFixtureNames.GetMetadataTypeFolder(document, metadataType, metadataName);

            return UnzipFile(ConfigurationFixtureNames.GetMetadataTypeFileName(currentMetadataTypeFolder,metadataName));
        }

	    public dynamic GetMenu(string menuName)
	    {
			var menuFolder = ConfigurationFixtureNames.GetMenuFolderName(menuName);

			return UnzipFile(ConfigurationFixtureNames.GetMenuFileName(menuFolder, menuName));
	    }

	    public dynamic GetReport(string reportName)
	    {
		    var reportFolder = ConfigurationFixtureNames.GetReportFolderName(reportName);

		    return UnzipFile(ConfigurationFixtureNames.GetDocumentFileName(reportFolder, reportName));
	    }

	    public void AddMenu(string menuName, IEnumerable<string> menu)
        {
			var currentFolderMenu = ConfigurationFixtureNames.GetMenuFolderName(menuName);

			string menuFileName = ConfigurationFixtureNames.GetMenuFileName(currentFolderMenu, menuName);

			_zipArchive.AddFile(menuFileName, menu);
        }

        public void AddAssembly(string assemblyName, IEnumerable<string> assembly)
        {
            var currentAssemblyFolder = ConfigurationFixtureNames.GetAssemblyFolderName(assemblyName);

            var currentAssemblyFileName = ConfigurationFixtureNames.GetAssemblyFileName(currentAssemblyFolder,
                                                                                        assemblyName);

            _zipArchive.AddFile(currentAssemblyFileName,assembly);
        }


        private dynamic UnzipFile(string entryName)
        {
            dynamic result = null;
            _zipArchive.UnzipFile(entryName, stream =>
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
