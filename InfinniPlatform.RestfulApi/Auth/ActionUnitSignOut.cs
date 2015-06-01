using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfinniPlatform.Api.ContextTypes;
using InfinniPlatform.Api.RestApi.Auth;

namespace InfinniPlatform.RestfulApi.Auth
{
	public sealed class ActionUnitSignOut
	{
		public void Action(IApplyContext target)
		{
			target.Context.GetComponent<SignInApi>().SignOutInternal();
		}
		
	}
}
