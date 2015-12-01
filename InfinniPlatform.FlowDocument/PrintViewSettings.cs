namespace InfinniPlatform.FlowDocument
{
    /// <summary>
    /// Настройка печатных представлений.
    /// </summary>
    public sealed class PrintViewSettings
    {
        public const string SectionName = "printView";


        public static readonly PrintViewSettings Default = new PrintViewSettings();


        /// <summary>
        /// Формат команды вызова утилиты wkhtmltopdf - http://wkhtmltopdf.org/ (по умолчанию - генерируется автоматически с учетом операционной системы).
        /// </summary>
        public string HtmlToPdfUtil { get; set; }

        /// <summary>
        /// Каталог для хранения временных файлов при генерации HTML/PDF (по умолчанию - каталог временных файлов учетной записи пользователя).
        /// </summary>
        public string HtmlToPdfTemp { get; set; }
    }
}