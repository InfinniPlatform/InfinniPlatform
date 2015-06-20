using InfinniPlatform.Api.ContextComponents;
using InfinniPlatform.RestfulApi.Extensions;
using InfinniPlatform.Sdk.Application.Contracts;

namespace InfinniPlatform.RestfulApi.ActionUnits
{
    public sealed class ActionUnitGetFromIndex
    {
        public void Action(IApplyResultContext target)
        {
            target.Result = IndexedStorageExtension.GetDocument(target.Item.Id, target.Item.Configuration,
                                                                target.Item.Metadata ?? string.Empty);
            if (target.Result != null)
            {
                target.Context.GetComponent<ILogComponent>(target.Version).GetLog().Info(
                    "find \"{0}\" document from index \"{1}\", type \"{2}\" ", target.Result.ToString(),
                    target.Item.Configuration, target.Item.Metadata);
            }
            else
            {
                target.Context.GetComponent<ILogComponent>(target.Version)
                      .GetLog()
                      .Error("no documents found from type \"{0}\"", target.Item.Metadata);
            }
        }
    }
}