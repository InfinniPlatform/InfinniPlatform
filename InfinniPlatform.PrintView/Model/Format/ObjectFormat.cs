namespace InfinniPlatform.PrintView.Model.Format
{
    /// <summary>
    /// Формат отображения объекта.
    /// </summary>
    public class ObjectFormat : ValueFormat
    {
        /// <summary>
        /// Имя типа для сериализации.
        /// </summary>
        public const string TypeName = "ObjectFormat";


        /// <summary>
        /// Строка форматирования значения.
        /// </summary>
        public string Format { get; set; }
    }
}