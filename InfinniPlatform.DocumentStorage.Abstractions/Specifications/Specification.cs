using System;

namespace InfinniPlatform.DocumentStorage.Specifications
{
    /// <summary>
    /// Спецификация условия фильтрации данных.
    /// </summary>
    public class Specification : ISpecification
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="filter">Условие фильтрации данных.</param>
        public Specification(Func<IDocumentFilterBuilder, object> filter = null)
        {
            Filter = filter;
        }


        /// <summary>
        /// Условие фильтрации данных.
        /// </summary>
        public virtual Func<IDocumentFilterBuilder, object> Filter { get; }


        public static implicit operator Func<IDocumentFilterBuilder, object>(Specification value)
        {
            return value.Filter;
        }

        public static implicit operator Specification(Func<IDocumentFilterBuilder, object> value)
        {
            return new Specification(value);
        }


        public static Specification operator !(Specification value)
        {
            return new Specification(f => f.Not(value));
        }

        public static Specification operator &(Specification left, Specification right)
        {
            return new Specification(f => f.And(left, right));
        }

        public static Specification operator |(Specification left, Specification right)
        {
            return new Specification(f => f.Or(left, right));
        }
    }
}