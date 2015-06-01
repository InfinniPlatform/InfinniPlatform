using InfinniPlatform.Api.ContextTypes;
using InfinniPlatform.Api.Dynamic;
using InfinniPlatform.Api.RestApi.Auth;
using InfinniPlatform.Api.RestApi.CommonApi;
using InfinniPlatform.Api.RestApi.DataApi;

namespace InfinniPlatform.SystemConfig.Administration.User.ActionUnits
{
	public sealed class ActionUnitDeleteUser
	{
		public void Action(IApplyContext target)
		{
			var aclApi = target.Context.GetComponent<AuthApi>();

			var user = target.Item.Document ?? target.Item;

			aclApi.RemoveUser(user.UserName);

		    if (user.UserRoles != null)
		    {
		        foreach (var userRole in user.UserRoles)
		        {
		            aclApi.RemoveUserRole(user.UserName, userRole.DisplayName);
		        }
		    }

		    var api = target.Context.GetComponent<DocumentApi>();
			api.DeleteDocument(AuthorizationStorageExtensions.AdministrationConfigId,"user",user.Id);

            RestQueryApi.QueryPostJsonRaw("AdministrationCustomization", "Common", "OnRemoveUserEvent", null, user);

		    target.Result = new DynamicWrapper();
		    target.Result.IsValid = true;
		    target.Result.ValidationMessage = string.Format("User \"{0}\" deleted successfully", user.UserName);
		}
	}
}
