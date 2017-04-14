using System;
using System.Collections.Generic;
using System.Linq;

using InfinniPlatform.MessageQueue.Contract;
using InfinniPlatform.Sdk.Serialization;
using InfinniPlatform.Sdk.Settings;

using RabbitMQ.Client.Framing;

namespace InfinniPlatform.MessageQueue.RabbitMq
{
    internal sealed class BasicPropertiesProvider : IBasicPropertiesProvider
    {
        public BasicPropertiesProvider(IAppEnvironment appEnvironment,
                                       IJsonObjectSerializer serializer)
        {
            _appEnvironment = appEnvironment;
            _serializer = serializer;
        }

        private readonly IAppEnvironment _appEnvironment;
        private readonly IJsonObjectSerializer _serializer;

        public BasicProperties Get()
        {
            return new BasicProperties
                   {
                       AppId = _appEnvironment.InstanceId,
                       Headers = new Dictionary<string, object>()
                   };
        }

        public BasicProperties GetPersistent()
        {
            return new BasicProperties
                   {
                       AppId = _appEnvironment.InstanceId,
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