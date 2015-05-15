using System;
using System.Collections.Generic;
using System.Linq;

namespace InfinniPlatform.Runtime
{
	/// <summary>
	///   Кэш объектов 
	/// </summary>
	public sealed class IdentityMap
	{
		private readonly Dictionary<object,object> _identityObjects = new Dictionary<object, object>();

		public void RegisterIdentity(object identity, object instance)
		{
			if (_identityObjects.ContainsKey(identity))
			{
				throw new ArgumentException(string.Format("object identity \"{0}\" already exists. ",identity));
			}
			_identityObjects.Add(identity,instance);
		}

		public object GetInstance(object identity)
		{
			if (_identityObjects.ContainsKey(identity))
			{
				return _identityObjects[identity];
			}
			return null;
		}

		/// <summary>
		///   Список закэшированных объектов
		/// </summary>
		public IEnumerable<object> Entities
		{
			get { return _identityObjects.Select(i => i.Value).ToList(); }
		}

		public void Clear()
		{
			_identityObjects.Clear();

		}
	}
}