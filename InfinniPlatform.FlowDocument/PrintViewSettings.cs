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
        /// Местоположение утилиты wkhtmltopdf - http://wkhtmltopdf.org/ (по умолчанию - генерируется автоматически с учетом операционной системы).
        /// </summary>
        public string HtmlToPdfUtilCommand { get; set; }

        /// <summary>
        /// Формат аргументов утилиты wkhtmltopdf - http://wkhtmltopdf.org/ (по умолчанию - генерируется автоматически с учетом операционной системы).
        /// </summary>
        public string HtmlToPdfUtilArguments { get; set; }

        /// <summary>
        /// Каталог для хранения временных файлов при генерации HTML/PDF (по умолчанию - каталог временных файлов учетной записи пользователя).
        /// </summary>
        public string HtmlToPdfTemp { get; set; }
    }
}