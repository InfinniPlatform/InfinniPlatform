namespace InfinniPlatform.Server.Settings
{
    public class ServerSettings
    {
        public const string SectionName = "serverSettings";

        public ServerSettings()
        {
            AgentsInfo = new AgentInfo[] { };
        }

        /// <summary>
        /// Информация об экземплярах приложения InfinniPlatform.Agent.
        /// </summary>
        public AgentInfo[] AgentsInfo { get; set; }
    }


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