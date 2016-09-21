using System;
using System.Collections.Generic;

namespace InfinniPlatform.PrintView.Expressions.BuiltInTypes
{
    internal class PredicateEqualityComparer : IEqualityComparer<object>
    {
        private readonly Func<object, object, bool> _comparer;

        public PredicateEqualityComparer(Func<object, object, bool> comparer)
        {
            _comparer = comparer;
        }

        bool IEqualityComparer<object>.Equals(object x, object y)
        {
            if (!ReferenceEquals(x, y) && !Equals(x, y))
            {
                return _comparer(x, y);
            }

            return true;
        }

        int IEqualityComparer<object>.GetHashCode(object obj)
        {
            return 0;
        }
    }
}