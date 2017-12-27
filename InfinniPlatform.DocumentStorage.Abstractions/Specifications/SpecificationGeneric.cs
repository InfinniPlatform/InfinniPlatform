using System;
using System.Linq.Expressions;

namespace InfinniPlatform.DocumentStorage.Specifications
{
    /// <inheritdoc />
    public class Specification<TDocument> : ISpecification<TDocument>
    {
        /// <summary>
        /// Initializes a new instance of <see cref="Specification{TDocument}" />.
        /// </summary>
        /// <param name="filter">Data filter condition.</param>
        public Specification(Expression<Func<TDocument, bool>> filter = null)
        {
            Filter = filter;
        }


        /// <inheritdoc />
        public virtual Expression<Func<TDocument, bool>> Filter { get; }


        /// <summary>
        /// Implicit conversion from <see cref="Specification{TDocument}"/> to <see cref="Expression{TDelegate}"/>.
        /// </summary>
        /// <param name="value">Specification instance.</param>
        public static implicit operator Expression<Func<TDocument, bool>>(Specification<TDocument> value)
        {
            return value.Filter;
        }

        /// <summary>
        /// Implicit conversion from <see cref="Expression{TDelegate}"/> to <see cref="Specification{TDocument}"/>.
        /// </summary>
        /// <param name="value">Specification instance.</param>
        public static implicit operator Specification<TDocument>(Expression<Func<TDocument, bool>> value)
        {
            return new Specification<TDocument>(value);
        }


        /// <summary>
        /// Overloaded logical negation operator.
        /// </summary>
        /// <param name="value">Specification instance.</param>
        public static Specification<TDocument> operator !(Specification<TDocument> value)
        {
            return new Specification<TDocument>(value.Filter.Not());
        }

        /// <summary>
        /// Overloaded logical AND operator.
        /// </summary>
        /// <param name="left">Specification instance.</param>
        /// <param name="right">Specification instance.</param>
        public static Specification<TDocument> operator &(Specification<TDocument> left, Specification<TDocument> right)
        {
            return new Specification<TDocument>(left.Filter.And(right));
        }

        /// <summary>
        /// Overloaded logical OR operator.
        /// </summary>
        /// <param name="left">Specification instance.</param>
        /// <param name="right">Specification instance.</param>
        public static Specification<TDocument> operator |(Specification<TDocument> left, Specification<TDocument> right)
        {
            return new Specification<TDocument>(left.Filter.Or(right));
        }
    }
}