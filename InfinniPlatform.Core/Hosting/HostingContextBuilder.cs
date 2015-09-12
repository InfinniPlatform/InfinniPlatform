using System;
using InfinniPlatform.Api.Hosting;
using InfinniPlatform.Sdk.Api;

namespace InfinniPlatform.Hosting
{
    /// <summary>
    ///     Построитель контекста подсистемы хостинга.
    /// </summary>
    public sealed class HostingContextBuilder
    {
        private readonly HostingContext _context = new HostingContext();

        /// <summary>
        ///     Контекст подсистемы хостинга.
        /// </summary>
        public IHostingContext Context
        {
            get { return _context; }
        }

        /// <summary>
        ///     Настройки подсистемы хостинга.
        /// </summary>
        public HostingContextBuilder Configuration(HostingConfig value)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }

            _context.Configuration = value;

            return this;
        }

        /// <summary>
        ///     Устанавливает значение переменной окружения подсистемы хостинга.
        /// </summary>
        public HostingContextBuilder SetEnvironment<T>(T value)
        {
            _context.Set(value);

            return this;
        }

        /// <summary>
        ///     Устанавливает значение переменной окружения подсистемы хостинга.
        /// </summary>
        public HostingContextBuilder SetEnvironment<T>(string key, T value)
        {
            _context.Set(key, value);

            return this;
        }
    }
}