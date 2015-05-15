using System;
using System.Windows.Threading;

namespace InfinniPlatform.UserInterface.ViewBuilders.DataElements
{
	/// <summary>
	/// Менеджер для выполнения отложенных действий.
	/// </summary>
	sealed class DelayManager
	{
		/// <summary>
		/// Конструктор.
		/// </summary>
		/// <param name="delay">Задержка в миллисекундах.</param>
		/// <param name="action">Функция отложенного действия.</param>
		public DelayManager(double delay, Action action)
		{
			_timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(delay) };
			_timer.Tick += OnTimerTick;
			_action = action;
		}


		private readonly DispatcherTimer _timer;
		private readonly Action _action;


		private void OnTimerTick(object sender, EventArgs eventArgs)
		{
			_timer.Stop();
			_action();
		}


		/// <summary>
		/// Откладывает выполнение действия.
		/// </summary>
		public void Delay()
		{
			_timer.Stop();
			_timer.Start();
		}
	}
}