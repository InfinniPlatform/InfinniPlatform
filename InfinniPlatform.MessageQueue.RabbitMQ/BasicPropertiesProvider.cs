using System;
using System.Collections.Generic;
using System.Linq;

using InfinniPlatform.Core.Serialization;
using InfinniPlatform.Core.Settings;
using InfinniPlatform.MessageQueue.Abstractions;

using RabbitMQ.Client.Framing;

namespace InfinniPlatform.MessageQueue.RabbitMQ
{
    internal sealed class BasicPropertiesProvider : IBasicPropertiesProvider
    {
        public BasicPropertiesProvider(AppOptions appOptions,
                                       IJsonObjectSerializer serializer)
        {
            _appOptions = appOptions;
            _serializer = serializer;
        }

        private readonly AppOptions _appOptions;
        private readonly IJsonObjectSerializer _serializer;

        public BasicProperties Get()
        {
            return new BasicProperties
                   {
                       AppId = _appOptions.AppInstance,
                       Headers = new Dictionary<string, object>()
                   };
        }

        public BasicProperties GetPersistent()
        {
            return new BasicProperties
                   {
                       AppId = _appOptions.AppInstance,
                       Headers = new Dictionary<string, object>(),
                       Persistent = true
                   };
        }

        public Dictionary<string, Func<string>> GetHeaders(IMessage message)
        {
            var dictionary = new Dictionary<string, Func<string>>();

            foreach (var header in message.Headers.Where(pair => pair.Value is byte[]))
            {
                var value = (byte[])header.Value;

                if (value != null)
                {
                    dictionary.Add(header.Key, () => _serializer.Deserialize<string>(value));
                }
            }

            return dictionary;
        }
    }
}