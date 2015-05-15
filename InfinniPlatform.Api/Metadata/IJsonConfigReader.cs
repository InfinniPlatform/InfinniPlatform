namespace InfinniPlatform.Api.Metadata
{
    public interface IJsonConfigReader
    {
        IDataReader GetDocumentReader(string configurationId);
        IDataReader GetRegisterReader(string configurationId);
        IDataReader GetScenarioReader(string configurationId, string documentName);
        IDataReader GetProcessReader(string configurationId, string documentName);
        IDataReader GetServiceReader(string configurationId, string documentName);
        IDataReader GetGeneratorReader(string configurationId, string documentName);
	    IDataReader GetViewReader(string configurationId, string documentName);
	    IDataReader GetPrintViewReader(string configurationId, string documentName);
        IDataReader BuildReaderByType(string configurationId, string documentName, string metadataType);
	    IDataReader GetValidationWarningsReader(string configurationId, string documentName);
	    IDataReader GetValidationErrorsReader(string configurationId, string documentName);
        IDataReader GetStatusesReader(string configurationId, string documentName);
    }
}
