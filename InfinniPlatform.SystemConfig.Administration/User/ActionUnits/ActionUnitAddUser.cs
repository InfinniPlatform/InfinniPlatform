using System.Linq;
using InfinniPlatform.Api.ContextTypes;
using InfinniPlatform.Api.RestApi.AuthApi;
using InfinniPlatform.Api.RestApi.CommonApi;
using InfinniPlatform.Api.RestApi.DataApi;

namespace InfinniPlatform.SystemConfig.Administration.User.ActionUnits
{
	public sealed class ActionUnitAddUser
	{
		public void Action(IApplyContext target)
		{
			var aclApi = target.Context.GetComponent<AclApi>();

			var user = target.Item.Document;

			if (user == null || string.IsNullOrEmpty(user.UserName))
			{
				target.IsValid = false;
				target.ValidationMessage = "User name is not specified";
				return;				
			}

			var userFound = aclApi.GetUsers().FirstOrDefault(r => r.UserName.ToLowerInvariant() == user.UserName.ToLowerInvariant());

			if (userFound != null && user.Password != null)
			{
				target.IsValid = false;
				target.ValidationMessage = "User with user name " + user.UserName + " already exists.";
				return;
			}

		    if (user.Password != null)
		    {
		        aclApi.AddUser(user.UserName, user.Password);
		    }

		    if (user.UserRoles != null)
		    {

		        foreach (var userRole in user.UserRoles)
		        {
		            aclApi.AddUserToRole(user.UserName, userRole.DisplayName);
		        }
		    }


		    if (target.Item.Document.IsAdmin == true)
			{				
				aclApi.AddUserToRole(user.UserName,AuthorizationStorageExtensions.AdminRole);
			}

            target.Item.Document.Password = null;

			target.Context.GetComponent<DocumentApi>()
			      .SetDocument(AuthorizationStorageExtensions.AdministrationConfigId, "User",
			                   target.Item.Document);
		   
			RestQueryApi.QueryPostJsonRaw("AdministrationCustomization", "Common", "OnAddUserEvent", null, target.Item.Document);

		}
	}
}
