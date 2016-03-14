using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

using InfinniPlatform.DocumentStorage.Services.QueryFactory;
using InfinniPlatform.DocumentStorage.Services.QuerySyntax;
using InfinniPlatform.DocumentStorage.Tests.TestEntities;

using NUnit.Framework;

namespace InfinniPlatform.DocumentStorage.Tests.Services.QueryFactory
{
    [TestFixture]
    [Category(TestCategories.UnitTest)]
    public class ExpressionFilterQuerySyntaxVisitorTest
    {
        private static readonly IQuerySyntaxTreeParser SyntaxTreeParser = new QuerySyntaxTreeParser();


        [Test]
        public void ShouldFilterByNot()
        {
            // Given

            const string notFilter = "not(eq(prop1, 'F'))";

            Expression<Func<SimpleEntity, bool>> notExpectedFilter = i => !(i.prop1 == "F");

            var items = new[]
                        {
                            new SimpleEntity { _id = 1, prop1 = "A" },
                            new SimpleEntity { _id = 2, prop1 = "B" },
                            new SimpleEntity { _id = 3, prop1 = "C" },
                            new SimpleEntity { _id = 4, prop1 = "D" },
                            new SimpleEntity { _id = 5, prop1 = "F" }
                        };

            // When
            var notActualFilter = ParseFilter<SimpleEntity>(notFilter);

            // Then
            AssertFilter(items, notExpectedFilter, notActualFilter);
        }

        [Test]
        public void ShouldFilterByOr()
        {
            // Given

            const string orFilter = "or(lt(prop2, 2), gt(prop2, 4))";

            Expression<Func<SimpleEntity, bool>> orExpectedFilter = i => i.prop2 < 2 || i.prop2 > 4;

            var items = new[]
                        {
                            new SimpleEntity { _id = 1, prop2 = 5 },
                            new SimpleEntity { _id = 2, prop2 = 4 },
                            new SimpleEntity { _id = 3, prop2 = 3 },
                            new SimpleEntity { _id = 4, prop2 = 2 },
                            new SimpleEntity { _id = 5, prop2 = 1 }
                        };

            // When
            var orActualFilter = ParseFilter<SimpleEntity>(orFilter);

            // Then
            AssertFilter(items, orExpectedFilter, orActualFilter);
        }

        [Test]
        public void ShouldFilterByAnd()
        {
            // Given

            const string andFilter = "and(gte(prop2, 2), lte(prop2, 4))";

            Expression<Func<SimpleEntity, bool>> andExpectedFilter = i => i.prop2 >= 2 && i.prop2 <= 4;

            var items = new[]
                        {
                            new SimpleEntity { _id = 1, prop2 = 5 },
                            new SimpleEntity { _id = 2, prop2 = 4 },
                            new SimpleEntity { _id = 3, prop2 = 3 },
                            new SimpleEntity { _id = 4, prop2 = 2 },
                            new SimpleEntity { _id = 5, prop2 = 1 }
                        };

            // When
            var andActualFilter = ParseFilter<SimpleEntity>(andFilter);

            // Then
            AssertFilter(items, andExpectedFilter, andActualFilter);
        }

        [Test]
        public void ShouldFilterByExists()
        {
            // Given

            const string existsFilter = "exists(prop2)";
            const string existsTrueFilter = "exists(prop2, true)";
            const string existsFalseFilter = "exists(prop2, false)";

            Expression<Func<SimpleEntity, bool>> existsExpectedFilter = i => i.prop2 != null;
            Expression<Func<SimpleEntity, bool>> existsTrueExpectedFilter = i => i.prop2 != null;
            Expression<Func<SimpleEntity, bool>> existsFalseExpectedFilter = i => i.prop2 == null;

            var items = new[]
                        {
                            new SimpleEntity { _id = 1 },
                            new SimpleEntity { _id = 2, prop2 = 22 },
                            new SimpleEntity { _id = 3 },
                            new SimpleEntity { _id = 4, prop2 = 44 },
                            new SimpleEntity { _id = 5 }
                        };

            // When
            var existsActualFilter = ParseFilter<SimpleEntity>(existsFilter);
            var existsTrueActualFilter = ParseFilter<SimpleEntity>(existsTrueFilter);
            var existsFalseActualFilter = ParseFilter<SimpleEntity>(existsFalseFilter);

            // Then
            AssertFilter(items, existsExpectedFilter, existsActualFilter);
            AssertFilter(items, existsTrueExpectedFilter, existsTrueActualFilter);
            AssertFilter(items, existsFalseExpectedFilter, existsFalseActualFilter);
        }

