using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using InfinniPlatform.Api.RestQuery;
using InfinniPlatform.Hosting;

namespace InfinniPlatform.WebApi.WebApi
{
    public class ApiControllerFactory : IApiControllerFactory {
	    private readonly Func<IContainer> _container;
	    
	    private Dictionary<string, RestVerbsContainer> _restVerbsContainers = new Dictionary<string, RestVerbsContainer>();

        public ApiControllerFactory(Func<IContainer> container)
        {
	        _container = container;
        }

		private string FormatTemplateName(string metadataConfigurationId, string metadataName)
		{
			return string.Format("{0}_{1}", metadataConfigurationId, metadataName).ToLowerInvariant();
		}

	    public IRestVerbsRegistrator CreateTemplate(string metadataConfigurationId, string metadataName)
	    {

			var verbsContainer = GetRegistrator(metadataConfigurationId, metadataName);
	        if (verbsContainer == null)
	        {
	            verbsContainer = new RestVerbsContainer(FormatTemplateName(metadataConfigurationId,metadataName), _container);
				_restVerbsContainers.Add(FormatTemplateName(metadataConfigurationId, metadataName), verbsContainer);
	        }
	        return verbsContainer;
	    }

		private RestVerbsContainer GetRegistrator(string metadataConfigurationId, string metadataName)
		{
			return _restVerbsContainers.Where(r => r.Key.ToLowerInvariant() == FormatTemplateName(metadataConfigurationId, metadataName)).Select(r => r.Value).FirstOrDefault();
		}


		public IRestVerbsContainer GetTemplate(string metadataConfigurationId, string metadataName)
		{
			return _restVerbsContainers.Where(r => r.Key.ToLowerInvariant() == FormatTemplateName(metadataConfigurationId,metadataName)).Select(r => r.Value).FirstOrDefault();
        }

	    public void RemoveTemplates(string metadataConfigurationId)
	    {
		    _restVerbsContainers = _restVerbsContainers.Where(r => !r.Key.ToLowerInvariant().StartsWith(metadataConfigurationId.ToLowerInvariant())).ToDictionary(r => r.Key, r => r.Value);
	    }
    }
}
