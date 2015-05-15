namespace InfinniPlatform.Hosting.WebApi.Implementation.WebApi
{
	/// <summary>
	///   Factory for creating instances of REST service
	/// </summary>
    public interface IApiControllerFactory
    {
		/// <summary>
		///   get service template with specified metadata identifier
		/// </summary>
		/// <param name="apiControllerName">metadata identifier belong to specified service</param>
		/// <returns>template of rest service</returns>
        IRestVerbsContainer GetTemplate(string apiControllerName);

		/// <summary>
		///   create new REST template
		/// </summary>
		/// <param name="apiControllerName">metadata identifier</param>
		/// <returns>template</returns>
        IRestVerbsContainer CreateTemplate(string apiControllerName);
    }
}
