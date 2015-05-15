using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfinniPlatform.Api.ContextComponents;
using InfinniPlatform.Api.ContextTypes;
using InfinniPlatform.Api.RestApi.AuthApi;

namespace InfinniPlatform.RestfulApi.Auth
{
	public sealed class ActionUnitSignIn
	{
		public void Action(IApplyContext target)
		{			
            //возвращаем список Cookie, полученных в ходе регистрации
			target.Result = target.Context.GetComponent<SignInApi>().SignInInternal(target.Item.UserName, target.Item.Password, target.Item.Remember);
		}
	}
}
