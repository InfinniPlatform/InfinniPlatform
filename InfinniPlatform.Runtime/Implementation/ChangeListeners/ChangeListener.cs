using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Security.Policy;

namespace InfinniPlatform.Runtime.Implementation.ChangeListeners
{
	/// <summary>
	///   Слушатель изменений 
	/// </summary>
    public class ChangeListener : IChangeListener
    {
        private readonly ConcurrentDictionary<OrderedRegistration, Action<string, string>> _registeredActions = new ConcurrentDictionary<OrderedRegistration, Action<string, string>>();

		public void RegisterOnChange(string registrator, Action<string, string> action, Order order)
		{
			_registeredActions.AddOrUpdate(new OrderedRegistration()
			{
			    Order = order,
                Registrator = registrator
			}, action, (key,newAction) =>  action);
		}

		public void Invoke(string version, string changedModule)
		{
			var invokeActions = _registeredActions.OrderBy(r => r.Key.Order).Select(r => r.Value).ToList();
			for (int i = 0; i < invokeActions.Count(); i++)
			{
				invokeActions[i].Invoke(version, changedModule);
			}
		}

	    class OrderedRegistration
	    {
            public string Registrator { get; set; }

            public Order Order { get; set; }
	    }
    }
}
