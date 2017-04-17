using System;

using InfinniPlatform.Core.Abstractions.Logging;
using InfinniPlatform.Core.Abstractions.Serialization;

namespace InfinniPlatform.Log4NetAdapter
{
    // ReSharper disable InconsistentNaming
    // ReSharper disable NotAccessedField.Global
    // ReSharper disable MemberCanBePrivate.Global

    /// <summary>
    /// Представляет запись события журнала сообщений для <see cref="IPerformanceLog"/>.
    /// </summary>
    [Serializable]
    internal sealed class PerformanceLogEvent
    {
        public PerformanceLogEvent(string method, long duration, Exception exception, IJsonObjectSerializer serializer)
        {
            _method = method;
            _duration = duration;
            _exception = exception;
            _serializer = serializer;
        }


        [NonSerialized]
        private readonly string _method;
        [NonSerialized]
        private readonly long _duration;
        [NonSerialized]
        private readonly Exception _exception;
        [NonSerialized]
        private readonly IJsonObjectSerializer _serializer;


        /// <summary>
        /// Метод компонента.
        /// </summary>
        public string m;

        /// <summary>
        /// Длительность выполнения метода.
        /// </summary>
        public long d;

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

                _toString = ExecuteSilent(() => _serializer.ConvertToString(this)) ?? string.Empty;
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