        [Test]
        public void ShouldFilterByIn()
        {
            // Given

            const string inFilter = "in(prop2, 11, 33, 55)";
            const string notInFilter = "notIn(prop2, 11, 33, 55)";

            Expression<Func<SimpleEntity, bool>> inExpectedFilter = i => new int?[] { 11, 33, 55 }.Contains(i.prop2);
            Expression<Func<SimpleEntity, bool>> notInExpectedFilter = i => !new int?[] { 11, 33, 55 }.Contains(i.prop2);

            var items = new[]
                        {
                            new SimpleEntity { _id = 1, prop2 = 55 },
                            new SimpleEntity { _id = 2, prop2 = 44 },
                            new SimpleEntity { _id = 3, prop2 = 33 },
                            new SimpleEntity { _id = 4, prop2 = 22 },
                            new SimpleEntity { _id = 5, prop2 = 11 }
                        };

            // When
            var inActualFilter = ParseFilter<SimpleEntity>(inFilter);
            var notInActualFilter = ParseFilter<SimpleEntity>(notInFilter);

            // Then
            AssertFilter(items, inExpectedFilter, inActualFilter);
            AssertFilter(items, notInExpectedFilter, notInActualFilter);
        }

        [Test]
        public void ShouldFilterByRegex()
        {
            // Given

            const string caseInsensitiveFilter = "regex(sku, '^ABC', 'IgnoreCase')";
            const string multilineMatchFilter = "regex(description, '^S', 'Multiline')";
            const string ignoreNewLineFilter = "regex(description, 'm.*line', 'Singleline', 'IgnoreCase')";
            const string ignoreWhiteSpacesFilter = @"regex(description, '\\S+\\s+line$', 'Singleline')";

            Expression<Func<Product, bool>> caseInsensitiveExpectedFilter = i => Regex.IsMatch(i.sku, "^ABC", RegexOptions.IgnoreCase);
            Expression<Func<Product, bool>> multilineMatchExpectedFilter = i => Regex.IsMatch(i.description, "^S", RegexOptions.Multiline);
            Expression<Func<Product, bool>> ignoreNewLineExpectedFilter = i => Regex.IsMatch(i.description, "m.*line", RegexOptions.Singleline | RegexOptions.IgnoreCase);
            Expression<Func<Product, bool>> ignoreWhiteSpacesExpectedFilter = i => Regex.IsMatch(i.description, "\\S+\\s+line$", RegexOptions.Singleline);

            var items = new[]
                        {
                            new Product { _id = 100, sku = "abc123", description = "Single line description." },
                            new Product { _id = 101, sku = "abc789", description = "First line\nSecond line" },
                            new Product { _id = 102, sku = "xyz456", description = "Many spaces before     line" },
                            new Product { _id = 103, sku = "xyz789", description = "Multiple\nline description" }
                        };

            // When
            var caseInsensitiveActualFilter = ParseFilter<Product>(caseInsensitiveFilter);
            var multilineMatchActualFilter = ParseFilter<Product>(multilineMatchFilter);
            var ignoreNewLineActualFilter = ParseFilter<Product>(ignoreNewLineFilter);
            var ignoreWhiteSpacesActualFilter = ParseFilter<Product>(ignoreWhiteSpacesFilter);

            // Then
            AssertFilter(items, caseInsensitiveExpectedFilter, caseInsensitiveActualFilter);
            AssertFilter(items, multilineMatchExpectedFilter, multilineMatchActualFilter);
            AssertFilter(items, ignoreNewLineExpectedFilter, ignoreNewLineActualFilter);
            AssertFilter(items, ignoreWhiteSpacesExpectedFilter, ignoreWhiteSpacesActualFilter);
        }

