using System;
using System.Linq.Expressions;

namespace InfinniPlatform.DocumentStorage.Contract.Specifications
{
    /// <summary>
    /// Спецификация условия фильтрации данных.
    /// </summary>
    public class Specification<TDocument> : ISpecification<TDocument>
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="filter">Условие фильтрации данных.</param>
        public Specification(Expression<Func<TDocument, bool>> filter = null)
        {
            Filter = filter;
        }


        /// <summary>
        /// Условие фильтрации данных.
        /// </summary>
        public virtual Expression<Func<TDocument, bool>> Filter { get; }


        public static implicit operator Expression<Func<TDocument, bool>>(Specification<TDocument> value)
        {
            return value.Filter;
        }

        public static implicit operator Specification<TDocument>(Expression<Func<TDocument, bool>> value)
        {
            return new Specification<TDocument>(value);
        }
    }
}