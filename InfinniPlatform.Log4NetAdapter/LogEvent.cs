using System;
using System.Collections.Generic;

using InfinniPlatform.Core.Abstractions.Logging;
using InfinniPlatform.Core.Abstractions.Serialization;

namespace InfinniPlatform.Log4NetAdapter
{
    // ReSharper disable InconsistentNaming
    // ReSharper disable NotAccessedField.Global
    // ReSharper disable MemberCanBePrivate.Global

    /// <summary>
    /// Представляет запись события журнала сообщений для <see cref="ILog"/>.
    /// </summary>
    [Serializable]
    internal sealed class LogEvent
    {
        public LogEvent(string message, Exception exception, Func<Dictionary<string, object>> context, IJsonObjectSerializer serializer)
        {
            _message = message;
            _exception = exception;
            _context = context;
            _serializer = serializer;
        }


        [NonSerialized]
        private readonly string _message;
        [NonSerialized]
        private readonly Exception _exception;
        [NonSerialized]
        private readonly Func<Dictionary<string, object>> _context;
        [NonSerialized]
        private readonly IJsonObjectSerializer _serializer;


        /// <summary>
        /// Сообщение события.
        /// </summary>
        public string msg;

        /// <summary>
        /// Сообщение исключения.
        /// </summary>
        public string ex;

        /// <summary>
        /// Стек вызова исключения.
        /// </summary>
        public string stack;

        /// <summary>
        /// Контекст возникновения события.
        /// </summary>
        public Dictionary<string, object> ctx;


        /// <summary>
        /// Строковое представление события.
        /// </summary>
        [NonSerialized]
        private string _toString;


        public override string ToString()
        {
            if (_toString == null)
            {
                msg = _message;

                if (_exception != null)
                {
                    ex = ExecuteSilent(() => _exception.GetFullMessage());
                    stack = ExecuteSilent(() => _exception.GetFullStackTrace());
                }

                if (_context != null)
                {
                    ctx = ExecuteSilent(() => _context());
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