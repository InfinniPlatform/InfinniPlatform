using InfinniPlatform.Api.ContextTypes;
using InfinniPlatform.Api.Dynamic;
using InfinniPlatform.Api.RestApi.AuthApi;

namespace InfinniPlatform.SystemConfig.Administration.User.ActionUnits
{
	public sealed class ActionUnitChangePassword
	{
		public void Action(IApplyContext target)
		{
			target.Context.GetComponent<SignInApi>()
			      .ChangePassword(target.Item.Document.UserName, target.Item.Document.OldPassword, target.Item.Document.NewPassword);
			target.Result = new DynamicWrapper();
		}
	}
}
