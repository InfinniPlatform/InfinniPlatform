﻿namespace InfinniPlatform.MessageQueue.RabbitMQNew
{
    /// <summary>
    /// Настройки подключения к MongoDB.
    /// </summary>
    internal sealed class RabbitMqConnectionSettings
    {
        public const string SectionName = "rabbitmq";

        public static RabbitMqConnectionSettings Default = new RabbitMqConnectionSettings();


        public RabbitMqConnectionSettings()
        {
            HostName = "localhost";
            Port = 5672;
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
    }
}