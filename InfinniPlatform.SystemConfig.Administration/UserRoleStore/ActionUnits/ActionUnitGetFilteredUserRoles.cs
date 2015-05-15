using System.Collections.Generic;
using System.Linq;

using InfinniPlatform.Api.ContextTypes;
using InfinniPlatform.Api.RestApi.DataApi;

namespace InfinniPlatform.SystemConfig.Administration.UserRoleStore.ActionUnits
{
	public sealed class ActionUnitGetFilteredUserRoles
	{
		public void Action(IApplyContext target)
		{
			var api = target.Context.GetComponent<DocumentApiUnsecured>();

			var session = api.GetDocument("Administration", "UserOrganizationSession",
				f => f.AddCriteria(cr => cr.Property("User.DisplayName").IsEquals(target.UserName)), 0, 1).FirstOrDefault();

			if (session != null)
			{
				//получаем список ролей для организации
				var resultRoles = api.GetDocument("Administration", "User",
				                f => f.AddCriteria(cr => cr.Property("UserName").IsEquals(target.UserName)),0, 1).FirstOrDefault();

				//если существует пользователь с именем, указанным в контексте
				if (resultRoles != null)
				{
					IEnumerable<dynamic> selectedOrganizationRoles = resultRoles.UserRoles;

					var roles = selectedOrganizationRoles.Select(r => r.DisplayName).ToList();

					target.Result = roles.Select(r => new
					{
						RoleName = r,
						UserName = target.UserName
					}).ToList();

				}
				else
				{
					target.Result = new List<dynamic>();
				}
			}
		}
	}
}
