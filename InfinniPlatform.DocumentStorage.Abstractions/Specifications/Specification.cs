using System;

namespace InfinniPlatform.DocumentStorage.Specifications
{
    /// <inheritdoc />
    public class Specification : ISpecification
    {
        /// <summary>
        /// Initializes a new instance of <see cref="Specification" />.
        /// </summary>
        /// <param name="filter">Data filter condition.</param>
        public Specification(Func<IDocumentFilterBuilder, object> filter = null)
        {
            Filter = filter;
        }


        /// <inheritdoc />
        public virtual Func<IDocumentFilterBuilder, object> Filter { get; }


        /// <summary>
        /// Implicit conversion from <see cref="Specification"/> to <see cref="Func{T1, TResult}"/>.
        /// </summary>
        /// <param name="value">Specification instance.</param>
        public static implicit operator Func<IDocumentFilterBuilder, object>(Specification value)
        {
            return value.Filter;
        }

        /// <summary>
        /// Implicit conversion from <see cref="Func{T1, TResult}"/> to <see cref="Specification"/>.
        /// </summary>
        /// <param name="value">Specification instance.</param>
        public static implicit operator Specification(Func<IDocumentFilterBuilder, object> value)
        {
            return new Specification(value);
        }


        /// <summary>
        /// Overloaded logical negation operator.
        /// </summary>
        /// <param name="value">Specification instance.</param>
        public static Specification operator !(Specification value)
        {
            return new Specification(f => f.Not(value));
        }

        /// <summary>
        /// Overloaded logical AND operator.
        /// </summary>
        /// <param name="left">Specification instance.</param>
        /// <param name="right">Specification instance.</param>
        public static Specification operator &(Specification left, Specification right)
        {
            return new Specification(f => f.And(left, right));
        }

        /// <summary>
        /// Overloaded logical OR operator.
        /// </summary>
        /// <param name="left">Specification instance.</param>
        /// <param name="right">Specification instance.</param>
        public static Specification operator |(Specification left, Specification right)
        {
            return new Specification(f => f.Or(left, right));
        }
    }
}