        [Test]
        public void ShouldFilterByMatch()
        {
            // Given

            const string filter = "match(results, and(eq(product, 'xyz'), gte(score, 8)))";

            Expression<Func<Survey, bool>> expectedFilter = i => i.results.Any(r => r.product == "xyz" && r.score >= 8);

            var items = new[]
                        {
                            new Survey
                            {
                                _id = 1,
                                results = new[]
                                          {
                                              new SurveyResult { product = "abc", score = 10 },
                                              new SurveyResult { product = "xyz", score = 5 }
                                          }
                            },
                            new Survey
                            {
                                _id = 2,
                                results = new[]
                                          {
                                              new SurveyResult { product = "abc", score = 8 },
                                              new SurveyResult { product = "xyz", score = 7 }
                                          }
                            },
                            new Survey
                            {
                                _id = 3,
                                results = new[]
                                          {
                                              new SurveyResult { product = "abc", score = 7 },
                                              new SurveyResult { product = "xyz", score = 8 }
                                          }
                            }
                        };

            // When
            var actualFilter = ParseFilter<Survey>(filter);

            // Then
            AssertFilter(items, expectedFilter, actualFilter);
        }

        [Test]
        public void ShouldFilterByCompareNumbers()
        {
            // Given
            const string eqFilter = "eq(prop2, 2)";
            const string notEqFilter = "notEq(prop2, 2)";
            const string gtFilter = "gt(prop2, 2)";
            const string gteFilter = "gte(prop2, 2)";
            const string ltFilter = "lt(prop2, 2)";
            const string lteFilter = "lte(prop2, 2)";

            Expression<Func<SimpleEntity, bool>> eqExpectedFilter = i => i.prop2 == 2;
            Expression<Func<SimpleEntity, bool>> notEqExpectedFilter = i => i.prop2 != 2;
            Expression<Func<SimpleEntity, bool>> gtExpectedFilter = i => i.prop2 > 2;
            Expression<Func<SimpleEntity, bool>> gteExpectedFilter = i => i.prop2 >= 2;
            Expression<Func<SimpleEntity, bool>> ltExpectedFilter = i => i.prop2 < 2;
            Expression<Func<SimpleEntity, bool>> lteExpectedFilter = i => i.prop2 <= 2;

            var items = new[]
                        {
                            new SimpleEntity { _id = 1, prop2 = 1 },
                            new SimpleEntity { _id = 2, prop2 = 2 },
                            new SimpleEntity { _id = 3, prop2 = 3 },
                            new SimpleEntity { _id = 4, prop2 = 4 }
                        };

            // When
            var eqActualFilter = ParseFilter<SimpleEntity>(eqFilter);
            var notEqActualFilter = ParseFilter<SimpleEntity>(notEqFilter);
            var gtActualFilter = ParseFilter<SimpleEntity>(gtFilter);
            var gteActualFilter = ParseFilter<SimpleEntity>(gteFilter);
            var ltActualFilter = ParseFilter<SimpleEntity>(ltFilter);
            var lteActualFilter = ParseFilter<SimpleEntity>(lteFilter);

            // Then
            AssertFilter(items, eqExpectedFilter, eqActualFilter);
            AssertFilter(items, notEqExpectedFilter, notEqActualFilter);
            AssertFilter(items, gtExpectedFilter, gtActualFilter);
            AssertFilter(items, gteExpectedFilter, gteActualFilter);
            AssertFilter(items, ltExpectedFilter, ltActualFilter);
            AssertFilter(items, lteExpectedFilter, lteActualFilter);
        }

