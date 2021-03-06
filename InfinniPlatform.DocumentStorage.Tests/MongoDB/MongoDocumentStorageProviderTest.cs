﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using InfinniPlatform.DocumentStorage.Metadata;
using InfinniPlatform.Dynamic;
using InfinniPlatform.Tests;

using NUnit.Framework;

namespace InfinniPlatform.DocumentStorage.MongoDB
{
    [TestFixture]
    [Category(TestCategories.IntegrationTest)]
    public class MongoDocumentStorageProviderTest
    {
        [Test]
        public void ShouldCount()
        {
            // Given
            var storage = MongoTestHelpers.GetEmptyStorageProvider(nameof(ShouldCount));

            // When
            storage.InsertMany(new[] { new DynamicDocument(), new DynamicDocument(), new DynamicDocument() });
            var count = storage.Count();

            // Then
            Assert.AreEqual(3, count);
        }

        [Test]
        public async Task ShouldCountAsync()
        {
            // Given
            var storage = MongoTestHelpers.GetEmptyStorageProvider(nameof(ShouldCountAsync));

            // When
            storage.InsertMany(new[] { new DynamicDocument(), new DynamicDocument(), new DynamicDocument() });
            var count = await storage.CountAsync();

            // Then
            Assert.AreEqual(3, count);
        }

        [Test]
        public void ShouldCountWithExpression()
        {
            // Given
            var storage = MongoTestHelpers.GetEmptyStorageProvider(nameof(ShouldCountWithExpression));

            // When

            var findValue = Guid.NewGuid().ToString();
            var anotherValue = Guid.NewGuid().ToString();

            storage.InsertMany(new[]
                               {
                                   new DynamicDocument { { "prop1", findValue } },
                                   new DynamicDocument { { "prop1", anotherValue } },
                                   new DynamicDocument { { "prop1", findValue } }
                               });

            var count = storage.Count(f => f.Eq("prop1", findValue));

            // Then
            Assert.AreEqual(2, count);
        }

        [Test]
        public async Task ShouldCountWithExpressionAsync()
        {
            // Given
            var storage = MongoTestHelpers.GetEmptyStorageProvider(nameof(ShouldCountWithExpression));

            // When

            var findValue = Guid.NewGuid().ToString();
            var anotherValue = Guid.NewGuid().ToString();

            storage.InsertMany(new[]
                               {
                                   new DynamicDocument { { "prop1", findValue } },
                                   new DynamicDocument { { "prop1", anotherValue } },
                                   new DynamicDocument { { "prop1", findValue } }
                               });

            var count = await storage.CountAsync(f => f.Eq("prop1", findValue));

            // Then
            Assert.AreEqual(2, count);
        }

        [Test]
        public void ShouldDistinct()
        {
            // Given
            var storage = MongoTestHelpers.GetEmptyStorageProvider(nameof(ShouldDistinct));

            // When

            storage.InsertMany(new[]
                               {
                                   new DynamicDocument { { "_id", 1 }, { "dept", "A" }, { "item", new DynamicDocument { { "sku", 111 }, { "color", "red" } } }, { "sizes", new[] { "S", "M" } } },
                                   new DynamicDocument { { "_id", 2 }, { "dept", "A" }, { "item", new DynamicDocument { { "sku", 111 }, { "color", "blue" } } }, { "sizes", new[] { "M", "L" } } },
                                   new DynamicDocument { { "_id", 3 }, { "dept", "B" }, { "item", new DynamicDocument { { "sku", 222 }, { "color", "blue" } } }, { "sizes", new[] { "S" } } },
                                   new DynamicDocument { { "_id", 4 }, { "dept", "A" }, { "item", new DynamicDocument { { "sku", 333 }, { "color", "black" } } }, { "sizes", new[] { "S" } } },
                               });

            var deptList = storage.Distinct<string>("dept").ToList();
            var skuList = storage.Distinct<int>("item.sku").ToList();
            var sizeList = storage.Distinct<string>("sizes").ToList();
            var deptSkuList = storage.Distinct<int>("item.sku", f => f.Eq("dept", "A")).ToList();

            // Then
            CollectionAssert.AreEquivalent(new[] { "A", "B" }, deptList);
            CollectionAssert.AreEquivalent(new[] { 111, 222, 333 }, skuList);
            CollectionAssert.AreEquivalent(new[] { "M", "S", "L" }, sizeList);
            CollectionAssert.AreEquivalent(new[] { 111, 333 }, deptSkuList);
        }

        [Test]
        public async Task ShouldDistinctAsync()
        {
            // Given
            var storage = MongoTestHelpers.GetEmptyStorageProvider(nameof(ShouldDistinctAsync));

            // When

            storage.InsertMany(new[]
                               {
                                   new DynamicDocument { { "_id", 1 }, { "dept", "A" }, { "item", new DynamicDocument { { "sku", 111 }, { "color", "red" } } }, { "sizes", new[] { "S", "M" } } },
                                   new DynamicDocument { { "_id", 2 }, { "dept", "A" }, { "item", new DynamicDocument { { "sku", 111 }, { "color", "blue" } } }, { "sizes", new[] { "M", "L" } } },
                                   new DynamicDocument { { "_id", 3 }, { "dept", "B" }, { "item", new DynamicDocument { { "sku", 222 }, { "color", "blue" } } }, { "sizes", new[] { "S" } } },
                                   new DynamicDocument { { "_id", 4 }, { "dept", "A" }, { "item", new DynamicDocument { { "sku", 333 }, { "color", "black" } } }, { "sizes", new[] { "S" } } }
                               });

            var deptList = (await storage.DistinctAsync<string>("dept")).ToList();
            var skuList = (await storage.DistinctAsync<int>("item.sku")).ToList();
            var sizeList = (await storage.DistinctAsync<string>("sizes")).ToList();
            var deptSkuList = (await storage.DistinctAsync<int>("item.sku", f => f.Eq("dept", "A"))).ToList();

            // Then
            CollectionAssert.AreEquivalent(new[] { "A", "B" }, deptList);
            CollectionAssert.AreEquivalent(new[] { 111, 222, 333 }, skuList);
            CollectionAssert.AreEquivalent(new[] { "M", "S", "L" }, sizeList);
            CollectionAssert.AreEquivalent(new[] { 111, 333 }, deptSkuList);
        }

        [Test]
        public void ShouldFindByEmpty()
        {
            // Given
            var storage = MongoTestHelpers.GetEmptyStorageProvider(nameof(ShouldFindByEmpty));

            // When

            storage.InsertMany(new[]
                               {
                                   new DynamicDocument { { "_id", 1 }, { "prop1", 1 } },
                                   new DynamicDocument { { "_id", 2 }, { "prop1", 2 } },
                                   new DynamicDocument { { "_id", 3 }, { "prop1", 3 } },
                                   new DynamicDocument { { "_id", 4 }, { "prop1", 4 } },
                                   new DynamicDocument { { "_id", 5 }, { "prop1", 5 } }
                               });

            var result = storage.Find(f => f.Empty()).ToList();

            // Then

            Assert.AreEqual(5, result.Count);
            Assert.AreEqual(1, ((dynamic)result[0])._id);
            Assert.AreEqual(2, ((dynamic)result[1])._id);
            Assert.AreEqual(3, ((dynamic)result[2])._id);
            Assert.AreEqual(4, ((dynamic)result[3])._id);
            Assert.AreEqual(5, ((dynamic)result[4])._id);
        }

        [Test]
        public void ShouldFindByNot()
        {
            // Given
            var storage = MongoTestHelpers.GetEmptyStorageProvider(nameof(ShouldFindByNot));

            // When

            storage.InsertMany(new[]
                               {
                                   new DynamicDocument { { "_id", 1 }, { "prop1", 1 } },
                                   new DynamicDocument { { "_id", 2 }, { "prop1", 2 } },
                                   new DynamicDocument { { "_id", 3 }, { "prop1", 3 } },
                                   new DynamicDocument { { "_id", 4 }, { "prop1", 4 } },
                                   new DynamicDocument { { "_id", 5 }, { "prop1", 5 } }
                               });

            var result = storage.Find(f => f.Not(f.Eq("prop1", 5))).ToList();

            // Then

            Assert.AreEqual(4, result.Count);
            Assert.AreEqual(1, ((dynamic)result[0])._id);
            Assert.AreEqual(2, ((dynamic)result[1])._id);
            Assert.AreEqual(3, ((dynamic)result[2])._id);
            Assert.AreEqual(4, ((dynamic)result[3])._id);
        }

        [Test]
        public void ShouldFindByOr()
        {
            // Given
            var storage = MongoTestHelpers.GetEmptyStorageProvider(nameof(ShouldFindByEmpty));

            // When

            storage.InsertMany(new[]
                               {
                                   new DynamicDocument { { "_id", 1 }, { "prop1", 1 } },
                                   new DynamicDocument { { "_id", 2 }, { "prop1", 2 } },
                                   new DynamicDocument { { "_id", 3 }, { "prop1", 3 } },
                                   new DynamicDocument { { "_id", 4 }, { "prop1", 4 } },
                                   new DynamicDocument { { "_id", 5 }, { "prop1", 5 } }
                               });

            var result = storage.Find(f => f.Or(f.Lt("prop1", 2), f.Gt("prop1", 4))).ToList();

            // Then

            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(1, ((dynamic)result[0])._id);
            Assert.AreEqual(5, ((dynamic)result[1])._id);
        }

        [Test]
        public void ShouldFindByAnd()
        {
            // Given
            var storage = MongoTestHelpers.GetEmptyStorageProvider(nameof(ShouldFindByAnd));

            // When

            storage.InsertMany(new[]
                               {
                                   new DynamicDocument { { "_id", 1 }, { "prop1", 1 } },
                                   new DynamicDocument { { "_id", 2 }, { "prop1", 2 } },
                                   new DynamicDocument { { "_id", 3 }, { "prop1", 3 } },
                                   new DynamicDocument { { "_id", 4 }, { "prop1", 4 } },
                                   new DynamicDocument { { "_id", 5 }, { "prop1", 5 } }
                               });

            var result = storage.Find(f => f.And(f.Gte("prop1", 2), f.Lte("prop1", 4))).ToList();

            // Then

            Assert.AreEqual(3, result.Count);
            Assert.AreEqual(2, ((dynamic)result[0])._id);
            Assert.AreEqual(3, ((dynamic)result[1])._id);
            Assert.AreEqual(4, ((dynamic)result[2])._id);
        }

        [Test]
        public void ShouldFindByExists()
        {
            // Given
            var storage = MongoTestHelpers.GetEmptyStorageProvider(nameof(ShouldFindByExists));

            // When

            storage.InsertMany(new[]
                               {
                                   new DynamicDocument { { "_id", 1 }, { "prop1", 11 } },
                                   new DynamicDocument { { "_id", 2 }, { "prop1", 22 }, { "prop2", "A" } },
                                   new DynamicDocument { { "_id", 3 }, { "prop1", 33 }, { "prop2", "B" } }
                               });

            var existsResult = storage.Find(f => f.Exists("prop2")).ToList();
            var notExistsResult = storage.Find(f => f.Not(f.Exists("prop2"))).ToList();

            // Then

            Assert.AreEqual(2, existsResult.Count);
            Assert.AreEqual(2, ((dynamic)existsResult[0])._id);
            Assert.AreEqual(3, ((dynamic)existsResult[1])._id);

            Assert.AreEqual(1, notExistsResult.Count);
            Assert.AreEqual(1, ((dynamic)notExistsResult[0])._id);
        }

        [Test]
        public void ShouldFindByType()
        {
            // Given
            var storage = MongoTestHelpers.GetEmptyStorageProvider(nameof(ShouldFindByType));

            // When

            storage.InsertMany(new[]
                               {
                                   new DynamicDocument { { "_id", "Boolean" }, { "prop1", true } },
                                   new DynamicDocument { { "_id", "Int32" }, { "prop1", 123 } },
                                   new DynamicDocument { { "_id", "Int64" }, { "prop1", int.MaxValue + 100L } },
                                   new DynamicDocument { { "_id", "Double" }, { "prop1", 123.456 } },
                                   new DynamicDocument { { "_id", "String" }, { "prop1", "abc" } },
                                   new DynamicDocument { { "_id", "DateTime" }, { "prop1", new DateTime(2015, 2, 9, 1, 2, 3, 4) } },
                                   new DynamicDocument { { "_id", "Binary" }, { "prop1", new byte[] { 1, 2, 3, 4, 5 } } },
                                   new DynamicDocument { { "_id", "Object" }, { "prop1", new DynamicDocument { { "subProp1", true } } } },
                                   new DynamicDocument { { "_id", "Array" }, { "prop1", new object[] { new object[] { 1, 2, 3 } } } }
                               });

            var booleanResult = storage.Find(f => f.Type("prop1", DataType.Boolean)).ToList();
            var int32Result = storage.Find(f => f.Type("prop1", DataType.Int32)).ToList();
            var int64Result = storage.Find(f => f.Type("prop1", DataType.Int64)).ToList();
            var doubleResult = storage.Find(f => f.Type("prop1", DataType.Double)).ToList();
            var stringResult = storage.Find(f => f.Type("prop1", DataType.String)).ToList();
            var dateTimeResult = storage.Find(f => f.Type("prop1", DataType.DateTime)).ToList();
            var binaryResult = storage.Find(f => f.Type("prop1", DataType.Binary)).ToList();
            var objectResult = storage.Find(f => f.Type("prop1", DataType.Object)).ToList();
            var arrayResult = storage.Find(f => f.Type("prop1", DataType.Array)).ToList();

            // Then

            Assert.AreEqual(1, booleanResult.Count);
            Assert.AreEqual("Boolean", ((dynamic)booleanResult[0])._id);
            Assert.AreEqual(true, ((dynamic)booleanResult[0]).prop1);

            Assert.AreEqual(1, int32Result.Count);
            Assert.AreEqual("Int32", ((dynamic)int32Result[0])._id);
            Assert.AreEqual(123, ((dynamic)int32Result[0]).prop1);

            Assert.AreEqual(1, int64Result.Count);
            Assert.AreEqual("Int64", ((dynamic)int64Result[0])._id);
            Assert.AreEqual(int.MaxValue + 100L, ((dynamic)int64Result[0]).prop1);

            Assert.AreEqual(1, doubleResult.Count);
            Assert.AreEqual("Double", ((dynamic)doubleResult[0])._id);
            Assert.AreEqual(123.456, ((dynamic)doubleResult[0]).prop1);

            Assert.AreEqual(1, stringResult.Count);
            Assert.AreEqual("String", ((dynamic)stringResult[0])._id);
            Assert.AreEqual("abc", ((dynamic)stringResult[0]).prop1);

            Assert.AreEqual(1, dateTimeResult.Count);
            Assert.AreEqual("DateTime", ((dynamic)dateTimeResult[0])._id);
            Assert.AreEqual(new DateTime(2015, 2, 9, 1, 2, 3, 4).ToUniversalTime(), ((dynamic)dateTimeResult[0]).prop1);

            Assert.AreEqual(1, binaryResult.Count);
            Assert.AreEqual("Binary", ((dynamic)binaryResult[0])._id);
            Assert.AreEqual(new byte[] { 1, 2, 3, 4, 5 }, ((dynamic)binaryResult[0]).prop1);

            Assert.AreEqual(1, objectResult.Count);
            Assert.AreEqual("Object", ((dynamic)objectResult[0])._id);
            Assert.AreEqual(true, ((dynamic)objectResult[0]).prop1.subProp1);

            Assert.AreEqual(1, arrayResult.Count);
            Assert.AreEqual("Array", ((dynamic)arrayResult[0])._id);
            Assert.AreEqual(new object[] { 1, 2, 3 }, ((dynamic)arrayResult[0]).prop1[0]);
        }

