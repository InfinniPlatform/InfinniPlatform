using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfinniPlatform.Api.ContextComponents;
using InfinniPlatform.Api.ContextTypes;
using InfinniPlatform.Api.RestApi.Auth;
using InfinniPlatform.Api.RestApi.CommonApi;
using InfinniPlatform.ContextComponents;
using InfinniPlatform.RestfulApi.ActionUnits;


namespace InfinniPlatform.RestfulApi.Auth
{
	public sealed class ActionUnitGetUsers
	{
		public void Action(IApplyContext target)
		{
		    if (target.Item.FromCache)
		    {
		        target.Result = target.Context.GetComponent<ISecurityComponent>(target.Version).Users;
		    }

			else if (new IndexApi().IndexExists(AuthorizationStorageExtensions.AuthorizationConfigId,
									 AuthorizationStorageExtensions.UserStore))
			{
				target.Item.Configuration = AuthorizationStorageExtensions.AuthorizationConfigId;
				target.Item.Metadata = AuthorizationStorageExtensions.UserStore;

				var documentProvider = target.Context.GetComponent<InprocessDocumentComponent>(target.Version)
						  .GetDocumentProvider(target.Version, target.Item.Configuration, target.Item.Metadata, target.UserName);


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
