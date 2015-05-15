﻿using System;
using System.Collections.Concurrent;
using System.Linq;

namespace InfinniPlatform.Runtime.Implementation.ChangeListeners
{
	/// <summary>
	///   Слушатель изменений 
	/// </summary>
    public class ChangeListener : IChangeListener
    {
		private readonly ConcurrentDictionary<string, Action<string>> _registeredActions = new ConcurrentDictionary<string, Action<string>>();

		public void RegisterOnChange(string registrator, Action<string> action)
		{
			_registeredActions.AddOrUpdate(registrator,action, (key,newAction) =>  action);
		}

		public void Invoke(string changedModule)
		{
			var invokeActions = _registeredActions.Select(r => r.Value).ToList();
			for (int i = 0; i < invokeActions.Count(); i++)
			{
				invokeActions[i].Invoke(changedModule);
			}
		}
    }
}
