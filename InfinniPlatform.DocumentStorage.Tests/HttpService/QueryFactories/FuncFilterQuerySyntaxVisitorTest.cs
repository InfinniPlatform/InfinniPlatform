using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

using InfinniPlatform.Core.Dynamic;
using InfinniPlatform.DocumentStorage.Abstractions;
using InfinniPlatform.DocumentStorage.Abstractions.Metadata;
using InfinniPlatform.DocumentStorage.HttpService.QueryFactories;
using InfinniPlatform.DocumentStorage.HttpService.QuerySyntax;
using InfinniPlatform.DocumentStorage.Tests.MongoDB;

using NUnit.Framework;

namespace InfinniPlatform.DocumentStorage.Tests.HttpService.QueryFactories
{
    [TestFixture]
    [Category(TestCategories.IntegrationTest)]
    public class FuncFilterQuerySyntaxVisitorTest
    {
        private static readonly IQuerySyntaxTreeParser SyntaxTreeParser = new QuerySyntaxTreeParser();


        [Test]
        public void ShouldFilterByNot()
        {
            // Given

            const string notFilter = "not(eq(prop1, 'F'))";

            Func<IDocumentFilterBuilder, object> notExpectedFilter = f => f.Not(f.Eq("prop1", "F"));

            var items = new[]
                        {
                            new DynamicWrapper { { "_id", 1 }, { "prop1", "A" } },
                            new DynamicWrapper { { "_id", 2 }, { "prop1", "B" } },
                            new DynamicWrapper { { "_id", 3 }, { "prop1", "C" } },
                            new DynamicWrapper { { "_id", 4 }, { "prop1", "D" } },
                            new DynamicWrapper { { "_id", 5 }, { "prop1", "F" } }
                        };

            // When
            var notActualFilter = ParseFilter(notFilter);

            // Then
            AssertFilter(nameof(ShouldFilterByNot), items, notExpectedFilter, notActualFilter);
        }

        [Test]
        public void ShouldFilterByOr()
        {
            // Given

            const string orFilter = "or(lt(prop2, 2), gt(prop2, 4))";

            Func<IDocumentFilterBuilder, object> orExpectedFilter = f => f.Or(f.Lt("prop2", 2), f.Gt("prop2", 4));

            var items = new[]
                        {
                            new DynamicWrapper { { "_id", 1 }, { "prop2", 5 }},
                            new DynamicWrapper { { "_id", 2 }, { "prop2", 4 }},
                            new DynamicWrapper { { "_id", 3 }, { "prop2", 3 }},
                            new DynamicWrapper { { "_id", 4 }, { "prop2", 2 }},
                            new DynamicWrapper { { "_id", 5 }, { "prop2", 1 }}
                        };

            // When
            var orActualFilter = ParseFilter(orFilter);

            // Then
            AssertFilter(nameof(ShouldFilterByOr), items, orExpectedFilter, orActualFilter);
        }

        [Test]
        public void ShouldFilterByAnd()
        {
            // Given

            const string andFilter = "and(gte(prop2, 2), lte(prop2, 4))";

            Func<IDocumentFilterBuilder, object> andExpectedFilter = f => f.And(f.Gte("prop2", 2), f.Lte("prop2", 4));

            var items = new[]
                        {
                            new DynamicWrapper { { "_id", 1 }, { "prop2", 5 }},
                            new DynamicWrapper { { "_id", 2 }, { "prop2", 4 }},
                            new DynamicWrapper { { "_id", 3 }, { "prop2", 3 }},
                            new DynamicWrapper { { "_id", 4 }, { "prop2", 2 }},
                            new DynamicWrapper { { "_id", 5 }, { "prop2", 1 }}
                        };

            // When
            var andActualFilter = ParseFilter(andFilter);

            // Then
            AssertFilter(nameof(ShouldFilterByAnd), items, andExpectedFilter, andActualFilter);
        }

        [Test]
        public void ShouldFilterByExists()
        {
            // Given

            const string existsFilter = "exists(prop2)";
            const string existsTrueFilter = "exists(prop2, true)";
            const string existsFalseFilter = "exists(prop2, false)";

            Func<IDocumentFilterBuilder, object> existsExpectedFilter = f => f.Exists("prop2");
            Func<IDocumentFilterBuilder, object> existsTrueExpectedFilter = f => f.Exists("prop2");
            Func<IDocumentFilterBuilder, object> existsFalseExpectedFilter = f => f.Exists("prop2", false);

            var items = new[]
                        {
                            new DynamicWrapper { { "_id", 1 } },
                            new DynamicWrapper { { "_id", 2 }, { "prop2", 22 }},
                            new DynamicWrapper { { "_id", 3 } },
                            new DynamicWrapper { { "_id", 4 }, { "prop2", 44 }},
                            new DynamicWrapper { { "_id", 5 } }
                        };

            // When
            var existsActualFilter = ParseFilter(existsFilter);
            var existsTrueActualFilter = ParseFilter(existsTrueFilter);
            var existsFalseActualFilter = ParseFilter(existsFalseFilter);

            // Then
            AssertFilter(nameof(ShouldFilterByExists), items, existsExpectedFilter, existsActualFilter);
            AssertFilter(nameof(ShouldFilterByExists), items, existsTrueExpectedFilter, existsTrueActualFilter);
            AssertFilter(nameof(ShouldFilterByExists), items, existsFalseExpectedFilter, existsFalseActualFilter);
        }

