namespace InfinniPlatform.Server.Settings
{
    public class AgentInfo
    {
        /// <summary>
        /// Адресс сервера агента.
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