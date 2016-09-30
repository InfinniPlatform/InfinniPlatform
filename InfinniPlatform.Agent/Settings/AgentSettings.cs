namespace InfinniPlatform.Agent.Settings
{
    public class AgentSettings
    {
        public const string SectionName = "agentSettings";

        public AgentSettings()
        {
            NodeDirectory = string.Empty;
            ServerAddress = string.Empty;
            Token = string.Empty;
        }

        /// <summary>
        /// Директория, содержащая утилиту Infinni.Node.
        /// </summary>
        public string NodeDirectory { get; set; }

        /// <summary>
        /// Адрес сервера InfinniPlatform.Server.
        /// </summary>
        public string ServerAddress { get; set; }

        /// <summary>
        /// Уникальный token для аутентификации при обмене данными между агентом и сервером.
        /// </summary>
        public string Token { get; set; }
    }
}