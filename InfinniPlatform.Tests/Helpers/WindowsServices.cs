using System;
using System.ServiceProcess;

namespace InfinniPlatform.Helpers
{
	/// <summary>
	/// Вспомогательные методы для управления службами Windows.
	/// </summary>
	public static class WindowsServices
	{
		/// <summary>
		/// Запустить службу.
		/// </summary>
		/// <param name="name">Наименование службы.</param>
		/// <param name="timeout">Таймаут в миллисекундах.</param>
		public static void StartService(string name, int timeout)
		{
			ForService(name, service => service.Start(), ServiceControllerStatus.Running, timeout);
		}

		/// <summary>
		/// Остановить службу.
		/// </summary>
		/// <param name="name">Наименование службы.</param>
		/// <param name="timeout">Таймаут в миллисекундах.</param>
		public static void StopService(string name, int timeout)
		{
			ForService(name, service => service.Stop(), ServiceControllerStatus.Stopped, timeout);
		}

		/// <summary>
		/// Перезапустить службу.
		/// </summary>
		/// <param name="name">Наименование службы.</param>
		/// <param name="timeout">Таймаут в миллисекундах.</param>
		public static void RestartService(string name, int timeout)
		{
			var millisec1 = Environment.TickCount;
			StopService(name, timeout);

			var millisec2 = Environment.TickCount;
			StartService(name, timeout - (millisec2 - millisec1));
		}


		private static void ForService(string name, Action<ServiceController> action, ServiceControllerStatus status, int timeout)
		{
			var service = new ServiceController(name);

			try
			{
				action(service);

				service.WaitForStatus(status, TimeSpan.FromMilliseconds(timeout));
			}
			catch
			{
			}
		}
	}
}