using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

using InfinniPlatform.Scheduler.Contract;
using InfinniPlatform.Scheduler.Properties;
using InfinniPlatform.Sdk.IoC;

namespace InfinniPlatform.Scheduler.Common
{
    internal class JobHandlerTypeSerializer : IJobHandlerTypeSerializer
    {
        /// <summary>
        /// Регулярное выражение для десериализации типа обработчика заданий.
        /// </summary>
        private static readonly Regex HandlerTypeRegex = new Regex(@"^\s*(?<typeName>.+?)\s*,\s*(?<assemblyName>.+?)\s*$", RegexOptions.Compiled);


        public JobHandlerTypeSerializer(IContainerResolver resolver)
        {
            _resolver = resolver;
            _cache = new ConcurrentDictionary<string, Type>(StringComparer.Ordinal);
        }


        private readonly IContainerResolver _resolver;
        private readonly ConcurrentDictionary<string, Type> _cache;


        public bool CanSerialize(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            var typeInfo = type.GetTypeInfo();

            return typeInfo.IsClass
                   && !typeInfo.IsAbstract
                   && !typeInfo.IsGenericType
                   && typeof(IJobHandler).GetTypeInfo().IsAssignableFrom(type)
                   && _resolver.Services.Any(t => t == type);
        }

        public string Serialize(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            // Полное имя типа
            var typeName = type.FullName;

            // Только имя сборки (без версии и т.п.)
            var assemblyName = type.GetTypeInfo().Assembly.GetName().Name;

            return $"{typeName},{assemblyName}";
        }

        public IJobHandler Deserialize(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException(nameof(value));
            }

            Type type;

            if (_cache.TryGetValue(value, out type))
            {
                return (IJobHandler)_resolver.Resolve(type);
            }

            var match = HandlerTypeRegex.Match(value);

            if (!match.Success)
            {
                throw new ArgumentException(string.Format(Resources.HandlerTypeIsNotParsed, value), nameof(value));
            }

            // Полное имя типа
            var typeName = match.Groups["typeName"].Value;

            // Только имя сборки (без версии и т.п.)
            var assemblyName = match.Groups["assemblyName"].Value;

            // Поиск типа обработчика заданий в контейнере зависимостей
            type = _resolver.Services.FirstOrDefault(t => string.Equals(t.FullName, typeName, StringComparison.Ordinal)
                                                          && string.Equals(t.GetTypeInfo().Assembly.GetName().Name, assemblyName, StringComparison.Ordinal));

            if (type == null)
            {
                throw new InvalidOperationException(string.Format(Resources.HandlerTypeIsNotRegistered, value));
            }

            type = _cache.GetOrAdd(value, type);

            return (IJobHandler)_resolver.Resolve(type);
        }
    }
}