        [Test]
        public void ShouldFindByIn()
        {
            // Given
            var storage = MongoTestHelpers.GetEmptyStorageProvider(nameof(ShouldFindByIn));

            // When

            storage.InsertMany(new[]
                               {
                                   new DynamicDocument { { "_id", 1 }, { "prop1", 11 } },
                                   new DynamicDocument { { "_id", 2 }, { "prop1", 22 } },
                                   new DynamicDocument { { "_id", 3 }, { "prop1", 33 } },
                                   new DynamicDocument { { "_id", 4 }, { "prop1", 44 } },
                                   new DynamicDocument { { "_id", 5 }, { "prop1", 55 } }
                               });

            var inResult = storage.Find(f => f.In("prop1", new[] { 11, 33, 55 })).ToList();
            var notInResult = storage.Find(f => f.NotIn("prop1", new[] { 11, 33, 55 })).ToList();

            // Then

            Assert.AreEqual(3, inResult.Count);
            Assert.AreEqual(1, ((dynamic)inResult[0])._id);
            Assert.AreEqual(3, ((dynamic)inResult[1])._id);
            Assert.AreEqual(5, ((dynamic)inResult[2])._id);

            Assert.AreEqual(2, notInResult.Count);
            Assert.AreEqual(2, ((dynamic)notInResult[0])._id);
            Assert.AreEqual(4, ((dynamic)notInResult[1])._id);
        }

        [Test]
        public void ShouldFindByEq()
        {
            // Given
            var storage = MongoTestHelpers.GetEmptyStorageProvider(nameof(ShouldFindByEq));

            // When

            storage.InsertMany(new[]
                               {
                                   new DynamicDocument { { "_id", 1 }, { "prop1", 11 } },
                                   new DynamicDocument { { "_id", 2 }, { "prop1", 22 } },
                                   new DynamicDocument { { "_id", 3 }, { "prop1", 11 } },
                                   new DynamicDocument { { "_id", 4 }, { "prop1", 22 } },
                                   new DynamicDocument { { "_id", 5 }, { "prop1", 11 } }
                               });

            var eqResult = storage.Find(f => f.Eq("prop1", 11)).ToList();
            var notEqResult = storage.Find(f => f.NotEq("prop1", 11)).ToList();

            // Then

            Assert.AreEqual(3, eqResult.Count);
            Assert.AreEqual(1, ((dynamic)eqResult[0])._id);
            Assert.AreEqual(3, ((dynamic)eqResult[1])._id);
            Assert.AreEqual(5, ((dynamic)eqResult[2])._id);

            Assert.AreEqual(2, notEqResult.Count);
            Assert.AreEqual(2, ((dynamic)notEqResult[0])._id);
            Assert.AreEqual(4, ((dynamic)notEqResult[1])._id);
        }

        [Test]
        public void ShouldFindByCompareNumbers()
        {
            // Given
            var storage = MongoTestHelpers.GetEmptyStorageProvider(nameof(ShouldFindByCompareNumbers));

            // When

            storage.InsertMany(new[]
                               {
                                   new DynamicDocument { { "_id", 1 }, { "prop1", 1 } },
                                   new DynamicDocument { { "_id", 2 }, { "prop1", 1.5 } },
                                   new DynamicDocument { { "_id", 3 }, { "prop1", 2 } },
                                   new DynamicDocument { { "_id", 4 }, { "prop1", 2.5 } },
                                   new DynamicDocument { { "_id", 5 }, { "prop1", 3 } },
                                   new DynamicDocument { { "_id", 6 }, { "prop1", 3.5 } },
                                   new DynamicDocument { { "_id", 7 }, { "prop1", 4 } }
                               });

            var gtResult = storage.Find(f => f.Gt("prop1", 3)).ToList();
            var gteResult = storage.Find(f => f.Gte("prop1", 3.5)).ToList();

            var ltResult = storage.Find(f => f.Lt("prop1", 2)).ToList();
            var lteResult = storage.Find(f => f.Lte("prop1", 1.5)).ToList();

            var betweenResult = storage.Find(f => f.And(f.Gt("prop1", 1.5), f.Lt("prop1", 3.5))).ToList();

            // Then

            Assert.AreEqual(2, gtResult.Count);
            Assert.AreEqual(6, ((dynamic)gtResult[0])._id);
            Assert.AreEqual(7, ((dynamic)gtResult[1])._id);

            Assert.AreEqual(2, gteResult.Count);
            Assert.AreEqual(6, ((dynamic)gteResult[0])._id);
            Assert.AreEqual(7, ((dynamic)gteResult[1])._id);

            Assert.AreEqual(2, ltResult.Count);
            Assert.AreEqual(1, ((dynamic)ltResult[0])._id);
            Assert.AreEqual(2, ((dynamic)ltResult[1])._id);

            Assert.AreEqual(2, lteResult.Count);
            Assert.AreEqual(1, ((dynamic)lteResult[0])._id);
            Assert.AreEqual(2, ((dynamic)lteResult[1])._id);

            Assert.AreEqual(3, betweenResult.Count);
            Assert.AreEqual(3, ((dynamic)betweenResult[0])._id);
            Assert.AreEqual(4, ((dynamic)betweenResult[1])._id);
            Assert.AreEqual(5, ((dynamic)betweenResult[2])._id);
        }

        [Test]
        public void ShouldFindByCompareDateTimes()
        {
            // Given
            var storage = MongoTestHelpers.GetEmptyStorageProvider(nameof(ShouldFindByCompareDateTimes));

            // When

            storage.InsertMany(new[]
                               {
                                   new DynamicDocument { { "_id", 1 }, { "prop1", DateTime.Today.AddHours(1) } },
                                   new DynamicDocument { { "_id", 2 }, { "prop1", DateTime.Today.AddHours(1.5) } },
                                   new DynamicDocument { { "_id", 3 }, { "prop1", DateTime.Today.AddHours(2) } },
                                   new DynamicDocument { { "_id", 4 }, { "prop1", DateTime.Today.AddHours(2.5) } },
                                   new DynamicDocument { { "_id", 5 }, { "prop1", DateTime.Today.AddHours(3) } },
                                   new DynamicDocument { { "_id", 6 }, { "prop1", DateTime.Today.AddHours(3.5) } },
                                   new DynamicDocument { { "_id", 7 }, { "prop1", DateTime.Today.AddHours(4) } }
                               });

            var gtResult = storage.Find(f => f.Gt("prop1", DateTime.Today.AddHours(3))).ToList();
            var gteResult = storage.Find(f => f.Gte("prop1", DateTime.Today.AddHours(3.5))).ToList();

            var ltResult = storage.Find(f => f.Lt("prop1", DateTime.Today.AddHours(2))).ToList();
            var lteResult = storage.Find(f => f.Lte("prop1", DateTime.Today.AddHours(1.5))).ToList();

            var betweenResult = storage.Find(f => f.And(f.Gt("prop1", DateTime.Today.AddHours(1.5)), f.Lt("prop1", DateTime.Today.AddHours(3.5)))).ToList();

            // Then

            Assert.AreEqual(2, gtResult.Count);
            Assert.AreEqual(6, ((dynamic)gtResult[0])._id);
            Assert.AreEqual(7, ((dynamic)gtResult[1])._id);

            Assert.AreEqual(2, gteResult.Count);
            Assert.AreEqual(6, ((dynamic)gteResult[0])._id);
            Assert.AreEqual(7, ((dynamic)gteResult[1])._id);

            Assert.AreEqual(2, ltResult.Count);
            Assert.AreEqual(1, ((dynamic)ltResult[0])._id);
            Assert.AreEqual(2, ((dynamic)ltResult[1])._id);

            Assert.AreEqual(2, lteResult.Count);
            Assert.AreEqual(1, ((dynamic)lteResult[0])._id);
            Assert.AreEqual(2, ((dynamic)lteResult[1])._id);

            Assert.AreEqual(3, betweenResult.Count);
            Assert.AreEqual(3, ((dynamic)betweenResult[0])._id);
            Assert.AreEqual(4, ((dynamic)betweenResult[1])._id);
            Assert.AreEqual(5, ((dynamic)betweenResult[2])._id);
        }

        [Test]
        public void ShouldFindByRegex()
        {
            // Given
            var storage = MongoTestHelpers.GetEmptyStorageProvider(nameof(ShouldFindByRegex));

            // When

            storage.InsertMany(new[]
                               {
                                   new DynamicDocument { { "_id", 100 }, { "sku", "abc123" }, { "description", "Single line description." } },
                                   new DynamicDocument { { "_id", 101 }, { "sku", "abc789" }, { "description", "First line\nSecond line" } },
                                   new DynamicDocument { { "_id", 102 }, { "sku", "xyz456" }, { "description", "Many spaces before     line" } },
                                   new DynamicDocument { { "_id", 103 }, { "sku", "xyz789" }, { "description", "Multiple\nline description" } }
                               });

            var caseInsensitiveRegexResult = storage.Find(f => f.Regex("sku", new Regex("^ABC", RegexOptions.IgnoreCase))).ToList();
            var multilineMatchRegexResult = storage.Find(f => f.Regex("description", new Regex("^S", RegexOptions.Multiline))).ToList();
            var ignoreNewLineRegexResult = storage.Find(f => f.Regex("description", new Regex("m.*line", RegexOptions.Singleline | RegexOptions.IgnoreCase))).ToList();
            var ignoreWhiteSpacesRegexResult = storage.Find(f => f.Regex("description", new Regex("\\S+\\s+line$", RegexOptions.Singleline))).ToList();

            // Then

            Assert.AreEqual(2, caseInsensitiveRegexResult.Count);
            Assert.AreEqual(100, ((dynamic)caseInsensitiveRegexResult[0])._id);
            Assert.AreEqual(101, ((dynamic)caseInsensitiveRegexResult[1])._id);

            Assert.AreEqual(2, multilineMatchRegexResult.Count);
            Assert.AreEqual(100, ((dynamic)caseInsensitiveRegexResult[0])._id);
            Assert.AreEqual(101, ((dynamic)caseInsensitiveRegexResult[1])._id);

            Assert.AreEqual(2, ignoreNewLineRegexResult.Count);
            Assert.AreEqual(102, ((dynamic)ignoreNewLineRegexResult[0])._id);
            Assert.AreEqual(103, ((dynamic)ignoreNewLineRegexResult[1])._id);

            Assert.AreEqual(2, ignoreWhiteSpacesRegexResult.Count);
            Assert.AreEqual(101, ((dynamic)ignoreWhiteSpacesRegexResult[0])._id);
            Assert.AreEqual(102, ((dynamic)ignoreWhiteSpacesRegexResult[1])._id);
        }

        [Test]
        public void ShouldFindByStartsWith()
        {
            // Given
            var storage = MongoTestHelpers.GetEmptyStorageProvider(nameof(ShouldFindByStartsWith));

            // When

            storage.InsertMany(new[]
                               {
                                   new DynamicDocument { { "_id", 1 }, { "prop1", "It starts with some text." } },
                                   new DynamicDocument { { "_id", 2 }, { "prop1", "it starts with some text." } },
                                   new DynamicDocument { { "_id", 3 }, { "prop1", "Does it start with some text?" } }
                               });

            var caseInsensitiveResult= storage.Find(f => f.StartsWith("prop1", "It")).ToList();
            var caseSensitiveResult = storage.Find(f => f.StartsWith("prop1", "It", false)).ToList();

            // Then

            Assert.AreEqual(2, caseInsensitiveResult.Count);
            Assert.AreEqual(1, ((dynamic)caseInsensitiveResult[0])._id);
            Assert.AreEqual(2, ((dynamic)caseInsensitiveResult[1])._id);

            Assert.AreEqual(1, caseSensitiveResult.Count);
            Assert.AreEqual(1, ((dynamic)caseSensitiveResult[0])._id);
        }

        [Test]
        public void ShouldFindByEndsWith()
        {
            // Given
            var storage = MongoTestHelpers.GetEmptyStorageProvider(nameof(ShouldFindByEndsWith));

            // When

            storage.InsertMany(new[]
                               {
                                   new DynamicDocument { { "_id", 1 }, { "prop1", "It ends with some Text." } },
                                   new DynamicDocument { { "_id", 2 }, { "prop1", "It ends with some text." } },
                                   new DynamicDocument { { "_id", 3 }, { "prop1", "Does it end with some text?" } }
                               });

            var caseInsensitiveResult= storage.Find(f => f.EndsWith("prop1", "Text.")).ToList();
            var caseSensitiveResult = storage.Find(f => f.EndsWith("prop1", "Text.", false)).ToList();

            // Then

            Assert.AreEqual(2, caseInsensitiveResult.Count);
            Assert.AreEqual(1, ((dynamic)caseInsensitiveResult[0])._id);
            Assert.AreEqual(2, ((dynamic)caseInsensitiveResult[1])._id);

            Assert.AreEqual(1, caseSensitiveResult.Count);
            Assert.AreEqual(1, ((dynamic)caseSensitiveResult[0])._id);
        }

