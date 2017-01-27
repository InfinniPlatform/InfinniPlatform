namespace InfinniPlatform.PrintView.Contract
{
    /// <summary>
    /// Настройки утилиты wkhtmltopdf - http://wkhtmltopdf.org/. Поддерживаемая версия - 0.12.2.4.
    /// </summary>
    public class HtmlToPdfSettings
    {
        /// <summary>
        /// Имя секции в файле конфигурации.
        /// </summary>
        public const string SectionName = "htmlToPdf";


        /// <summary>
        /// Настройка планировщика заданий по умолчанию.
        /// </summary>
        public static readonly HtmlToPdfSettings Default = new HtmlToPdfSettings();


        /// <summary>
        /// Конструктор.
        /// </summary>
        public HtmlToPdfSettings()
        {
        }


        /// <summary>
        /// Местоположение утилиты wkhtmltopdf - http://wkhtmltopdf.org/ (по умолчанию - генерируется автоматически с учетом операционной системы).
        /// </summary>
        public string UtilCommand { get; set; }

        /// <summary>
        /// Формат аргументов утилиты wkhtmltopdf - http://wkhtmltopdf.org/ (по умолчанию - генерируется автоматически с учетом операционной системы).
        /// </summary>
        public string UtilArguments { get; set; }

        /// <summary>
        /// Каталог для хранения временных файлов при генерации HTML/PDF (по умолчанию - каталог временных файлов учетной записи пользователя).
        /// </summary>
        public string TempDirectory { get; set; }
    }
}