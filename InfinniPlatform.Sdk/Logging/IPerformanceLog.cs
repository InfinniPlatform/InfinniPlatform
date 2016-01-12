using System;

namespace InfinniPlatform.Sdk.Logging
{
    /// <summary>
    /// Сервис регистрации длительности выполнения методов.
    /// </summary>
    public interface IPerformanceLog
    {
        /// <summary>
        /// Фиксирует в логе информацию о длительности вызова указанного метода.
        /// </summary>
        /// <param name="component">Компонент, чей метод вызван.</param>
        /// <param name="method">Метод компонента, длительность вызова которого была замерена.</param>
        /// <param name="duration">Длительность выполнения метода.</param>
        /// <param name="outcome">Результат выполнения: <c>null</c> - если вызов метода завершился успешно; <c>текст исключения</c> - иначе.</param>
        void Log(string component, string method, TimeSpan duration, string outcome);

        /// <summary>
        /// Фиксирует в логе информацию о длительности работы указанного метода, используя <c>DateTime.Now</c> в качестве момента окончания выполнения метода.
        /// </summary>
        /// <param name="component">Компонент, чей метод вызван.</param>
        /// <param name="method">Метод компонента, длительность вызова которого была замерена.</param>
        /// <param name="start">Момент начала выполнения метода.</param>
        /// <param name="outcome">Результат выполнения: <c>null</c> - если вызов метода завершился успешно; <c>текст исключения</c> - иначе.</param>
        void Log(string component, string method, DateTime start, string outcome);
    }
}