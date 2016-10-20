using System;
using System.IO;

using InfinniPlatform.Agent.Settings;

namespace InfinniPlatform.Agent.InfinniNode
{
    public class ConfigurationFileProvider : IConfigurationFileProvider
    {
        private const string AppsDirectoryName = "install";

        public ConfigurationFileProvider(AgentSettings settings)
        {
            _settings = settings;
        }

        private readonly AgentSettings _settings;

        public Func<Stream> Get(string appFullName, string fileName)
        {
            var configFilePath = Path.Combine(_settings.NodeDirectory, AppsDirectoryName, appFullName, fileName);

            return () => new FileStream(configFilePath, FileMode.Open, FileAccess.Read);
        }

        public void Set(string appFullName, string fileName, string content)
        {
            var configFilePath = Path.Combine(_settings.NodeDirectory, AppsDirectoryName, appFullName, fileName);

            File.WriteAllText(configFilePath, content);
        }
    }
}