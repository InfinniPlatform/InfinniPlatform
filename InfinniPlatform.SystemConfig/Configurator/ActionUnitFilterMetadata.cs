using System;
using InfinniPlatform.Api.ContextComponents;
using InfinniPlatform.Api.ContextTypes;

namespace InfinniPlatform.SystemConfig.Configurator
{
	/// <summary>
	///   Поиск объекта метаданных по наименованию, если оно предоставлено в качестве идентификатора
	///   Возвращается первый найденный элемент
	/// </summary>
	public sealed class ActionUnitFilterMetadata
	{
		public void Action(IFilterEventContext target)
		{
			if (target.Id == null)
			{
				return;
			}

			Guid guid;

			if (!Guid.TryParse(target.Id, out guid))
			{

				var eventNameMetadata = target.Id;

			    var managerIdentifiers = target.Context.GetComponent<ISystemComponent>().ManagerIdentifiers;

			    target.Id = managerIdentifiers.GetConfigurationUid(eventNameMetadata);

			}


		}
	}
}
