using InfinniPlatform.Api.Metadata.ConfigurationManagers.Standard.Factories;

namespace InfinniPlatform.Api.Metadata.ConfigurationManagers.Standard
{
    public sealed class JsonConfigReaderStandard : IJsonConfigReader
    {
        private readonly string _version;

        public JsonConfigReaderStandard(string version)
        {
            _version = version;
        }

        public IDataReader GetDocumentReader(string configurationId)
        {
            return new ManagerFactoryConfiguration(configurationId).BuildDocumentManager().MetadataReader;
        }

        public IDataReader GetRegisterReader(string configurationId)
        {
            return new ManagerFactoryConfiguration(configurationId).BuildRegisterManager().MetadataReader;
        }

        public IDataReader GetScenarioReader(string configurationId, string documentName)
        {
            return new ManagerFactoryDocument(configurationId, documentName).BuildScenarioMetadataReader();
        }

        public IDataReader GetProcessReader(string configurationId, string documentName)
        {
            return new ManagerFactoryDocument(configurationId, documentName).BuildProcessMetadataReader();
        }

        public IDataReader GetServiceReader(string configurationId, string documentName)
        {
            return new ManagerFactoryDocument(configurationId, documentName).BuildServiceMetadataReader();
        }

        public IDataReader GetGeneratorReader(string configurationId, string documentName)
        {
            return new ManagerFactoryDocument(configurationId, documentName).BuildGeneratorMetadataReader();
        }

        public IDataReader GetViewReader(string configurationId, string documentName)
        {
            return new ManagerFactoryDocument(configurationId, documentName).BuildViewMetadataReader();
        }

        public IDataReader GetPrintViewReader(string configurationId, string documentName)
        {
            return new ManagerFactoryDocument(configurationId, documentName).BuildPrintViewMetadataReader();
        }

        public IDataReader GetValidationWarningsReader(string configurationId, string documentName)
        {
            return
                new ManagerFactoryDocument(configurationId, documentName)
                    .BuildValidationWarningsMetadataReader();
        }

        public IDataReader GetValidationErrorsReader(string configurationId, string documentName)
        {
            return
                new ManagerFactoryDocument(configurationId, documentName).BuildValidationErrorsMetadataReader();
        }

        public IDataReader GetStatusesReader(string configurationId, string documentName)
        {
            return new ManagerFactoryDocument(configurationId, documentName).BuildStatusMetadataReader();
        }

        public IDataReader BuildReaderByType(string configurationId, string documentName, string metadataType)
        {
            var manager =
                new ManagerFactoryDocument(configurationId, documentName).BuildManagerByType(metadataType);
            return manager != null ? manager.MetadataReader : null;
        }
    }
}