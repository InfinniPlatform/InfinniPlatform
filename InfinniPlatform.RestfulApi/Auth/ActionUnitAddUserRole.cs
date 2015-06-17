﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfinniPlatform.Api.ContextTypes;
using InfinniPlatform.ContextComponents;
using InfinniPlatform.SystemConfig.UserStorage;

namespace InfinniPlatform.RestfulApi.Auth
{
    /// <summary>
    ///   Модуль добавления роли пользователя
    /// </summary>
    public sealed class ActionUnitAddUserRole
    {
        public void Action(IApplyContext target)
        {
            var storage = new ApplicationUserStorePersistentStorage();

	        dynamic userRoleParams = target.Item;
			if (target.Item.Document != null)
			{
				userRoleParams = target.Item.Document;
			}

			var user = storage.FindUserByName(userRoleParams.UserName);
            if (user == null)
            {
				throw new ArgumentException(string.Format("User {0} not found", userRoleParams.UserName));
            }

			storage.AddUserToRole(user, userRoleParams.RoleName);
            target.Context.GetComponent<CachedSecurityComponent>(target.Version).UpdateUserRoles();
            target.Context.GetComponent<CachedSecurityComponent>(target.Version).UpdateAcl();
        }
    }
}
