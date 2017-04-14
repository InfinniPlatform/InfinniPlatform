using System;

namespace InfinniPlatform.DocumentStorage.Abstractions.Specifications
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