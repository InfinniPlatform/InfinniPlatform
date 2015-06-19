using System;
using System.Windows.Threading;

namespace InfinniPlatform.UserInterface.ViewBuilders.DataElements
{
    /// <summary>
    ///     Менеджер для выполнения отложенных действий.
    /// </summary>
    internal sealed class DelayManager
    {
        private readonly Action _action;
        private readonly DispatcherTimer _timer;

        /// <summary>
        ///     Конструктор.
        /// </summary>
        /// <param name="delay">Задержка в миллисекундах.</param>
        /// <param name="action">Функция отложенного действия.</param>
        public DelayManager(double delay, Action action)
        {
            _timer = new DispatcherTimer {Interval = TimeSpan.FromMilliseconds(delay)};
            _timer.Tick += OnTimerTick;
            _action = action;
        }

        private void OnTimerTick(object sender, EventArgs eventArgs)
        {
            _timer.Stop();
            _action();
        }

        /// <summary>
        ///     Откладывает выполнение действия.
        /// </summary>
        public void Delay()
        {
            _timer.Stop();
            _timer.Start();
        }
    }
}