        [Test]
        public void ShouldFindByContains()
        {
            // Given
            var storage = MongoTestHelpers.GetEmptyStorageProvider(nameof(ShouldFindByContains));

            // When

            storage.InsertMany(new[]
                               {
                                   new DynamicDocument { { "_id", 1 }, { "prop1", "It Contains some text." } },
                                   new DynamicDocument { { "_id", 2 }, { "prop1", "It contains some text." } },
                                   new DynamicDocument { { "_id", 3 }, { "prop1", "Does it contain some text?" } }
                               });

            var caseInsensitiveResult= storage.Find(f => f.Contains("prop1", "Contains")).ToList();
            var caseSensitiveResult = storage.Find(f => f.Contains("prop1", "Contains", false)).ToList();

            // Then

            Assert.AreEqual(2, caseInsensitiveResult.Count);
            Assert.AreEqual(1, ((dynamic)caseInsensitiveResult[0])._id);
            Assert.AreEqual(2, ((dynamic)caseInsensitiveResult[1])._id);

            Assert.AreEqual(1, caseSensitiveResult.Count);
            Assert.AreEqual(1, ((dynamic)caseSensitiveResult[0])._id);
        }

        [Test]
        public void ShouldFindByMatch()
        {
            // Given
            var storage = MongoTestHelpers.GetEmptyStorageProvider(nameof(ShouldFindByMatch));

            // When

            storage.InsertMany(new[]
                               {
                                   new DynamicDocument
                                   {
                                       { "_id", 1 },
                                       {
                                           "results", new[]
                                                      {
                                                          new DynamicDocument { { "product", "abc" }, { "score", 10 } },
                                                          new DynamicDocument { { "product", "xyz" }, { "score", 5 } }
                                                      }
                                       }
                                   },
                                   new DynamicDocument
                                   {
                                       { "_id", 2 },
                                       {
                                           "results", new[]
                                                      {
                                                          new DynamicDocument { { "product", "abc" }, { "score", 8 } },
                                                          new DynamicDocument { { "product", "xyz" }, { "score", 7 } }
                                                      }
                                       }
                                   },
                                   new DynamicDocument
                                   {
                                       { "_id", 3 },
                                       {
                                           "results", new[]
                                                      {
                                                          new DynamicDocument { { "product", "abc" }, { "score", 7 } },
                                                          new DynamicDocument { { "product", "xyz" }, { "score", 8 } }
                                                      }
                                       }
                                   }
                               });

            var elemMatchResult = storage.Find(f => f.Match("results", f.And(f.Eq("product", "xyz"), f.Gte("score", 8)))).ToList();

            // Then

            Assert.AreEqual(1, elemMatchResult.Count);
            Assert.AreEqual(3, ((dynamic)elemMatchResult[0])._id);
        }

        [Test]
        public void ShouldFindByAll()
        {
            // Given
            var storage = MongoTestHelpers.GetEmptyStorageProvider(nameof(ShouldFindByAll));

            // When

            storage.InsertMany(new[]
                               {
                                   new DynamicDocument
                                   {
                                       { "_id", 1 },
                                       { "code", "xyz" },
                                       { "tags", new[] { "school", "book", "bag", "headphone", "appliance" } },
                                       {
                                           "qty", new[]
                                                  {
                                                      new DynamicDocument { { "size", "S" }, { "num", 10 }, { "color", "blue" } },
                                                      new DynamicDocument { { "size", "M" }, { "num", 45 }, { "color", "blue" } },
                                                      new DynamicDocument { { "size", "L" }, { "num", 100 }, { "color", "green" } }
                                                  }
                                       }
                                   },
                                   new DynamicDocument
                                   {
                                       { "_id", 2 },
                                       { "code", "abc" },
                                       { "tags", new[] { "appliance", "school", "book" } },
                                       {
                                           "qty", new[]
                                                  {
                                                      new DynamicDocument { { "size", "6" }, { "num", 100 }, { "color", "green" } },
                                                      new DynamicDocument { { "size", "6" }, { "num", 50 }, { "color", "blue" } },
                                                      new DynamicDocument { { "size", "8" }, { "num", 100 }, { "color", "brown" } }
                                                  }
                                       }
                                   },
                                   new DynamicDocument
                                   {
                                       { "_id", 3 },
                                       { "code", "efg" },
                                       { "tags", new[] { "school", "book" } },
                                       {
                                           "qty", new[]
                                                  {
                                                      new DynamicDocument { { "size", "S" }, { "num", 10 }, { "color", "blue" } },
                                                      new DynamicDocument { { "size", "M" }, { "num", 100 }, { "color", "blue" } },
                                                      new DynamicDocument { { "size", "L" }, { "num", 100 }, { "color", "green" } }
                                                  }
                                       }
                                   },
                                   new DynamicDocument
                                   {
                                       { "_id", 4 },
                                       { "code", "ijk" },
                                       { "tags", new[] { "electronics", "school" } },
                                       {
                                           "qty", new[]
                                                  {
                                                      new DynamicDocument { { "size", "M" }, { "num", 100 }, { "color", "green" } }
                                                  }
                                       }
                                   }
                               });

            var allResult = storage.Find(f => f.All("tags", new[] { "appliance", "school", "book" })).ToList();

            // Then

            Assert.AreEqual(2, allResult.Count);
            Assert.AreEqual(1, ((dynamic)allResult[0])._id);
            Assert.AreEqual(2, ((dynamic)allResult[1])._id);
        }

        [Test]
        public void ShouldFindByAnyIn()
        {
            // Given
            var storage = MongoTestHelpers.GetEmptyStorageProvider(nameof(ShouldFindByAnyIn));

            // When

            storage.InsertMany(new[]
                               {
                                   new DynamicDocument { { "_id", 1 }, { "items", new[] { 1, 2, 3 } } },
                                   new DynamicDocument { { "_id", 2 }, { "items", new[] { 2, 3, 4 } } },
                                   new DynamicDocument { { "_id", 3 }, { "items", new[] { 3, 4, 5 } } },
                                   new DynamicDocument { { "_id", 4 }, { "items", new[] { 4, 5, 6 } } },
                                   new DynamicDocument { { "_id", 5 }, { "items", new[] { 5, 6, 7 } } }
                               });

            var anyInResult = storage.Find(f => f.AnyIn("items", new[] { 3, 4 })).ToList();
            var anyNotInResult = storage.Find(f => f.AnyNotIn("items", new[] { 3, 4 })).ToList();

            // Then

            Assert.AreEqual(4, anyInResult.Count);
            Assert.AreEqual(1, ((dynamic)anyInResult[0])._id);
            Assert.AreEqual(2, ((dynamic)anyInResult[1])._id);
            Assert.AreEqual(3, ((dynamic)anyInResult[2])._id);
            Assert.AreEqual(4, ((dynamic)anyInResult[3])._id);

            Assert.AreEqual(1, anyNotInResult.Count);
            Assert.AreEqual(5, ((dynamic)anyNotInResult[0])._id);
        }

        [Test]
        public void ShouldFindByAnyEq()
        {
            // Given
            var storage = MongoTestHelpers.GetEmptyStorageProvider(nameof(ShouldFindByAnyEq));

            // When

            storage.InsertMany(new[]
                               {
                                   new DynamicDocument { { "_id", 1 }, { "items", new[] { 1, 2, 3 } } },
                                   new DynamicDocument { { "_id", 2 }, { "items", new[] { 2, 3, 4 } } },
                                   new DynamicDocument { { "_id", 3 }, { "items", new[] { 3, 4, 5 } } },
                                   new DynamicDocument { { "_id", 4 }, { "items", new[] { 4, 5, 6 } } },
                                   new DynamicDocument { { "_id", 5 }, { "items", new[] { 5, 6, 7 } } }
                               });

            var anyEqResult = storage.Find(f => f.AnyEq("items", 4)).ToList();
            var anyNotEqResult = storage.Find(f => f.AnyNotEq("items", 4)).ToList();

            // Then

            Assert.AreEqual(3, anyEqResult.Count);
            Assert.AreEqual(2, ((dynamic)anyEqResult[0])._id);
            Assert.AreEqual(3, ((dynamic)anyEqResult[1])._id);
            Assert.AreEqual(4, ((dynamic)anyEqResult[2])._id);

            Assert.AreEqual(2, anyNotEqResult.Count);
            Assert.AreEqual(1, ((dynamic)anyNotEqResult[0])._id);
            Assert.AreEqual(5, ((dynamic)anyNotEqResult[1])._id);
        }

        [Test]
        public void ShouldFindByAnyCompareNumbers()
        {
            // Given
            var storage = MongoTestHelpers.GetEmptyStorageProvider(nameof(ShouldFindByAnyCompareNumbers));

            // When

            storage.InsertMany(new[]
                               {
                                   new DynamicDocument { { "_id", 1 }, { "items", new[] { 1, 2, 3 } } },
                                   new DynamicDocument { { "_id", 2 }, { "items", new[] { 2, 3, 4 } } },
                                   new DynamicDocument { { "_id", 3 }, { "items", new[] { 3, 4, 5 } } },
                                   new DynamicDocument { { "_id", 4 }, { "items", new[] { 4, 5, 6 } } },
                                   new DynamicDocument { { "_id", 5 }, { "items", new[] { 5, 6, 7 } } }
                               });

            var anyGtResult = storage.Find(f => f.AnyGt("items", 4)).ToList();
            var anyGteResult = storage.Find(f => f.AnyGte("items", 6)).ToList();
            var anyLtResult = storage.Find(f => f.AnyLt("items", 3)).ToList();
            var anyLteResult = storage.Find(f => f.AnyLte("items", 4)).ToList();

            // Then

            Assert.AreEqual(3, anyGtResult.Count);
            Assert.AreEqual(3, ((dynamic)anyGtResult[0])._id);
            Assert.AreEqual(4, ((dynamic)anyGtResult[1])._id);
            Assert.AreEqual(5, ((dynamic)anyGtResult[2])._id);

            Assert.AreEqual(2, anyGteResult.Count);
            Assert.AreEqual(4, ((dynamic)anyGteResult[0])._id);
            Assert.AreEqual(5, ((dynamic)anyGteResult[1])._id);

            Assert.AreEqual(2, anyLtResult.Count);
            Assert.AreEqual(1, ((dynamic)anyLtResult[0])._id);
            Assert.AreEqual(2, ((dynamic)anyLtResult[1])._id);

            Assert.AreEqual(4, anyLteResult.Count);
            Assert.AreEqual(1, ((dynamic)anyLteResult[0])._id);
            Assert.AreEqual(2, ((dynamic)anyLteResult[1])._id);
            Assert.AreEqual(3, ((dynamic)anyLteResult[2])._id);
            Assert.AreEqual(4, ((dynamic)anyLteResult[3])._id);
        }

        [Test]
        public void ShouldFindByAnyCompareDateTimes()
        {
            // Given
            var storage = MongoTestHelpers.GetEmptyStorageProvider(nameof(ShouldFindByAnyCompareDateTimes));

            // When

            var today = DateTime.Today;

            storage.InsertMany(new[]
                               {
                                   new DynamicDocument { { "_id", 1 }, { "items", new[] { today.AddHours(1), today.AddHours(2), today.AddHours(3) } } },
                                   new DynamicDocument { { "_id", 2 }, { "items", new[] { today.AddHours(2), today.AddHours(3), today.AddHours(4) } } },
                                   new DynamicDocument { { "_id", 3 }, { "items", new[] { today.AddHours(3), today.AddHours(4), today.AddHours(5) } } },
                                   new DynamicDocument { { "_id", 4 }, { "items", new[] { today.AddHours(4), today.AddHours(5), today.AddHours(6) } } },
                                   new DynamicDocument { { "_id", 5 }, { "items", new[] { today.AddHours(5), today.AddHours(6), today.AddHours(7) } } }
                               });

            var anyGtResult = storage.Find(f => f.AnyGt("items", today.AddHours(4))).ToList();
            var anyGteResult = storage.Find(f => f.AnyGte("items", today.AddHours(6))).ToList();
            var anyLtResult = storage.Find(f => f.AnyLt("items", today.AddHours(3))).ToList();
            var anyLteResult = storage.Find(f => f.AnyLte("items", today.AddHours(4))).ToList();

            // Then

            Assert.AreEqual(3, anyGtResult.Count);
            Assert.AreEqual(3, ((dynamic)anyGtResult[0])._id);
            Assert.AreEqual(4, ((dynamic)anyGtResult[1])._id);
            Assert.AreEqual(5, ((dynamic)anyGtResult[2])._id);

            Assert.AreEqual(2, anyGteResult.Count);
            Assert.AreEqual(4, ((dynamic)anyGteResult[0])._id);
            Assert.AreEqual(5, ((dynamic)anyGteResult[1])._id);

            Assert.AreEqual(2, anyLtResult.Count);
            Assert.AreEqual(1, ((dynamic)anyLtResult[0])._id);
            Assert.AreEqual(2, ((dynamic)anyLtResult[1])._id);

            Assert.AreEqual(4, anyLteResult.Count);
            Assert.AreEqual(1, ((dynamic)anyLteResult[0])._id);
            Assert.AreEqual(2, ((dynamic)anyLteResult[1])._id);
            Assert.AreEqual(3, ((dynamic)anyLteResult[2])._id);
            Assert.AreEqual(4, ((dynamic)anyLteResult[3])._id);
        }

