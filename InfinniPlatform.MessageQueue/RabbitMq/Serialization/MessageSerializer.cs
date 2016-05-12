using System;
using System.Text;

using InfinniPlatform.Sdk.Queues;

using Newtonsoft.Json;

namespace InfinniPlatform.MessageQueue.RabbitMq.Serialization
{
    public class MessageSerializer : IMessageSerializer
    {
        private readonly JsonSerializerSettings _serializerSettings = new JsonSerializerSettings
                                                                      {
                                                                          TypeNameHandling = TypeNameHandling.Auto
                                                                      };

        public byte[] MessageToBytes(IMessage message)
        {
            return Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message, _serializerSettings));
        }

        public IMessage BytesToMessage(byte[] bytes, Type type)
        {
            if (bytes == null)
            {
                return null;
            }

            var value = Encoding.UTF8.GetString(bytes);
            return (IMessage)JsonConvert.DeserializeObject(value, type, _serializerSettings);
        }
    }
}