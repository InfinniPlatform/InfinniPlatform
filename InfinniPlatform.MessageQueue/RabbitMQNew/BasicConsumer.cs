﻿using System;
using System.Text;

using RabbitMQ.Client;

namespace InfinniPlatform.MessageQueue.RabbitMQNew
{
    internal sealed class BasicConsumer : IBasicConsumer
    {
        private const string QueueKey = "test_queue";

        public BasicConsumer(RabbitMqManager manager)
        {
            _channel = manager.GetConnection().CreateModel();
            manager.GetQueue(QueueKey);
            _channel.BasicQos(0, 1, false);
        }

        private IModel _channel;

        public string Get()
        {
            var message = _channel.BasicGet(QueueKey, false);
            var serializedMessage = message == null
                                        ? null
                                        : Encoding.UTF8.GetString(message.Body);

            Console.WriteLine($"BasicConsumer: {serializedMessage}");

            return serializedMessage;
        }

        public void Dispose()
        {
            if (_channel != null)
            {
                _channel.Close();
                _channel = null;
            }
        }
    }
}