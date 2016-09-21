using System;

namespace InfinniPlatform.MessageQueue.RabbitMq.Management
{
    /// <summary>
    /// Настройки подключения к MongoDB.
    /// </summary>
    public sealed class RabbitMqConnectionSettings
    {
        public const string SectionName = "rabbitmq";

        public static RabbitMqConnectionSettings Default = new RabbitMqConnectionSettings();

        public RabbitMqConnectionSettings()
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