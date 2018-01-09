using System.Collections.Generic;

namespace InfinniPlatform.DocumentStorage
{
    /// <summary>
    /// Result of document validation.
    /// </summary>
    public class DocumentValidationResult
    {
        /// <summary>
        /// Initializes a new instance of <see cref="DocumentValidationResult" />.
        /// </summary>
        /// <param name="isValid">Flag indicating if document is valid.</param>
        public DocumentValidationResult(bool isValid = true)
        {
            IsValid = isValid;
            Items = new List<DocumentValidationResultItem>();
        }

        /// <summary>
        /// Flag indicating if document is valid.
        /// </summary>
        public bool IsValid { get; set; }

        /// <summary>
        /// Document fields validation result.
        /// </summary>
        public IList<DocumentValidationResultItem> Items { get; set; }
    }
}