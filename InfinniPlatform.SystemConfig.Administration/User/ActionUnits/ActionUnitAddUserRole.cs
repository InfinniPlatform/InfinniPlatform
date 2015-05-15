using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Administration.Common;
using Administration.RolePermissions.ActionUnits;
using InfinniPlatform.Api.ContextTypes;
using InfinniPlatform.Api.Dynamic;
using InfinniPlatform.Api.RestApi.AuthApi;
using InfinniPlatform.Api.RestApi.DataApi;

namespace Administration.User.ActionUnits
{
	public sealed class ActionUnitAddUserRole
	{
		public void Action(IApplyContext target)
		{
			var api = target.Context.GetComponent<DocumentApi>();

			dynamic user =  api.GetDocument("Administration", "User",
			                f => f.AddCriteria(cr => cr.Property("Id").IsEquals(target.Item.Document.User.Id)), 0, 1).FirstOrDefault();

            dynamic userPermissions = new DynamicWrapper();
			userPermissions.Id = Guid.NewGuid().ToString();
			userPermissions.Organization = target.Item.Document.Organization;
			userPermissions.Section = target.Item.Document.Section;

			var roleName = CommonUtils.GetRoleName(target.Item.Document.User.DisplayName,
			                                       target.Item.Document.Organization.DisplayName); 

			var menuRoleName = CommonUtils.GetMenuRoleName(target.Item.Document.User.DisplayName); 

			//добавляем роль в конфигурации Authorization
			var aclApi = new AclApi();

			aclApi.AddRole(roleName, roleName, roleName);

			//добавляем роль для меню
			aclApi.AddRole(menuRoleName, menuRoleName, menuRoleName);

			api.SetDocument("Administration", "RolePermissions", userPermissions);

			if (user != null)
			{
                dynamic documentRole = new DynamicWrapper();
				documentRole.Id = userPermissions.Id;
				documentRole.DisplayName = roleName;
				user.UserRoles.Add(documentRole);

				IEnumerable<dynamic> userRoles = user.UserRoles;
				if (userRoles.FirstOrDefault(r => r.Name == menuRoleName) == null)
				{
					dynamic menuRole = new DynamicWrapper();
					menuRole.Id = Guid.NewGuid().ToString();
					menuRole.DisplayName = menuRoleName;
					user.UserRoles.Add(menuRole);					
				}

				api.SetDocument("Administration", "User", user);
			}
		}
	}
}
