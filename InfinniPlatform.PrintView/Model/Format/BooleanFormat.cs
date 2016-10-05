namespace InfinniPlatform.PrintView.Model.Format
{
    /// <summary>
    /// Формат отображения логического значения.
    /// </summary>
    public class BooleanFormat : ValueFormat
    {
        /// <summary>
        /// Имя типа для сериализации.
        /// </summary>
        public const string TypeName = "BooleanFormat";


        /// <summary>
        /// Текст для отображения истинного значения.
        /// </summary>
        public string TrueText { get; set; }

        /// <summary>
        /// Текст для отображения ложного значения.
        /// </summary>
        public string FalseText { get; set; }
    }
}