        [Test]
        public void ShouldFilterByCompareDateTimes()
        {
            // Given

            var today = DateTime.Today;
            var value = today.AddDays(2);

            var eqFilter = $"eq(date, date('{value}'))";
            var notEqFilter = $"notEq(date, date('{value}'))";
            var gtFilter = $"gt(date, date('{value}'))";
            var gteFilter = $"gte(date, date('{value}'))";
            var ltFilter = $"lt(date, date('{value}'))";
            var lteFilter = $"lte(date, date('{value}'))";

            Expression<Func<SimpleEntity, bool>> eqExpectedFilter = i => i.date == value;
            Expression<Func<SimpleEntity, bool>> notEqExpectedFilter = i => i.date != value;
            Expression<Func<SimpleEntity, bool>> gtExpectedFilter = i => i.date > value;
            Expression<Func<SimpleEntity, bool>> gteExpectedFilter = i => i.date >= value;
            Expression<Func<SimpleEntity, bool>> ltExpectedFilter = i => i.date < value;
            Expression<Func<SimpleEntity, bool>> lteExpectedFilter = i => i.date <= value;

            var items = new[]
                        {
                            new SimpleEntity { _id = 1, date = today.AddDays(1) },
                            new SimpleEntity { _id = 2, date = today.AddDays(2) },
                            new SimpleEntity { _id = 3, date = today.AddDays(3) },
                            new SimpleEntity { _id = 4, date = today.AddDays(4) }
                        };

            // When
            var eqActualFilter = ParseFilter<SimpleEntity>(eqFilter);
            var notEqActualFilter = ParseFilter<SimpleEntity>(notEqFilter);
            var gtActualFilter = ParseFilter<SimpleEntity>(gtFilter);
            var gteActualFilter = ParseFilter<SimpleEntity>(gteFilter);
            var ltActualFilter = ParseFilter<SimpleEntity>(ltFilter);
            var lteActualFilter = ParseFilter<SimpleEntity>(lteFilter);

            // Then
            AssertFilter(items, eqExpectedFilter, eqActualFilter);
            AssertFilter(items, notEqExpectedFilter, notEqActualFilter);
            AssertFilter(items, gtExpectedFilter, gtActualFilter);
            AssertFilter(items, gteExpectedFilter, gteActualFilter);
            AssertFilter(items, ltExpectedFilter, ltActualFilter);
            AssertFilter(items, lteExpectedFilter, lteActualFilter);
        }

        [Test]
        public void ShouldFilterArrayByCompareNumbers()
        {
            // Given

            const string allFilter = "all(items, 2, 3)";
            const string anyInFilter = "anyIn(items, 3, 4)";
            const string anyNotInFilter = "anyNotIn(items, 3, 4)";
            const string anyEqFilter = "anyEq(items, 4)";
            const string anyNotEqFilter = "anyNotEq(items, 4)";
            const string anyGtFilter = "anyGt(items, 4)";
            const string anyGteFilter = "anyGte(items, 6)";
            const string anyLtFilter = "anyLt(items, 3)";
            const string anyLteFilter = "anyLte(items, 4)";

            Expression<Func<CollectionEntity<int>, bool>> allExpectedFilter = i => new[] { 2, 3 }.All(ii => i.items.Contains(ii));
            Expression<Func<CollectionEntity<int>, bool>> anyInExpectedFilter = i => new[] { 3, 4 }.Any(ii => i.items.Contains(ii));
            Expression<Func<CollectionEntity<int>, bool>> anyNotInExpectedFilter = i => !new[] { 3, 4 }.Any(ii => i.items.Contains(ii));
            Expression<Func<CollectionEntity<int>, bool>> anyEqExpectedFilter = i => i.items.Any(ii => ii == 4);
            Expression<Func<CollectionEntity<int>, bool>> anyNotEqExpectedFilter = i => !i.items.Any(ii => ii == 4);
            Expression<Func<CollectionEntity<int>, bool>> anyGtExpectedFilter = i => i.items.Any(ii => ii > 4);
            Expression<Func<CollectionEntity<int>, bool>> anyGteExpectedFilter = i => i.items.Any(ii => ii >= 6);
            Expression<Func<CollectionEntity<int>, bool>> anyLtExpectedFilter = i => i.items.Any(ii => ii < 3);
            Expression<Func<CollectionEntity<int>, bool>> anyLteExpectedFilter = i => i.items.Any(ii => ii <= 4);

            var items = new[]
                        {
                            new CollectionEntity<int> { _id = 1, items = new[] { 1, 2, 3 } },
                            new CollectionEntity<int> { _id = 2, items = new[] { 2, 3, 4 } },
                            new CollectionEntity<int> { _id = 3, items = new[] { 3, 4, 5 } },
                            new CollectionEntity<int> { _id = 4, items = new[] { 4, 5, 6 } },
                            new CollectionEntity<int> { _id = 5, items = new[] { 5, 6, 7 } }
                        };

            // When
            var allActualFilter = ParseFilter<CollectionEntity<int>>(allFilter);
            var anyInActualFilter = ParseFilter<CollectionEntity<int>>(anyInFilter);
            var anyNotInActualFilter = ParseFilter<CollectionEntity<int>>(anyNotInFilter);
            var anyEqActualFilter = ParseFilter<CollectionEntity<int>>(anyEqFilter);
            var anyNotEqActualFilter = ParseFilter<CollectionEntity<int>>(anyNotEqFilter);
            var anyGtActualFilter = ParseFilter<CollectionEntity<int>>(anyGtFilter);
            var anyGteFilterActualFilter = ParseFilter<CollectionEntity<int>>(anyGteFilter);
            var anyLtFilterActualFilter = ParseFilter<CollectionEntity<int>>(anyLtFilter);
            var anyLteFilterActualFilter = ParseFilter<CollectionEntity<int>>(anyLteFilter);

            // Then
            AssertFilter(items, allExpectedFilter, allActualFilter);
            AssertFilter(items, anyInExpectedFilter, anyInActualFilter);
            AssertFilter(items, anyNotInExpectedFilter, anyNotInActualFilter);
            AssertFilter(items, anyEqExpectedFilter, anyEqActualFilter);
            AssertFilter(items, anyNotEqExpectedFilter, anyNotEqActualFilter);
            AssertFilter(items, anyGtExpectedFilter, anyGtActualFilter);
            AssertFilter(items, anyGteExpectedFilter, anyGteFilterActualFilter);
            AssertFilter(items, anyLtExpectedFilter, anyLtFilterActualFilter);
            AssertFilter(items, anyLteExpectedFilter, anyLteFilterActualFilter);
        }

