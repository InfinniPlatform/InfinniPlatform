namespace Infinni.Server.Settings
{
    public class ServerSettings
    {
        public const string SectionName = "server";

        public ServerSettings()
        {
            AgentsInfoFilePath = "agents.json";
        }

        /// <summary>
        /// Путь до файла с информацией об экземплярах приложения Infinni.Agent.
        /// </summary>
        public string AgentsInfoFilePath { get; set; }
    }
}