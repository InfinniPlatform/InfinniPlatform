using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace InfinniPlatform.Api.Validation.ValidationBuilders
{
    public static class Reflection
    {
        public static string GetPropertyPath<T, TResult>(Expression<Func<T, TResult>> property)
        {
            return string.Join(".", property.Body.ToString().Split('.').Skip(1));
        }

        public static string GetCollectionPath<T, TItem>(Expression<Func<T, IEnumerable<TItem>>> collection)
        {
            return string.Join(".", collection.Body.ToString().Split('.').Skip(1)) + ".$";
        }
    }
}