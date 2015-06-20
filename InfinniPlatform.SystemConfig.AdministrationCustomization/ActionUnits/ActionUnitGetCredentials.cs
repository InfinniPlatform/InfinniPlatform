using InfinniPlatform.Sdk.Application.Contracts;

namespace InfinniPlatform.SystemConfig.AdministrationCustomization.ActionUnits
{
	/// <summary>
	///   Модуль для получения привилегий системного администратора для управления регистрацией пользователя с полными правами
	/// </summary>
	public sealed class ActionUnitGetCredentials
	{
		public void Action(IApplyContext target)
		{
			target.UserName = "Admin";
		}
	}
}