        [Test]
        public void ShouldFilterArrayByCompareDateTimes()
        {
            // Given

            var today = DateTime.Today;

            var allFilter = $"all(items, date('{today.AddDays(2)}'),  date('{today.AddDays(3)}'))";
            var anyInFilter = $"anyIn(items,  date('{today.AddDays(3)}'),  date('{today.AddDays(4)}'))";
            var anyNotInFilter = $"anyNotIn(items,  date('{today.AddDays(3)}'),  date('{today.AddDays(4)}'))";
            var anyEqFilter = $"anyEq(items,  date('{today.AddDays(4)}'))";
            var anyNotEqFilter = $"anyNotEq(items,  date('{today.AddDays(4)}'))";
            var anyGtFilter = $"anyGt(items, date('{today.AddDays(4)}'))";
            var anyGteFilter = $"anyGte(items, date('{today.AddDays(6)}'))";
            var anyLtFilter = $"anyLt(items, date('{today.AddDays(3)}'))";
            var anyLteFilter = $"anyLte(items, date('{today.AddDays(4)}'))";

            Expression<Func<CollectionEntity<DateTime>, bool>> allExpectedFilter = i => new[] { today.AddDays(2), today.AddDays(3) }.All(ii => i.items.Contains(ii));
            Expression<Func<CollectionEntity<DateTime>, bool>> anyInExpectedFilter = i => new[] { today.AddDays(3), today.AddDays(4) }.Any(ii => i.items.Contains(ii));
            Expression<Func<CollectionEntity<DateTime>, bool>> anyNotInExpectedFilter = i => !new[] { today.AddDays(3), today.AddDays(4) }.Any(ii => i.items.Contains(ii));
            Expression<Func<CollectionEntity<DateTime>, bool>> anyEqExpectedFilter = i => i.items.Any(ii => ii == today.AddDays(4));
            Expression<Func<CollectionEntity<DateTime>, bool>> anyNotEqExpectedFilter = i => !i.items.Any(ii => ii == today.AddDays(4));
            Expression<Func<CollectionEntity<DateTime>, bool>> anyGtExpectedFilter = i => i.items.Any(ii => ii > today.AddDays(4));
            Expression<Func<CollectionEntity<DateTime>, bool>> anyGteExpectedFilter = i => i.items.Any(ii => ii >= today.AddDays(6));
            Expression<Func<CollectionEntity<DateTime>, bool>> anyLtExpectedFilter = i => i.items.Any(ii => ii < today.AddDays(3));
            Expression<Func<CollectionEntity<DateTime>, bool>> anyLteExpectedFilter = i => i.items.Any(ii => ii <= today.AddDays(4));

            var items = new[]
                        {
                            new CollectionEntity<DateTime> { _id = 1, items = new[] { today.AddDays(1), today.AddDays(2), today.AddDays(3) } },
                            new CollectionEntity<DateTime> { _id = 2, items = new[] { today.AddDays(2), today.AddDays(3), today.AddDays(4) } },
                            new CollectionEntity<DateTime> { _id = 3, items = new[] { today.AddDays(3), today.AddDays(4), today.AddDays(5) } },
                            new CollectionEntity<DateTime> { _id = 4, items = new[] { today.AddDays(4), today.AddDays(5), today.AddDays(6) } },
                            new CollectionEntity<DateTime> { _id = 5, items = new[] { today.AddDays(5), today.AddDays(6), today.AddDays(7) } }
                        };

            // When
            var allActualFilter = ParseFilter<CollectionEntity<DateTime>>(allFilter);
            var anyInActualFilter = ParseFilter<CollectionEntity<DateTime>>(anyInFilter);
            var anyNotInActualFilter = ParseFilter<CollectionEntity<DateTime>>(anyNotInFilter);
            var anyEqActualFilter = ParseFilter<CollectionEntity<DateTime>>(anyEqFilter);
            var anyNotEqActualFilter = ParseFilter<CollectionEntity<DateTime>>(anyNotEqFilter);
            var anyGtActualFilter = ParseFilter<CollectionEntity<DateTime>>(anyGtFilter);
            var anyGteFilterActualFilter = ParseFilter<CollectionEntity<DateTime>>(anyGteFilter);
            var anyLtFilterActualFilter = ParseFilter<CollectionEntity<DateTime>>(anyLtFilter);
            var anyLteFilterActualFilter = ParseFilter<CollectionEntity<DateTime>>(anyLteFilter);

            // Then
            AssertFilter(items, allExpectedFilter, allActualFilter);
            AssertFilter(items, anyInExpectedFilter, anyInActualFilter);
            AssertFilter(items, anyNotInExpectedFilter, anyNotInActualFilter);
            AssertFilter(items, anyEqExpectedFilter, anyEqActualFilter);
            AssertFilter(items, anyNotEqExpectedFilter, anyNotEqActualFilter);
            AssertFilter(items, anyGtExpectedFilter, anyGtActualFilter);
            AssertFilter(items, anyGteExpectedFilter, anyGteFilterActualFilter);
            AssertFilter(items, anyLtExpectedFilter, anyLtFilterActualFilter);
            AssertFilter(items, anyLteExpectedFilter, anyLteFilterActualFilter);
        }