        [Test]
        public void ShouldFilterByType()
        {
            // Given

            const string booleanFilter = "type(prop1, Boolean)";
            const string int32Filter = "type(prop1, Int32)";
            const string int64Filter = "type(prop1, Int64)";
            const string doubleFilter = "type(prop1, Double)";
            const string stringFilter = "type(prop1, String)";
            const string dateTimeFilter = "type(prop1, DateTime)";
            const string binaryFilter = "type(prop1, Binary)";
            const string objectFilter = "type(prop1, Object)";
            const string arrayFilter = "type(prop1, Array)";

            Func<IDocumentFilterBuilder, object> booleanExpectedFilter = f => f.Type("prop1", DataType.Boolean);
            Func<IDocumentFilterBuilder, object> int32ExpectedFilter = f => f.Type("prop1", DataType.Int32);
            Func<IDocumentFilterBuilder, object> int64ExpectedFilter = f => f.Type("prop1", DataType.Int64);
            Func<IDocumentFilterBuilder, object> doubleExpectedFilter = f => f.Type("prop1", DataType.Double);
            Func<IDocumentFilterBuilder, object> stringExpectedFilter = f => f.Type("prop1", DataType.String);
            Func<IDocumentFilterBuilder, object> dateTimeExpectedFilter = f => f.Type("prop1", DataType.DateTime);
            Func<IDocumentFilterBuilder, object> binaryExpectedFilter = f => f.Type("prop1", DataType.Binary);
            Func<IDocumentFilterBuilder, object> objectExpectedFilter = f => f.Type("prop1", DataType.Object);
            Func<IDocumentFilterBuilder, object> arrayExpectedFilter = f => f.Type("prop1", DataType.Array);

            var items = new[]
                        {
                            new DynamicWrapper { { "_id", "Boolean" }, { "prop1", true } },
                            new DynamicWrapper { { "_id", "Int32" }, { "prop1", 123 } },
                            new DynamicWrapper { { "_id", "Int64" }, { "prop1", int.MaxValue + 100L } },
                            new DynamicWrapper { { "_id", "Double" }, { "prop1", 123.456 } },
                            new DynamicWrapper { { "_id", "String" }, { "prop1", "abc" } },
                            new DynamicWrapper { { "_id", "DateTime" }, { "prop1", new DateTime(2015, 2, 9, 1, 2, 3, 4) } },
                            new DynamicWrapper { { "_id", "Binary" }, { "prop1", new byte[] { 1, 2, 3, 4, 5 } } },
                            new DynamicWrapper { { "_id", "Object" }, { "prop1", new DynamicWrapper { { "subProp1", true } } } },
                            new DynamicWrapper { { "_id", "Array" }, { "prop1", new object[] { new object[] { 1, 2, 3 } } } }
                        };

            // When
            var booleanActualFilter = ParseFilter(booleanFilter);
            var int32ActualFilter = ParseFilter(int32Filter);
            var int64ActualFilter = ParseFilter(int64Filter);
            var doubleActualFilter = ParseFilter(doubleFilter);
            var stringActualFilter = ParseFilter(stringFilter);
            var dateTimeActualFilter = ParseFilter(dateTimeFilter);
            var binaryActualFilter = ParseFilter(binaryFilter);
            var objectActualFilter = ParseFilter(objectFilter);
            var arrayActualFilter = ParseFilter(arrayFilter);

            // Then
            AssertFilter(nameof(ShouldFilterByType), items, booleanExpectedFilter, booleanActualFilter);
            AssertFilter(nameof(ShouldFilterByType), items, int32ExpectedFilter, int32ActualFilter);
            AssertFilter(nameof(ShouldFilterByType), items, int64ExpectedFilter, int64ActualFilter);
            AssertFilter(nameof(ShouldFilterByType), items, doubleExpectedFilter, doubleActualFilter);
            AssertFilter(nameof(ShouldFilterByType), items, stringExpectedFilter, stringActualFilter);
            AssertFilter(nameof(ShouldFilterByType), items, dateTimeExpectedFilter, dateTimeActualFilter);
            AssertFilter(nameof(ShouldFilterByType), items, binaryExpectedFilter, binaryActualFilter);
            AssertFilter(nameof(ShouldFilterByType), items, objectExpectedFilter, objectActualFilter);
            AssertFilter(nameof(ShouldFilterByType), items, arrayExpectedFilter, arrayActualFilter);
        }

        [Test]
        public void ShouldFilterByIn()
        {
            // Given

            const string inFilter = "in(prop2, 11, 33, 55)";
            const string notInFilter = "notIn(prop2, 11, 33, 55)";

            Func<IDocumentFilterBuilder, object> inExpectedFilter = f => f.In("prop2", new[] { 11, 33, 55 });
            Func<IDocumentFilterBuilder, object> notInExpectedFilter = f => f.NotIn("prop2", new[] { 11, 33, 55 });

            var items = new[]
                        {
                            new DynamicWrapper { { "_id", 1 }, { "prop2", 55 }},
                            new DynamicWrapper { { "_id", 2 }, { "prop2", 44 }},
                            new DynamicWrapper { { "_id", 3 }, { "prop2", 33 }},
                            new DynamicWrapper { { "_id", 4 }, { "prop2", 22 }},
                            new DynamicWrapper { { "_id", 5 }, { "prop2", 11 }}
                        };

            // When
            var inActualFilter = ParseFilter(inFilter);
            var notInActualFilter = ParseFilter(notInFilter);

            // Then
            AssertFilter(nameof(ShouldFilterByIn), items, inExpectedFilter, inActualFilter);
            AssertFilter(nameof(ShouldFilterByIn), items, notInExpectedFilter, notInActualFilter);
        }

