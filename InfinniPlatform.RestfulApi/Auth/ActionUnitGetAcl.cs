using System.Collections.Generic;
using InfinniPlatform.Api.ContextComponents;
using InfinniPlatform.Api.ContextTypes;
using InfinniPlatform.Api.RestApi.Auth;
using InfinniPlatform.Api.RestApi.CommonApi;
using InfinniPlatform.ContextComponents;
using InfinniPlatform.RestfulApi.ActionUnits;

namespace InfinniPlatform.RestfulApi.Auth
{
    /// <summary>
    ///   Модуль получения всех существующих прав доступа пользователей
    /// </summary>
    public sealed class ActionUnitGetAcl
    {
         public void Action(IApplyContext target)
         {
             if (target.Item.FromCache)
             {
                 target.Result = target.Context.GetComponent<ISecurityComponent>().Acl;
             }

	         else if (IndexApi.IndexExists(AuthorizationStorageExtensions.AuthorizationConfigId,
	                                  AuthorizationStorageExtensions.AclStore))
	         {
				 target.Item.Configuration = AuthorizationStorageExtensions.AuthorizationConfigId;
				 target.Item.Metadata = AuthorizationStorageExtensions.AclStore;

		         var documentProvider =
			         target.Context.GetComponent<InprocessDocumentComponent>()
			               .GetDocumentProvider(target.Item.Configuration, target.Item.Metadata, target.UserName);

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
