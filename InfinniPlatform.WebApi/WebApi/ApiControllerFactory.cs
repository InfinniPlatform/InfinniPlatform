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
	    
	    private List<RestVerbsContainer> _restVerbsContainers = new List<RestVerbsContainer>();

        public ApiControllerFactory(Func<IContainer> container)
        {
	        _container = container;
        }



	    public IRestVerbsRegistrator CreateTemplate(string version, string metadataConfigurationId, string metadataName)
	    {

			var verbsContainer = GetRegistrator(version, metadataConfigurationId, metadataName);
	        if (verbsContainer == null)
	        {
	            verbsContainer = new RestVerbsContainer(version, metadataConfigurationId,metadataName, _container);
				_restVerbsContainers.Add(verbsContainer);
	        }
	        return verbsContainer;
	    }

		private RestVerbsContainer GetRegistrator(string version, string metadataConfigurationId, string metadataName)
		{
			return _restVerbsContainers.FirstOrDefault(r => r.HasRoute(version, metadataConfigurationId, metadataName));
		}


		public IRestVerbsContainer GetTemplate(string version, string metadataConfigurationId, string metadataName)
		{
			return _restVerbsContainers.FirstOrDefault(r => r.HasRoute(version, metadataConfigurationId,metadataName));
        }

	    public void RemoveTemplates(string version, string metadataConfigurationId)
	    {
            _restVerbsContainers = _restVerbsContainers.Where(r => !r.HasRoute(version, metadataConfigurationId)).ToList();
	    }
    }
}
