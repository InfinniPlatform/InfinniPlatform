using System;
using System.Text;

using InfinniPlatform.Sdk.Queues;

using Newtonsoft.Json;

namespace InfinniPlatform.MessageQueue.RabbitMq
{
    public interface IMessageSerializer
    {
        /// <summary>
        /// Преобразует сообщение в массив байтов для передачи в шину сообщений.
        /// </summary>
        /// <param name="message">Сообщение.</param>
        byte[] MessageToBytes(IMessage message);

        /// <summary>
        /// Преобразует массив байтов из шины сообщения в строготипизированный объект.
        /// </summary>
        /// <param name="bytes">Массив байтов из шины сообщений.</param>
        /// <param name="type">Тип тела сообщения.</param>
        /// <returns></returns>
        IMessage BytesToMessage(byte[] bytes, Type type);
    }


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