using System;
using System.Collections.Generic;
using System.Linq;

using Autofac;

using InfinniPlatform.Api.RestQuery;

namespace InfinniPlatform.WebApi.WebApi
{
    public class ApiControllerFactory : IApiControllerFactory
    {
        public ApiControllerFactory(Func<IContainer> container)
        {
            _container = container;
        }

        private readonly Func<IContainer> _container;
        private List<RestVerbsContainer> _restVerbsContainers = new List<RestVerbsContainer>();
        public List<Tuple<string, string>> Versions { get; private set; } = new List<Tuple<string, string>>();

        /// <summary>
        /// Получить шаблон контейнера регистрации сервиса для указанной версии конфигурации и указанного пользователя
        /// </summary>
        /// <param name="metadataConfigurationId">Идентификатор конфигурации</param>
        /// <param name="metadataName">Идентификатор контейнера метаданных</param>
        /// <param name="userName">Логин пользователя</param>
        /// <returns>Шаблон регистрации сервиса</returns>
        public IRestVerbsContainer GetTemplate(string metadataConfigurationId, string metadataName, string userName)
        {
            return _restVerbsContainers.FirstOrDefault(r => r.HasRoute(null, metadataConfigurationId, metadataName));
        }

        public void RemoveTemplates(string version, string metadataConfigurationId)
        {
            _restVerbsContainers = _restVerbsContainers.Where(r => !r.HasRoute(version, metadataConfigurationId)).ToList();
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

        public IRestVerbsRegistrator CreateTemplate(string version, string metadataConfigurationId, string metadataName)
        {
            var verbsContainer = GetRegistrator(version, metadataConfigurationId, metadataName);
            if (verbsContainer == null)
            {
                verbsContainer = new RestVerbsContainer(version, metadataConfigurationId, metadataName, _container);
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