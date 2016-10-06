namespace InfinniPlatform.PrintView.Model.Format
{
    /// <summary>
    /// Формат отображения значения даты и времени.
    /// </summary>
    public class DateTimeFormat : ValueFormat
    {
        /// <summary>
        /// Имя типа для сериализации.
        /// </summary>
        public const string TypeName = "DateTimeFormat";


        /// <summary>
        /// Строка форматирования значения.
        /// </summary>
        public string Format { get; set; }
    }
}