using System;
using System.IO;

using InfinniPlatform.Sdk.Settings;

using Newtonsoft.Json.Linq;

namespace InfinniPlatform.Core.Settings
{
    public sealed class AppConfiguration : IAppConfiguration
    {
        private const string AppConfigPath = "AppConfig.json";

        public AppConfiguration()
        {
            _appConfig = new Lazy<JObject>(() => TryReadConfig(AppConfigPath));
            _appConfigPlaceholder = new AppConfigurationPlaceholder();
        }


        private readonly Lazy<JObject> _appConfig;
        private readonly AppConfigurationPlaceholder _appConfigPlaceholder;


        public dynamic GetSection(string sectionName)
        {
            if (string.IsNullOrEmpty(sectionName))
            {
                throw new ArgumentException(nameof(sectionName));
            }

            var sectionValue = (JObject)_appConfig.Value.GetValue(sectionName, StringComparison.OrdinalIgnoreCase);

            return sectionValue ?? new JObject();
        }

        public TSection GetSection<TSection>(string sectionName) where TSection : new()
        {
            if (string.IsNullOrEmpty(sectionName))
            {
                throw new ArgumentException(nameof(sectionName));
            }

            var sectionValue = _appConfig.Value.GetValue(sectionName, StringComparison.OrdinalIgnoreCase);

            return (sectionValue != null) ? sectionValue.ToObject<TSection>() : new TSection();
        }


        /// <summary>
        /// Производит чтение конфигурационного файла.
        /// </summary>
        private JObject TryReadConfig(string configPath)
        {
            if (File.Exists(configPath))
            {
                var config = File.ReadAllText(configPath);

                config = _appConfigPlaceholder.ExpandEnvironmentVariables(config);

                return JObject.Parse(config);
            }

            return null;
        }
    }
}