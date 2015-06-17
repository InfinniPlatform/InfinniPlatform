using System.Collections.Generic;
using System.Linq;

using InfinniPlatform.Api.ContextTypes;
using InfinniPlatform.Api.RestApi.Auth;
using InfinniPlatform.Api.RestApi.DataApi;

namespace InfinniPlatform.SystemConfig.Administration.Common
{
	public sealed class ActionUnitVerifyPassword
	{
		public void Action(IApplyContext target)
		{
			var providedPassword = target.Item.ProvidedPassword;

			var api = target.Context.GetComponent<DocumentApi>(target.Version);
			IEnumerable<dynamic> users = api.GetDocument(AuthorizationStorageExtensions.AuthorizationConfigId, "UserStore", null, 0, 1000);

			var userWithPassword = users.FirstOrDefault(r => StringHasher.VerifyValue(r.PasswordHash, target.Item.ProvidedPassword));

			//дебильный барс со своими тупыми пими
			//проверяем тупо первого попавшегося пользователя
			if (userWithPassword != null)
			{
				var userLoginAs = api.GetDocument("Administration", "UserLoginAs",
				                                  cr => cr.AddCriteria(
					                                  f => f.Property("User.DisplayName").IsEquals(userWithPassword.UserName)), 0,
				                                  1000).ToList();
				//проверяем совпадение хэшей паролей
				foreach (var userLogin in userLoginAs)
				{
					//находим пользователя, который указан в списках, как пользователь, от имени которого можно подключаться 
					var loginAs = users.FirstOrDefault(u => u.UserName == userLogin.LoginAs.DisplayName);
					if (loginAs != null)
					{
						target.IsValid = true;
						return;
					}
				}
			}
			target.IsValid = false;
		}
	}
}
