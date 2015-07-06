using System.Collections.Generic;
using System.Linq;
using InfinniPlatform.Api.RestApi.DataApi;
using InfinniPlatform.Sdk.ContextComponents;
using InfinniPlatform.Sdk.Contracts;
using InfinniPlatform.Sdk.Dynamic;

namespace InfinniPlatform.SystemConfig.Configurator
{
    public sealed class ActionUnitGetItemId
    {
        public void Action(IApplyResultContext target)
        {
            //получаем список всех прикладных конфигураций в системе
            target.Result = new DynamicWrapper();

            var profiler =
                target.Context.GetComponent<IProfilerComponent>()
                      .GetOperationProfiler("GetItemId", target.Item.Name);
            profiler.Reset();
            IEnumerable<dynamic> documents =
                target.Context.GetComponent<DocumentApi>()
                      .GetDocument("systemconfig", target.Metadata,
                                   f => f.AddCriteria(c => c.Property("Name").IsEquals(target.Item.Name)), 0, 10000);
            target.Result.Id = documents.Count() != 0 ? documents.First().Id : null;
            profiler.TakeSnapshot();
        }
    }
}