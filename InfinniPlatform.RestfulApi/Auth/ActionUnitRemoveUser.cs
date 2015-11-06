using InfinniPlatform.Api.Security;
using InfinniPlatform.ContextComponents;
using InfinniPlatform.Sdk.Contracts;
using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.SystemConfig.UserStorage;

namespace InfinniPlatform.RestfulApi.Auth
{
    /// <summary>
    ///     Модуль удаления пользователей системы
    /// </summary>
    public sealed class ActionUnitRemoveUser
    {
        public void Action(IApplyContext target)
        {
	        if (target.Item.UserName !=null)
	        {
				var storage = ApplicationUserStorePersistentStorage.Instance;
				ApplicationUser user = storage.FindUserByName(target.Item.UserName);
				if (user != null)
				{
					storage.DeleteUser(user);
					//добавляем доступ на чтение пользователей
					target.Context.GetComponent<CachedSecurityComponent>().UpdateUsers();
					target.Context.GetComponent<CachedSecurityComponent>().UpdateAcl();
					target.Context.GetComponent<CachedSecurityComponent>().UpdateUserRoles();
					target.Result = new DynamicWrapper();
					target.Result.IsValid = true;
					target.Result.ValidationMessage = "User deleted";
				}
			}
        }
    }
}