using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfinniPlatform.Api.ContextTypes;
using InfinniPlatform.Api.Dynamic;
using InfinniPlatform.Api.Security;
using InfinniPlatform.ContextComponents;
using InfinniPlatform.Security;
using InfinniPlatform.SystemConfig.UserStorage;

namespace InfinniPlatform.RestfulApi.Auth
{
	/// <summary>
	///   Добавить пользователя
	/// </summary>
	public sealed class ActionUnitAddUser
	{
		public void Action(IApplyContext target)
		{
			var storage = new ApplicationUserStorePersistentStorage();

			dynamic userParams = target.Item.Document ?? target.Item;

			var user = storage.FindUserByName(userParams.UserName);
			if (user == null)
			{
				storage.CreateUser(new ApplicationUser()
					                   {
										   UserName = userParams.UserName,
										   PasswordHash = new CustomApplicationUserPasswordHasher(target.Context).HashPassword(userParams.Password)
					                   });
				//добавляем доступ на чтение пользователей
                var securityComponent = target.Context.GetComponent<CachedSecurityComponent>(target.Version);

				securityComponent.UpdateUsers();
				target.Result = new DynamicWrapper();
				target.Result.ValidationMessage = "User created successfully";
				target.Result.IsValid = true;
			}
		}
	}
}