        [Test]
        public void ShouldFilterByRegex()
        {
            // Given

            const string caseInsensitiveFilter = "regex(sku, '^ABC', IgnoreCase)";
            const string multilineMatchFilter = "regex(description, '^S', Multiline)";
            const string ignoreNewLineFilter = "regex(description, 'm.*line', Singleline, IgnoreCase)";
            const string ignoreWhiteSpacesFilter = @"regex(description, '\\S+\\s+line$', Singleline)";

            Func<IDocumentFilterBuilder, object> caseInsensitiveExpectedFilter = f => f.Regex("sku", new Regex("^ABC", RegexOptions.IgnoreCase));
            Func<IDocumentFilterBuilder, object> multilineMatchExpectedFilter = f => f.Regex("description", new Regex("^S", RegexOptions.Multiline));
            Func<IDocumentFilterBuilder, object> ignoreNewLineExpectedFilter = f => f.Regex("description", new Regex("m.*line", RegexOptions.Singleline | RegexOptions.IgnoreCase));
            Func<IDocumentFilterBuilder, object> ignoreWhiteSpacesExpectedFilter = f => f.Regex("description", new Regex("\\S+\\s+line$", RegexOptions.Singleline));

            var items = new[]
                        {
                            new DynamicWrapper { { "_id", 100 }, { "sku", "abc123" }, { "description", "Single line description." } },
                            new DynamicWrapper { { "_id", 101 }, { "sku", "abc789" }, { "description", "First line\nSecond line" } },
                            new DynamicWrapper { { "_id", 102 }, { "sku", "xyz456" }, { "description", "Many spaces before     line" } },
                            new DynamicWrapper { { "_id", 103 }, { "sku", "xyz789" }, { "description", "Multiple\nline description" } }
                        };

            // When
            var caseInsensitiveActualFilter = ParseFilter(caseInsensitiveFilter);
            var multilineMatchActualFilter = ParseFilter(multilineMatchFilter);
            var ignoreNewLineActualFilter = ParseFilter(ignoreNewLineFilter);
            var ignoreWhiteSpacesActualFilter = ParseFilter(ignoreWhiteSpacesFilter);

            // Then
            AssertFilter(nameof(ShouldFilterByRegex), items, caseInsensitiveExpectedFilter, caseInsensitiveActualFilter);
            AssertFilter(nameof(ShouldFilterByRegex), items, multilineMatchExpectedFilter, multilineMatchActualFilter);
            AssertFilter(nameof(ShouldFilterByRegex), items, ignoreNewLineExpectedFilter, ignoreNewLineActualFilter);
            AssertFilter(nameof(ShouldFilterByRegex), items, ignoreWhiteSpacesExpectedFilter, ignoreWhiteSpacesActualFilter);
        }

        [Test]
        public void ShouldFilterByStartsWith()
        {
            // Given

            const string caseInsensitiveFilter = "startsWith(prop1, 'It')";
            const string caseSensitiveFilter = "startsWith(prop1, 'It', false)";

            Func<IDocumentFilterBuilder, object> caseInsensitiveExpectedFilter = f => f.StartsWith("prop1", "It");
            Func<IDocumentFilterBuilder, object> caseSensitiveExpectedFilter = f => f.StartsWith("prop1", "It", false);

            var items = new[]
                        {
                            new DynamicWrapper { { "_id", 1 }, { "prop1", "It starts with some text." } },
                            new DynamicWrapper { { "_id", 2 }, { "prop1", "it starts with some text." } },
                            new DynamicWrapper { { "_id", 3 }, { "prop1", "Does it start with some text?" } }
                        };

            // When
            var caseInsensitiveActualFilter = ParseFilter(caseInsensitiveFilter);
            var caseSensitiveActualFilter = ParseFilter(caseSensitiveFilter);

            // Then
            AssertFilter(nameof(ShouldFilterByStartsWith), items, caseInsensitiveExpectedFilter, caseInsensitiveActualFilter);
            AssertFilter(nameof(ShouldFilterByStartsWith), items, caseSensitiveExpectedFilter, caseSensitiveActualFilter);
        }

        [Test]
        public void ShouldFilterByStartsEndsWith()
        {
            // Given

            const string caseInsensitiveFilter = "endsWith(prop1, 'Text.')";
            const string caseSensitiveFilter = "endsWith(prop1, 'Text.', false)";

            Func<IDocumentFilterBuilder, object> caseInsensitiveExpectedFilter = f => f.EndsWith("prop1", "Text.");
            Func<IDocumentFilterBuilder, object> caseSensitiveExpectedFilter = f => f.EndsWith("prop1", "Text.", false);

            var items = new[]
                        {
                            new DynamicWrapper { { "_id", 1 }, { "prop1", "It ends with some Text." } },
                            new DynamicWrapper { { "_id", 2 }, { "prop1", "It ends with some text." } },
                            new DynamicWrapper { { "_id", 3 }, { "prop1", "Does it end with some text?" } }
                        };

            // When
            var caseInsensitiveActualFilter = ParseFilter(caseInsensitiveFilter);
            var caseSensitiveActualFilter = ParseFilter(caseSensitiveFilter);

            // Then
            AssertFilter(nameof(ShouldFilterByStartsEndsWith), items, caseInsensitiveExpectedFilter, caseInsensitiveActualFilter);
            AssertFilter(nameof(ShouldFilterByStartsEndsWith), items, caseSensitiveExpectedFilter, caseSensitiveActualFilter);
        }

