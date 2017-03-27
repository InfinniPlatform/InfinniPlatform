using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace InfinniPlatform.Core.Settings
{
    /// <summary>
    /// Provides access to the current application's default configuration.
    /// </summary>
    public static class AppSettings
    {
        static AppSettings()
        {
            AppDomainSettings = new Lazy<Dictionary<string, string>>(GetAppDomainSettings);
        }


        private static readonly Lazy<Dictionary<string, string>> AppDomainSettings;


        /// <summary>
        /// Gets the setting value associated with the specified name.
        /// </summary>
        /// <param name="name">Name of the setting.</param>
        /// <param name="defaultValue">Default value of the setting (if not defined).</param>
        public static string GetValue(string name, string defaultValue = null)
        {
            var value = GetSettingValue(name);

            return value ?? defaultValue;
        }

        /// <summary>
        /// Gets the setting value associated with the specified name.
        /// </summary>
        /// <typeparam name="T">Type of the setting.</typeparam>
        /// <param name="name">Name of the setting.</param>
        /// <param name="defaultValue">Default value of the setting (if not defined).</param>
        /// <returns></returns>
        public static T GetValue<T>(string name, T defaultValue = default(T))
        {
            var result = defaultValue;

            var value = GetSettingValue(name);

            if (string.IsNullOrEmpty(value) == false)
            {
                var converter = TypeDescriptor.GetConverter(typeof(T));
                result = (T)converter.ConvertFromInvariantString(value);
            }

            return result;
        }

        /// <summary>
        /// Gets the setting values associated with the specified name.
        /// </summary>
        /// <typeparam name="T">Type of the setting.</typeparam>
        /// <param name="name">Name of the setting.</param>
        /// <param name="defaultValue">Default value of the setting (if not defined).</param>
        public static T[] GetValues<T>(string name, params T[] defaultValue)
        {
            var result = defaultValue;

            var value = GetSettingValue(name);

            if (string.IsNullOrEmpty(value) == false)
            {
                var items = value.Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);

                if (items.Length > 0)
                {
                    var converter = TypeDescriptor.GetConverter(typeof(T));
                    result = items.Select(i => (T)converter.ConvertFromInvariantString(i)).ToArray();
                }
            }

            return result;
        }


        private static Dictionary<string, string> GetAppDomainSettings()
        {
            // Это оказался самый рабочий и надежный способ в Mono/Linux!

            var appSettings = new Dictionary<string, string>();

            try
            {
                //TODO Find a way to read XML config or remove it.
                var configFile = File.ReadAllText("app.config");

                if (!string.IsNullOrEmpty(configFile) && File.Exists(configFile))
                {
                    var config = XDocument.Load(configFile);

                    if (config.Document != null && config.Document.Root != null)
                    {
                        var appSettingsNodes = config.Document.Root
                            .Elements("appSettings")
                            .SelectMany(i => i.Elements("add"))
                            .Select(i => new { key = i.Attribute("key"), value = i.Attribute("value") });

                        foreach (var item in appSettingsNodes)
                        {
                            if (item.key != null && !string.IsNullOrEmpty(item.key.Value))
                            {
                                appSettings[item.key.Value] = (item.value != null) ? item.value.Value : null;
                            }
                        }
                    }
                }
            }
            catch
            {
            }

            return appSettings;
        }

        private static string GetSettingValue(string key)
        {
            string value;

            if (!string.IsNullOrEmpty(key) && AppDomainSettings.Value.TryGetValue(key, out value))
            {
                return value;
            }

            return null;
        }
    }
}