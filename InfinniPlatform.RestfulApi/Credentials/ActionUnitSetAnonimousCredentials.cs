using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfinniPlatform.Api.ContextTypes;
using InfinniPlatform.Api.RestApi.AuthApi;

namespace InfinniPlatform.RestfulApi.Credentials
{
	public sealed class ActionUnitSetAnonimousCredentials
	{
		public void Action(IApplyContext target)
		{
			target.UserName = AuthorizationStorageExtensions.AnonimousUser;
		}
	}
}
