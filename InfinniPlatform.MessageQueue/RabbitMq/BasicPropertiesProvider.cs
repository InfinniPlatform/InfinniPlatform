using System;
using System.Collections.Generic;
using System.Linq;

using InfinniPlatform.Sdk.Queues;
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

        public BasicProperties Create()
        {
            var userIdentity = _identityProvider.GetUserIdentity();

            return new BasicProperties
                   {
                       AppId = _appEnvironment.Id,
                       Headers = new Dictionary<string, object>
                                 {
                                     { MessageHeadersTypes.UserName, _serializer.Serialize(userIdentity.Name) },
                                     { MessageHeadersTypes.TenantId, _serializer.Serialize(userIdentity.FindFirstClaim(ApplicationClaimTypes.TenantId)) }
                                 }
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