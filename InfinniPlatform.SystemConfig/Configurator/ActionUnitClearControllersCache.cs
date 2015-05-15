using InfinniPlatform.Api.ContextTypes;
using InfinniPlatform.WebApi.WebApi;

namespace InfinniPlatform.SystemConfig.Configurator
{
	/// <summary>
	///   Очищает кэш зарегистрированных контроллеров
	/// (вызов необходим для добавления пользовательского контроллера)
	/// </summary>
	public sealed class ActionUnitClearControllersCache
	{
		public void Action(IApplyContext target)
		{
            HttpControllerSelector.ClearCache();
		}
	}
}
