using System.Collections.Generic;
using InfinniPlatform.Api.Hosting;
using InfinniPlatform.Sdk.Api;

namespace InfinniPlatform.Hosting
{
    /// <summary>
    ///     Контекст подсистемы хостинга.
    /// </summary>
    internal sealed class HostingContext : IHostingContext
    {
        public HostingContext()
        {
            Configuration = new HostingConfig();
            Environment = new Dictionary<string, object>();
        }

        /// <summary>
        ///     Настройки подсистемы хостинга.
        /// </summary>
        public HostingConfig Configuration { get; set; }

        /// <summary>
        ///     Окружение подсистемы хостинга.
        /// </summary>
        public IDictionary<string, object> Environment { get; set; }

        /// <summary>
        ///     Возвращает значение переменной окружения подсистемы хостинга.
        /// </summary>
        public T Get<T>()
        {
            object value;
            return Environment.TryGetValue(typeof (T).FullName, out value) ? (T) value : default(T);
        }

        /// <summary>
        ///     Возвращает значение переменной окружения подсистемы хостинга.
        /// </summary>
        public T Get<T>(string key)
        {
            object value;
            return Environment.TryGetValue(key, out value) ? (T) value : default(T);
        }

        /// <summary>
        ///     Устанавливает значение переменной окружения подсистемы хостинга.
        /// </summary>
        public void Set<T>(T value)
        {
            Environment[typeof (T).FullName] = value;
        }

        /// <summary>
        ///     Устанавливает значение переменной окружения подсистемы хостинга.
        /// </summary>
        public void Set<T>(string key, T value)
        {
            Environment[key] = value;
        }
    }
}