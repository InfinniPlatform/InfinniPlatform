namespace InfinniPlatform.MessageQueue
{
    /// <summary>
    /// Настройки подключения к RqbbitMq. 
    /// </summary>
    public sealed class RabbitMqSettings
    {
        public static readonly RabbitMqSettings Default = new RabbitMqSettings();


        public RabbitMqSettings()
        {
            Node = "localhost";
            Port = -1;
        }


        /// <summary>
        /// Узел для подключения.
        /// </summary>
        public string Node { get; set; }

        /// <summary>
        /// Порт для подключения.
        /// </summary>
        public int Port { get; set; }
    }
}