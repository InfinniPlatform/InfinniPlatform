using RabbitMQ.Client;

namespace InfinniPlatform.MessageQueue.Management
{
    internal static class RabbitMqDefaults
    {
        public static class Exchange
        {
            /// <summary>
            /// Точка обмена не удаляется при перезапуске сервера.
            /// </summary>
            public const bool Durable = true;

            /// <summary>
            /// Точка обмена не удаляется при отключении всех очередей.
            /// </summary>
            public const bool AutoDelete = false;


            /// <summary>
            /// Типы точек обмена.
            /// </summary>
            public static class Type
            {
                public const string Topic = ExchangeType.Topic;

                public const string Direct = ExchangeType.Direct;

                public const string Fanout = ExchangeType.Fanout;
            }
        }


        public static class Queue
        {
            /// <summary>
            /// Очередь не удаляется при перезапуске сервера.
            /// </summary>
            public const bool Durable = true;

            /// <summary>
            /// Очередь может быть доступна для всех подключений.
            /// </summary>
            public const bool Exclusive = false;

            /// <summary>
            /// Очередь не удаляется после отключения всех потребителей.
            /// </summary>
            public const bool AutoDelete = false;
        }
    }
}