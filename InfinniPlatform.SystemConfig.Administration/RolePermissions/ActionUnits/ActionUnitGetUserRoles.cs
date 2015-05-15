using System;
using System.Collections.Generic;
using System.Linq;

using InfinniPlatform.Api.ContextTypes;
using InfinniPlatform.Api.Dynamic;
using InfinniPlatform.Api.RestApi.DataApi;

namespace InfinniPlatform.SystemConfig.Administration.RolePermissions.ActionUnits
{
	public sealed class ActionUnitGetUserRoles 
	{
		public void Action(IApplyContext target)
		{
			IEnumerable<dynamic> result = target.Context.GetComponent<DocumentApi>().GetDocument("Administration", "RolePermissions", null, 0, 100000).ToList();

			
			var distinctResult = new List<dynamic>();
			foreach (var role in result)
			{
                dynamic res = new DynamicWrapper();
				res.Id = Guid.NewGuid().ToString();
				res.DisplayName = role.RoleFullName;
				res.RoleName = role.Role.DisplayName;

				distinctResult.Add(res);
			}

			target.Result = distinctResult;

		}
	}
}
