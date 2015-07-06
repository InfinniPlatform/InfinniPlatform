using InfinniPlatform.RestfulApi.Extensions;
using InfinniPlatform.Sdk.ContextComponents;
using InfinniPlatform.Sdk.Contracts;

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
                target.Context.GetComponent<ILogComponent>().GetLog().Info(
                    "find \"{0}\" document from index \"{1}\", type \"{2}\" ", target.Result.ToString(),
                    target.Item.Configuration, target.Item.Metadata);
            }
            else
            {
                target.Context.GetComponent<ILogComponent>()
                      .GetLog()
                      .Error("no documents found from type \"{0}\"", target.Item.Metadata);
            }
        }
    }
}