        [Test]
        public void ShouldFilterByStartsContains()
        {
            // Given

            const string caseInsensitiveFilter = "contains(prop1, 'Contains')";
            const string caseSensitiveFilter = "contains(prop1, 'Contains', false)";

            Func<IDocumentFilterBuilder, object> caseInsensitiveExpectedFilter = f => f.Contains("prop1", "Contains");
            Func<IDocumentFilterBuilder, object> caseSensitiveExpectedFilter = f => f.Contains("prop1", "Contains", false);

            var items = new[]
                        {
                            new DynamicWrapper { { "_id", 1 }, { "prop1", "It Contains some text." } },
                            new DynamicWrapper { { "_id", 2 }, { "prop1", "It contains some text." } },
                            new DynamicWrapper { { "_id", 3 }, { "prop1", "Does it contain some text?" } }
                        };

            // When
            var caseInsensitiveActualFilter = ParseFilter(caseInsensitiveFilter);
            var caseSensitiveActualFilter = ParseFilter(caseSensitiveFilter);

            // Then
            AssertFilter(nameof(ShouldFilterByStartsContains), items, caseInsensitiveExpectedFilter, caseInsensitiveActualFilter);
            AssertFilter(nameof(ShouldFilterByStartsContains), items, caseSensitiveExpectedFilter, caseSensitiveActualFilter);
        }

