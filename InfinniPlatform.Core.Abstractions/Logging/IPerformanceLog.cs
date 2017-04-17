using System;

namespace InfinniPlatform.Core.Abstractions.Logging
{
    /// <summary>
    /// Сервис регистрации длительности выполнения методов.
    /// </summary>
    public interface IPerformanceLog
    {
        /// <summary>
        /// Фиксирует в логе информацию о длительности вызова указанного метода.
        /// </summary>
        /// <param name="method">Метод компонента, длительность вызова которого была замерена.</param>
        /// <param name="duration">Длительность выполнения метода.</param>
        /// <param name="exception">Исключение при выполнении метода.</param>
        void Log(string method, TimeSpan duration, Exception exception = null);

        /// <summary>
        /// Фиксирует в логе информацию о длительности работы указанного метода, используя <see cref="DateTime.Now"/> в качестве момента окончания выполнения метода.
        /// </summary>
        /// <param name="method">Метод компонента, длительность вызова которого была замерена.</param>
        /// <param name="start">Момент начала выполнения метода.</param>
        /// <param name="exception">Исключение при выполнении метода.</param>
        void Log(string method, DateTime start, Exception exception = null);
    }
}