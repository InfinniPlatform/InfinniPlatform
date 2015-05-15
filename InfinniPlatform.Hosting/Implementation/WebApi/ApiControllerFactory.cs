using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;

namespace InfinniPlatform.Hosting.WebApi.Implementation.WebApi
{
    public class ApiControllerFactory : IApiControllerFactory {
	    private readonly Func<IContainer> _container;
	    
	    private readonly Dictionary<string, IRestVerbsContainer> _restVerbsContainers = new Dictionary<string, IRestVerbsContainer>();

        public ApiControllerFactory(Func<IContainer> container)
        {
	        _container = container;
        }


	    /// <summary>
	    ///   create new REST template
	    /// </summary>
	    /// <param name="apiControllerName">metadata identifier</param>
	    /// <returns>template</returns>
	    public IRestVerbsContainer CreateTemplate(string apiControllerName)
	    {

	        var verbsContainer = GetTemplate(apiControllerName);
	        if (verbsContainer == null)
	        {
	            verbsContainer = new RestVerbsContainer(apiControllerName, _container);
	            _restVerbsContainers.Add(apiControllerName, verbsContainer);
	        }
	        return verbsContainer;
	    }

	    /// <summary>
	    ///   get service template with specified metadata identifier
	    /// </summary>
	    /// <param name="apiControllerName">metadata identifier belong to specified service</param>
	    /// <returns>template of rest service</returns>
	    public IRestVerbsContainer GetTemplate(string apiControllerName) {
            return _restVerbsContainers.Where(r => r.Key.ToLowerInvariant() == apiControllerName.ToLowerInvariant()).Select(r => r.Value).FirstOrDefault();
        }
    }
}
