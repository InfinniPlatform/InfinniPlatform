namespace InfinniPlatform.Sdk.Registers
{
    /// <summary>
    /// “ип движени€ ресурсов в рамках регистра
    /// </summary>
    public static class RegisterEntryType
    {
        /// <summary>
        /// ѕриход (увеличение хранимых ресурсов)
        /// </summary>
        public const string Income = "Income";

        /// <summary>
        /// –асход (уменьшение хранимых ресурсов)
        /// </summary>
        public const string Consumption = "Consumption";

        /// <summary>
        /// ƒругое (тип движени€ не определен, допустимо дл€ регистра сведений)
        /// </summary>
        public const string Other = "Other";
    }
}