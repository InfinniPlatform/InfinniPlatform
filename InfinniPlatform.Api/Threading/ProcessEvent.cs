using System;
using System.Security.AccessControl;
using System.Threading;

namespace InfinniPlatform.Api.Threading
{
	/// <summary>
	/// Примитив события для синхронизации процессов.
	/// </summary>
	/// <remarks>
	/// Инкапсулирует сложность работы в разных процессах с <see cref="EventWaitHandle"/>.
	/// </remarks>
	/// <code>
	/// using (var e = new ProcessEvent("event1"))
	/// {
	///		e.Wait();
	/// }
	/// </code>
	/// <code>
	/// using (var e = new ProcessEvent("event1"))
	/// {
	///		e.Set();
	/// }
	/// </code>
	public sealed class ProcessEvent : IDisposable
	{
		public ProcessEvent(string eventName)
		{
			if (eventName == null)
			{
				throw new ArgumentNullException("eventName");
			}

			try
			{
				_event = EventWaitHandle.OpenExisting(eventName, EventWaitHandleRights.Modify);
			}
			catch (WaitHandleCannotBeOpenedException)
			{
				_event = new EventWaitHandle(false, EventResetMode.ManualReset, eventName);
			}
		}


		private readonly EventWaitHandle _event;


		/// <summary>
		/// Ожидает событие.
		/// </summary>
		/// <returns>Успешность ожидания.</returns>
		public bool Wait()
		{
			try
			{
				return _event.WaitOne();
			}
			catch
			{
			}

			return false;
		}

		/// <summary>
		/// Ожидает событие.
		/// </summary>
		/// <param name="timeout">Таймаут ожидания.</param>
		/// <returns>Успешность ожидания.</returns>
		public bool Wait(TimeSpan timeout)
		{
			try
			{
				return _event.WaitOne(timeout);
			}
			catch
			{
			}

			return false;
		}

		/// <summary>
		/// Сигнализирует о событии.
		/// </summary>
		public void Set()
		{
			_event.Set();
		}

		/// <summary>
		/// Освобождает объект.
		/// </summary>
		public void Dispose()
		{
			_event.Dispose();
		}
	}
}