        [Test]
        public void ShouldFilterByMatch()
        {
            // Given

            const string filter = "match(results, and(eq(product, 'xyz'), gte(score, 8)))";

            Func<IDocumentFilterBuilder, object> expectedFilter = f => f.Match("results", f.And(f.Eq("product", "xyz"), f.Gte("score", 8)));

            var items = new[]
                        {
                            new DynamicWrapper
                            {
                                { "_id", 1 },
                                {
                                    "results", new[]
                                               {
                                                   new DynamicWrapper { { "product", "abc" }, { "score", 10 } },
                                                   new DynamicWrapper { { "product", "xyz" }, { "score", 5 } }
                                               }
                                }
                            },
                            new DynamicWrapper
                            {
                                { "_id", 2 },
                                {
                                    "results", new[]
                                               {
                                                   new DynamicWrapper { { "product", "abc" }, { "score", 8 } },
                                                   new DynamicWrapper { { "product", "xyz" }, { "score", 7 } }
                                               }
                                }
                            },
                            new DynamicWrapper
                            {
                                { "_id", 3 },
                                {
                                    "results", new[]
                                               {
                                                   new DynamicWrapper { { "product", "abc" }, { "score", 7 } },
                                                   new DynamicWrapper { { "product", "xyz" }, { "score", 8 } }
                                               }
                                }
                            }
                        };

            // When
            var actualFilter = ParseFilter(filter);

            // Then
            AssertFilter(nameof(ShouldFilterByMatch), items, expectedFilter, actualFilter);
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

            Func<IDocumentFilterBuilder, object> eqExpectedFilter = f => f.Eq("prop2", 2);
            Func<IDocumentFilterBuilder, object> notEqExpectedFilter = f => f.NotEq("prop2", 2);
            Func<IDocumentFilterBuilder, object> gtExpectedFilter = f => f.Gt("prop2", 2);
            Func<IDocumentFilterBuilder, object> gteExpectedFilter = f => f.Gte("prop2", 2);
            Func<IDocumentFilterBuilder, object> ltExpectedFilter = f => f.Lt("prop2", 2);
            Func<IDocumentFilterBuilder, object> lteExpectedFilter = f => f.Lte("prop2", 2);

            var items = new[]
                        {
                            new DynamicWrapper { { "_id", 1 }, { "prop2", 1 }},
                            new DynamicWrapper { { "_id", 2 }, { "prop2", 2 }},
                            new DynamicWrapper { { "_id", 3 }, { "prop2", 3 }},
                            new DynamicWrapper { { "_id", 4 }, { "prop2", 4 }}
                        };

            // When
            var eqActualFilter = ParseFilter(eqFilter);
            var notEqActualFilter = ParseFilter(notEqFilter);
            var gtActualFilter = ParseFilter(gtFilter);
            var gteActualFilter = ParseFilter(gteFilter);
            var ltActualFilter = ParseFilter(ltFilter);
            var lteActualFilter = ParseFilter(lteFilter);

            // Then
            AssertFilter(nameof(ShouldFilterByCompareNumbers), items, eqExpectedFilter, eqActualFilter);
            AssertFilter(nameof(ShouldFilterByCompareNumbers), items, notEqExpectedFilter, notEqActualFilter);
            AssertFilter(nameof(ShouldFilterByCompareNumbers), items, gtExpectedFilter, gtActualFilter);
            AssertFilter(nameof(ShouldFilterByCompareNumbers), items, gteExpectedFilter, gteActualFilter);
            AssertFilter(nameof(ShouldFilterByCompareNumbers), items, ltExpectedFilter, ltActualFilter);
            AssertFilter(nameof(ShouldFilterByCompareNumbers), items, lteExpectedFilter, lteActualFilter);
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

            Func<IDocumentFilterBuilder, object> eqExpectedFilter = f => f.Eq("date", value);
            Func<IDocumentFilterBuilder, object> notEqExpectedFilter = f => f.NotEq("date", value);
            Func<IDocumentFilterBuilder, object> gtExpectedFilter = f => f.Gt("date", value);
            Func<IDocumentFilterBuilder, object> gteExpectedFilter = f => f.Gte("date", value);
            Func<IDocumentFilterBuilder, object> ltExpectedFilter = f => f.Lt("date", value);
            Func<IDocumentFilterBuilder, object> lteExpectedFilter = f => f.Lte("date", value);

            var items = new[]
                        {
                            new DynamicWrapper { { "_id", 1 }, { "date", today.AddDays(1) }},
                            new DynamicWrapper { { "_id", 2 }, { "date", today.AddDays(2) }},
                            new DynamicWrapper { { "_id", 3 }, { "date", today.AddDays(3) }},
                            new DynamicWrapper { { "_id", 4 }, { "date", today.AddDays(4) }}
                        };

            // When
            var eqActualFilter = ParseFilter(eqFilter);
            var notEqActualFilter = ParseFilter(notEqFilter);
            var gtActualFilter = ParseFilter(gtFilter);
            var gteActualFilter = ParseFilter(gteFilter);
            var ltActualFilter = ParseFilter(ltFilter);
            var lteActualFilter = ParseFilter(lteFilter);

            // Then
            AssertFilter(nameof(ShouldFilterByCompareDateTimes), items, eqExpectedFilter, eqActualFilter);
            AssertFilter(nameof(ShouldFilterByCompareDateTimes), items, notEqExpectedFilter, notEqActualFilter);
            AssertFilter(nameof(ShouldFilterByCompareDateTimes), items, gtExpectedFilter, gtActualFilter);
            AssertFilter(nameof(ShouldFilterByCompareDateTimes), items, gteExpectedFilter, gteActualFilter);
            AssertFilter(nameof(ShouldFilterByCompareDateTimes), items, ltExpectedFilter, ltActualFilter);
            AssertFilter(nameof(ShouldFilterByCompareDateTimes), items, lteExpectedFilter, lteActualFilter);
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

            Func<IDocumentFilterBuilder, object> allExpectedFilter = f => f.All("items", new[] { 2, 3 });
            Func<IDocumentFilterBuilder, object> anyInExpectedFilter = f => f.AnyIn("items", new[] { 3, 4 });
            Func<IDocumentFilterBuilder, object> anyNotInExpectedFilter = f => f.AnyNotIn("items", new[] { 3, 4 });
            Func<IDocumentFilterBuilder, object> anyEqExpectedFilter = f => f.AnyEq("items", 4);
            Func<IDocumentFilterBuilder, object> anyNotEqExpectedFilter = f => f.AnyNotEq("items", 4);
            Func<IDocumentFilterBuilder, object> anyGtExpectedFilter = f => f.AnyGt("items", 4);
            Func<IDocumentFilterBuilder, object> anyGteExpectedFilter = f => f.AnyGte("items", 6);
            Func<IDocumentFilterBuilder, object> anyLtExpectedFilter = f => f.AnyLt("items", 3);
            Func<IDocumentFilterBuilder, object> anyLteExpectedFilter = f => f.AnyLte("items", 4);

            var items = new[]
                        {
                            new DynamicWrapper { { "_id", 1 }, { "items", new[] { 1, 2, 3 } } },
                            new DynamicWrapper { { "_id", 2 }, { "items", new[] { 2, 3, 4 } } },
                            new DynamicWrapper { { "_id", 3 }, { "items", new[] { 3, 4, 5 } } },
                            new DynamicWrapper { { "_id", 4 }, { "items", new[] { 4, 5, 6 } } },
                            new DynamicWrapper { { "_id", 5 }, { "items", new[] { 5, 6, 7 } } }
                        };

            // When
            var allActualFilter = ParseFilter(allFilter);
            var anyInActualFilter = ParseFilter(anyInFilter);
            var anyNotInActualFilter = ParseFilter(anyNotInFilter);
            var anyEqActualFilter = ParseFilter(anyEqFilter);
            var anyNotEqActualFilter = ParseFilter(anyNotEqFilter);
            var anyGtActualFilter = ParseFilter(anyGtFilter);
            var anyGteFilterActualFilter = ParseFilter(anyGteFilter);
            var anyLtFilterActualFilter = ParseFilter(anyLtFilter);
            var anyLteFilterActualFilter = ParseFilter(anyLteFilter);

            // Then
            AssertFilter(nameof(ShouldFilterArrayByCompareNumbers), items, allExpectedFilter, allActualFilter);
            AssertFilter(nameof(ShouldFilterArrayByCompareNumbers), items, anyInExpectedFilter, anyInActualFilter);
            AssertFilter(nameof(ShouldFilterArrayByCompareNumbers), items, anyNotInExpectedFilter, anyNotInActualFilter);
            AssertFilter(nameof(ShouldFilterArrayByCompareNumbers), items, anyEqExpectedFilter, anyEqActualFilter);
            AssertFilter(nameof(ShouldFilterArrayByCompareNumbers), items, anyNotEqExpectedFilter, anyNotEqActualFilter);
            AssertFilter(nameof(ShouldFilterArrayByCompareNumbers), items, anyGtExpectedFilter, anyGtActualFilter);
            AssertFilter(nameof(ShouldFilterArrayByCompareNumbers), items, anyGteExpectedFilter, anyGteFilterActualFilter);
            AssertFilter(nameof(ShouldFilterArrayByCompareNumbers), items, anyLtExpectedFilter, anyLtFilterActualFilter);
            AssertFilter(nameof(ShouldFilterArrayByCompareNumbers), items, anyLteExpectedFilter, anyLteFilterActualFilter);
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

            Func<IDocumentFilterBuilder, object> allExpectedFilter = f => f.All("items", new[] { today.AddDays(2), today.AddDays(3) });
            Func<IDocumentFilterBuilder, object> anyInExpectedFilter = f => f.AnyIn("items", new[] { today.AddDays(3), today.AddDays(4) });
            Func<IDocumentFilterBuilder, object> anyNotInExpectedFilter = f => f.AnyNotIn("items", new[] { today.AddDays(3), today.AddDays(4) });
            Func<IDocumentFilterBuilder, object> anyEqExpectedFilter = f => f.Eq("items", today.AddDays(4));
            Func<IDocumentFilterBuilder, object> anyNotEqExpectedFilter = f => f.NotEq("items", today.AddDays(4));
            Func<IDocumentFilterBuilder, object> anyGtExpectedFilter = f => f.AnyGt("items", today.AddDays(4));
            Func<IDocumentFilterBuilder, object> anyGteExpectedFilter = f => f.AnyGte("items", today.AddDays(6));
            Func<IDocumentFilterBuilder, object> anyLtExpectedFilter = f => f.AnyLt("items", today.AddDays(3));
            Func<IDocumentFilterBuilder, object> anyLteExpectedFilter = f => f.AnyLte("items", today.AddDays(4));

            var items = new[]
                        {
                            new DynamicWrapper { { "_id", 1 }, { "items", new[] { today.AddDays(1), today.AddDays(2), today.AddDays(3) } } },
                            new DynamicWrapper { { "_id", 2 }, { "items", new[] { today.AddDays(2), today.AddDays(3), today.AddDays(4) } } },
                            new DynamicWrapper { { "_id", 3 }, { "items", new[] { today.AddDays(3), today.AddDays(4), today.AddDays(5) } } },
                            new DynamicWrapper { { "_id", 4 }, { "items", new[] { today.AddDays(4), today.AddDays(5), today.AddDays(6) } } },
                            new DynamicWrapper { { "_id", 5 }, { "items", new[] { today.AddDays(5), today.AddDays(6), today.AddDays(7) } } }
                        };

            // When
            var allActualFilter = ParseFilter(allFilter);
            var anyInActualFilter = ParseFilter(anyInFilter);
            var anyNotInActualFilter = ParseFilter(anyNotInFilter);
            var anyEqActualFilter = ParseFilter(anyEqFilter);
            var anyNotEqActualFilter = ParseFilter(anyNotEqFilter);
            var anyGtActualFilter = ParseFilter(anyGtFilter);
            var anyGteFilterActualFilter = ParseFilter(anyGteFilter);
            var anyLtFilterActualFilter = ParseFilter(anyLtFilter);
            var anyLteFilterActualFilter = ParseFilter(anyLteFilter);

            // Then
            AssertFilter(nameof(ShouldFilterArrayByCompareDateTimes), items, allExpectedFilter, allActualFilter);
            AssertFilter(nameof(ShouldFilterArrayByCompareDateTimes), items, anyInExpectedFilter, anyInActualFilter);
            AssertFilter(nameof(ShouldFilterArrayByCompareDateTimes), items, anyNotInExpectedFilter, anyNotInActualFilter);
            AssertFilter(nameof(ShouldFilterArrayByCompareDateTimes), items, anyEqExpectedFilter, anyEqActualFilter);
            AssertFilter(nameof(ShouldFilterArrayByCompareDateTimes), items, anyNotEqExpectedFilter, anyNotEqActualFilter);
            AssertFilter(nameof(ShouldFilterArrayByCompareDateTimes), items, anyGtExpectedFilter, anyGtActualFilter);
            AssertFilter(nameof(ShouldFilterArrayByCompareDateTimes), items, anyGteExpectedFilter, anyGteFilterActualFilter);
            AssertFilter(nameof(ShouldFilterArrayByCompareDateTimes), items, anyLtExpectedFilter, anyLtFilterActualFilter);
            AssertFilter(nameof(ShouldFilterArrayByCompareDateTimes), items, anyLteExpectedFilter, anyLteFilterActualFilter);
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

            Func<IDocumentFilterBuilder, object> sizeEq0ExpectedFilter = f => f.SizeEq("items", 0);
            Func<IDocumentFilterBuilder, object> sizeEq1ExpectedFilter = f => f.SizeEq("items", 1);
            Func<IDocumentFilterBuilder, object> sizeEq2ExpectedFilter = f => f.SizeEq("items", 2);
            Func<IDocumentFilterBuilder, object> sizeGtExpectedFilter = f => f.SizeGt("items", 3);
            Func<IDocumentFilterBuilder, object> sizeGteExpectedFilter = f => f.SizeGte("items", 3);
            Func<IDocumentFilterBuilder, object> sizeLtExpectedFilter = f => f.SizeLt("items", 2);
            Func<IDocumentFilterBuilder, object> sizeLteExpectedFilter = f => f.SizeLte("items", 2);

            var items = new[]
                        {
                            new DynamicWrapper { { "_id", 1 }, { "items", new int[] { } } },
                            new DynamicWrapper { { "_id", 2 }, { "items", new[] { 1 } } },
                            new DynamicWrapper { { "_id", 3 }, { "items", new[] { 1, 2 } } },
                            new DynamicWrapper { { "_id", 4 }, { "items", new[] { 1, 2, 3 } } },
                            new DynamicWrapper { { "_id", 5 }, { "items", new[] { 1, 2, 3, 4 } } },
                            new DynamicWrapper { { "_id", 6 }, { "items", new[] { 1, 2, 3, 4, 5 } } }
                        };

            // When
            var sizeEq0ActualFilter = ParseFilter(sizeEq0Filter);
            var sizeEq1ActualFilter = ParseFilter(sizeEq1Filter);
            var sizeEq2ActualFilter = ParseFilter(sizeEq2Filter);
            var sizeGtActualFilter = ParseFilter(sizeGtFilter);
            var sizeGteActualFilter = ParseFilter(sizeGteFilter);
            var sizeLtActualFilter = ParseFilter(sizeLtFilter);
            var sizeLteActualFilter = ParseFilter(sizeLteFilter);

            // Then
            AssertFilter(nameof(ShouldFilterBySize), items, sizeEq0ExpectedFilter, sizeEq0ActualFilter);
            AssertFilter(nameof(ShouldFilterBySize), items, sizeEq1ExpectedFilter, sizeEq1ActualFilter);
            AssertFilter(nameof(ShouldFilterBySize), items, sizeEq2ExpectedFilter, sizeEq2ActualFilter);
            AssertFilter(nameof(ShouldFilterBySize), items, sizeGtExpectedFilter, sizeGtActualFilter);
            AssertFilter(nameof(ShouldFilterBySize), items, sizeGteExpectedFilter, sizeGteActualFilter);
            AssertFilter(nameof(ShouldFilterBySize), items, sizeLtExpectedFilter, sizeLtActualFilter);
            AssertFilter(nameof(ShouldFilterBySize), items, sizeLteExpectedFilter, sizeLteActualFilter);
        }

