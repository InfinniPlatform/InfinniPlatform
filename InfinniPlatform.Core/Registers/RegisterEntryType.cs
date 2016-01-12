namespace InfinniPlatform.Core.Registers
{
    /// <summary>
    /// Тип движения ресурсов в рамках регистра
    /// </summary>
    public static class RegisterEntryType
    {
        /// <summary>
        /// Приход (увеличение хранимых ресурсов)
        /// </summary>
        public const string Income = "Income";

        /// <summary>
        /// Расход (уменьшение хранимых ресурсов)
        /// </summary>
        public const string Consumption = "Consumption";

        /// <summary>
        /// Другое (тип движения не определен, допустимо для регистра сведений)
        /// </summary>
        public const string Other = "Other";
    }
}