using System;

namespace InfinniPlatform.DocumentStorage.Specifications
{
    /// <summary>
    /// Спецификация условия фильтрации данных.
    /// </summary>
    public interface ISpecification
    {
        /// <summary>
        /// Условие фильтрации данных.
        /// </summary>
        Func<IDocumentFilterBuilder, object> Filter { get; }
    }
}