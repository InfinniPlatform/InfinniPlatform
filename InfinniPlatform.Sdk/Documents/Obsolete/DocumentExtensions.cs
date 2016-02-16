using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace InfinniPlatform.Sdk.Documents
{
    public static class DocumentExtensions
    {
        public static IEnumerable<FilterCriteria> ToFilterCriterias(this Action<FilterBuilder> target)
        {
            if (target != null)
            {
                var filterBuilder = new FilterBuilder();
                target.Invoke(filterBuilder);

                return filterBuilder.CriteriaList;
            }

            return null;
        }

        public static IEnumerable<SortingCriteria> ToSortingCriterias(this Action<SortingBuilder> target)
        {
            if (target != null)
            {
                var sortingBuilder = new SortingBuilder();
                target.Invoke(sortingBuilder);

                return sortingBuilder.SortingList;
            }

            return null;
        }


        public static Func<IDocumentFilterBuilder, object> ToDocumentStorageFilter(this Action<FilterBuilder> criterias)
        {
            return ToDocumentStorageFilter(ToFilterCriterias(criterias));
        }

        public static Func<IDocumentFilterBuilder, object> ToDocumentStorageFilter(this IEnumerable<FilterCriteria> criterias)
        {
            if (criterias != null)
            {
                return f =>
                {
                    var conditions = new List<object>();

                    foreach (var criteria in criterias)
                    {
                        switch (criteria.CriteriaType)
                        {
                            case CriteriaType.IsEquals:
                                conditions.Add(f.Eq(criteria.Property, criteria.Value));
                                break;
                            case CriteriaType.IsNotEquals:
                                conditions.Add(f.NotEq(criteria.Property, criteria.Value));
                                break;
                            case CriteriaType.IsMoreThan:
                                conditions.Add(f.Gt(criteria.Property, criteria.Value));
                                break;
                            case CriteriaType.IsMoreThanOrEquals:
                                conditions.Add(f.Gte(criteria.Property, criteria.Value));
                                break;
                            case CriteriaType.IsLessThan:
                                conditions.Add(f.Lt(criteria.Property, criteria.Value));
                                break;
                            case CriteriaType.IsLessThanOrEquals:
                                conditions.Add(f.Lte(criteria.Property, criteria.Value));
                                break;
                            case CriteriaType.IsEmpty:
                                conditions.Add(f.Or(f.Exists(criteria.Property, false), f.Eq(criteria.Property, ""), f.Eq<object>(criteria.Property, null)));
                                break;
                            case CriteriaType.IsNotEmpty:
                                conditions.Add(f.And(f.Exists(criteria.Property), f.NotEq(criteria.Property, ""), f.NotEq<object>(criteria.Property, null)));
                                break;
                            case CriteriaType.IsContains:
                                conditions.Add(f.Regex(criteria.Property, new Regex($"{criteria.Value}", RegexOptions.IgnoreCase)));
                                break;
                            case CriteriaType.IsNotContains:
                                conditions.Add(f.Not(f.Regex(criteria.Property, new Regex($"{criteria.Value}", RegexOptions.IgnoreCase))));
                                break;
                            case CriteriaType.IsStartsWith:
                                conditions.Add(f.Regex(criteria.Property, new Regex($"^{criteria.Value}", RegexOptions.IgnoreCase)));
                                break;
                            case CriteriaType.IsNotStartsWith:
                                conditions.Add(f.Not(f.Regex(criteria.Property, new Regex($"^{criteria.Value}", RegexOptions.IgnoreCase))));
                                break;
                            case CriteriaType.IsEndsWith:
                                conditions.Add(f.Regex(criteria.Property, new Regex($"{criteria.Value}$", RegexOptions.IgnoreCase)));
                                break;
                            case CriteriaType.IsNotEndsWith:
                                conditions.Add(f.Not(f.Regex(criteria.Property, new Regex($"{criteria.Value}$", RegexOptions.IgnoreCase))));
                                break;
                            case CriteriaType.IsIn:
                                conditions.Add(f.In(criteria.Property, ((IEnumerable)criteria.Value).Cast<object>()));
                                break;
                            case CriteriaType.IsIdIn:
                                conditions.Add(f.In("_id", ((IEnumerable)criteria.Value).Cast<object>()));
                                break;
                            case CriteriaType.FullTextSearch:
                                conditions.Add(f.Text((string)criteria.Value));
                                break;
                        }
                    }

                    return (conditions.Count > 0) ? f.And(conditions) : null;
                };
            }

            return null;
        }

        public static IDocumentFindCursor ToSortedCursor(this IDocumentFindCursor documentFindCursor, Action<SortingBuilder> sort)
        {
            return ToSortedCursor(documentFindCursor, ToSortingCriterias(sort));
        }

        public static IDocumentFindCursor ToSortedCursor(this IDocumentFindCursor documentFindCursor, IEnumerable<SortingCriteria> sort)
        {
            var result = documentFindCursor;

            if (sort != null)
            {
                IDocumentFindSortedCursor sortedCursor = null;

                foreach (var criteria in sort)
                {
                    if (string.Equals(criteria.SortingOrder, "ascending", StringComparison.OrdinalIgnoreCase))
                    {
                        if (sortedCursor == null)
                        {
                            sortedCursor = result.SortBy(criteria.PropertyName);
                            result = sortedCursor;
                        }
                        else
                        {
                            sortedCursor = sortedCursor.ThenBy(criteria.PropertyName);
                            result = sortedCursor;
                        }
                    }
                    else if (string.Equals(criteria.SortingOrder, "descending", StringComparison.OrdinalIgnoreCase))
                    {
                        if (sortedCursor == null)
                        {
                            sortedCursor = result.SortByDescending(criteria.PropertyName);
                            result = sortedCursor;
                        }
                        else
                        {
                            sortedCursor = sortedCursor.ThenByDescending(criteria.PropertyName);
                            result = sortedCursor;
                        }
                    }
                }
            }

            return result;
        }
    }
}