        [Test]
        public void ShouldFindBySize()
        {
            // Given
            var storage = MongoTestHelpers.GetEmptyStorageProvider(nameof(ShouldFindBySize));

            // When

            storage.InsertMany(new[]
                               {
                                   new DynamicDocument { { "_id", 1 } },
                                   new DynamicDocument { { "_id", 2 }, { "items", new int[] { } } },
                                   new DynamicDocument { { "_id", 3 }, { "items", new[] { 1 } } },
                                   new DynamicDocument { { "_id", 4 }, { "items", new[] { 1, 2 } } },
                                   new DynamicDocument { { "_id", 5 }, { "items", new[] { 1, 2, 3 } } },
                                   new DynamicDocument { { "_id", 6 }, { "items", new[] { 1, 2, 3, 4 } } },
                                   new DynamicDocument { { "_id", 7 }, { "items", new[] { 1, 2, 3, 4, 5 } } }
                               });

            var sizeEq0Result = storage.Find(f => f.SizeEq("items", 0)).ToList();
            var sizeEq1Result = storage.Find(f => f.SizeEq("items", 1)).ToList();
            var sizeEq2Result = storage.Find(f => f.SizeEq("items", 2)).ToList();

            var sizeGtResult = storage.Find(f => f.SizeGt("items", 3)).ToList();
            var sizeGteResult = storage.Find(f => f.SizeGte("items", 3)).ToList();

            var sizeLtResult = storage.Find(f => f.SizeLt("items", 2)).ToList();
            var sizeLteResult = storage.Find(f => f.SizeLte("items", 2)).ToList();

            // Then

            Assert.AreEqual(1, sizeEq0Result.Count);
            Assert.AreEqual(2, ((dynamic)sizeEq0Result[0])._id);

            Assert.AreEqual(1, sizeEq1Result.Count);
            Assert.AreEqual(3, ((dynamic)sizeEq1Result[0])._id);

            Assert.AreEqual(1, sizeEq2Result.Count);
            Assert.AreEqual(4, ((dynamic)sizeEq2Result[0])._id);

            Assert.AreEqual(2, sizeGtResult.Count);
            Assert.AreEqual(6, ((dynamic)sizeGtResult[0])._id);
            Assert.AreEqual(7, ((dynamic)sizeGtResult[1])._id);

            Assert.AreEqual(3, sizeGteResult.Count);
            Assert.AreEqual(5, ((dynamic)sizeGteResult[0])._id);
            Assert.AreEqual(6, ((dynamic)sizeGteResult[1])._id);
            Assert.AreEqual(7, ((dynamic)sizeGteResult[2])._id);

            Assert.AreEqual(3, sizeLtResult.Count);
            Assert.AreEqual(1, ((dynamic)sizeLtResult[0])._id);
            Assert.AreEqual(2, ((dynamic)sizeLtResult[1])._id);
            Assert.AreEqual(3, ((dynamic)sizeLtResult[2])._id);

            Assert.AreEqual(4, sizeLteResult.Count);
            Assert.AreEqual(1, ((dynamic)sizeLteResult[0])._id);
            Assert.AreEqual(2, ((dynamic)sizeLteResult[1])._id);
            Assert.AreEqual(3, ((dynamic)sizeLteResult[2])._id);
            Assert.AreEqual(4, ((dynamic)sizeLteResult[3])._id);
        }

        [Test]
        public void ShouldFindByText()
        {
            // Given

            var textIndex = new DocumentIndex
            {
                Key = new Dictionary<string, DocumentIndexKeyType>
                                      {
                                          { "subject", DocumentIndexKeyType.Text }
                                      }
            };

            var storage = MongoTestHelpers.GetEmptyStorageProvider(nameof(ShouldFindByText), textIndex);

            // When

            storage.InsertMany(new[]
                               {
                                   new DynamicDocument { { "_id", 1 }, { "subject", "coffee" }, { "author", "xyz" }, { "views", 50 } },
                                   new DynamicDocument { { "_id", 2 }, { "subject", "Coffee Shopping" }, { "author", "efg" }, { "views", 5 } },
                                   new DynamicDocument { { "_id", 3 }, { "subject", "Baking a cake" }, { "author", "abc" }, { "views", 90 } },
                                   new DynamicDocument { { "_id", 4 }, { "subject", "baking" }, { "author", "xyz" }, { "views", 100 } },
                                   new DynamicDocument { { "_id", 5 }, { "subject", "Café Con Leche" }, { "author", "abc" }, { "views", 200 } },
                                   new DynamicDocument { { "_id", 6 }, { "subject", "Сырники" }, { "author", "jkl" }, { "views", 80 } },
                                   new DynamicDocument { { "_id", 7 }, { "subject", "coffee and cream" }, { "author", "efg" }, { "views", 10 } },
                                   new DynamicDocument { { "_id", 8 }, { "subject", "Cafe con Leche" }, { "author", "xyz" }, { "views", 10 } }
                               });

            var searchSingleWordResult = storage.Find(f => f.Text("coffee")).ToList();
            var searchWithoutTermResult = storage.Find(f => f.Text("coffee -shop")).ToList();
            var searchWithLanguageResult = storage.Find(f => f.Text("leche", "es")).ToList();
            var diacriticInsensitiveSearchResult = storage.Find(f => f.Text("сы́рники CAFÉS")).ToList();
            var caseSensitiveSearchForTermResult = storage.Find(f => f.Text("Coffee", caseSensitive: true)).ToList();
            var caseSensitiveSearchForPhraseResult = storage.Find(f => f.Text("\"Café Con Leche\"", caseSensitive: true)).ToList();
            var caseSensitiveSearchWithNegatedTermResult = storage.Find(f => f.Text("Coffee -shop", caseSensitive: true)).ToList();
            var diacriticSensitiveSearchForTermResult = storage.Find(f => f.Text("CAFÉ", diacriticSensitive: true)).ToList();
            var diacriticSensitiveSearchWithNegatedTermResult = storage.Find(f => f.Text("leches -cafés", diacriticSensitive: true)).ToList();
            var searchWithTextScoreResult = storage.Find(f => f.Text("coffee")).Project(p => p.IncludeTextScore("score")).SortByTextScore("score").ToList();

            // Then

            Assert.AreEqual(3, searchSingleWordResult.Count, nameof(searchSingleWordResult));
            CollectionAssert.AreEquivalent(new[] { 2, 7, 1 }, new[] { searchSingleWordResult[0]["_id"], searchSingleWordResult[1]["_id"], searchSingleWordResult[2]["_id"] });

            Assert.AreEqual(2, searchWithoutTermResult.Count, nameof(searchWithoutTermResult));
            CollectionAssert.AreEquivalent(new[] { 7, 1 }, new[] { searchWithoutTermResult[0]["_id"], searchWithoutTermResult[1]["_id"] });

            Assert.AreEqual(2, searchWithLanguageResult.Count, nameof(searchWithLanguageResult));
            CollectionAssert.AreEquivalent(new[] { 5, 8 }, new[] { searchWithLanguageResult[0]["_id"], searchWithLanguageResult[1]["_id"] });

            Assert.AreEqual(3, diacriticInsensitiveSearchResult.Count, nameof(diacriticInsensitiveSearchResult));
            CollectionAssert.AreEquivalent(new[] { 6, 5, 8 }, new[] { diacriticInsensitiveSearchResult[0]["_id"], diacriticInsensitiveSearchResult[1]["_id"], diacriticInsensitiveSearchResult[2]["_id"] });

            Assert.AreEqual(1, caseSensitiveSearchForTermResult.Count, nameof(caseSensitiveSearchForTermResult));
            Assert.AreEqual(2, caseSensitiveSearchForTermResult[0]["_id"], nameof(caseSensitiveSearchForTermResult));

            Assert.AreEqual(1, caseSensitiveSearchForPhraseResult.Count, nameof(caseSensitiveSearchForPhraseResult));
            Assert.AreEqual(5, caseSensitiveSearchForPhraseResult[0]["_id"], nameof(caseSensitiveSearchForPhraseResult));

            Assert.AreEqual(1, caseSensitiveSearchWithNegatedTermResult.Count, nameof(caseSensitiveSearchWithNegatedTermResult));
            Assert.AreEqual(2, caseSensitiveSearchWithNegatedTermResult[0]["_id"], nameof(caseSensitiveSearchWithNegatedTermResult));

            Assert.AreEqual(1, diacriticSensitiveSearchForTermResult.Count, nameof(diacriticSensitiveSearchForTermResult));
            Assert.AreEqual(5, diacriticSensitiveSearchForTermResult[0]["_id"], nameof(diacriticSensitiveSearchForTermResult));

            Assert.AreEqual(1, diacriticSensitiveSearchWithNegatedTermResult.Count, nameof(diacriticSensitiveSearchWithNegatedTermResult));
            Assert.AreEqual(8, diacriticSensitiveSearchWithNegatedTermResult[0]["_id"], nameof(diacriticSensitiveSearchWithNegatedTermResult));

            Assert.AreEqual(3, searchWithTextScoreResult.Count, nameof(searchWithTextScoreResult));
            Assert.AreEqual(1, searchWithTextScoreResult[0]["_id"], nameof(searchWithTextScoreResult));
            Assert.AreEqual(2, searchWithTextScoreResult[1]["_id"], nameof(searchWithTextScoreResult));
            Assert.AreEqual(7, searchWithTextScoreResult[2]["_id"], nameof(searchWithTextScoreResult));
        }

        [Test]
        public void ShouldFindByWhere()
        {
            // Given
            var storage = MongoTestHelpers.GetEmptyStorageProvider(nameof(ShouldFindByWhere));

            // When

            storage.InsertMany(new[]
                               {
                                   new DynamicDocument { { "_id", 1 }, { "prop1", "A" }, { "prop2", 11 } },
                                   new DynamicDocument { { "_id", 2 }, { "prop1", "A" }, { "prop2", 11 } },
                                   new DynamicDocument { { "_id", 3 }, { "prop1", "A" }, { "prop2", 12 } },
                                   new DynamicDocument { { "_id", 4 }, { "prop1", "B" }, { "prop2", 11 } },
                                   new DynamicDocument { { "_id", 5 }, { "prop1", "B" }, { "prop2", 11 } },
                                   new DynamicDocument { { "_id", 6 }, { "prop1", "B" }, { "prop2", 12 } }
                               });

            var resultA11 = storage.Find()
                                   .Where(f => f.Eq("prop1", "A"))
                                   .Where(f => f.Eq("prop2", 11))
                                   .ToList();

            var resultA12 = storage.Find()
                                   .Where(f => f.Eq("prop1", "A"))
                                   .Where(f => f.Eq("prop2", 12))
                                   .ToList();

            // Then

            Assert.AreEqual(2, resultA11.Count);
            Assert.AreEqual(1, ((dynamic)resultA11[0])._id);
            Assert.AreEqual(2, ((dynamic)resultA11[1])._id);

            Assert.AreEqual(1, resultA12.Count);
            Assert.AreEqual(3, ((dynamic)resultA12[0])._id);
        }

        [Test]
        public void ShouldFindWithSort()
        {
            // Given
            var storage = MongoTestHelpers.GetEmptyStorageProvider(nameof(ShouldFindWithSort));

            // When

            storage.InsertMany(new[]
                               {
                                   new DynamicDocument { { "_id", 1 }, { "item", new DynamicDocument { { "category", "cake" }, { "type", "chiffon" } } }, { "amount", 10 } },
                                   new DynamicDocument { { "_id", 2 }, { "item", new DynamicDocument { { "category", "cookies" }, { "type", "chocolate chip" } } }, { "amount", 50 } },
                                   new DynamicDocument { { "_id", 3 }, { "item", new DynamicDocument { { "category", "cookies" }, { "type", "chocolate chip" } } }, { "amount", 15 } },
                                   new DynamicDocument { { "_id", 4 }, { "item", new DynamicDocument { { "category", "cake" }, { "type", "lemon" } } }, { "amount", 30 } },
                                   new DynamicDocument { { "_id", 5 }, { "item", new DynamicDocument { { "category", "cake" }, { "type", "carrot" } } }, { "amount", 20 } },
                                   new DynamicDocument { { "_id", 6 }, { "item", new DynamicDocument { { "category", "brownies" }, { "type", "blondie" } } }, { "amount", 10 } }
                               });

            var sortAscByOneFieldResult = storage.Find().SortBy("amount").ToList();
            var sortDescByOneFieldResult = storage.Find().SortByDescending("amount").ToList();
            var sortAscByTwoFieldsResult = storage.Find().SortBy("item.category").ThenBy("item.type").ToList();
            var sortAscDescByTwoFieldsResult = storage.Find().SortBy("item.category").ThenByDescending("item.type").ToList();

            // Then

            Assert.AreEqual(6, sortAscByOneFieldResult.Count);
            Assert.AreEqual(1, ((dynamic)sortAscByOneFieldResult[0])._id);
            Assert.AreEqual(6, ((dynamic)sortAscByOneFieldResult[1])._id);
            Assert.AreEqual(3, ((dynamic)sortAscByOneFieldResult[2])._id);
            Assert.AreEqual(5, ((dynamic)sortAscByOneFieldResult[3])._id);
            Assert.AreEqual(4, ((dynamic)sortAscByOneFieldResult[4])._id);
            Assert.AreEqual(2, ((dynamic)sortAscByOneFieldResult[5])._id);

            Assert.AreEqual(6, sortDescByOneFieldResult.Count);
            Assert.AreEqual(2, ((dynamic)sortDescByOneFieldResult[0])._id);
            Assert.AreEqual(4, ((dynamic)sortDescByOneFieldResult[1])._id);
            Assert.AreEqual(5, ((dynamic)sortDescByOneFieldResult[2])._id);
            Assert.AreEqual(3, ((dynamic)sortDescByOneFieldResult[3])._id);
            Assert.AreEqual(1, ((dynamic)sortDescByOneFieldResult[4])._id);
            Assert.AreEqual(6, ((dynamic)sortDescByOneFieldResult[5])._id);

            Assert.AreEqual(6, sortAscByTwoFieldsResult.Count);
            Assert.AreEqual(6, ((dynamic)sortAscByTwoFieldsResult[0])._id);
            Assert.AreEqual(5, ((dynamic)sortAscByTwoFieldsResult[1])._id);
            Assert.AreEqual(1, ((dynamic)sortAscByTwoFieldsResult[2])._id);
            Assert.AreEqual(4, ((dynamic)sortAscByTwoFieldsResult[3])._id);
            Assert.AreEqual(2, ((dynamic)sortAscByTwoFieldsResult[4])._id);
            Assert.AreEqual(3, ((dynamic)sortAscByTwoFieldsResult[5])._id);

            Assert.AreEqual(6, sortAscDescByTwoFieldsResult.Count);
            Assert.AreEqual(6, ((dynamic)sortAscDescByTwoFieldsResult[0])._id);
            Assert.AreEqual(4, ((dynamic)sortAscDescByTwoFieldsResult[1])._id);
            Assert.AreEqual(1, ((dynamic)sortAscDescByTwoFieldsResult[2])._id);
            Assert.AreEqual(5, ((dynamic)sortAscDescByTwoFieldsResult[3])._id);
            Assert.AreEqual(2, ((dynamic)sortAscDescByTwoFieldsResult[4])._id);
            Assert.AreEqual(3, ((dynamic)sortAscDescByTwoFieldsResult[5])._id);
        }

