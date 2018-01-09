using System;
using System.Collections.Generic;
using System.Linq;
using InfinniPlatform.Serialization;
using RabbitMQ.Client.Framing;

namespace InfinniPlatform.MessageQueue
{
    /// <inheritdoc />
    public class BasicPropertiesProvider : IBasicPropertiesProvider
    {
        private readonly AppOptions _appOptions;
        private readonly IJsonObjectSerializer _serializer;

        /// <summary>
        /// Initializes a new instance of <see cref="BasicPropertiesProvider" />.
        /// </summary>
        /// <param name="appOptions">Common application settings.</param>
        /// <param name="serializer">JSON objects serializer.</param>
        public BasicPropertiesProvider(AppOptions appOptions, IJsonObjectSerializer serializer)
        {
            _appOptions = appOptions;
            _serializer = serializer;
        }


        /// <inheritdoc />
        public BasicProperties Get()
        {
            return new BasicProperties
            {
                AppId = _appOptions.AppInstance,
                Headers = new Dictionary<string, object>()
            };
        }

        /// <inheritdoc />
        public BasicProperties GetPersistent()
        {
            return new BasicProperties
            {
                AppId = _appOptions.AppInstance,
                Headers = new Dictionary<string, object>(),
                Persistent = true
            };
        }

        /// <inheritdoc />
        public Dictionary<string, Func<string>> GetHeaders(IMessage message)
        {
            var dictionary = new Dictionary<string, Func<string>>();

            foreach (var header in message.Headers.Where(pair => pair.Value is byte[]))
            {
                var value = (byte[]) header.Value;

                if (value != null)
                {
                    dictionary.Add(header.Key, () => _serializer.Deserialize<string>(value));
                }
            }

            return dictionary;
        }
    }
}