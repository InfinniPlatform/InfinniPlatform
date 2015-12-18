using System.Collections.Generic;

using InfinniPlatform.Api.RestApi.Auth;
using InfinniPlatform.Api.RestApi.CommonApi;
using InfinniPlatform.ContextComponents;
using InfinniPlatform.Sdk.Contracts;

namespace InfinniPlatform.RestfulApi.Auth
{
    /// <summary>
    /// Модуль получения списка ролей пользователей
    /// </summary>
    public sealed class ActionUnitGetUserRoles
    {
        public ActionUnitGetUserRoles(IndexApi indexApi)
        {
            _indexApi = indexApi;
        }

        private readonly IndexApi _indexApi;

        public void Action(IApplyContext target)
        {
            if (_indexApi.IndexExists(AuthorizationStorageExtensions.AuthorizationConfigId,
                AuthorizationStorageExtensions.UserRoleStore))
            {
                target.Item.Configuration = AuthorizationStorageExtensions.AuthorizationConfigId;
                target.Item.Metadata = AuthorizationStorageExtensions.UserRoleStore;


                var documentProvider = target.Context.GetComponent<InprocessDocumentComponent>()
                                             .GetDocumentProvider(target.Item.Configuration,
                                                 target.Item.Metadata);

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