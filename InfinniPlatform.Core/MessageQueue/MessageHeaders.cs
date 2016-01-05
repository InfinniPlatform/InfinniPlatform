using System;
using System.Collections;
using System.Collections.Generic;

namespace InfinniPlatform.Core.MessageQueue
{
    /// <summary>
    ///     Заголовок сообщения.
    /// </summary>
    public sealed class MessageHeaders : IEnumerable<KeyValuePair<string, byte[]>>
    {
        // ReSharper disable InconsistentNaming

        private readonly Dictionary<string, byte[]> Headers;

        // ReSharper restore InconsistentNaming
        /// <summary>
        ///     Конструктор.
        /// </summary>
        /// <param name="headers">Заголовочная информация сообщения.</param>
        public MessageHeaders(Dictionary<string, byte[]> headers = null)
        {
            Headers = headers ?? new Dictionary<string, byte[]>();
        }

        /// <summary>
        ///     Задать или получить значение свойства.
        /// </summary>
        /// <param name="key">Ключ свойства.</param>
        /// <returns>Значение свойства.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public byte[] this[string key]
        {
            get { return Get(key); }
            set { Set(key, value); }
        }

        public IEnumerator<KeyValuePair<string, byte[]>> GetEnumerator()
        {
            return Headers.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        ///     Получить значение свойства.
        /// </summary>
        /// <param name="key">Ключ свойства.</param>
        /// <returns>Значение свойства.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public byte[] Get(string key)
        {
            if (key == null)
            {
                throw new ArgumentNullException();
            }

            byte[] value;

            Headers.TryGetValue(key, out value);

            return value;
        }

        /// <summary>
        ///     Задать значение свойства.
        /// </summary>
        /// <param name="key">Ключ свойства.</param>
        /// <param name="value">Значение свойства.</param>
        public void Set(string key, byte[] value)
        {
            if (key == null)
            {
                throw new ArgumentNullException();
            }

            Headers[key] = value;
        }
    }
}