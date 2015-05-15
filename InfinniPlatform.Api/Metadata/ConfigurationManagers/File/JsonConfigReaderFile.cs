namespace InfinniPlatform.Api.Metadata.ConfigurationManagers.File
{
    public sealed class JsonConfigReaderFile : IJsonConfigReader
    {
        private readonly JsonFileConfigManager _manager;

        public JsonConfigReaderFile(JsonFileConfigManager manager)
        {
            _manager = manager;
        }

        public IDataReader GetRegisterReader(string configurationId)
        {
            return _manager.BuildRegisterReader(configurationId);
        }

        public IDataReader GetDocumentReader(string configurationId)
        {
            return _manager.BuildDocumentReader(configurationId);
        }

        public IDataReader GetScenarioReader(string configurationId, string documentName)
        {
            return _manager.BuildDocumentElementReader(configurationId, documentName, MetadataType.Scenario);
        }

        public IDataReader GetProcessReader(string configurationId, string documentName)
        {
            return _manager.BuildDocumentElementReader(configurationId, documentName, MetadataType.Process);
        }

        public IDataReader GetServiceReader(string configurationId, string documentName)
        {
            return _manager.BuildDocumentElementReader(configurationId, documentName, MetadataType.Service);
        }

        public IDataReader GetGeneratorReader(string configurationId, string documentName)
        {
            return _manager.BuildDocumentElementReader(configurationId, documentName, MetadataType.Generator);
        }

	    public IDataReader GetViewReader(string configurationId, string documentName)
	    {
		    return _manager.BuildDocumentElementReader(configurationId, documentName, MetadataType.View);
	    }

	    public IDataReader GetPrintViewReader(string configurationId, string documentName)
	    {
		    return _manager.BuildDocumentElementReader(configurationId, documentName, MetadataType.PrintView);
	    }

	    public IDataReader BuildReaderByType(string configurationId, string documentName, string metadataType)
        {
            return _manager.BuildDocumentElementReader(configurationId, documentName, metadataType);
        }

	    public IDataReader GetValidationWarningsReader(string configurationId, string documentName)
	    {
		    return _manager.BuildDocumentElementReader(configurationId, documentName, MetadataType.ValidationWarning);
	    }

	    public IDataReader GetValidationErrorsReader(string configurationId, string documentName)
	    {
		    return _manager.BuildDocumentElementReader(configurationId, documentName, MetadataType.ValidationError);
	    }

        public IDataReader GetStatusesReader(string configurationId, string documentName)
        {
            return _manager.BuildDocumentElementReader(configurationId, documentName, MetadataType.Status);
        }
    }
}
