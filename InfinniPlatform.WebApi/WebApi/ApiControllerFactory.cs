using System;
using System.Collections.Generic;
using System.Linq;

using InfinniPlatform.Api.RestApi;
using InfinniPlatform.Api.RestQuery;
using InfinniPlatform.Sdk.IoC;

namespace InfinniPlatform.WebApi.WebApi
{
    public class ApiControllerFactory : IApiControllerFactory
    {
        public ApiControllerFactory(Func<IContainerResolver> containerResolverFactory)
        {
            _containerResolverFactory = containerResolverFactory;
        }

        private readonly Func<IContainerResolver> _containerResolverFactory;
        private List<RestVerbsContainer> _restVerbsContainers = new List<RestVerbsContainer>();
        public List<Tuple<string, string>> Versions { get; private set; } = new List<Tuple<string, string>>();

        /// <summary>
        /// Получить шаблон контейнера регистрации сервиса для указанной версии конфигурации и указанного пользователя
        /// </summary>
        /// <param name="configId">Идентификатор конфигурации</param>
        /// <param name="metadataName">Идентификатор контейнера метаданных</param>
        /// <param name="userName">Логин пользователя</param>
        /// <returns>Шаблон регистрации сервиса</returns>
        public IRestVerbsContainer GetTemplate(string configId, string metadataName, string userName)
        {
            return _restVerbsContainers.FirstOrDefault(r => r.HasRoute(null, configId, metadataName));
        }

        public void RemoveTemplates(string version, string configId)
        {
            _restVerbsContainers = _restVerbsContainers.Where(r => !r.HasRoute(version, configId)).ToList();
        }

        public void RegisterVersion(string metadataConfigurationId, string version)
        {
            if (!Versions.Any(v => string.Equals(v.Item1, metadataConfigurationId, StringComparison.InvariantCultureIgnoreCase) &&
                                   v.Item2 == version))
            {
                Versions.Add(new Tuple<string, string>(metadataConfigurationId, version));
            }
        }

        public void UnregisterVersion(string metadataConfigurationId, string version)
        {
            var versions = Versions.Where(v => !(string.Equals(v.Item1, metadataConfigurationId, StringComparison.InvariantCultureIgnoreCase) &&
                                                 v.Item2 == version))
                                   .ToList();
            Versions = versions;
        }

        public IRestVerbsRegistrator CreateTemplate(string version, string configId, string metadataName)
        {
            var verbsContainer = GetRegistrator(version, configId, metadataName);
            if (verbsContainer == null)
            {
                verbsContainer = new RestVerbsContainer(version, configId, metadataName, _containerResolverFactory);
                _restVerbsContainers.Add(verbsContainer);
            }
            return verbsContainer;
        }

        private RestVerbsContainer GetRegistrator(string version, string metadataConfigurationId, string metadataName)
        {
            return _restVerbsContainers.FirstOrDefault(r => r.HasRoute(version, metadataConfigurationId, metadataName));
        }
    }
}