namespace InfinniPlatform.Api.RestQuery
{
	/// <summary>
	///   Factory for creating instances of REST service
	/// </summary>
    public interface IApiControllerFactory
    {

		IRestVerbsContainer GetTemplate(string metadataConfigurationId, string metadataName);


		void RemoveTemplates(string metadataConfigurationId);
    }
}