        [Test]
        public void ShouldFilterBySize()
        {
            // Given

            const string sizeEq0Filter = "sizeEq(items, 0)";
            const string sizeEq1Filter = "sizeEq(items, 1)";
            const string sizeEq2Filter = "sizeEq(items, 2)";
            const string sizeGtFilter = "sizeGt(items, 3)";
            const string sizeGteFilter = "sizeGte(items, 3)";
            const string sizeLtFilter = "sizeLt(items, 2)";
            const string sizeLteFilter = "sizeLte(items, 2)";

            Expression<Func<CollectionEntity<int>, bool>> sizeEq0ExpectedFilter = i => i.items.Count() == 0;
            Expression<Func<CollectionEntity<int>, bool>> sizeEq1ExpectedFilter = i => i.items.Count() == 1;
            Expression<Func<CollectionEntity<int>, bool>> sizeEq2ExpectedFilter = i => i.items.Count() == 2;
            Expression<Func<CollectionEntity<int>, bool>> sizeGtExpectedFilter = i => i.items.Count() > 3;
            Expression<Func<CollectionEntity<int>, bool>> sizeGteExpectedFilter = i => i.items.Count() >= 3;
            Expression<Func<CollectionEntity<int>, bool>> sizeLtExpectedFilter = i => i.items.Count() < 2;
            Expression<Func<CollectionEntity<int>, bool>> sizeLteExpectedFilter = i => i.items.Count() <= 2;

            var items = new[]
                        {
                            new CollectionEntity<int> { _id = 1, items = new int[] { } },
                            new CollectionEntity<int> { _id = 2, items = new[] { 1 } },
                            new CollectionEntity<int> { _id = 3, items = new[] { 1, 2 } },
                            new CollectionEntity<int> { _id = 4, items = new[] { 1, 2, 3 } },
                            new CollectionEntity<int> { _id = 5, items = new[] { 1, 2, 3, 4 } },
                            new CollectionEntity<int> { _id = 6, items = new[] { 1, 2, 3, 4, 5 } }
                        };

            // When
            var sizeEq0ActualFilter = ParseFilter<CollectionEntity<int>>(sizeEq0Filter);
            var sizeEq1ActualFilter = ParseFilter<CollectionEntity<int>>(sizeEq1Filter);
            var sizeEq2ActualFilter = ParseFilter<CollectionEntity<int>>(sizeEq2Filter);
            var sizeGtActualFilter = ParseFilter<CollectionEntity<int>>(sizeGtFilter);
            var sizeGteActualFilter = ParseFilter<CollectionEntity<int>>(sizeGteFilter);
            var sizeLtActualFilter = ParseFilter<CollectionEntity<int>>(sizeLtFilter);
            var sizeLteActualFilter = ParseFilter<CollectionEntity<int>>(sizeLteFilter);

            // Then
            AssertFilter(items, sizeEq0ExpectedFilter, sizeEq0ActualFilter);
            AssertFilter(items, sizeEq1ExpectedFilter, sizeEq1ActualFilter);
            AssertFilter(items, sizeEq2ExpectedFilter, sizeEq2ActualFilter);
            AssertFilter(items, sizeGtExpectedFilter, sizeGtActualFilter);
            AssertFilter(items, sizeGteExpectedFilter, sizeGteActualFilter);
            AssertFilter(items, sizeLtExpectedFilter, sizeLtActualFilter);
            AssertFilter(items, sizeLteExpectedFilter, sizeLteActualFilter);
        }

