using System;
using System.Linq.Expressions;

namespace InfinniPlatform.DocumentStorage.Specifications
{
    /// <summary>
    /// Data filter specification.
    /// </summary>
    public interface ISpecification<TDocument>
    {
        /// <summary>
        /// Data filter condition.
        /// </summary>
        Expression<Func<TDocument, bool>> Filter { get; }
    }
}