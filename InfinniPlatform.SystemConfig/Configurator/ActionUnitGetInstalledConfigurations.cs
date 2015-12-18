using System.Linq;

using InfinniPlatform.Api.RestApi.DataApi;
using InfinniPlatform.Sdk.Contracts;
using InfinniPlatform.Sdk.Dynamic;

namespace InfinniPlatform.SystemConfig.Configurator
{
    public sealed class ActionUnitGetInstalledConfigurations
    {
        public ActionUnitGetInstalledConfigurations(DocumentApi documentApi)
        {
            _documentApi = documentApi;
        }

        private readonly DocumentApi _documentApi;

        public void Action(ISearchContext target)
        {
            //получаем список всех прикладных конфигураций в системе
            var versionList = _documentApi.GetDocument("update", "package", null, 0, 10000).ToEnumerable();

            target.SearchResult = versionList.Select(v => CreateVersionInfo(v)).ToList();
        }

        private dynamic CreateVersionInfo(dynamic version)
        {
            dynamic instance = new DynamicWrapper();
            instance.Version = version.Version;
            instance.ConfigurationName = version.ConfigurationName;
            return instance;
        }
    }
}