namespace InfinniPlatform.MessageQueue
{
    /// <summary>
    /// RabbitMQ message queue configuration options.
    /// </summary>
    public class RabbitMqMessageQueueOptions : IOptions
    {
        /// <inheritdoc />
        public string SectionName => "rabbitmqMessageQueue";

        /// <summary>
        /// Default instance of <see cref="RabbitMqMessageQueueOptions" />.
        /// </summary>
        public static readonly RabbitMqMessageQueueOptions Default = new RabbitMqMessageQueueOptions();


        /// <summary>
        /// Initializes a new instance of <see cref="AppOptions" />.
        /// </summary>
        public RabbitMqMessageQueueOptions()
        {
            HostName = "localhost";
            Port = 5672;
            UserName = "guest";
            Password = "guest";
            ManagementApiPort = 15672;
            PrefetchCount = 1;
            MaxConcurrentThreads = 200;
            ReconnectTimeout = 10;
            MaxReconnectRetries = 10;
        }


        /// <summary>
        /// RabbitMQ server address.
        /// </summary>
        public string HostName { get; set; }

        /// <summary>
        /// RabbitMQ server port.
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// User name.
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Password.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// RabbitMQ Management API port.
        /// </summary>
        public int ManagementApiPort { get; set; }

        /// <summary>
        /// Amount of messages sended to consumer at once.
        /// </summary>
        public ushort PrefetchCount { get; set; }

        /// <summary>
        /// Maximum concurrents threads for message processing.
        /// </summary>
        public int MaxConcurrentThreads { get; set; }

        /// <summary>
        /// Reconnect timeout in seconds.
        /// </summary>
        public int ReconnectTimeout { get; set; }

        /// <summary>
        /// Maximum number of reconnet retries.
        /// </summary>
        public int MaxReconnectRetries { get; set; }
    }
}