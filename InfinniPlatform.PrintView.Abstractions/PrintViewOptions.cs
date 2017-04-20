namespace InfinniPlatform.PrintView
{
    /// <summary>
    /// Настройки печатных представлений.
    /// </summary>
    /// <remarks>
    /// Поддерживаемая версия утилиты wkhtmltopdf - 0.12.2.4 - http://wkhtmltopdf.org/.
    /// </remarks>
    public class PrintViewOptions
    {
        /// <summary>
        /// Имя секции в файле конфигурации.
        /// </summary>
        public const string SectionName = "printView";


        /// <summary>
        /// Настройка планировщика заданий по умолчанию.
        /// </summary>
        public static readonly PrintViewOptions Default = new PrintViewOptions();


        /// <summary>
        /// Конструктор.
        /// </summary>
        public PrintViewOptions()
        {
        }


        /// <summary>
        /// Каталог для хранения временных файлов при генерации HTML/PDF.
        /// </summary>
        /// <remarks>
        /// По умолчанию - каталог временных файлов учетной записи пользователя.
        /// </remarks>
        public string TempDirectory { get; set; }

        /// <summary>
        /// Местоположение утилиты wkhtmltopdf - http://wkhtmltopdf.org/.
        /// </summary>
        /// <remarks>
        /// По умолчанию - генерируется автоматически с учетом операционной системы.
        /// </remarks>
        public string WkHtmlToPdfPath { get; set; }

        /// <summary>
        /// Формат аргументов утилиты wkhtmltopdf - http://wkhtmltopdf.org/.
        /// </summary>
        /// <remarks>
        /// По умолчанию - генерируется автоматически с учетом операционной системы.
        /// </remarks>
        public string WkHtmlToPdfArguments { get; set; }
    }
}