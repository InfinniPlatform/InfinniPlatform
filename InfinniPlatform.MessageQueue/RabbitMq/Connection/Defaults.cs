using RabbitMQ.Client;

namespace InfinniPlatform.MessageQueue.RabbitMq.Connection
{
    internal static class Defaults
    {
        internal static class Connection
        {
            public static bool AutomaticRecoveryEnabled => true;
        }


        internal static class Exchange
        {
            public static bool Durable => true;

            public static bool AutoDelete => false;


            internal static class Type
            {
                public static string Topic => ExchangeType.Topic;

                public static string Direct => ExchangeType.Direct;

                public static string Fanout => ExchangeType.Fanout;
            }
        }


        internal static class Queue
        {
            public static bool Durable => true;

            public static bool Exclusive => false;

            public static bool AutoDelete => false;
        }
    }
}