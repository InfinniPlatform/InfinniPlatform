using System;

namespace InfinniPlatform.DocumentStorage.Contract.Specifications
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