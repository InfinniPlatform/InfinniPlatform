﻿using System.Collections.Generic;
using InfinniPlatform.Api.ContextComponents;
using InfinniPlatform.Api.ContextTypes;
using InfinniPlatform.Api.ContextTypes.ContextImpl;
using InfinniPlatform.Api.Metadata;
using InfinniPlatform.Api.RestApi.AuthApi;
using InfinniPlatform.Api.RestApi.CommonApi;
using InfinniPlatform.ContextComponents;
using InfinniPlatform.RestfulApi.ActionUnits;

namespace InfinniPlatform.RestfulApi.Auth
{
    /// <summary>
    ///   Модуль получения списка ролей пользователей
    /// </summary>
    public sealed class ActionUnitGetUserRoles
    {
        public void Action(IApplyContext target)
        {
	        if (IndexApi.IndexExists(AuthorizationStorageExtensions.AuthorizationConfigId,
	                                 AuthorizationStorageExtensions.UserRoleStore))
	        {


				target.Item.Configuration = AuthorizationStorageExtensions.AuthorizationConfigId;
				target.Item.Metadata = AuthorizationStorageExtensions.UserRoleStore;


				var documentProvider = target.Context.GetComponent<InprocessDocumentComponent>()
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
