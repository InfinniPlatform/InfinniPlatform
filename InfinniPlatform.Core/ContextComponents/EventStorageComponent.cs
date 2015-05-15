using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfinniPlatform.Api.ContextComponents;
using InfinniPlatform.Api.Factories;
using InfinniPlatform.EventStorage;

namespace InfinniPlatform.ContextComponents
{
	/// <summary>
	///   Компонент для сохранения событий из контекста
	/// </summary>
	public sealed class EventStorageComponent : IEventStorageComponent
	{
		private readonly IEventStorage _eventStorage;

		public EventStorageComponent(IEventStorage eventStorage)
		{
			_eventStorage = eventStorage;
		}

		public IEventStorage GetEventStorage()
		{
			return _eventStorage;
		}
	}
}
