using System;
using System.Collections.Generic;
using System.Linq;

using InfinniPlatform.Core.RestApi;
using InfinniPlatform.Core.RestQuery;
using InfinniPlatform.Sdk.IoC;

namespace InfinniPlatform.WebApi.WebApi
{
    internal sealed class ApiControllerFactory : IApiControllerFactory
    {
        public ApiControllerFactory(IContainerResolver containerResolver)
        {
            _containerResolver = containerResolver;
            _restVerbsContainers = new List<RestVerbsContainer>();
        }


        private readonly IContainerResolver _containerResolver;
        private readonly List<RestVerbsContainer> _restVerbsContainers;


        public IRestVerbsRegistrator CreateTemplate(string configId, string documentType)
        {
            var verbsContainer = _restVerbsContainers.FirstOrDefault(r => r.HasRoute(configId, documentType));

            if (verbsContainer == null)
            {
                verbsContainer = new RestVerbsContainer(configId, documentType, _containerResolver);

                _restVerbsContainers.Add(verbsContainer);
            }

            return verbsContainer;
        }

        public IRestVerbsContainer GetTemplate(string configId, string documentType)
        {
            return _restVerbsContainers.FirstOrDefault(r => r.HasRoute(configId, documentType));
        }

        public void RemoveTemplates(string configId)
        {
            _restVerbsContainers.RemoveAll(r => r.HasRoute(configId));
        }
    }
}