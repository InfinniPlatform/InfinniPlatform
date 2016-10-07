namespace InfinniPlatform.Agent.Helpers
{
    public static class CommandArgsHelper
    {
        /// <summary>
        /// Добавляет к строке запуска команды аргумент, если он не пустой.
        /// </summary>
        /// <param name="args">Строка запуска команды.</param>
        /// <param name="key">Ключ для аргумента.</param>
        /// <param name="value">Значение аргумента.</param>
        /// <returns></returns>
        public static string AppendArg(this string args, string key, string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                args += $" -{key} {value}";
            }

            return args;
        }

        /// <summary>
        /// Добавляет к строке запуска команды аргумент, если он не пустой.
        /// </summary>
        /// <param name="args">Строка запуска команды.</param>
        /// <param name="key">Ключ для аргумента.</param>
        /// <param name="value">Значение аргумента.</param>
        /// <returns></returns>
        public static string AppendArg(this string args, string key, bool? value)
        {
            if (value != null && value.Value)
            {
                args += $" -{key}";
            }

            return args;
        }
    }
}