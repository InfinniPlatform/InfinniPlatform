namespace InfinniPlatform.MessageQueue
{
    /// <summary>
    /// Настройки очереди сообщений на основе RabbitMQ.
    /// </summary>
    public class RabbitMqMessageQueueOptions
    {
        public const string SectionName = "rabbitmqMessageQueue";

        public static readonly RabbitMqMessageQueueOptions Default = new RabbitMqMessageQueueOptions();


        public RabbitMqMessageQueueOptions()
        {
            HostName = "localhost";
            Port = 5672;
            UserName = "guest";
            Password = "guest";
            ManagementApiPort = 15672;
            PrefetchCount = 1;
            MaxConcurrentThreads = 200;
            ReconnectTimeout = 5;
        }


        /// <summary>
        /// Имя/адрес сервера.
        /// </summary>
        public string HostName { get; set; }

        /// <summary>
        /// Порт, прослушиваемый сервером.
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// Имя пользователя.
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Пароль пользователя.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Порт .
        /// </summary>
        public int ManagementApiPort { get; set; }

        /// <summary>
        /// Количество сообщений, единовременно передаваемых потребителю.
        /// </summary>
        public ushort PrefetchCount { get; set; }

        /// <summary>
        /// Максимальное количество одновременно обрабатываемых сообщений.
        /// </summary>
        public int MaxConcurrentThreads { get; set; }

        /// <summary>
        /// Время между попытками переподключения к серверу RabbitMQ в cекундах.
        /// </summary>
        public int ReconnectTimeout { get; set; }
    }
}