        [Test]
        public void ShouldFilterByNestedProperty()
        {
            // Given

            const string filter = "eq(item.category, 'cake')";

            Func<IDocumentFilterBuilder, object> expectedFilter = f => f.Eq("item.category", "cake");

            var items = new[]
                        {
                            new DynamicWrapper { { "_id", 1 }, { "item", new DynamicWrapper { { "category", "cake" }, { "type", "chiffon" } } }, { "amount", 10 } },
                            new DynamicWrapper { { "_id", 2 }, { "item", new DynamicWrapper { { "category", "cookies" }, { "type", "chocolate chip" } } }, { "amount", 50 } },
                            new DynamicWrapper { { "_id", 3 }, { "item", new DynamicWrapper { { "category", "cookies" }, { "type", "chocolate chip" } } }, { "amount", 15 } },
                            new DynamicWrapper { { "_id", 4 }, { "item", new DynamicWrapper { { "category", "cake" }, { "type", "lemon" } } }, { "amount", 30 } },
                            new DynamicWrapper { { "_id", 5 }, { "item", new DynamicWrapper { { "category", "cake" }, { "type", "carrot" } } }, { "amount", 20 } },
                            new DynamicWrapper { { "_id", 6 }, { "item", new DynamicWrapper { { "category", "brownies" }, { "type", "blondie" } } }, { "amount", 10 } }
                        };

            // When
            var actualFilter = ParseFilter(filter);

            // Then
            AssertFilter(nameof(ShouldFilterByNestedProperty), items, expectedFilter, actualFilter);
        }

