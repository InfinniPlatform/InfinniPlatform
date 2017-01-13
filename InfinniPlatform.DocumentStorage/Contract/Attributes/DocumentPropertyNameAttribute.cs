using System;

namespace InfinniPlatform.DocumentStorage.Contract.Attributes
{
    /// <summary>
    /// Определяет имя свойства документа в хранилище при работе с <see cref="IDocumentStorage{TDocument}" /> и <see cref="IDocumentStorageProvider{TDocument}"/>.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class DocumentPropertyNameAttribute : Attribute
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="name">Имя свойства.</param>
        public DocumentPropertyNameAttribute(string name)
        {
            Name = name;
        }


        /// <summary>
        /// Имя свойства.
        /// </summary>
        public string Name { get; }
    }
}