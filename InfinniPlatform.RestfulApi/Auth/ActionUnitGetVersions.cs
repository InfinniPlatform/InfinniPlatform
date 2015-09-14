using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfinniPlatform.Api.RestApi.Auth;
using InfinniPlatform.Api.RestApi.CommonApi;
using InfinniPlatform.ContextComponents;
using InfinniPlatform.Sdk.ContextComponents;
using InfinniPlatform.Sdk.Contracts;

namespace InfinniPlatform.RestfulApi.Auth
{
    public sealed class ActionUnitGetVersions
    {
        public void Action(IApplyContext target)
        {
            if (target.Item.FromCache)
            {
                target.Result = target.Context.GetComponent<ISecurityComponent>().Versions;
            }

            else if (new IndexApi().IndexExists(AuthorizationStorageExtensions.AuthorizationConfigId,
                                                AuthorizationStorageExtensions.VersionStore))
            {
                target.Item.Configuration = AuthorizationStorageExtensions.AuthorizationConfigId;
                target.Item.Metadata = AuthorizationStorageExtensions.VersionStore;

                //системная конфигурация всегда содержит версию null
                var documentProvider =
                    target.Context.GetComponent<InprocessDocumentComponent>()
                          .GetDocumentProvider(null, target.Item.Configuration, target.Item.Metadata,
                                               target.UserName);

                if (documentProvider != null)
                {
                    target.Result =
                        documentProvider.GetDocument(
                            null,
                            0,
                            1000000);
                }
                else
                {
                    target.Result = new List<dynamic>();
                }
            }
            else
            {
                target.Result = new List<dynamic>();
            }
        }
    }
}
