namespace InfinniPlatform.PrintView.Contract
{
    /// <summary>
    /// Настройки печатных представлений.
    /// </summary>
    public class PrintViewSettings
    {
        /// <summary>
        /// Имя секции в файле конфигурации.
        /// </summary>
        public const string SectionName = "printView";


        /// <summary>
        /// Настройка планировщика заданий по умолчанию.
        /// </summary>
        public static readonly PrintViewSettings Default = new PrintViewSettings();


        /// <summary>
        /// Конструктор.
        /// </summary>
        public PrintViewSettings()
        {
        }


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