using InfinniPlatform.Api.ContextComponents;
using InfinniPlatform.Api.ContextTypes;
using InfinniPlatform.Api.RestApi.AuthApi;
using InfinniPlatform.RestfulApi.Extensions;

namespace InfinniPlatform.RestfulApi.ActionUnits
{
    public sealed class ActionUnitIndexWithTimeStamp
    {
        public void Action(IApplyResultContext target)
        {
            IndexedStorageExtension.IndexWithTimestamp(target.Item.Item, target.Item.Configuration, target.Item.Metadata, target.Item.TimeStamp, 
				target.Context.GetComponent<ISecurityComponent>().GetClaim(AuthorizationStorageExtensions.OrganizationClaim,target.UserName) ?? AuthorizationStorageExtensions.AnonimousUser);
            target.Context.GetComponent<ILogComponent>().GetLog().Info("insert \"{0}\" document to index \"{1}\" (type: \"{2}\") with timestamp \"{3}\" ", target.Item.ToString(),
                                                target.Item.Configuration, target.Item.Metadata, target.Item.TimeStamp);
        }
    }
}