        [Test]
        public void ShouldFindWithSkipAndLimit()
        {
            // Given
            var storage = MongoTestHelpers.GetEmptyStorageProvider(nameof(ShouldFindWithSkipAndLimit));

            // When

            storage.InsertMany(new[]
                               {
                                   new DynamicDocument { { "_id", 1 } },
                                   new DynamicDocument { { "_id", 2 } },
                                   new DynamicDocument { { "_id", 3 } },
                                   new DynamicDocument { { "_id", 4 } },
                                   new DynamicDocument { { "_id", 5 } },
                                   new DynamicDocument { { "_id", 6 } },
                                   new DynamicDocument { { "_id", 7 } },
                                   new DynamicDocument { { "_id", 8 } },
                                   new DynamicDocument { { "_id", 9 } }
                               });

            var skipResult = storage.Find().Skip(7).ToList();
            var sortAndSkipResult = storage.Find().SortByDescending("_id").Skip(7).ToList();
            var limitResult = storage.Find().Limit(3).ToList();
            var sortAndLimitResult = storage.Find().SortByDescending("_id").Limit(3).ToList();
            var skipAndLimitResult = storage.Find().Skip(3).Limit(3).ToList();
            var sortSkipAndLimitResult = storage.Find().SortByDescending("_id").Skip(3).Limit(3).ToList();

            // Then

            Assert.AreEqual(2, skipResult.Count);
            Assert.AreEqual(8, ((dynamic)skipResult[0])._id);
            Assert.AreEqual(9, ((dynamic)skipResult[1])._id);

            Assert.AreEqual(2, sortAndSkipResult.Count);
            Assert.AreEqual(2, ((dynamic)sortAndSkipResult[0])._id);
            Assert.AreEqual(1, ((dynamic)sortAndSkipResult[1])._id);

            Assert.AreEqual(3, limitResult.Count);
            Assert.AreEqual(1, ((dynamic)limitResult[0])._id);
            Assert.AreEqual(2, ((dynamic)limitResult[1])._id);
            Assert.AreEqual(3, ((dynamic)limitResult[2])._id);

            Assert.AreEqual(3, sortAndLimitResult.Count);
            Assert.AreEqual(9, ((dynamic)sortAndLimitResult[0])._id);
            Assert.AreEqual(8, ((dynamic)sortAndLimitResult[1])._id);
            Assert.AreEqual(7, ((dynamic)sortAndLimitResult[2])._id);

            Assert.AreEqual(3, skipAndLimitResult.Count);
            Assert.AreEqual(4, ((dynamic)skipAndLimitResult[0])._id);
            Assert.AreEqual(5, ((dynamic)skipAndLimitResult[1])._id);
            Assert.AreEqual(6, ((dynamic)skipAndLimitResult[2])._id);

            Assert.AreEqual(3, sortSkipAndLimitResult.Count);
            Assert.AreEqual(6, ((dynamic)sortSkipAndLimitResult[0])._id);
            Assert.AreEqual(5, ((dynamic)sortSkipAndLimitResult[1])._id);
            Assert.AreEqual(4, ((dynamic)sortSkipAndLimitResult[2])._id);
        }

        [Test]
        public void ShouldFindWithProjection()
        {
            // Given
            var storage = MongoTestHelpers.GetEmptyStorageProvider(nameof(ShouldFindWithProjection));

            // When

            storage.InsertOne(new DynamicDocument
                              {
                                  { "_id", 1 },
                                  { "type", "food" },
                                  { "item", "Super Dark Chocolate" },
                                  { "ratings", new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 } },
                                  {
                                      "classification", new DynamicDocument
                                                        {
                                                            { "dept", "grocery" },
                                                            { "category", "chocolate" }
                                                        }
                                  },
                                  {
                                      "vendor", new DynamicDocument
                                                {
                                                    {
                                                        "primary", new DynamicDocument
                                                                   {
                                                                       { "name", "Marsupial Vending Co" },
                                                                       { "address", "Wallaby Rd" },
                                                                       { "delivery", new[] { "M", "W", "F" } }
                                                                   }
                                                    },
                                                    {
                                                        "secondary", new DynamicDocument
                                                                     {
                                                                         { "name", "Intl. Chocolatiers" },
                                                                         { "address", "Cocoa Plaza" },
                                                                         { "delivery", new[] { "Sa" } }
                                                                     }
                                                    }
                                                }
                                  }
                              });

            var specifiedFieldsWithIdResult = storage.Find().Project(i => i.Include("type").Include("item")).ToList();
            var specifiedFieldsWithoutIdResult = storage.Find().Project(i => i.Include("type").Include("item").Exclude("_id")).ToList();
            var onlyExcludeSpecifiedFieldsResult = storage.Find().Project(i => i.Exclude("classification").Exclude("vendor")).ToList();
            var specificFieldsInEmbeddedDocumentsResult = storage.Find().Project(i => i.Include("classification.category")).ToList();
            var suppressSpecificFieldsInEmbeddedDocumentsResult = storage.Find().Project(i => i.Exclude("classification.category")).ToList();
            var sliceFirstCountArrayItemsResult = storage.Find().Project(i => i.Slice("ratings", 2)).ToList();
            var sliceLastCountArrayItemsResult = storage.Find().Project(i => i.Slice("ratings", -2)).ToList();
            var sliceFirstArrayItemsResult = storage.Find().Project(i => i.Slice("ratings", 5, 2)).ToList();
            var sliceLastArrayItemsResult = storage.Find().Project(i => i.Slice("ratings", -5, 2)).ToList();
            var sliceFirstCountWithOutOfRangeResult = storage.Find().Project(i => i.Slice("ratings", 100)).ToList();
            var sliceLastCountWithOutOfRangeResult = storage.Find().Project(i => i.Slice("ratings", -100)).ToList();
            var sliceFirstWithOutOfRangeResult = storage.Find().Project(i => i.Slice("ratings", 5, 100)).ToList();
            var sliceLastWithOutOfRangeResult = storage.Find().Project(i => i.Slice("ratings", -5, 100)).ToList();

            // Then

            Assert.AreEqual(1, specifiedFieldsWithIdResult.Count);
            Assert.AreEqual(1, ((dynamic)specifiedFieldsWithIdResult[0])._id);
            Assert.AreEqual("food", ((dynamic)specifiedFieldsWithIdResult[0]).type);
            Assert.AreEqual("Super Dark Chocolate", ((dynamic)specifiedFieldsWithIdResult[0]).item);

            Assert.AreEqual(1, specifiedFieldsWithoutIdResult.Count);
            Assert.AreEqual(null, ((dynamic)specifiedFieldsWithoutIdResult[0])._id);
            Assert.AreEqual("food", ((dynamic)specifiedFieldsWithoutIdResult[0]).type);
            Assert.AreEqual("Super Dark Chocolate", ((dynamic)specifiedFieldsWithoutIdResult[0]).item);

            Assert.AreEqual(1, onlyExcludeSpecifiedFieldsResult.Count);
            Assert.AreEqual(1, ((dynamic)onlyExcludeSpecifiedFieldsResult[0])._id);
            Assert.AreEqual("food", ((dynamic)onlyExcludeSpecifiedFieldsResult[0]).type);
            Assert.AreEqual("Super Dark Chocolate", ((dynamic)onlyExcludeSpecifiedFieldsResult[0]).item);
            Assert.AreEqual(null, ((dynamic)onlyExcludeSpecifiedFieldsResult[0]).classification);
            Assert.AreEqual(null, ((dynamic)onlyExcludeSpecifiedFieldsResult[0]).vendor);

            Assert.AreEqual(1, specificFieldsInEmbeddedDocumentsResult.Count);
            Assert.AreEqual(1, ((dynamic)specificFieldsInEmbeddedDocumentsResult[0])._id);
            Assert.AreEqual("chocolate", ((dynamic)specificFieldsInEmbeddedDocumentsResult[0]).classification.category);

            Assert.AreEqual(1, suppressSpecificFieldsInEmbeddedDocumentsResult.Count);
            Assert.AreEqual(1, ((dynamic)suppressSpecificFieldsInEmbeddedDocumentsResult[0])._id);
            Assert.AreEqual("grocery", ((dynamic)suppressSpecificFieldsInEmbeddedDocumentsResult[0]).classification.dept);
            Assert.AreEqual(null, ((dynamic)suppressSpecificFieldsInEmbeddedDocumentsResult[0]).classification.category);

            Assert.AreEqual(1, sliceFirstCountArrayItemsResult.Count);
            CollectionAssert.AreEqual(new[] { 1, 2 }, ((dynamic)sliceFirstCountArrayItemsResult[0]).ratings);

            Assert.AreEqual(1, sliceLastCountArrayItemsResult.Count);
            CollectionAssert.AreEqual(new[] { 8, 9 }, ((dynamic)sliceLastCountArrayItemsResult[0]).ratings);

            Assert.AreEqual(1, sliceFirstArrayItemsResult.Count);
            CollectionAssert.AreEqual(new[] { 6, 7 }, ((dynamic)sliceFirstArrayItemsResult[0]).ratings);

            Assert.AreEqual(1, sliceLastArrayItemsResult.Count);
            CollectionAssert.AreEqual(new[] { 5, 6 }, ((dynamic)sliceLastArrayItemsResult[0]).ratings);

            Assert.AreEqual(1, sliceFirstCountWithOutOfRangeResult.Count);
            CollectionAssert.AreEqual(new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 }, ((dynamic)sliceFirstCountWithOutOfRangeResult[0]).ratings);

