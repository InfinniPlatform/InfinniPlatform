using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace InfinniPlatform.Expressions.BuiltInTypes
{
    internal static class EnumerableFunctions
    {
        // Функции создания коллекций

        public static IEnumerable<T> Empty<T>()
        {
            return Enumerable.Empty<T>();
        }

        public static IEnumerable<int> Range(int start, int count)
        {
            return Enumerable.Range(start, count);
        }

        public static IEnumerable<int> Repeat(int value, int count)
        {
            return Enumerable.Repeat(value, count);
        }

        // Функции агрегации коллекций

        public static int Count(IEnumerable items, Delegate predicate = null)
        {
            if (items != null)
            {
                var objectItems = items.Cast<object>();

                return (predicate != null)
                    ? objectItems.Count(ToFunc<object, bool>(predicate))
                    : objectItems.Count();
            }

            return 0;
        }

        public static object Min(IEnumerable items, Delegate selector = null)
        {
            if (items != null)
            {
                var objectItems = items.Cast<object>();

                if (objectItems.Any())
                {
                    return (selector != null)
                        ? objectItems.Min(ToFunc<object, object>(selector))
                        : objectItems.Min();
                }
            }

            return null;
        }

        public static object Max(IEnumerable items, Delegate selector = null)
        {
            if (items != null)
            {
                var objectItems = items.Cast<object>();

                if (objectItems.Any())
                {
                    return (selector != null)
                        ? objectItems.Max(ToFunc<object, object>(selector))
                        : objectItems.Max();
                }
            }

            return null;
        }

        public static double? Sum(IEnumerable items, Delegate selector = null)
        {
            if (items != null)
            {
                var objectItems = items.Cast<object>();

                return objectItems.Sum((selector != null) ? ToFunc<object, double>(selector) : ToType<double>);
            }

            return null;
        }

        public static double? Average(IEnumerable items, Delegate selector = null)
        {
            if (items != null)
            {
                var objectItems = items.Cast<object>();

                if (objectItems.Any())
                {
                    return objectItems.Average((selector != null) ? ToFunc<object, double>(selector) : ToType<double>);
                }
            }

            return null;
        }

        public static object Aggregate(IEnumerable items, Delegate aggregator)
        {
            if (items != null && aggregator != null)
            {
                var objectItems = items.Cast<object>();

                if (objectItems.Any())
                {
                    return objectItems.Aggregate(ToFunc<object, object, object>(aggregator));
                }
            }

            return null;
        }

        public static object Aggregate(IEnumerable items, object seed, Delegate aggregator)
        {
            if (items != null && aggregator != null)
            {
                var objectItems = items.Cast<object>();

                return objectItems.Aggregate(seed, ToFunc<object, object, object>(aggregator));
            }

            return seed;
        }

        // Функции проверки элементов коллекций

        public static bool Equals(IEnumerable first, IEnumerable second, Delegate comparer = null)
        {
            if (first != null && second != null)
            {
                var firstItems = first.Cast<object>();
                var secondItems = second.Cast<object>();

                return (comparer != null)
                    ? firstItems.SequenceEqual(secondItems,
                        new PredicateEqualityComparer(ToFunc<object, object, bool>(comparer)))
                    : firstItems.SequenceEqual(secondItems);
            }

            return (first == null && second == null);
        }

        public static bool Contains(IEnumerable items, object item)
        {
            return (items != null)
                   && items.Cast<object>().Contains(item);
        }

        public static bool All(IEnumerable items, Delegate predicate)
        {
            return (items == null)
                   || (predicate != null && items.Cast<object>().All(ToFunc<object, bool>(predicate)));
        }

        public static bool Any(IEnumerable items, Delegate predicate)
        {
            return (items != null)
                   && (predicate != null && items.Cast<object>().Any(ToFunc<object, bool>(predicate)));
        }

        // Выборка элементов коллекций

        public static object ElementAt(IEnumerable items, int index)
        {
            return (items != null) ? items.Cast<object>().ElementAtOrDefault(index) : null;
        }

        public static object First(IEnumerable items, Delegate predicate = null)
        {
            if (items != null)
            {
                var objectItems = items.Cast<object>();

                return (predicate != null)
                    ? objectItems.FirstOrDefault(ToFunc<object, bool>(predicate))
                    : objectItems.FirstOrDefault();
            }

            return null;
        }

        public static object Last(IEnumerable items, Delegate predicate = null)
        {
            if (items != null)
            {
                var objectItems = items.Cast<object>();

                return (predicate != null)
                    ? objectItems.LastOrDefault(ToFunc<object, bool>(predicate))
                    : objectItems.LastOrDefault();
            }

            return null;
        }

        // Функции фильтрации коллекций

        public static IEnumerable Distinct(IEnumerable items)
        {
            return (items != null) ? items.Cast<object>().Distinct() : null;
        }

        public static IEnumerable<T> OfType<T>(IEnumerable items)
        {
            return (items != null) ? items.OfType<T>() : null;
        }

        public static IEnumerable Where(IEnumerable items, Delegate predicate)
        {
            if (items != null && predicate != null)
            {
                var objectItems = items.Cast<object>();

                return GetParameterCount(predicate) == 2
                    ? objectItems.Where(ToFunc<object, bool>(predicate))
                    : objectItems.Where(ToFunc<object, int, bool>(predicate));
            }

            return null;
        }

        public static IEnumerable Skip(IEnumerable items, int count)
        {
            return (items != null) ? items.Cast<object>().Skip(count) : null;
        }

        public static IEnumerable SkipWhile(IEnumerable items, Delegate predicate)
        {
            if (items != null && predicate != null)
            {
                var objectItems = items.Cast<object>();

                return GetParameterCount(predicate) == 2
                    ? objectItems.SkipWhile(ToFunc<object, bool>(predicate))
                    : objectItems.SkipWhile(ToFunc<object, int, bool>(predicate));
            }

            return null;
        }

        public static IEnumerable Take(IEnumerable items, int count)
        {
            return (items != null) ? items.Cast<object>().Take(count) : null;
        }

        public static IEnumerable TakeWhile(IEnumerable items, Delegate predicate)
        {
            if (items != null && predicate != null)
            {
                var objectItems = items.Cast<object>();

                return GetParameterCount(predicate) == 2
                    ? objectItems.TakeWhile(ToFunc<object, bool>(predicate))
                    : objectItems.TakeWhile(ToFunc<object, int, bool>(predicate));
            }

            return null;
        }

        // Функции объединения коллекций

        public static IEnumerable Concat(IEnumerable first, IEnumerable second)
        {
            if (first != null && second != null)
            {
                var firstItems = first.Cast<object>();
                var secondItems = second.Cast<object>();

                return firstItems.Concat(secondItems);
            }

            if (first != null)
            {
                return first;
            }

            if (second != null)
            {
                return second;
            }

            return null;
        }

        public static IEnumerable Union(IEnumerable first, IEnumerable second, Delegate comparer = null)
        {
            if (first != null && second != null)
            {
                var firstItems = first.Cast<object>();
                var secondItems = second.Cast<object>();

                return (comparer != null)
                    ? firstItems.Union(secondItems,
                        new PredicateEqualityComparer(ToFunc<object, object, bool>(comparer)))
                    : firstItems.Union(secondItems);
            }

            if (first != null)
            {
                return first;
            }

            if (second != null)
            {
                return second;
            }

            return null;
        }

        public static IEnumerable Except(IEnumerable first, IEnumerable second, Delegate comparer = null)
        {
            if (first != null && second != null)
            {
                var firstItems = first.Cast<object>();
                var secondItems = second.Cast<object>();

                return (comparer != null)
                    ? firstItems.Except(secondItems,
                        new PredicateEqualityComparer(ToFunc<object, object, bool>(comparer)))
                    : firstItems.Except(secondItems);
            }

            return first;
        }

        public static IEnumerable Intersect(IEnumerable first, IEnumerable second, Delegate comparer = null)
        {
            if (first != null && second != null)
            {
                var firstItems = first.Cast<object>();
                var secondItems = second.Cast<object>();

                return (comparer != null)
                    ? firstItems.Intersect(secondItems,
                        new PredicateEqualityComparer(ToFunc<object, object, bool>(comparer)))
                    : firstItems.Intersect(secondItems);
            }

            return null;
        }

        // Функции преобразования коллекций

        public static IEnumerable Reverse(IEnumerable items)
        {
            return (items != null) ? items.Cast<object>().Reverse() : null;
        }

        public static IEnumerable Select(IEnumerable items, Delegate selector)
        {
            if (items != null && selector != null)
            {
                var objectItems = items.Cast<object>();

                return GetParameterCount(selector) == 2
                    ? objectItems.Select(ToFunc<object, object>(selector))
                    : objectItems.Select(ToFunc<object, int, object>(selector));
            }

            return null;
        }

        public static IEnumerable SelectMany(IEnumerable items, Delegate selector)
        {
            if (items != null && selector != null)
            {
                var objectItems = items.Cast<object>();

                if (GetParameterCount(selector) == 2)
                {
                    var selectorFunc = ToFunc<object, IEnumerable>(selector);

                    return objectItems.SelectMany(i =>
                    {
                        var result = selectorFunc(i);
                        return (result != null) ? result.Cast<object>() : null;
                    });
                }
                else
                {
                    var selectorFunc = ToFunc<object, int, IEnumerable>(selector);

                    return objectItems.SelectMany((i, index) =>
                    {
                        var result = selectorFunc(i, index);
                        return (result != null) ? result.Cast<object>() : null;
                    });
                }
            }

            return null;
        }

        public static IEnumerable GroupBy(IEnumerable items, Delegate keySelector, Delegate elementSelector = null,
            Delegate keyComparer = null)
        {
            if (items != null && keySelector != null)
            {
                var objectItems = items.Cast<object>();

                if (elementSelector != null && keyComparer != null)
                {
                    return objectItems.GroupBy(ToFunc<object, object>(keySelector),
                        ToFunc<object, object>(elementSelector),
                        (key, elements) => new {Key = key, Items = elements},
                        new PredicateEqualityComparer(ToFunc<object, object, bool>(keyComparer)));
                }

                if (elementSelector != null)
                {
                    return objectItems.GroupBy(ToFunc<object, object>(keySelector),
                        ToFunc<object, object>(elementSelector),
                        (key, elements) => new {Key = key, Items = elements});
                }

                if (keyComparer != null)
                {
                    return objectItems.GroupBy(ToFunc<object, object>(keySelector),
                        (key, elements) => new {Key = key, Items = elements},
                        new PredicateEqualityComparer(ToFunc<object, object, bool>(keyComparer)));
                }

                return objectItems.GroupBy(ToFunc<object, object>(keySelector),
                    (key, elements) => new {Key = key, Items = elements});
            }

            return null;
        }

        public static IEnumerable OrderBy(IEnumerable items, Delegate keySelector = null)
        {
            return OrderEnumerable(items, keySelector, false);
        }

        public static IEnumerable OrderByDescending(IEnumerable items, Delegate keySelector = null)
        {
            return OrderEnumerable(items, keySelector, true);
        }

        private static IEnumerable OrderEnumerable(IEnumerable items, Delegate keySelector, bool descending)
        {
            if (items != null)
            {
                var keySelectorFunc = (keySelector != null)
                    ? ToFunc<object, object>(keySelector)
                    : i => i;

                var orderedItems = items as IOrderedEnumerable<object>;

                if (orderedItems != null)
                {
                    return descending
                        ? orderedItems.ThenByDescending(keySelectorFunc)
                        : orderedItems.ThenBy(keySelectorFunc);
                }

                var objectItems = items.Cast<object>();

                return descending
                    ? objectItems.OrderByDescending(keySelectorFunc)
                    : objectItems.OrderBy(keySelectorFunc);
            }

            return null;
        }

        // Helpers

        private static int GetParameterCount(Delegate value)
        {
            return value.Method.GetParameters().Length;
        }

        private static Func<T1, TResult> ToFunc<T1, TResult>(Delegate value)
        {
            return arg1 =>
            {
                try
                {
                    var result = value.DynamicInvoke(arg1);

                    return ToType<TResult>(result);
                }
                catch (TargetInvocationException error)
                {
                    if (error.InnerException != null)
                    {
                        throw error.InnerException;
                    }

                    throw;
                }
            };
        }

        private static Func<T1, T2, TResult> ToFunc<T1, T2, TResult>(Delegate value)
        {
            return (arg1, arg2) =>
            {
                try
                {
                    var result = value.DynamicInvoke(arg1, arg2);

                    return ToType<TResult>(result);
                }
                catch (TargetInvocationException error)
                {
                    if (error.InnerException != null)
                    {
                        throw error.InnerException;
                    }

                    throw;
                }
            };
        }

        private static T ToType<T>(object value)
        {
            if (value is T)
            {
                return (T) value;
            }

            if (value != null)
            {
                return (T) Convert.ChangeType(value, typeof (T));
            }

            return default(T);
        }
    }
}