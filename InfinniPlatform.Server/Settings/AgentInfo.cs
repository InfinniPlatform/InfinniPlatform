namespace InfinniPlatform.Server.Settings
{
    /// <summary>
    /// Информация об агенте.
    /// </summary>
    public class AgentInfo
    {
        /// <summary>
        /// Наименование агента.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Адрес сервера агента.
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Порт сервера агента.
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// Токен.
        /// </summary>
        public string Token { get; set; }
    }
}