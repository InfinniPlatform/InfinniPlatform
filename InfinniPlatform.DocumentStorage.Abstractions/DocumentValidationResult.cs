using System.Collections.Generic;

namespace InfinniPlatform.DocumentStorage.Abstractions
{
    /// <summary>
    /// Результат выполнения проверки корректности документа.
    /// </summary>
    public class DocumentValidationResult
    {
        public DocumentValidationResult(bool isValid = true)
        {
            IsValid = isValid;
            Items = new List<DocumentValidationResultItem>();
        }

        /// <summary>
        /// Признак успешности проверки.
        /// </summary>
        public bool IsValid { get; set; }

        /// <summary>
        /// Список результатов проверки свойств объекта.
        /// </summary>
        public IList<DocumentValidationResultItem> Items { get; set; }
    }
}