        [Test]
        public void ShouldFilterByNestedProperty()
        {
            // Given

            const string filter = "eq(item.category, 'cake')";

            Expression<Func<Order, bool>> expectedFilter = i => i.item.category == "cake";

            var items = new[]
                        {
                            new Order { _id = 1, item = new OrderItem { category = "cake", type = "chiffon" }, amount = 10 },
                            new Order { _id = 2, item = new OrderItem { category = "cookies", type = "chocolate chip" }, amount = 50 },
                            new Order { _id = 3, item = new OrderItem { category = "cookies", type = "chocolate chip" }, amount = 15 },
                            new Order { _id = 4, item = new OrderItem { category = "cake", type = "lemon" }, amount = 30 },
                            new Order { _id = 5, item = new OrderItem { category = "cake", type = "carrot" }, amount = 20 },
                            new Order { _id = 6, item = new OrderItem { category = "brownies", type = "blondie" }, amount = 10 }
                        };

            // When
            var actualFilter = ParseFilter<Order>(filter);

            // Then
            AssertFilter(items, expectedFilter, actualFilter);
        }


        private static void AssertFilter<T>(IList<T> items, Expression<Func<T, bool>> expectedFilter, Expression<Func<T, bool>> actualFilter)
        {
            CollectionAssert.AreEqual(items.Where(expectedFilter.Compile()), items.Where(actualFilter.Compile()));
        }


        private static Expression<Func<T, bool>> ParseFilter<T>(string filter)
        {
            var filterMethods = SyntaxTreeParser.Parse(filter);

            Assert.IsNotNull(filterMethods);
            Assert.AreEqual(1, filterMethods.Count);
            Assert.IsInstanceOf<InvocationQuerySyntaxNode>(filterMethods[0]);

            return (Expression<Func<T, bool>>)ExpressionFilterQuerySyntaxVisitor.CreateFilterExpression(typeof(T), (InvocationQuerySyntaxNode)filterMethods[0]);
        }
    }
}