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
}