using System.Collections.Generic;
using InfinniPlatform.Api.ContextComponents;
using InfinniPlatform.Api.ContextTypes;
using InfinniPlatform.Api.RestApi.AuthApi;
using InfinniPlatform.Api.RestApi.CommonApi;
using InfinniPlatform.ContextComponents;
using InfinniPlatform.RestfulApi.ActionUnits;

namespace InfinniPlatform.RestfulApi.Auth
{
	/// <summary>
	///   Модуль получения существующих ролей
	/// </summary>
	public sealed class ActionUnitGetRoles
	{
		public void Action(IApplyContext target)
		{
			if (IndexApi.IndexExists(AuthorizationStorageExtensions.AuthorizationConfigId,
									 AuthorizationStorageExtensions.RoleStore))
			{
				target.Item.Configuration = AuthorizationStorageExtensions.AuthorizationConfigId;
				target.Item.Metadata = AuthorizationStorageExtensions.RoleStore;

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
