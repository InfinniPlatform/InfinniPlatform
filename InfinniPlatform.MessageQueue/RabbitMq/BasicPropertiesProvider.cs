using System;
using System.Collections.Generic;
using System.Linq;

using InfinniPlatform.MessageQueue.Contract;
using InfinniPlatform.Sdk.Security;
using InfinniPlatform.Sdk.Serialization;
using InfinniPlatform.Sdk.Settings;

using RabbitMQ.Client.Framing;

namespace InfinniPlatform.MessageQueue.RabbitMq
{
    internal sealed class BasicPropertiesProvider : IBasicPropertiesProvider
    {
        public BasicPropertiesProvider(IAppEnvironment appEnvironment,
                                       IUserIdentityProvider identityProvider,
                                       IJsonObjectSerializer serializer)
        {
            _appEnvironment = appEnvironment;
            _identityProvider = identityProvider;
            _serializer = serializer;
        }

        private readonly IAppEnvironment _appEnvironment;
        private readonly IUserIdentityProvider _identityProvider;
        private readonly IJsonObjectSerializer _serializer;

        public BasicProperties Get()
        {
            return new BasicProperties
                   {
                       AppId = _appEnvironment.InstanceId,
                       Headers = BuildHeaders()
                   };
        }

        public BasicProperties GetPersistent()
        {
            return new BasicProperties
                   {
                       AppId = _appEnvironment.InstanceId,
                       Headers = BuildHeaders(),
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

        private Dictionary<string, object> BuildHeaders()
        {
            var headers = new Dictionary<string, object>();

            var userIdentity = _identityProvider.GetUserIdentity();

            if (userIdentity != null)
            {
                headers.Add(MessageHeadersTypes.UserName, _serializer.Serialize(userIdentity.Name));
                headers.Add(MessageHeadersTypes.TenantId, _serializer.Serialize(userIdentity.FindFirstClaim(ApplicationClaimTypes.TenantId)));
            }

            return headers;
        }
    }
}