using System;
using System.IO;

using InfinniPlatform.Sdk.Settings;

using Newtonsoft.Json.Linq;

namespace InfinniPlatform.Core.Settings
{
    public sealed class AppConfiguration : IAppConfiguration
    {
        /// <summary>
        /// Статический экземпляр.
        /// </summary>
        public static readonly AppConfiguration Instance = new AppConfiguration();


        public AppConfiguration()
        {
            _appConfig = new Lazy<JObject>(ReadAppConfig);
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


        private JObject ReadAppConfig()
        {
            var appCommonConfigPath = AppSettings.GetValue("AppCommonConfig", "AppCommon.json");
            var appExtensionConfigPath = AppSettings.GetValue("AppExtensionConfig", "AppExtension.json");

            var appCommonConfig = TryReadConfig(appCommonConfigPath);
            var appExtensionConfig = TryReadConfig(appExtensionConfigPath);

            var appConfig = TryMergeConfig(appCommonConfig, appExtensionConfig);

            return appConfig;
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


        /// <summary>
        /// Производит слияние общей и дополнительной конфигурации.
        /// </summary>
        private static JObject TryMergeConfig(JObject commonConfig, JObject extensionConfig)
        {
            if (commonConfig != null && extensionConfig != null)
            {
                var mergeSettings = new JsonMergeSettings { MergeArrayHandling = MergeArrayHandling.Replace };

                commonConfig.Merge(extensionConfig, mergeSettings);

                return commonConfig;
            }

            return (commonConfig ?? extensionConfig) ?? new JObject();
        }
    }
}