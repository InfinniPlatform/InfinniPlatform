using System;

using InfinniPlatform.Sdk.Logging;
using InfinniPlatform.Sdk.Serialization;

namespace InfinniPlatform.Core.Logging
{
    // ReSharper disable InconsistentNaming
    // ReSharper disable NotAccessedField.Global
    // ReSharper disable MemberCanBePrivate.Global

    /// <summary>
    /// Представляет запись события журнала сообщений для <see cref="IPerformanceLog"/>.
    /// </summary>
    [Serializable]
    internal sealed class LogEventPerformance
    {
        public LogEventPerformance(string method, double duration, Exception exception)
        {
            _method = method;
            _duration = duration;
            _exception = exception;
        }


        [NonSerialized]
        private readonly string _method;
        [NonSerialized]
        private readonly double _duration;
        [NonSerialized]
        private readonly Exception _exception;


        /// <summary>
        /// Метод компонента.
        /// </summary>
        public string m;

        /// <summary>
        /// Длительность выполнения метода.
        /// </summary>
        public double d;

        /// <summary>
        /// Сообщение исключения при выполнении метода.
        /// </summary>
        public string ex;


        /// <summary>
        /// Строковое представление события.
        /// </summary>
        [NonSerialized]
        private string _toString;


        public override string ToString()
        {
            if (_toString == null)
            {
                m = _method;
                d = _duration;

                if (_exception != null)
                {
                    ex = ExecuteSilent(() => _exception.GetFullMessage());
                }

                _toString = ExecuteSilent(() => JsonObjectSerializer.Default.ConvertToString(this)) ?? string.Empty;
            }

            return _toString;
        }


        private static TResult ExecuteSilent<TResult>(Func<TResult> action)
        {
            try
            {
                return action();
            }
            catch
            {
                // Лог не может генерировать исключения

                return default(TResult);
            }
        }
    }

    // ReSharper restore MemberCanBePrivate.Global
    // ReSharper restore NotAccessedField.Global
    // ReSharper restore InconsistentNaming
}