        [Test]
        public void ShouldFilterByText()
        {
            // Given

            const string searchSingleWordFilter = "text('coffee')";
            const string searchWithoutTermFilter = "text('coffee -shop')";
            const string searchWithLanguageFilter = "text('leche', 'es')";
            const string diacriticInsensitiveSearchFilter = "text('сы́рники CAFÉS')";
            const string caseSensitiveSearchForTermFilter = "text('Coffee', null, true)";
            const string caseSensitiveSearchForPhraseFilter = "text('\"Café Con Leche\"', null, true)";
            const string caseSensitiveSearchWithNegatedTermFilter = "text('Coffee -shop', null, true)";
            const string diacriticSensitiveSearchForTermFilter = "text('CAFÉ', null, false, true)";
            const string diacriticSensitiveSearchWithNegatedTermFilter = "text('leches -cafés', null, false, true)";

            Func<IDocumentFilterBuilder, object> searchSingleWordExpectedFilter = f => f.Text("coffee");
            Func<IDocumentFilterBuilder, object> searchWithoutTermExpectedFilter = f => f.Text("coffee -shop");
            Func<IDocumentFilterBuilder, object> searchWithLanguageExpectedFilter = f => f.Text("leche", "es");
            Func<IDocumentFilterBuilder, object> diacriticInsensitiveSearchExpectedFilter = f => f.Text("сы́рники CAFÉS");
            Func<IDocumentFilterBuilder, object> caseSensitiveSearchForTermExpectedFilter = f => f.Text("Coffee", caseSensitive: true);
            Func<IDocumentFilterBuilder, object> caseSensitiveSearchForPhraseExpectedFilter = f => f.Text("\"Café Con Leche\"", caseSensitive: true);
            Func<IDocumentFilterBuilder, object> caseSensitiveSearchWithNegatedTermExpectedFilter = f => f.Text("Coffee -shop", caseSensitive: true);
            Func<IDocumentFilterBuilder, object> diacriticSensitiveSearchForTermExpectedFilter = f => f.Text("CAFÉ", diacriticSensitive: true);
            Func<IDocumentFilterBuilder, object> diacriticSensitiveSearchWithNegatedTermExpectedFilter = f => f.Text("leches -cafés", diacriticSensitive: true);

            var items = new[]
                        {
                            new DynamicWrapper { { "_id", 1 }, { "subject", "coffee" }, { "author", "xyz" }, { "views", 50 } },
                            new DynamicWrapper { { "_id", 2 }, { "subject", "Coffee Shopping" }, { "author", "efg" }, { "views", 5 } },
                            new DynamicWrapper { { "_id", 3 }, { "subject", "Baking a cake" }, { "author", "abc" }, { "views", 90 } },
                            new DynamicWrapper { { "_id", 4 }, { "subject", "baking" }, { "author", "xyz" }, { "views", 100 } },
                            new DynamicWrapper { { "_id", 5 }, { "subject", "Café Con Leche" }, { "author", "abc" }, { "views", 200 } },
                            new DynamicWrapper { { "_id", 6 }, { "subject", "Сырники" }, { "author", "jkl" }, { "views", 80 } },
                            new DynamicWrapper { { "_id", 7 }, { "subject", "coffee and cream" }, { "author", "efg" }, { "views", 10 } },
                            new DynamicWrapper { { "_id", 8 }, { "subject", "Cafe con Leche" }, { "author", "xyz" }, { "views", 10 } }
                        };

            // When
            var searchSingleWordActualFilter = ParseFilter(searchSingleWordFilter);
            var searchWithoutTermActualFilter = ParseFilter(searchWithoutTermFilter);
            var searchWithLanguageActualFilter = ParseFilter(searchWithLanguageFilter);
            var diacriticInsensitiveSearchActualFilter = ParseFilter(diacriticInsensitiveSearchFilter);
            var caseSensitiveSearchForTermActualFilter = ParseFilter(caseSensitiveSearchForTermFilter);
            var caseSensitiveSearchForPhraseActualFilter = ParseFilter(caseSensitiveSearchForPhraseFilter);
            var caseSensitiveSearchWithNegatedTermActualFilter = ParseFilter(caseSensitiveSearchWithNegatedTermFilter);
            var diacriticSensitiveSearchForTermActualFilter = ParseFilter(diacriticSensitiveSearchForTermFilter);
            var diacriticSensitiveSearchWithNegatedTermActualFilter = ParseFilter(diacriticSensitiveSearchWithNegatedTermFilter);

            // Then

            var textIndex = new DocumentIndex { Key = new Dictionary<string, DocumentIndexKeyType> { { "subject", DocumentIndexKeyType.Text } } };
            var documentStorage = DocumentStorageTestHelpers.GetEmptyStorage(nameof(ShouldFilterByText), textIndex);
            documentStorage.InsertMany(items);

            AssertFilter(documentStorage, searchSingleWordExpectedFilter, searchSingleWordActualFilter);
            AssertFilter(documentStorage, searchWithoutTermExpectedFilter, searchWithoutTermActualFilter);
            AssertFilter(documentStorage, searchWithLanguageExpectedFilter, searchWithLanguageActualFilter);
            AssertFilter(documentStorage, diacriticInsensitiveSearchExpectedFilter, diacriticInsensitiveSearchActualFilter);
            AssertFilter(documentStorage, caseSensitiveSearchForTermExpectedFilter, caseSensitiveSearchForTermActualFilter);
            AssertFilter(documentStorage, caseSensitiveSearchForPhraseExpectedFilter, caseSensitiveSearchForPhraseActualFilter);
            AssertFilter(documentStorage, caseSensitiveSearchWithNegatedTermExpectedFilter, caseSensitiveSearchWithNegatedTermActualFilter);
            AssertFilter(documentStorage, diacriticSensitiveSearchForTermExpectedFilter, diacriticSensitiveSearchForTermActualFilter);
            AssertFilter(documentStorage, diacriticSensitiveSearchWithNegatedTermExpectedFilter, diacriticSensitiveSearchWithNegatedTermActualFilter);
        }