            Assert.AreEqual(1, sliceLastCountWithOutOfRangeResult.Count);
            CollectionAssert.AreEqual(new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 }, ((dynamic)sliceLastCountWithOutOfRangeResult[0]).ratings);

            Assert.AreEqual(1, sliceFirstWithOutOfRangeResult.Count);
            CollectionAssert.AreEqual(new[] { 6, 7, 8, 9 }, ((dynamic)sliceFirstWithOutOfRangeResult[0]).ratings);

            Assert.AreEqual(1, sliceLastWithOutOfRangeResult.Count);
            CollectionAssert.AreEqual(new[] { 5, 6, 7, 8, 9 }, ((dynamic)sliceLastWithOutOfRangeResult[0]).ratings);
        }

        [Test]
        public void ShouldFindWithProjectionWithMatchArray()
        {
            // Given
            var storage = MongoTestHelpers.GetEmptyStorageProvider(nameof(ShouldFindWithProjectionWithMatchArray));

            // When

            storage.InsertMany(new[]
                               {
                                   new DynamicDocument
                                   {
                                       { "_id", 1 },
                                       { "zipcode", "63109" },
                                       {
                                           "students", new[]
                                                       {
                                                           new DynamicDocument { { "name", "john" }, { "school", 102 }, { "age", 10 } },
                                                           new DynamicDocument { { "name", "jess" }, { "school", 102 }, { "age", 11 } },
                                                           new DynamicDocument { { "name", "jeff" }, { "school", 108 }, { "age", 15 } }
                                                       }
                                       }
                                   },
                                   new DynamicDocument
                                   {
                                       { "_id", 2 },
                                       { "zipcode", "63110" },
                                       {
                                           "students", new[]
                                                       {
                                                           new DynamicDocument { { "name", "ajax" }, { "school", 100 }, { "age", 7 } },
                                                           new DynamicDocument { { "name", "achilles" }, { "school", 100 }, { "age", 8 } }
                                                       }
                                       }
                                   },
                                   new DynamicDocument
                                   {
                                       { "_id", 3 },
                                       { "zipcode", "63110" },
                                       {
                                           "students", new[]
                                                       {
                                                           new DynamicDocument { { "name", "ajax" }, { "school", 100 }, { "age", 7 } },
                                                           new DynamicDocument { { "name", "achilles" }, { "school", 100 }, { "age", 8 } }
                                                       }
                                       }
                                   },
                                   new DynamicDocument
                                   {
                                       { "_id", 4 },
                                       { "zipcode", "63109" },
                                       {
                                           "students", new[]
                                                       {
                                                           new DynamicDocument { { "name", "barney" }, { "school", 102 }, { "age", 7 } },
                                                           new DynamicDocument { { "name", "ruth" }, { "school", 102 }, { "age", 16 } }
                                                       }
                                       }
                                   }
                               });

            var simpleMatchResult = storage.Find(f => f.Eq("zipcode", "63109")).Project(p => p.Match("students", f => f.Eq("school", 102))).ToList();
            var complexMatchResult = storage.Find(f => f.Eq("zipcode", "63109")).Project(p => p.Match("students", f => f.And(f.Eq("school", 102), f.Gt("age", 10)))).ToList();

            // Then

            Assert.AreEqual(2, simpleMatchResult.Count);
            Assert.AreEqual(1, ((dynamic)simpleMatchResult[0])._id);
            Assert.AreNotEqual(null, ((dynamic)simpleMatchResult[0]).students);
            Assert.AreEqual(1, ((dynamic)simpleMatchResult[0]).students.Count);
            Assert.AreEqual("john", ((dynamic)simpleMatchResult[0]).students[0].name);
            Assert.AreEqual(102, ((dynamic)simpleMatchResult[0]).students[0].school);
            Assert.AreEqual(10, ((dynamic)simpleMatchResult[0]).students[0].age);
            Assert.AreEqual(4, ((dynamic)simpleMatchResult[1])._id);
            Assert.AreNotEqual(null, ((dynamic)simpleMatchResult[1]).students);
            Assert.AreEqual(1, ((dynamic)simpleMatchResult[1]).students.Count);
            Assert.AreEqual("barney", ((dynamic)simpleMatchResult[1]).students[0].name);
            Assert.AreEqual(102, ((dynamic)simpleMatchResult[1]).students[0].school);
            Assert.AreEqual(7, ((dynamic)simpleMatchResult[1]).students[0].age);

            Assert.AreEqual(2, complexMatchResult.Count);
            Assert.AreEqual(1, ((dynamic)complexMatchResult[0])._id);
            Assert.AreNotEqual(null, ((dynamic)complexMatchResult[0]).students);
            Assert.AreEqual(1, ((dynamic)complexMatchResult[0]).students.Count);
            Assert.AreEqual("jess", ((dynamic)complexMatchResult[0]).students[0].name);
            Assert.AreEqual(102, ((dynamic)complexMatchResult[0]).students[0].school);
            Assert.AreEqual(11, ((dynamic)complexMatchResult[0]).students[0].age);
            Assert.AreEqual(4, ((dynamic)complexMatchResult[1])._id);
            Assert.AreNotEqual(null, ((dynamic)complexMatchResult[1]).students);
            Assert.AreEqual(1, ((dynamic)complexMatchResult[1]).students.Count);
            Assert.AreEqual("ruth", ((dynamic)complexMatchResult[1]).students[0].name);
            Assert.AreEqual(102, ((dynamic)complexMatchResult[1]).students[0].school);
            Assert.AreEqual(16, ((dynamic)complexMatchResult[1]).students[0].age);
        }

        [Test]
        public void ShouldInsertOne()
        {
            // Given
            var storage = MongoTestHelpers.GetEmptyStorageProvider(nameof(ShouldInsertOne));

            // When
            storage.InsertOne(new DynamicDocument());
            var result = storage.Count();

            // Then

            Assert.AreEqual(1, result);
        }

        [Test]
        public async Task ShouldInsertOneAsync()
        {
            // Given
            var storage = MongoTestHelpers.GetEmptyStorageProvider(nameof(ShouldInsertOneAsync));

            // When
            await storage.InsertOneAsync(new DynamicDocument());
            var result = storage.Count();

            // Then

            Assert.AreEqual(1, result);
        }

        [Test]
        public void ShouldInsertMany()
        {
            // Given
            var storage = MongoTestHelpers.GetEmptyStorageProvider(nameof(ShouldInsertMany));

            // When
            storage.InsertMany(new[] { new DynamicDocument(), new DynamicDocument() });
            var result = storage.Count();

            // Then

            Assert.AreEqual(2, result);
        }

        [Test]
        public async Task ShouldInsertManyAsync()
        {
            // Given
            var storage = MongoTestHelpers.GetEmptyStorageProvider(nameof(ShouldInsertManyAsync));

            // When
            await storage.InsertManyAsync(new[] { new DynamicDocument(), new DynamicDocument() });
            var result = storage.Count();

            // Then

            Assert.AreEqual(2, result);
        }

        [Test]
        public void ShouldInsertDocumentWithDecimalField()
        {
            // Given

            const decimal value = 1234.56789m;

            var storage = MongoTestHelpers.GetEmptyStorageProvider(nameof(ShouldInsertDocumentWithDecimalField));

            // When

            storage.InsertOne(new DynamicDocument
                              {
                                  { "_id", 1 },
                                  { "value", value }
                              });

            var document = storage.Find(f => f.Eq("_id", 1)).FirstOrDefault();

            // Then
            Assert.AreEqual(value, document["value"]);
        }

        [Test]
        public void ShouldUpdateOne()
        {
            // Given
            var storage = MongoTestHelpers.GetEmptyStorageProvider(nameof(ShouldUpdateOne));

            // When
            storage.InsertMany(new[]
                               {
                                   new DynamicDocument { { "_id", 1 }, { "prop1", 11 } },
                                   new DynamicDocument { { "_id", 2 }, { "prop1", 22 } },
                                   new DynamicDocument { { "_id", 3 }, { "prop1", 33 } }
                               });

            var updateResult1 = storage.UpdateOne(u => u.Set("prop1", 44));
            var updateResult2 = storage.UpdateOne(u => u.Set("prop1", 55), f => f.Eq("_id", 3));
            var updateResult3 = storage.UpdateOne(u => u.Set("prop1", 66), f => f.Eq("_id", 4));
            var updateResult4 = storage.UpdateOne(u => u.Set("prop1", 77), f => f.Eq("_id", 5), true);

            var documents = storage.Find().ToList();

            // Then

            Assert.AreEqual(1, updateResult1.MatchedCount);
            Assert.AreEqual(1, updateResult1.ModifiedCount);
            Assert.AreEqual(DocumentUpdateStatus.Updated, updateResult1.UpdateStatus);

            Assert.AreEqual(1, updateResult2.MatchedCount);
            Assert.AreEqual(1, updateResult2.ModifiedCount);
            Assert.AreEqual(DocumentUpdateStatus.Updated, updateResult2.UpdateStatus);

            Assert.AreEqual(0, updateResult3.MatchedCount);
            Assert.AreEqual(0, updateResult3.ModifiedCount);
            Assert.AreEqual(DocumentUpdateStatus.None, updateResult3.UpdateStatus);

            Assert.AreEqual(0, updateResult4.MatchedCount);
            Assert.AreEqual(0, updateResult4.ModifiedCount);
            Assert.AreEqual(DocumentUpdateStatus.Inserted, updateResult4.UpdateStatus);

            Assert.AreEqual(4, documents.Count);
            Assert.AreEqual(44, ((dynamic)documents[0]).prop1);
            Assert.AreEqual(22, ((dynamic)documents[1]).prop1);
            Assert.AreEqual(55, ((dynamic)documents[2]).prop1);
            Assert.AreEqual(5, ((dynamic)documents[3])._id);
            Assert.AreEqual(77, ((dynamic)documents[3]).prop1);
        }

        [Test]
        public async Task ShouldUpdateOneAsync()
        {
            // Given
            var storage = MongoTestHelpers.GetEmptyStorageProvider(nameof(ShouldUpdateOneAsync));

            // When
            storage.InsertMany(new[]
                               {
                                   new DynamicDocument { { "_id", 1 }, { "prop1", 11 } },
                                   new DynamicDocument { { "_id", 2 }, { "prop1", 22 } },
                                   new DynamicDocument { { "_id", 3 }, { "prop1", 33 } }
                               });

            var updateResult1 = await storage.UpdateOneAsync(u => u.Set("prop1", 44));
            var updateResult2 = await storage.UpdateOneAsync(u => u.Set("prop1", 55), f => f.Eq("_id", 3));
            var updateResult3 = await storage.UpdateOneAsync(u => u.Set("prop1", 66), f => f.Eq("_id", 4));
            var updateResult4 = await storage.UpdateOneAsync(u => u.Set("prop1", 77), f => f.Eq("_id", 5), true);

            var documents = storage.Find().ToList();

            // Then

            Assert.AreEqual(1, updateResult1.MatchedCount);
            Assert.AreEqual(1, updateResult1.ModifiedCount);
            Assert.AreEqual(DocumentUpdateStatus.Updated, updateResult1.UpdateStatus);

            Assert.AreEqual(1, updateResult2.MatchedCount);
            Assert.AreEqual(1, updateResult2.ModifiedCount);
            Assert.AreEqual(DocumentUpdateStatus.Updated, updateResult2.UpdateStatus);

            Assert.AreEqual(0, updateResult3.MatchedCount);
            Assert.AreEqual(0, updateResult3.ModifiedCount);
            Assert.AreEqual(DocumentUpdateStatus.None, updateResult3.UpdateStatus);

            Assert.AreEqual(0, updateResult4.MatchedCount);
            Assert.AreEqual(0, updateResult4.ModifiedCount);
            Assert.AreEqual(DocumentUpdateStatus.Inserted, updateResult4.UpdateStatus);

            Assert.AreEqual(4, documents.Count);
            Assert.AreEqual(44, ((dynamic)documents[0]).prop1);
            Assert.AreEqual(22, ((dynamic)documents[1]).prop1);
            Assert.AreEqual(55, ((dynamic)documents[2]).prop1);
            Assert.AreEqual(5, ((dynamic)documents[3])._id);
            Assert.AreEqual(77, ((dynamic)documents[3]).prop1);
        }

        [Test]
        public void ShouldUpdateOneWhenDocumentIsNotExists()
        {
            // Given
            object id = Guid.NewGuid().ToString("N");
            var storage = MongoTestHelpers.GetEmptyStorageProvider(nameof(ShouldUpdateOneWhenDocumentIsNotExists));

            // When
            storage.UpdateOne(u => u.Set("_id", id), f => f.Eq("_id", id), true);
            var document = storage.Find(f => f.Eq("_id", id)).FirstOrDefault();

            // Then
            Assert.IsNotNull(document);
            Assert.AreEqual(id, document["_id"]);
        }

        [Test]
        public async Task ShouldUpdateOneAsynWhenDocumentIsNotExists()
        {
            // Given
            object id = Guid.NewGuid().ToString("N");
            var storage = MongoTestHelpers.GetEmptyStorageProvider(nameof(ShouldUpdateOneWhenDocumentIsNotExists));

            // When
            await storage.UpdateOneAsync(u => u.Set("_id", id), f => f.Eq("_id", id), true);
            var document = await storage.Find(f => f.Eq("_id", id)).FirstOrDefaultAsync();

            // Then
            Assert.IsNotNull(document);
            Assert.AreEqual(id, document["_id"]);
        }

        [Test]
        public void ShoudlUpdateMany()
        {
            // Given
            var storage = MongoTestHelpers.GetEmptyStorageProvider(nameof(ShoudlUpdateMany));

            // When
            storage.InsertMany(new[]
                               {
                                   new DynamicDocument { { "_id", 1 }, { "prop1", 11 }, { "prop2", 111 } },
                                   new DynamicDocument { { "_id", 2 }, { "prop1", 22 }, { "prop2", 111 } },
                                   new DynamicDocument { { "_id", 3 }, { "prop1", 33 }, { "prop2", 222 } }
                               });

            var updateResult1 = storage.UpdateMany(u => u.Set("prop1", 44));
            var updateResult2 = storage.UpdateMany(u => u.Set("prop2", 333), f => f.Eq("prop2", 111));
            var updateResult3 = storage.UpdateMany(u => u.Set("prop2", 444), f => f.Eq("_id", 4));
            var updateResult4 = storage.UpdateMany(u => u.Set("prop2", 555), f => f.And(f.Eq("_id", 5), f.Eq("prop1", 55)), true);

            var documents = storage.Find().ToList();

            // Then

            Assert.AreEqual(3, updateResult1.MatchedCount);
            Assert.AreEqual(3, updateResult1.ModifiedCount);
            Assert.AreEqual(DocumentUpdateStatus.Updated, updateResult1.UpdateStatus);

            Assert.AreEqual(2, updateResult2.MatchedCount);
            Assert.AreEqual(2, updateResult2.ModifiedCount);
            Assert.AreEqual(DocumentUpdateStatus.Updated, updateResult2.UpdateStatus);

            Assert.AreEqual(0, updateResult3.MatchedCount);
            Assert.AreEqual(0, updateResult3.ModifiedCount);
            Assert.AreEqual(DocumentUpdateStatus.None, updateResult3.UpdateStatus);

            Assert.AreEqual(0, updateResult4.MatchedCount);
            Assert.AreEqual(0, updateResult4.ModifiedCount);
            Assert.AreEqual(DocumentUpdateStatus.Inserted, updateResult4.UpdateStatus);

            Assert.AreEqual(4, documents.Count);

            Assert.AreEqual(1, ((dynamic)documents[0])._id);
            Assert.AreEqual(44, ((dynamic)documents[0]).prop1);
            Assert.AreEqual(333, ((dynamic)documents[0]).prop2);

            Assert.AreEqual(2, ((dynamic)documents[1])._id);
            Assert.AreEqual(44, ((dynamic)documents[1]).prop1);
            Assert.AreEqual(333, ((dynamic)documents[1]).prop2);

            Assert.AreEqual(3, ((dynamic)documents[2])._id);
            Assert.AreEqual(44, ((dynamic)documents[2]).prop1);
            Assert.AreEqual(222, ((dynamic)documents[2]).prop2);

            Assert.AreEqual(5, ((dynamic)documents[3])._id);
            Assert.AreEqual(55, ((dynamic)documents[3]).prop1);
            Assert.AreEqual(555, ((dynamic)documents[3]).prop2);
        }

        [Test]
        public async Task ShoudlUpdateManyAsync()
        {
            // Given
            var storage = MongoTestHelpers.GetEmptyStorageProvider(nameof(ShoudlUpdateManyAsync));

            // When
            storage.InsertMany(new[]
                               {
                                   new DynamicDocument { { "_id", 1 }, { "prop1", 11 }, { "prop2", 111 } },
                                   new DynamicDocument { { "_id", 2 }, { "prop1", 22 }, { "prop2", 111 } },
                                   new DynamicDocument { { "_id", 3 }, { "prop1", 33 }, { "prop2", 222 } }
                               });

            var updateResult1 = await storage.UpdateManyAsync(u => u.Set("prop1", 44));
            var updateResult2 = await storage.UpdateManyAsync(u => u.Set("prop2", 333), f => f.Eq("prop2", 111));
            var updateResult3 = await storage.UpdateManyAsync(u => u.Set("prop2", 444), f => f.Eq("_id", 4));
            var updateResult4 = await storage.UpdateManyAsync(u => u.Set("prop2", 555), f => f.And(f.Eq("_id", 5), f.Eq("prop1", 55)), true);

            var documents = storage.Find().ToList();

            // Then

            Assert.AreEqual(3, updateResult1.MatchedCount);
            Assert.AreEqual(3, updateResult1.ModifiedCount);
            Assert.AreEqual(DocumentUpdateStatus.Updated, updateResult1.UpdateStatus);

            Assert.AreEqual(2, updateResult2.MatchedCount);
            Assert.AreEqual(2, updateResult2.ModifiedCount);
            Assert.AreEqual(DocumentUpdateStatus.Updated, updateResult2.UpdateStatus);

            Assert.AreEqual(0, updateResult3.MatchedCount);
            Assert.AreEqual(0, updateResult3.ModifiedCount);
            Assert.AreEqual(DocumentUpdateStatus.None, updateResult3.UpdateStatus);

            Assert.AreEqual(0, updateResult4.MatchedCount);
            Assert.AreEqual(0, updateResult4.ModifiedCount);
            Assert.AreEqual(DocumentUpdateStatus.Inserted, updateResult4.UpdateStatus);

            Assert.AreEqual(4, documents.Count);

            Assert.AreEqual(1, ((dynamic)documents[0])._id);
            Assert.AreEqual(44, ((dynamic)documents[0]).prop1);
            Assert.AreEqual(333, ((dynamic)documents[0]).prop2);

            Assert.AreEqual(2, ((dynamic)documents[1])._id);
            Assert.AreEqual(44, ((dynamic)documents[1]).prop1);
            Assert.AreEqual(333, ((dynamic)documents[1]).prop2);

            Assert.AreEqual(3, ((dynamic)documents[2])._id);
            Assert.AreEqual(44, ((dynamic)documents[2]).prop1);
            Assert.AreEqual(222, ((dynamic)documents[2]).prop2);

            Assert.AreEqual(5, ((dynamic)documents[3])._id);
            Assert.AreEqual(55, ((dynamic)documents[3]).prop1);
            Assert.AreEqual(555, ((dynamic)documents[3]).prop2);
        }

        [Test]
        public void ShouldUpdateWithRename()
        {
            // Given
            var storage = MongoTestHelpers.GetEmptyStorageProvider(nameof(ShouldUpdateWithRename));

            // When

            storage.InsertOne(new DynamicDocument { { "prop1", 123 } });
            storage.UpdateOne(u => u.Rename("prop1", "prop2"));
            dynamic document = storage.Find().First();

            // Then
            Assert.AreEqual(123, document.prop2);
        }

        [Test]
        public void ShouldUpdateWithRemove()
        {
            // Given
            var storage = MongoTestHelpers.GetEmptyStorageProvider(nameof(ShouldUpdateWithRemove));

            // When

            storage.InsertOne(new DynamicDocument { { "prop1", 123 } });
            storage.UpdateOne(u => u.Remove("prop1"));
            dynamic document = storage.Find().First();

            // Then
            Assert.AreEqual(null, document.prop1);
        }

        [Test]
        public void ShouldUpdateWithSet()
        {
            // Given
            var storage = MongoTestHelpers.GetEmptyStorageProvider(nameof(ShouldUpdateWithSet));

            // When

            storage.InsertOne(new DynamicDocument());
            storage.UpdateOne(u => u.Set("prop1", 123));
            dynamic document = storage.Find().First();

            // Then
            Assert.AreEqual(123, document.prop1);
        }

        [Test]
        public void ShouldUpdateWithInc()
        {
            // Given
            var storage = MongoTestHelpers.GetEmptyStorageProvider(nameof(ShouldUpdateWithInc));

            // When

            storage.InsertOne(new DynamicDocument { { "intProp1", 1 }, { "intProp2", 2 }, { "doubleProp1", 1.5 }, { "doubleProp2", 2.5 } });
            storage.UpdateOne(u => u.Inc("intProp1", 3).Inc("intProp2", -3).Inc("doubleProp1", 3.3).Inc("doubleProp2", -3.3));
            dynamic document = storage.Find().First();

            // Then
            Assert.AreEqual(4, document.intProp1);
            Assert.AreEqual(-1, document.intProp2);
            Assert.AreEqual(4.8, document.doubleProp1, 0.0001);
            Assert.AreEqual(-0.8, document.doubleProp2, 0.0001);
        }

        [Test]
        public void ShouldUpdateWithMul()
        {
            // Given
            var storage = MongoTestHelpers.GetEmptyStorageProvider(nameof(ShouldUpdateWithMul));

            // When

            storage.InsertOne(new DynamicDocument { { "intProp1", 2 }, { "intProp2", 3 }, { "doubleProp1", 1.5 }, { "doubleProp2", 2.5 } });
            storage.UpdateOne(u => u.Mul("intProp1", 4).Mul("intProp2", -5).Mul("doubleProp1", 3.3).Mul("doubleProp2", -3.3));
            dynamic document = storage.Find().First();

            // Then
            Assert.AreEqual(8, document.intProp1);
            Assert.AreEqual(-15, document.intProp2);
            Assert.AreEqual(4.95, document.doubleProp1, 0.0001);
            Assert.AreEqual(-8.25, document.doubleProp2, 0.0001);
        }

        [Test]
        public void ShouldUpdateWithMin()
        {
            // Given
            var storage = MongoTestHelpers.GetEmptyStorageProvider(nameof(ShouldUpdateWithMin));

            // When

            storage.InsertOne(new DynamicDocument { { "intProp1", 2 }, { "intProp2", 2 }, { "doubleProp1", 1.5 }, { "doubleProp2", 1.5 } });
            storage.UpdateOne(u => u.Min("intProp1", 1).Min("intProp2", 3).Min("doubleProp1", 1.3).Min("doubleProp2", 1.7));
            dynamic document = storage.Find().First();

            // Then
            Assert.AreEqual(1, document.intProp1);
            Assert.AreEqual(2, document.intProp2);
            Assert.AreEqual(1.3, document.doubleProp1, 0.0001);
            Assert.AreEqual(1.5, document.doubleProp2, 0.0001);
        }

        [Test]
        public void ShouldUpdateWithMax()
        {
            // Given
            var storage = MongoTestHelpers.GetEmptyStorageProvider(nameof(ShouldUpdateWithMax));

            // When

            storage.InsertOne(new DynamicDocument { { "intProp1", 2 }, { "intProp2", 2 }, { "doubleProp1", 1.5 }, { "doubleProp2", 1.5 } });
            storage.UpdateOne(u => u.Max("intProp1", 1).Max("intProp2", 3).Max("doubleProp1", 1.3).Max("doubleProp2", 1.7));
            dynamic document = storage.Find().First();

            // Then
            Assert.AreEqual(2, document.intProp1);
            Assert.AreEqual(3, document.intProp2);
            Assert.AreEqual(1.5, document.doubleProp1, 0.0001);
            Assert.AreEqual(1.7, document.doubleProp2, 0.0001);
        }

        [Test]
        public void ShouldUpdateWithBitwiseOperations()
        {
            // Given
            var storage = MongoTestHelpers.GetEmptyStorageProvider(nameof(ShouldUpdateWithBitwiseOperations));

            // When

            storage.InsertOne(new DynamicDocument { { "intProp1", 0x1101 }, { "intProp2", 0x0011 }, { "intProp3", 0x0001 } });
            storage.UpdateOne(u => u.BitwiseAnd("intProp1", 0x1010).BitwiseOr("intProp2", 0x0101).BitwiseXor("intProp3", 0x0101));
            dynamic document = storage.Find().First();

            // Then
            Assert.AreEqual(0x1000, document.intProp1);
            Assert.AreEqual(0x0111, document.intProp2);
            Assert.AreEqual(0x0100, document.intProp3);
        }

        [Test]
        public void ShouldUpdateWithCurrentDate()
        {
            // Given
            var nowDate = DateTime.UtcNow;
            var storage = MongoTestHelpers.GetEmptyStorageProvider(nameof(ShouldUpdateWithCurrentDate));

            // When
            storage.InsertOne(new DynamicDocument());
            storage.UpdateOne(u => u.CurrentDate("date"));
            dynamic document = storage.Find().First();

            // Then
            Assert.IsInstanceOf<DateTime>(document.date);
            Assert.IsTrue(Math.Abs((nowDate - (DateTime)document.date).TotalSeconds) < 30);
        }

        [Test]
        public void ShouldUpdateWithPush()
        {
            // Given
            var storage = MongoTestHelpers.GetEmptyStorageProvider(nameof(ShouldUpdateWithPush));

            // When

            storage.InsertOne(new DynamicDocument());

            storage.UpdateOne(u => u.Push("propPush", 1).PushAll("propPushAll", new[] { 11, 22 }).PushUnique("propPushUnique", 111).PushAllUnique("propPushAllUnique", new[] { 1111, 2222 }));
            storage.UpdateOne(u => u.Push("propPush", 1).PushAll("propPushAll", new[] { 22, 33 }).PushUnique("propPushUnique", 111).PushAllUnique("propPushAllUnique", new[] { 2222, 3333 }));
            storage.UpdateOne(u => u.Push("propPush", 2).PushAll("propPushAll", new[] { 33, 44 }).PushUnique("propPushUnique", 222).PushAllUnique("propPushAllUnique", new[] { 3333, 4444 }));
            storage.UpdateOne(u => u.Push("propPush", 2).PushAll("propPushAll", new[] { 44, 55 }).PushUnique("propPushUnique", 222).PushAllUnique("propPushAllUnique", new[] { 4444, 5555 }));

            dynamic document = storage.Find().First();

            // Then
            CollectionAssert.AreEqual(new[] { 1, 1, 2, 2 }, document.propPush);
            CollectionAssert.AreEqual(new[] { 11, 22, 22, 33, 33, 44, 44, 55 }, document.propPushAll);
            CollectionAssert.AreEqual(new[] { 111, 222 }, document.propPushUnique);
            CollectionAssert.AreEqual(new[] { 1111, 2222, 3333, 4444, 5555 }, document.propPushAllUnique);
        }

        [Test]
        public void ShouldUpdateWithPopAndPull()
        {
            // Given
            var storage = MongoTestHelpers.GetEmptyStorageProvider(nameof(ShouldUpdateWithPopAndPull));

            // When

            storage.InsertOne(new DynamicDocument
                              {
                                  { "propPopFirst", new[] { 1, 2, 3, 4 } },
                                  { "propPopLast", new[] { 5, 6, 7, 8 } },
                                  { "propPull", new[] { 11, 22, 33 } },
                                  { "propPullAll", new[] { 44, 55, 66 } },
                                  { "propPullFilter", new[] { new DynamicDocument { { "value", 111 } }, new DynamicDocument { { "value", 111 } }, new DynamicDocument { { "value", 222 } } } }
                              });

            storage.UpdateOne(u => u.PopFirst("propPopFirst")
                                    .PopLast("propPopLast")
                                    .Pull("propPull", 22)
                                    .PullAll("propPullAll", new[] { 44, 66 })
                                    .PullFilter("propPullFilter", f => f.Eq("value", 111)));

            dynamic document = storage.Find().First();

            // Then
            CollectionAssert.AreEqual(new[] { 2, 3, 4 }, document.propPopFirst);
            CollectionAssert.AreEqual(new[] { 5, 6, 7 }, document.propPopLast);
            CollectionAssert.AreEqual(new[] { 11, 33 }, document.propPull);
            CollectionAssert.AreEqual(new[] { 55 }, document.propPullAll);
            Assert.AreEqual(1, document.propPullFilter.Count);
            Assert.AreEqual(222, document.propPullFilter[0].value);
        }

        [Test]
        public void ShouldReplaceOne()
        {
            // Given
            var storage = MongoTestHelpers.GetEmptyStorageProvider(nameof(ShouldReplaceOne));

            // When

            storage.InsertMany(new[]
                               {
                                   new DynamicDocument { { "_id", 1 } },
                                   new DynamicDocument { { "_id", 2 }, { "prop1", 22 }, { "prop2", "A" } },
                                   new DynamicDocument { { "_id", 3 }, { "prop1", 33 }, { "prop2", "A" } }
                               });

            var replaceResult1 = storage.ReplaceOne(new DynamicDocument { { "_id", 1 }, { "prop1", 11 } });
            var replaceResult2 = storage.ReplaceOne(new DynamicDocument { { "prop1", 22 }, { "prop2", "B" } }, f => f.Eq("prop2", "A"));
            var replaceResult3 = storage.ReplaceOne(new DynamicDocument { { "prop1", 44 }, { "prop2", "C" } }, f => f.Eq("_id", 4));
            var replaceResult4 = storage.ReplaceOne(new DynamicDocument { { "prop1", 55 }, { "prop2", "D" } }, f => f.Eq("_id", 5), true);

            var documents = storage.Find().ToList();

            // Then

            Assert.AreEqual(1, replaceResult1.MatchedCount);
            Assert.AreEqual(1, replaceResult1.ModifiedCount);
            Assert.AreEqual(DocumentUpdateStatus.Updated, replaceResult1.UpdateStatus);

            Assert.AreEqual(1, replaceResult2.MatchedCount);
            Assert.AreEqual(1, replaceResult2.ModifiedCount);
            Assert.AreEqual(DocumentUpdateStatus.Updated, replaceResult2.UpdateStatus);

            Assert.AreEqual(0, replaceResult3.MatchedCount);
            Assert.AreEqual(0, replaceResult3.ModifiedCount);
            Assert.AreEqual(DocumentUpdateStatus.None, replaceResult3.UpdateStatus);

            Assert.AreEqual(0, replaceResult4.MatchedCount);
            Assert.AreEqual(0, replaceResult4.ModifiedCount);
            Assert.AreEqual(DocumentUpdateStatus.Inserted, replaceResult4.UpdateStatus);

            Assert.AreEqual(4, documents.Count);

            Assert.AreEqual(1, ((dynamic)documents[0])._id);
            Assert.AreEqual(11, ((dynamic)documents[0]).prop1);
            Assert.AreEqual(null, ((dynamic)documents[0]).prop2);

            Assert.AreEqual(2, ((dynamic)documents[1])._id);
            Assert.AreEqual(22, ((dynamic)documents[1]).prop1);
            Assert.AreEqual("B", ((dynamic)documents[1]).prop2);

            Assert.AreEqual(3, ((dynamic)documents[2])._id);
            Assert.AreEqual(33, ((dynamic)documents[2]).prop1);
            Assert.AreEqual("A", ((dynamic)documents[2]).prop2);

            Assert.AreEqual(5, ((dynamic)documents[3])._id);
            Assert.AreEqual(55, ((dynamic)documents[3]).prop1);
            Assert.AreEqual("D", ((dynamic)documents[3]).prop2);
        }

        [Test]
        public async Task ShouldReplaceOneAsync()
        {
            // Given
            var storage = MongoTestHelpers.GetEmptyStorageProvider(nameof(ShouldReplaceOneAsync));

            // When

            storage.InsertMany(new[]
                               {
                                   new DynamicDocument { { "_id", 1 } },
                                   new DynamicDocument { { "_id", 2 }, { "prop1", 22 }, { "prop2", "A" } },
                                   new DynamicDocument { { "_id", 3 }, { "prop1", 33 }, { "prop2", "A" } }
                               });

            var replaceResult1 = await storage.ReplaceOneAsync(new DynamicDocument { { "_id", 1 }, { "prop1", 11 } });
            var replaceResult2 = await storage.ReplaceOneAsync(new DynamicDocument { { "prop1", 22 }, { "prop2", "B" } }, f => f.Eq("prop2", "A"));
            var replaceResult3 = await storage.ReplaceOneAsync(new DynamicDocument { { "prop1", 44 }, { "prop2", "C" } }, f => f.Eq("_id", 4));
            var replaceResult4 = await storage.ReplaceOneAsync(new DynamicDocument { { "prop1", 55 }, { "prop2", "D" } }, f => f.Eq("_id", 5), true);

            var documents = storage.Find().ToList();

            // Then

            Assert.AreEqual(1, replaceResult1.MatchedCount);
            Assert.AreEqual(1, replaceResult1.ModifiedCount);
            Assert.AreEqual(DocumentUpdateStatus.Updated, replaceResult1.UpdateStatus);

            Assert.AreEqual(1, replaceResult2.MatchedCount);
            Assert.AreEqual(1, replaceResult2.ModifiedCount);
            Assert.AreEqual(DocumentUpdateStatus.Updated, replaceResult2.UpdateStatus);

            Assert.AreEqual(0, replaceResult3.MatchedCount);
            Assert.AreEqual(0, replaceResult3.ModifiedCount);
            Assert.AreEqual(DocumentUpdateStatus.None, replaceResult3.UpdateStatus);

            Assert.AreEqual(0, replaceResult4.MatchedCount);
            Assert.AreEqual(0, replaceResult4.ModifiedCount);
            Assert.AreEqual(DocumentUpdateStatus.Inserted, replaceResult4.UpdateStatus);

            Assert.AreEqual(4, documents.Count);

            Assert.AreEqual(1, ((dynamic)documents[0])._id);
            Assert.AreEqual(11, ((dynamic)documents[0]).prop1);
            Assert.AreEqual(null, ((dynamic)documents[0]).prop2);

            Assert.AreEqual(2, ((dynamic)documents[1])._id);
            Assert.AreEqual(22, ((dynamic)documents[1]).prop1);
            Assert.AreEqual("B", ((dynamic)documents[1]).prop2);

            Assert.AreEqual(3, ((dynamic)documents[2])._id);
            Assert.AreEqual(33, ((dynamic)documents[2]).prop1);
            Assert.AreEqual("A", ((dynamic)documents[2]).prop2);

            Assert.AreEqual(5, ((dynamic)documents[3])._id);
            Assert.AreEqual(55, ((dynamic)documents[3]).prop1);
            Assert.AreEqual("D", ((dynamic)documents[3]).prop2);
        }

        [Test]
        public void ShouldReplacePropertyType()
        {
            // Given
            var storage = MongoTestHelpers.GetEmptyStorageProvider(nameof(ShouldReplacePropertyType));

            // When

            storage.InsertOne(new DynamicDocument { { "_id", 1 }, { "prop1", "123" /* string */ } });
            var beforeReplace = storage.Find(f => f.Eq("_id", 1)).FirstOrDefault();

            storage.ReplaceOne(new DynamicDocument { { "_id", 1 }, { "prop1", 123 /* int */ } }, f => f.Eq("_id", 1), true);
            var afterReplace = storage.Find(f => f.Eq("_id", 1)).FirstOrDefault();

            // Then

            Assert.IsNotNull(beforeReplace);
            Assert.IsInstanceOf<string>(beforeReplace["prop1"]);
            Assert.AreEqual("123", beforeReplace["prop1"]);

            Assert.IsNotNull(afterReplace);
            Assert.IsInstanceOf<int>(afterReplace["prop1"]);
            Assert.AreEqual(123, afterReplace["prop1"]);
        }

        [Test]
        public void ShouldDeleteOne()
        {
            // Given
            var storage = MongoTestHelpers.GetEmptyStorageProvider(nameof(ShouldDeleteOne));

            // When

            storage.InsertMany(new[]
                               {
                                   new DynamicDocument { { "_id", 1 }, { "prop1", 11 } }, // first delete
                                   new DynamicDocument { { "_id", 2 }, { "prop1", 11 } },
                                   new DynamicDocument { { "_id", 3 }, { "prop1", 22 } }, // second delete
                                   new DynamicDocument { { "_id", 4 }, { "prop1", 22 } },
                                   new DynamicDocument { { "_id", 5 }, { "prop1", 33 } }
                               });

            var deleteResult1 = storage.DeleteOne();
            var deleteResult2 = storage.DeleteOne(f => f.Eq("prop1", 22));
            var deleteResult3 = storage.DeleteOne(f => f.Eq("prop1", 44));

            var documents = storage.Find().ToList();

            // Then

            Assert.AreEqual(1, deleteResult1);
            Assert.AreEqual(1, deleteResult2);
            Assert.AreEqual(0, deleteResult3);

            Assert.AreEqual(3, documents.Count);
            Assert.AreEqual(2, ((dynamic)documents[0])._id);
            Assert.AreEqual(4, ((dynamic)documents[1])._id);
            Assert.AreEqual(5, ((dynamic)documents[2])._id);
        }

        [Test]
        public async Task ShouldDeleteOneAsync()
        {
            // Given
            var storage = MongoTestHelpers.GetEmptyStorageProvider(nameof(ShouldDeleteOneAsync));

            // When

            storage.InsertMany(new[]
                               {
                                   new DynamicDocument { { "_id", 1 }, { "prop1", 11 } }, // first delete
                                   new DynamicDocument { { "_id", 2 }, { "prop1", 11 } },
                                   new DynamicDocument { { "_id", 3 }, { "prop1", 22 } }, // second delete
                                   new DynamicDocument { { "_id", 4 }, { "prop1", 22 } },
                                   new DynamicDocument { { "_id", 5 }, { "prop1", 33 } }
                               });

            var deleteResult1 = await storage.DeleteOneAsync();
            var deleteResult2 = await storage.DeleteOneAsync(f => f.Eq("prop1", 22));
            var deleteResult3 = await storage.DeleteOneAsync(f => f.Eq("prop1", 44));

            var documents = storage.Find().ToList();

            // Then

            Assert.AreEqual(1, deleteResult1);
            Assert.AreEqual(1, deleteResult2);
            Assert.AreEqual(0, deleteResult3);

            Assert.AreEqual(3, documents.Count);
            Assert.AreEqual(2, ((dynamic)documents[0])._id);
            Assert.AreEqual(4, ((dynamic)documents[1])._id);
            Assert.AreEqual(5, ((dynamic)documents[2])._id);
        }

        [Test]
        public void ShouldDeleteMany()
        {
            // Given
            var storage = MongoTestHelpers.GetEmptyStorageProvider(nameof(ShouldDeleteMany));

            // When

            storage.InsertMany(new[]
                               {
                                   new DynamicDocument { { "_id", 1 }, { "prop1", 11 } },
                                   new DynamicDocument { { "_id", 2 }, { "prop1", 11 } },
                                   new DynamicDocument { { "_id", 3 }, { "prop1", 22 } },
                                   new DynamicDocument { { "_id", 4 }, { "prop1", 22 } },
                                   new DynamicDocument { { "_id", 5 }, { "prop1", 33 } }
                               });

            var deleteResult1 = storage.DeleteMany();

            storage.InsertMany(new[]
                               {
                                   new DynamicDocument { { "_id", 1 }, { "prop1", 11 } }, // second delete
                                   new DynamicDocument { { "_id", 2 }, { "prop1", 11 } }, // second delete
                                   new DynamicDocument { { "_id", 3 }, { "prop1", 22 } }, // third delete
                                   new DynamicDocument { { "_id", 4 }, { "prop1", 22 } }, // third delete
                                   new DynamicDocument { { "_id", 5 }, { "prop1", 33 } }
                               });

            var deleteResult2 = storage.DeleteMany(f => f.Eq("prop1", 11));
            var deleteResult3 = storage.DeleteMany(f => f.Eq("prop1", 22));
            var deleteResult4 = storage.DeleteMany(f => f.Eq("prop1", 44));

            var documents = storage.Find().ToList();

            // Then

            Assert.AreEqual(5, deleteResult1);
            Assert.AreEqual(2, deleteResult2);
            Assert.AreEqual(2, deleteResult3);
            Assert.AreEqual(0, deleteResult4);

            Assert.AreEqual(1, documents.Count);
            Assert.AreEqual(5, ((dynamic)documents[0])._id);
        }

        [Test]
        public async Task ShouldDeleteManyAsync()
        {
            // Given
            var storage = MongoTestHelpers.GetEmptyStorageProvider(nameof(ShouldDeleteManyAsync));

            // When

            storage.InsertMany(new[]
                               {
                                   new DynamicDocument { { "_id", 1 }, { "prop1", 11 } },
                                   new DynamicDocument { { "_id", 2 }, { "prop1", 11 } },
                                   new DynamicDocument { { "_id", 3 }, { "prop1", 22 } },
                                   new DynamicDocument { { "_id", 4 }, { "prop1", 22 } },
                                   new DynamicDocument { { "_id", 5 }, { "prop1", 33 } }
                               });

            var deleteResult1 = await storage.DeleteManyAsync();

            storage.InsertMany(new[]
                               {
                                   new DynamicDocument { { "_id", 1 }, { "prop1", 11 } }, // second delete
                                   new DynamicDocument { { "_id", 2 }, { "prop1", 11 } }, // second delete
                                   new DynamicDocument { { "_id", 3 }, { "prop1", 22 } }, // third delete
                                   new DynamicDocument { { "_id", 4 }, { "prop1", 22 } }, // third delete
                                   new DynamicDocument { { "_id", 5 }, { "prop1", 33 } }
                               });

            var deleteResult2 = await storage.DeleteManyAsync(f => f.Eq("prop1", 11));
            var deleteResult3 = await storage.DeleteManyAsync(f => f.Eq("prop1", 22));
            var deleteResult4 = await storage.DeleteManyAsync(f => f.Eq("prop1", 44));

            var documents = storage.Find().ToList();

            // Then

            Assert.AreEqual(5, deleteResult1);
            Assert.AreEqual(2, deleteResult2);
            Assert.AreEqual(2, deleteResult3);
            Assert.AreEqual(0, deleteResult4);

            Assert.AreEqual(1, documents.Count);
            Assert.AreEqual(5, ((dynamic)documents[0])._id);
        }

        [Test]
        public void ShouldAggregateWithGroup()
        {
            // Given
            var storage = MongoTestHelpers.GetEmptyStorageProvider(nameof(ShouldAggregateWithGroup));

            // When

            storage.InsertMany(new[]
                               {
                                   new DynamicDocument { { "_id", 1 }, { "item", "abc" }, { "price", 10 }, { "quantity", 2 }, { "date", DateTime.Parse("2016-03-01T08:00:00Z") } },
                                   new DynamicDocument { { "_id", 2 }, { "item", "jkl" }, { "price", 20 }, { "quantity", 1 }, { "date", DateTime.Parse("2016-03-01T09:00:00Z") } },
                                   new DynamicDocument { { "_id", 3 }, { "item", "xyz" }, { "price", 5 }, { "quantity", 10 }, { "date", DateTime.Parse("2016-03-15T09:00:00Z") } },
                                   new DynamicDocument { { "_id", 4 }, { "item", "xyz" }, { "price", 5 }, { "quantity", 20 }, { "date", DateTime.Parse("2016-04-04T11:21:39.736Z") } },
                                   new DynamicDocument { { "_id", 5 }, { "item", "abc" }, { "price", 10 }, { "quantity", 10 }, { "date", DateTime.Parse("2016-04-04T21:23:13.331Z") } }
                               });

            var groupByMonthDayYearResult = storage.Aggregate()
                                                   .Group(new DynamicDocument
                                                          {
                                                              {
                                                                  "_id", new DynamicDocument
                                                                         {
                                                                             { "month", new DynamicDocument { { "$month", "$date" } } },
                                                                             { "day", new DynamicDocument { { "$dayOfMonth", "$date" } } },
                                                                             { "year", new DynamicDocument { { "$year", "$date" } } }
                                                                         }
                                                              },
                                                              {
                                                                  "totalPrice", new DynamicDocument
                                                                                {
                                                                                    { "$sum", new DynamicDocument { { "$multiply", new[] { "$price", "$quantity" } } } }
                                                                                }
                                                              },
                                                              {
                                                                  "averageQuantity", new DynamicDocument
                                                                                     {
                                                                                         { "$avg", "$quantity" }
                                                                                     }
                                                              },
                                                              {
                                                                  "count", new DynamicDocument
                                                                           {
                                                                               { "$sum", 1 }
                                                                           }
                                                              }
                                                          })
                                                   .ToList();

            // Then

            Assert.AreEqual(3, groupByMonthDayYearResult.Count);

            var item1 = groupByMonthDayYearResult.FirstOrDefault((dynamic i) => i._id != null && i._id.month == 3 && i._id.day == 15 && i._id.year == 2016);
            Assert.IsNotNull(item1);
            Assert.AreEqual(3, item1._id.month);
            Assert.AreEqual(15, item1._id.day);
            Assert.AreEqual(2016, item1._id.year);
            Assert.AreEqual(50, item1.totalPrice);
            Assert.AreEqual(10, item1.averageQuantity);
            Assert.AreEqual(1, item1.count);

            var item2 = groupByMonthDayYearResult.FirstOrDefault((dynamic i) => i._id != null && i._id.month == 4 && i._id.day == 4 && i._id.year == 2016);
            Assert.IsNotNull(item2);
            Assert.AreEqual(4, item2._id.month);
            Assert.AreEqual(4, item2._id.day);
            Assert.AreEqual(2016, item2._id.year);
            Assert.AreEqual(200, item2.totalPrice);
            Assert.AreEqual(15, item2.averageQuantity);
            Assert.AreEqual(2, item2.count);

            var item3 = groupByMonthDayYearResult.FirstOrDefault((dynamic i) => i._id != null && i._id.month == 3 && i._id.day == 1 && i._id.year == 2016);
            Assert.IsNotNull(item3);
            Assert.AreEqual(3, item3._id.month);
            Assert.AreEqual(1, item3._id.day);
            Assert.AreEqual(2016, item3._id.year);
            Assert.AreEqual(40, item3.totalPrice);
            Assert.AreEqual(1.5, item3.averageQuantity);
            Assert.AreEqual(2, item3.count);
        }

        [Test]
        public void ShouldBulk()
        {
            // Given
            var storage = MongoTestHelpers.GetEmptyStorageProvider(nameof(ShouldBulk));

            // When

            storage.InsertMany(new[]
                               {
                                   new DynamicDocument { { "_id", 1 }, {"prop1", 1 } },
                                   new DynamicDocument { { "_id", 2 }, {"prop1", 1 } },
                                   new DynamicDocument { { "_id", 3 }, {"prop1", 2 } },
                                   new DynamicDocument { { "_id", 4 }, {"prop1", 2 } },
                                   new DynamicDocument { { "_id", 5 }, {"prop1", 3 } }
                               });

            var result = storage.Bulk(b => b.InsertOne(new DynamicDocument { { "_id", 6 }, { "prop1", 6 } })
                                            .UpdateOne(u => u.Set("prop1", 11), f => f.Eq("prop1", 1))
                                            .UpdateMany(u => u.Set("prop1", 22), f => f.Eq("prop1", 2))
                                            .ReplaceOne(new DynamicDocument { { "prop1", 7 } }, f => f.Eq("_id", 7), true)
                                            .DeleteOne(f => f.Eq("prop1", 3)));

            var documents = storage.Find().ToList();

            // Then

            Assert.IsNotNull(result);
            Assert.AreEqual(5, result.RequestCount);
            Assert.AreEqual(3, result.ModifiedCount);
            Assert.AreEqual(1, result.InsertedCount);
            Assert.AreEqual(1, result.DeletedCount);

            Assert.AreEqual(6, documents.Count);
            Assert.AreEqual(1, ((dynamic)documents[0])._id);
            Assert.AreEqual(11, ((dynamic)documents[0]).prop1);
            Assert.AreEqual(2, ((dynamic)documents[1])._id);
            Assert.AreEqual(1, ((dynamic)documents[1]).prop1);
            Assert.AreEqual(3, ((dynamic)documents[2])._id);
            Assert.AreEqual(22, ((dynamic)documents[2]).prop1);
            Assert.AreEqual(4, ((dynamic)documents[3])._id);
            Assert.AreEqual(22, ((dynamic)documents[3]).prop1);
            Assert.AreEqual(6, ((dynamic)documents[4])._id);
            Assert.AreEqual(6, ((dynamic)documents[4]).prop1);
            Assert.AreEqual(7, ((dynamic)documents[5])._id);
            Assert.AreEqual(7, ((dynamic)documents[5]).prop1);
        }
    }
}