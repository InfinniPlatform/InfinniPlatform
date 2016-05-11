using System;
using System.Collections.Generic;

namespace InfinniPlatform.MessageQueue.RabbitMq.Outdated
{
	/// <summary>
	/// Менеджер для управления ресурсами.
	/// </summary>
	/// <returns>
	/// Все публичные методы менеджера являются потокобезопасными. 
	/// </returns>
	sealed class ResourceManager : IDisposable
	{
		private bool _disposed;

		private readonly List<WeakReference<IDisposable>> _resources
			= new List<WeakReference<IDisposable>>();


		/// <summary>
		/// Зарегистрировать ресурс.
		/// </summary>
		public void RegisterObject(IDisposable resource)
		{
			var resourceRef = new WeakReference<IDisposable>(resource);

			lock (this)
			{
				_resources.Add(resourceRef);
			}

			if (_disposed)
			{
				Dispose();
			}
		}

		/// <summary>
		/// Освободить все ресурсы.
		/// </summary>
		public void Dispose()
		{
			WeakReference<IDisposable>[] resourcelRefs;

			lock (this)
			{
				resourcelRefs = _resources.ToArray();

				_resources.Clear();

				_disposed = true;
			}

			foreach (var resourceRef in resourcelRefs)
			{
				TryDisposeObject(resourceRef);
			}
		}

		private static void TryDisposeObject(WeakReference<IDisposable> resourceRef)
		{
			if (resourceRef != null)
			{
				IDisposable resource;

				if (resourceRef.TryGetTarget(out resource) && resource != null)
				{
					try
					{
						resource.Dispose();
					}
					catch
					{
					}
				}
			}
		}
	}
}