using InfinniPlatform.Api.ContextTypes;
using InfinniPlatform.Api.RestApi.AuthApi;
using InfinniPlatform.Api.RestApi.CommonApi;
using InfinniPlatform.Api.RestApi.DataApi;

namespace InfinniPlatform.SystemConfig.Administration.User.ActionUnits
{
	public sealed class ActionUnitDeleteUser
	{
		public void Action(IApplyContext target)
		{
			var aclApi = target.Context.GetComponent<AclApi>();

			var user = target.Item.Document;

			aclApi.RemoveUser(user.UserName);

			foreach (var userRole in user.UserRoles)
			{
				aclApi.RemoveUserRole(user.UserName, userRole.DisplayName);
			}

			var api = target.Context.GetComponent<DocumentApi>();
			api.DeleteDocument(AuthorizationStorageExtensions.AdministrationConfigId,"user",user.Id);

			RestQueryApi.QueryPostJsonRaw("AdministrationCustomization", "Common", "OnRemoveUserEvent", null, target.Item.Document);
		}
	}
}
