using System.Collections.Generic;
using System.Linq;
using InfinniPlatform.Api.ContextTypes;
using InfinniPlatform.Api.Dynamic;
using InfinniPlatform.Api.RestApi.DataApi;

namespace InfinniPlatform.SystemConfig.Configurator
{
    public sealed class ActionUnitGetVersionList
    {
        public void Action(ISearchContext target)
        {
            //получаем список всех прикладных конфигураций в системе
            IEnumerable<dynamic> versionList = new DocumentApi().GetDocument("update", "package", null, 0, 10000).ToEnumerable();

            target.SearchResult = versionList.Select(v => (string)v.Version).Distinct().ToList();
        }
    }
}
