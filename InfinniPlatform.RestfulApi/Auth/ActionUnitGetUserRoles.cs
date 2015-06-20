using System.Collections.Generic;
using InfinniPlatform.Api.RestApi.Auth;
using InfinniPlatform.Api.RestApi.CommonApi;
using InfinniPlatform.ContextComponents;
using InfinniPlatform.Sdk.Application.Contracts;

namespace InfinniPlatform.RestfulApi.Auth
{
    /// <summary>
    ///     Модуль получения списка ролей пользователей
    /// </summary>
    public sealed class ActionUnitGetUserRoles
    {
        public void Action(IApplyContext target)
        {
            if (new IndexApi().IndexExists(AuthorizationStorageExtensions.AuthorizationConfigId,
                                           AuthorizationStorageExtensions.UserRoleStore))
            {
                target.Item.Configuration = AuthorizationStorageExtensions.AuthorizationConfigId;
                target.Item.Metadata = AuthorizationStorageExtensions.UserRoleStore;


                var documentProvider = target.Context.GetComponent<InprocessDocumentComponent>(target.Version)
                                             .GetDocumentProvider(target.Version, target.Item.Configuration,
                                                                  target.Item.Metadata, target.UserName);

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