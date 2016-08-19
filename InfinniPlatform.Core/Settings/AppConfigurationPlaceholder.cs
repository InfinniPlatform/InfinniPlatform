using System;
using System.Text.RegularExpressions;

using Newtonsoft.Json;

namespace InfinniPlatform.Core.Settings
{
    /// <summary>
    /// Предоставляет методы для заполнения подстановок в конфигурационном файле приложения.
    /// </summary>
    public class AppConfigurationPlaceholder
    {
        /// <summary>
        /// Регулярное выражения для поиска подстановок для переменных окружения.
        /// </summary>
        /// <remarks>
        /// Синтаксис:
        /// <code>
        /// ${variable}
        /// ${variable=default}
        /// </code>
        /// Параметры:
        /// <list type="bullet">
        /// <item><term><c>variable</c></term><description> - имя переменной окружения;</description></item>
        /// <item><term><c>default</c></term><description> - значение по умолчанию.</description></item>
        /// </list>
        /// </remarks>
        /// <example>
        /// <code>
        /// ${INFINNI_BLOBSTORAGE}
        /// ${INFINNI_BLOBSTORAGE=BlobStorage}
        /// </code>
        /// </example>
        private static readonly Regex EnvironmentRegex = new Regex(@"\$\{(?<name>[a-z0-9_-]+)(=(?<defaultValue>.*?)){0,1}\}", RegexOptions.Compiled | RegexOptions.IgnoreCase);


        /// <summary>
        /// Заменяет подстановки для переменные окружения на их значения.
        /// </summary>
        public string ExpandEnvironmentVariables(string content)
        {
            return EnvironmentRegex.Replace(content, m =>
            {
                string value = null;

                // Имя переменной окружения
                var name = m.Groups["name"].Value.Trim();

                if (!string.IsNullOrEmpty(name))
                {
                    // Определение значения переменной окружения
                    value = GetEnvironmentVariable(name);

                    if (string.IsNullOrEmpty(value))
                    {
                        // Определение значения по умолчанию
                        var defaultValue = m.Groups["defaultValue"].Value;
                        value = defaultValue;
                    }
                    else
                    {
                        // Экранирование служебных символов
                        var escapedValue = JsonConvert.ToString(value);
                        value = escapedValue.Substring(1, escapedValue.Length - 2);
                    }
                }

                return value ?? string.Empty;
            });
        }


        private static string GetEnvironmentVariable(string name)
        {
            return ExecuteSilent(() => Environment.GetEnvironmentVariable(name, EnvironmentVariableTarget.Process),
                ExecuteSilent(() => Environment.GetEnvironmentVariable(name, EnvironmentVariableTarget.User),
                    ExecuteSilent(() => Environment.GetEnvironmentVariable(name, EnvironmentVariableTarget.Machine))));
        }


        private static string ExecuteSilent(Func<string> action, string defaultValue = null)
        {
            string value;

            try
            {
                value = action();
            }
            catch
            {
                value = null;
            }

            if (string.IsNullOrEmpty(value))
            {
                value = defaultValue;
            }

            return value;
        }
    }
}