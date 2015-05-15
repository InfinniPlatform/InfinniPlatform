using System.Collections.Generic;
using InfinniPlatform.Api.Events;

namespace InfinniPlatform.Factories
{
	/// <summary>
	///   Провайдер агрегатов. Формирует агрегаты структурированных записей документов
	///   и справочников на основе событий
	/// </summary>
	public interface IAggregateProvider
	{
		object CreateAggregate();

		void ApplyChanges(ref object item, IEnumerable<EventDefinition> events);
	}
}
