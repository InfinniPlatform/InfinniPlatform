using System;
using System.Collections.Generic;

namespace InfinniPlatform.UserInterface.ViewBuilders
{
    /// <summary>
    ///     Буфер обмена.
    /// </summary>
    public sealed class Clipboard
    {
        /// <summary>
        ///     Возвращает статический экземпляр буфера обмена.
        /// </summary>
        public static readonly Clipboard Instance = new Clipboard();

        private readonly Dictionary<string, ClipboardItem> _buffer = new Dictionary<string, ClipboardItem>();

        /// <summary>
        ///     Определяет, содержится ли значение буфере.
        /// </summary>
        /// <param name="key">Ключ.</param>
        /// <returns>Возвращает true, если значение содержится.</returns>
        public bool Contains(string key)
        {
            return _buffer.ContainsKey(key);
        }

        /// <summary>
        ///     Добавляет значение в буфер.
        /// </summary>
        /// <param name="key">Ключ.</param>
        /// <param name="value">Значение.</param>
        /// <param name="onSourceDequeue">Функция источника значения, вызываемая при извлечении значения из буфера.</param>
        public void Enqueue(string key, object value, Func<object, bool> onSourceDequeue = null)
        {
            var item = new ClipboardItem(value, onSourceDequeue);

            _buffer[key] = item;
        }

        /// <summary>
        ///     Извлекает значение из буфера.
        /// </summary>
        /// <param name="key">Ключ.</param>
        /// <param name="onDestinationDequeue">Функция приемника значения, вызываемая при извлечении значения из буфера.</param>
        public object Dequeue(string key, Func<object, bool> onDestinationDequeue = null)
        {
            ClipboardItem item;

            if (_buffer.TryGetValue(key, out item)
                && (onDestinationDequeue == null
                    || onDestinationDequeue(item.Value)))
            {
                if (item.TrySourceDequeue())
                {
                    _buffer.Remove(key);
                }

                return item.Value;
            }

            return null;
        }

        /// <summary>
        ///     Удаляет значение из буфера.
        /// </summary>
        /// <param name="key">Ключ.</param>
        public object Remove(string key)
        {
            ClipboardItem item;

            if (_buffer.TryGetValue(key, out item))
            {
                _buffer.Remove(key);

                return item.Value;
            }

            return null;
        }

        /// <summary>
        ///     Возвращает значение из буфера.
        /// </summary>
        /// <param name="key">Ключ.</param>
        public object Peek(string key)
        {
            ClipboardItem item;

            if (_buffer.TryGetValue(key, out item))
            {
                return item.Value;
            }

            return null;
        }

        /// <summary>
        ///     Очищает буфер.
        /// </summary>
        public void Clear()
        {
            _buffer.Clear();
        }

        public sealed class ClipboardItem
        {
            public readonly Func<object, bool> OnSourceDequeue;
            public readonly object Value;

            public ClipboardItem(object value, Func<object, bool> onSourceDequeue)
            {
                Value = value;
                OnSourceDequeue = onSourceDequeue;
            }

            public bool TrySourceDequeue()
            {
                return (OnSourceDequeue == null) || OnSourceDequeue(Value);
            }
        }
    }
}