using InfinniPlatform.RestfulApi.Extensions;
using InfinniPlatform.Sdk.ContextComponents;
using InfinniPlatform.Sdk.Contracts;

namespace InfinniPlatform.RestfulApi.ActionUnits
{
    public sealed class ActionUnitIndexWithTimeStamp
    {
        public void Action(IApplyResultContext target)
        {
			IndexedStorageExtension.IndexWithTimestamp(target.Item.Item, target.Item.Configuration, target.Item.Metadata, target.Item.TimeStamp);
            target.Context.GetComponent<ILogComponent>()
                  .GetLog()
                  .Info("insert \"{0}\" document to index \"{1}\" (type: \"{2}\") with timestamp \"{3}\" ",
                        target.Item.ToString(),
                        target.Item.Configuration, target.Item.Metadata, target.Item.TimeStamp);
        }
    }
}