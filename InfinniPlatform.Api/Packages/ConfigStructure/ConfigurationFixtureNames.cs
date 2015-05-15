using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfinniPlatform.Api.Metadata;
using InfinniPlatform.Api.Metadata.ConfigurationManagers.Standard.Factories;

namespace InfinniPlatform.Api.Packages.ConfigStructure
{
    public static class ConfigurationFixtureNames
    {
        public const string ConfigurationName = "Configuration";
        public const string ExportFileExtension = ".json";

        public static string GetConfigurationFileName()
        {
            return ConfigurationName + ExportFileExtension;
        }


        public static string GetExtendedFileName(string metadata)
        {
            return metadata + ExportFileExtension;
        }

        private static string GetMetadataTypeFolderName(string metadataType)
        {
            return new MetadataContainerInfoFactory().BuildMetadataContainerInfo(metadataType).GetMetadataContainerName();
        }

        private static string GetDocumentsFolderName()
        {
	        return MetadataType.DocumentContainer;
        }

        private static string GetRegistersFolderName()
        {
            return MetadataType.RegisterContainer;
        }

		public static string GetMenuListFolderName()
		{
			return MetadataType.MenuContainer;
		}

        private static string GetAssembliesFolderName()
        {
            return MetadataType.AssemblyContainer;
        }

		private static string GetReportsFolderName()
		{
			return MetadataType.ReportContainer;
		}

        public static string GetConfigurationFolder()
        {
            return string.Empty;
        }

        public static string GetMetadataTypeFolder(string document, string metadataType, string metadataName)
        {
            return GetDocumentsFolderName() + "/" + document + "/" + GetMetadataTypeFolderName(metadataType);
        }

		public static string GetMetadataTypeFileName(string metadataFolder, string metadataName)
		{
			return metadataFolder + "/" + GetExtendedFileName(metadataName);
		}

		public static string GetDocumentFolderName(string document)
		{
			return GetDocumentsFolderName() + "/" + document;
		}

        public static string GetRegisterFolderName(string register)
        {
            return GetRegistersFolderName() + "/" + register;
        }

        public static string GetDocumentFileName(string documentFolder, string documentName)
        {
            return documentFolder + "/" + GetExtendedFileName(documentName);
        }

        public static string GetRegisterFileName(string registerFolder, string registerName)
        {
            return registerFolder + "/" + GetExtendedFileName(registerName);
        }

		public static string GetMenuFileName(string menuFolder, string menuName)
		{
			return menuFolder + "/" + GetExtendedFileName(menuName);
		}

		public static string GetMenuFolderName(string menuName)
		{
			return GetMenuListFolderName() + "/" + menuName;
		}


        public static string GetAssemblyFolderName(string assemblyName)
        {
            return GetAssembliesFolderName() + "/" + assemblyName;
        }

		public static string GetAssemblyFileName(string assemblyFolder, string assemblyName)
		{
			return assemblyFolder + "/" + GetExtendedFileName(assemblyName);
		}


		public static string GetReportFolderName(string reportName)
		{
			return GetReportsFolderName() + "/" + reportName;
		}


	    public static string GetReportFileName(string reportFolder, string reportName)
	    {
			return reportFolder + "/" + GetExtendedFileName(reportName);
	    }
    }
}
