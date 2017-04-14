using System;
using System.Linq.Expressions;

namespace InfinniPlatform.DocumentStorage.Abstractions.Specifications
{
    /// <summary>
    /// Спецификация условия фильтрации данных.
    /// </summary>
    public interface ISpecification<TDocument>
    {
        /// <summary>
        /// Условие фильтрации данных.
        /// </summary>
        Expression<Func<TDocument, bool>> Filter { get; }
    }
}