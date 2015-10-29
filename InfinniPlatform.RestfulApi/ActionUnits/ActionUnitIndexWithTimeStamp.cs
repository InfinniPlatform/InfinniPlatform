using System.Collections.Generic;

using InfinniPlatform.Api.RestApi.Auth;
using InfinniPlatform.RestfulApi.Extensions;
using InfinniPlatform.Sdk.ContextComponents;
using InfinniPlatform.Sdk.Contracts;

namespace InfinniPlatform.RestfulApi.ActionUnits
{
    public sealed class ActionUnitIndexWithTimeStamp
    {
        public void Action(IApplyResultContext target)
        {
            IndexedStorageExtension.IndexWithTimestamp(target.Item.Item, target.Item.Configuration, target.Item.Metadata,
                                                       target.Item.TimeStamp,
                                                       target.Context.GetComponent<ISecurityComponent>()
                                                             .GetClaim(
                                                                 AuthorizationStorageExtensions.OrganizationClaim,
                                                                 target.UserName) ??
                                                       AuthorizationStorageExtensions.AnonimousUser);
            target.Context.GetComponent<ILogComponent>()
                  .GetLog()
                  .Info("Document inserted.", new Dictionary<string, object>
                                              {
                                                  { "document", target.Item.ToString() },
                                                  { "configurationId", target.Item.Configuration },
                                                  { "type", target.Item.Metadata },
                                                  { "timestamp", target.Item.TimeStamp },
                                              });
        }
    }
}