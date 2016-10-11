using System.IO;

using InfinniPlatform.Agent.Settings;
using InfinniPlatform.Sdk.Services;

namespace InfinniPlatform.Agent.InfinniNode
{
    public class ConfigurationFileProvider : IConfigurationFileProvider
    {
        public ConfigurationFileProvider(AgentSettings settings)
        {
            _settings = settings;
        }

        private readonly AgentSettings _settings;

        public StreamHttpResponse Get(string appFullName, string fileName)
        {
            var configFilePath = Path.Combine(_settings.NodeDirectory, "install", appFullName, fileName);

            return new StreamHttpResponse(configFilePath, "application/json");
        }

        public void Set(string appFullName, string fileName, string content)
        {
            var configFilePath = Path.Combine(_settings.NodeDirectory, "install", appFullName, fileName);

            File.WriteAllText(configFilePath, content);
        }
    }
}