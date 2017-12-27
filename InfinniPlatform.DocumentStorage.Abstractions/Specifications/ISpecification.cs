using System;

namespace InfinniPlatform.DocumentStorage.Specifications
{
    /// <summary>
    /// Data filter specification.
    /// </summary>
    public interface ISpecification
    {
        /// <summary>
        /// Data filter condition.
        /// </summary>
        Func<IDocumentFilterBuilder, object> Filter { get; }
    }
}