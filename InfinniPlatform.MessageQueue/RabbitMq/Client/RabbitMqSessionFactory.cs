namespace InfinniPlatform.MessageQueue.RabbitMq.Client
{
    /// <summary>
    /// Фабрика сессий очереди сообщений RabbitMq.
    /// </summary>
    public sealed class RabbitMqSessionFactory : IMessageQueueSessionFactory
    {
        public RabbitMqSessionFactory(string node, int port)
        {
            _node = node;
            _port = port;
        }


        private readonly string _node;
        private readonly int _port;


        /// <summary>
        /// Открыть сессию.
        /// </summary>
        public IMessageQueueSession OpenSession()
        {
            return new RabbitMqSession(_node, _port);
        }
    }
}