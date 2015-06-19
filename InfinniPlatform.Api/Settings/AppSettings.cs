using System;
using System.ComponentModel;
using System.Configuration;
using System.Linq;

namespace InfinniPlatform.Api.Settings
{
    /// <summary>
    ///     Provides access to the current application's default configuration.
    /// </summary>
    public static class AppSettings
    {
        /// <summary>
        ///     Gets the setting value associated with the specified name.
        /// </summary>
        /// <param name="name">Name of the setting.</param>
        /// <param name="defaultValue">Default value of the setting (if not defined).</param>
        public static string GetValue(string name, string defaultValue = null)
        {
            var value = ConfigurationManager.AppSettings[name];

            return value ?? defaultValue;
        }

        /// <summary>
        ///     Gets the setting value associated with the specified name.
        /// </summary>
        /// <typeparam name="T">Type of the setting.</typeparam>
        /// <param name="name">Name of the setting.</param>
        /// <param name="defaultValue">Default value of the setting (if not defined).</param>
        /// <returns></returns>
        public static T GetValue<T>(string name, T defaultValue = default(T))
        {
            var result = defaultValue;

            var value = ConfigurationManager.AppSettings[name];

            if (string.IsNullOrEmpty(value) == false)
            {
                var converter = TypeDescriptor.GetConverter(typeof (T));
                result = (T) converter.ConvertFromInvariantString(value);
            }

            return result;
        }

        /// <summary>
        ///     Gets the setting values associated with the specified name.
        /// </summary>
        /// <typeparam name="T">Type of the setting.</typeparam>
        /// <param name="name">Name of the setting.</param>
        /// <param name="defaultValue">Default value of the setting (if not defined).</param>
        public static T[] GetValues<T>(string name, params T[] defaultValue)
        {
            var result = defaultValue;

            var value = ConfigurationManager.AppSettings[name];

            if (string.IsNullOrEmpty(value) == false)
            {
                var items = value.Split(new[] {';'}, StringSplitOptions.RemoveEmptyEntries);

                if (items.Length > 0)
                {
                    var converter = TypeDescriptor.GetConverter(typeof (T));
                    result = items.Select(i => (T) converter.ConvertFromInvariantString(i)).ToArray();
                }
            }

            return result;
        }
    }
}