using System;
using System.IO;
using System.Text;

using InfinniPlatform.Sdk.Settings;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace InfinniPlatform.Core.Settings
{
    public sealed class AppConfiguration : IAppConfiguration
    {
        public static readonly AppConfiguration Instance = new AppConfiguration();


        public AppConfiguration()
        {
            _appConfig = new Lazy<JObject>(ReadAppConfig);
        }


        private readonly Lazy<JObject> _appConfig;


        public JObject GetSection(string sectionName)
        {
            if (string.IsNullOrEmpty(sectionName))
            {
                throw new ArgumentException(nameof(sectionName));
            }

            var sectionValue = (JObject) _appConfig.Value.GetValue(sectionName, StringComparison.OrdinalIgnoreCase);

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


        private static JObject ReadAppConfig()
        {
            var appCommonConfigPath = AppSettings.GetValue("AppCommonConfig", "AppCommon.json");
            var appExtensionConfigPath = AppSettings.GetValue("AppExtensionConfig", "AppExtension.json");

            var appCommonConfig = TryReadConfig(appCommonConfigPath);
            var appExtensionConfig = TryReadConfig(appExtensionConfigPath);

            var appConfig = TryMergeConfig(appCommonConfig, appExtensionConfig);

            return appConfig;
        }

        private static JObject TryReadConfig(string configPath)
        {
            if (File.Exists(configPath))
            {
                using (var reader = new StreamReader(configPath, Encoding.UTF8))
                {
                    using (var jReader = new JsonTextReader(reader))
                    {
                        return JObject.Load(jReader);
                    }
                }
            }

            return null;
        }

        private static JObject TryMergeConfig(JObject commonConfig, JObject extensionConfig)
        {
            if (commonConfig != null && extensionConfig != null)
            {
                var mergeSettings = new JsonMergeSettings { MergeArrayHandling = MergeArrayHandling.Replace };

                commonConfig.Merge(extensionConfig, mergeSettings);

                return commonConfig;
            }

            return commonConfig ?? extensionConfig;
        }
    }
}