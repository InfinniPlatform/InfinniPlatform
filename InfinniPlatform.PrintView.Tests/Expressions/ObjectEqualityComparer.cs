using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using InfinniPlatform.Core.Abstractions.Dynamic;
using InfinniPlatform.PrintView.Expressions;

namespace InfinniPlatform.PrintView.Tests.Expressions
{
    internal sealed class ObjectEqualityComparer : IEqualityComparer<object>
    {
        public static readonly ObjectEqualityComparer Default = new ObjectEqualityComparer();

        bool IEqualityComparer<object>.Equals(object expected, object actual)
        {
            return ObjectEquals(expected, actual);
        }

        int IEqualityComparer<object>.GetHashCode(object obj)
        {
            return 0;
        }

        private static bool ObjectEquals(object expected, object actual)
        {
            var result = ReferenceEquals(expected, actual) || Equals(expected, actual);

            if (!result
                && expected != null && actual != null
                && !expected.GetType().GetTypeInfo().IsValueType && !actual.GetType().GetTypeInfo().IsValueType)
            {
                if (expected is Array && actual is Array)
                {
                    result = ArrayEquals((Array) expected, (Array) actual);
                }
                else if (expected is IEnumerable && actual is IEnumerable)
                {
                    result = EnumerableEquals((IEnumerable) expected, (IEnumerable) actual);
                }
                else
                {
                    result = ObjectPropertyEquals(expected, actual);
                }
            }

            return result;
        }

        private static bool ArrayEquals(Array expected, Array actual)
        {
            var result = false;

            if (expected.Rank == actual.Rank)
            {
                var equalSize = true;

                for (var r = 0; r < expected.Rank; ++r)
                {
                    if (expected.GetLength(r) != actual.GetLength(r))
                    {
                        equalSize = false;
                        break;
                    }
                }

                if (equalSize)
                {
                    result = true;
                    ArrayEquals(expected, actual, new int[expected.Rank], 0, ref result);
                }
            }

            return result;
        }

        private static void ArrayEquals(Array expected, Array actual, int[] indexes, int rank, ref bool equalItems)
        {
            if (rank < expected.Rank)
            {
                for (var i = 0; i < expected.GetLength(rank); ++i)
                {
                    indexes[rank] = i;

                    ArrayEquals(expected, actual, indexes, rank + 1, ref equalItems);

                    if (!equalItems)
                    {
                        break;
                    }
                }
            }
            else
            {
                equalItems = ObjectEquals(expected.GetValue(indexes), actual.GetValue(indexes));
            }
        }

        private static bool ObjectPropertyEquals(object expected, object actual)
        {
            var result = true;

            var expectedProperties = expected.GetType().GetTypeInfo().GetProperties();

            foreach (var expectedProperty in expectedProperties)
            {
                var expectedValue = expectedProperty.GetValue(expected);
                var actualValue = actual.GetProperty(expectedProperty.Name);

                if (!ObjectEquals(expectedValue, actualValue))
                {
                    result = false;
                    break;
                }
            }

            return result;
        }

        private static bool EnumerableEquals(IEnumerable expected, IEnumerable actual)
        {
            return ArrayEquals(expected.Cast<object>().ToArray(), actual.Cast<object>().ToArray());
        }
    }
}