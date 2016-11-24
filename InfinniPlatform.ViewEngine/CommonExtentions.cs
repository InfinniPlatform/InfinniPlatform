namespace InfinniPlatform.ViewEngine
{
    /// <summary>
    /// Содержит общие и часто используемые методы расширения.
    /// </summary>
    public static class CommonExtensions
    {
        /// <summary>
        /// Заменяет обратную косую черту в строке на прямую косую черту.
        /// </summary>
        /// <param name="s">Исходная строка.</param>
        public static string ToWebPath(this string s)
        {
            return s.Replace("\\", "/").TrimEnd('/');
        }

        /// <summary>
        /// Заменяет прямую косую черту в строке на обратную косую черту.
        /// </summary>
        /// <param name="s">Исходная строка.</param>
        public static string ToFileSystemPath(this string s)
        {
            return s.Replace("/", "\\");
        }
    }
}