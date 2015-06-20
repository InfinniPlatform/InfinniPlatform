using System.Collections.Generic;
using System.Linq;
using InfinniPlatform.Api.RestApi.DataApi;
using InfinniPlatform.Sdk.Application.Contracts;
using InfinniPlatform.Sdk.Application.Dynamic;

namespace InfinniPlatform.SystemConfig.Configurator
{
    public sealed class ActionUnitGetVersionList
    {
        public void Action(ISearchContext target)
        {
            //получаем список всех прикладных конфигураций в системе
            IEnumerable<dynamic> versionList =
                new DocumentApi(target.Version).GetDocument("update", "package", null, 0, 10000).ToEnumerable();

            target.SearchResult = versionList.Select(v => (string) v.Version).Distinct().ToList();
        }
    }
}