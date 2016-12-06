namespace InfinniPlatform.Heartbeat.Settings
{
    public class HeartbeatSettings
    {
        public const string SectionName = "heartbeat";

        public HeartbeatSettings()
        {
            Period = 5;
        }

        /// <summary>
        /// Периодичность оповещения сервера (в секундах).
        /// </summary>
        public int Period { get; set; }

        /// <summary>
        /// Адрес сервера Infinni.Server.
        /// </summary>
        public string ServerAddress { get; set; }

        /// <summary>
        /// Уникальный token для аутентификации при обмене данными между агентом и сервером.
        /// </summary>
        public string Token { get; set; }
    }
}