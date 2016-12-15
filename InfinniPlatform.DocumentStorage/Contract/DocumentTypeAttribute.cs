using System;

namespace InfinniPlatform.DocumentStorage.Contract
{
    /// <summary>
    /// Атрибут для определения имени типа документа.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class DocumentTypeAttribute : Attribute
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="name">Имя типа документа.</param>
        public DocumentTypeAttribute(string name)
        {
            Name = name;
        }

        /// <summary>
        /// Имя типа документа.
        /// </summary>
        public string Name { get; }
    }
}