        private static void AssertFilter(string documentType, IEnumerable<DynamicWrapper> items, Func<IDocumentFilterBuilder, object> expectedFilter, Func<IDocumentFilterBuilder, object> actualFilter)
        {
            var documentStorage = DocumentStorageTestHelpers.GetEmptyStorage(documentType);
            documentStorage.InsertMany(items);
            AssertFilter(documentStorage, expectedFilter, actualFilter);
        }

        private static void AssertFilter(IDocumentStorage documentStorage, Func<IDocumentFilterBuilder, object> expectedFilter, Func<IDocumentFilterBuilder, object> actualFilter)
        {
            var expectedItems = documentStorage.Find(expectedFilter).ToList().Select(i => i["_id"]);
            var actualItems = documentStorage.Find(actualFilter).ToList().Select(i => i["_id"]);
            CollectionAssert.AreEqual(expectedItems, actualItems);
        }


        private static Func<IDocumentFilterBuilder, object> ParseFilter(string filter)
        {
            var filterMethods = SyntaxTreeParser.Parse(filter);

            Assert.IsNotNull(filterMethods);
            Assert.AreEqual(1, filterMethods.Count);
            Assert.IsInstanceOf<InvocationQuerySyntaxNode>(filterMethods[0]);

            return FuncFilterQuerySyntaxVisitor.CreateFilterExpression((InvocationQuerySyntaxNode)filterMethods[0]);
        }
    }
}