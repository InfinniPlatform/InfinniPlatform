using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using InfinniPlatform.DocumentStorage.Contract;
using InfinniPlatform.DocumentStorage.Tests.TestEntities;
using InfinniPlatform.Sdk.Metadata;

using NUnit.Framework;

namespace InfinniPlatform.DocumentStorage.Tests.MongoDB
{
    [TestFixture]
    [Category(TestCategories.IntegrationTest)]
    public class MongoDocumentStorageProviderGenericTests
    {
        [Test]
        public void ShouldCount()
        {
            // Given
            var storage = MongoTestHelpers.GetEmptyStorageProvider<SimpleEntity>(nameof(ShouldCount));

            // When
            storage.InsertMany(new[] { new SimpleEntity { _id = 1 }, new SimpleEntity { _id = 2 }, new SimpleEntity { _id = 3 } });
            var count = storage.Count();

            // Then
            Assert.AreEqual(3, count);
        }

        [Test]
        public async Task ShouldCountAsync()
        {
            // Given
            var storage = MongoTestHelpers.GetEmptyStorageProvider<SimpleEntity>(nameof(ShouldCountAsync));

            // When
            storage.InsertMany(new[] { new SimpleEntity { _id = 1 }, new SimpleEntity { _id = 2 }, new SimpleEntity { _id = 3 } });
            var count = await storage.CountAsync();

            // Then
            Assert.AreEqual(3, count);
        }

        [Test]
        public void ShouldCountWithExpression()
        {
            // Given
            var storage = MongoTestHelpers.GetEmptyStorageProvider<SimpleEntity>(nameof(ShouldCountWithExpression));

            // When

            var findValue = Guid.NewGuid().ToString();
            var anotherValue = Guid.NewGuid().ToString();

            storage.InsertMany(new[]
                               {
                                   new SimpleEntity { _id = 1, prop1 = findValue },
                                   new SimpleEntity { _id = 2, prop1 = anotherValue },
                                   new SimpleEntity { _id = 3, prop1 = findValue }
                               });

            var count = storage.Count(i => i.prop1 == findValue);

            // Then
            Assert.AreEqual(2, count);
        }

        [Test]
        public async Task ShouldCountWithExpressionAsync()
        {
            // Given
            var storage = MongoTestHelpers.GetEmptyStorageProvider<SimpleEntity>(nameof(ShouldCountWithExpressionAsync));

            // When

            var findValue = Guid.NewGuid().ToString();
            var anotherValue = Guid.NewGuid().ToString();

            storage.InsertMany(new[]
                               {
                                   new SimpleEntity { _id = 1, prop1 = findValue },
                                   new SimpleEntity { _id = 2, prop1 = anotherValue },
                                   new SimpleEntity { _id = 3, prop1 = findValue }
                               });

            var count = await storage.CountAsync(i => i.prop1 == findValue);

            // Then
            Assert.AreEqual(2, count);
        }

        [Test]
        public void ShouldDistinct()
        {
            // Given
            var storage = MongoTestHelpers.GetEmptyStorageProvider<Inventory>(nameof(ShouldDistinct));

            // When

            storage.InsertMany(new[]
                               {
                                   new Inventory { _id = 1, dept = "A", item = new InventoryItem { sku = 111, color = "red" }, sizes = new[] { "S", "M" } },
                                   new Inventory { _id = 2, dept = "A", item = new InventoryItem { sku = 111, color = "blue" }, sizes = new[] { "M", "L" } },
                                   new Inventory { _id = 3, dept = "B", item = new InventoryItem { sku = 222, color = "blue" }, sizes = new[] { "S" } },
                                   new Inventory { _id = 4, dept = "A", item = new InventoryItem { sku = 333, color = "black" }, sizes = new[] { "S" } }
                               });

            var deptList = storage.Distinct(i => i.dept).ToList();
            var skuList = storage.Distinct(i => i.item.sku).ToList();
            var sizeList = storage.Distinct(i => i.sizes).ToList();
            var deptSkuList = storage.Distinct(i => i.item.sku, i => i.dept == "A").ToList();

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
            var storage = MongoTestHelpers.GetEmptyStorageProvider<Inventory>(nameof(ShouldDistinctAsync));

            // When

            storage.InsertMany(new[]
                               {
                                   new Inventory { _id = 1, dept = "A", item = new InventoryItem { sku = 111, color = "red" }, sizes = new[] { "S", "M" } },
                                   new Inventory { _id = 2, dept = "A", item = new InventoryItem { sku = 111, color = "blue" }, sizes = new[] { "M", "L" } },
                                   new Inventory { _id = 3, dept = "B", item = new InventoryItem { sku = 222, color = "blue" }, sizes = new[] { "S" } },
                                   new Inventory { _id = 4, dept = "A", item = new InventoryItem { sku = 333, color = "black" }, sizes = new[] { "S" } }
                               });

            var deptList = (await storage.DistinctAsync(i => i.dept)).ToList();
            var skuList = (await storage.DistinctAsync(i => i.item.sku)).ToList();
            var sizeList = (await storage.DistinctAsync(i => i.sizes)).ToList();
            var deptSkuList = (await storage.DistinctAsync(i => i.item.sku, i => i.dept == "A")).ToList();

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
            var storage = MongoTestHelpers.GetEmptyStorageProvider<SimpleEntity>(nameof(ShouldFindByEmpty));

            // When

            storage.InsertMany(new[]
                               {
                                   new SimpleEntity { _id = 1, prop1 = "A" },
                                   new SimpleEntity { _id = 2, prop1 = "B" },
                                   new SimpleEntity { _id = 3, prop1 = "C" },
                                   new SimpleEntity { _id = 4, prop1 = "D" },
                                   new SimpleEntity { _id = 5, prop1 = "F" }
                               });

            var result = storage.Find().ToList();

            // Then

            Assert.AreEqual(5, result.Count);
            Assert.AreEqual(1, result[0]._id);
            Assert.AreEqual(2, result[1]._id);
            Assert.AreEqual(3, result[2]._id);
            Assert.AreEqual(4, result[3]._id);
            Assert.AreEqual(5, result[4]._id);
            Assert.AreEqual("A", result[0].prop1);
            Assert.AreEqual("B", result[1].prop1);
            Assert.AreEqual("C", result[2].prop1);
            Assert.AreEqual("D", result[3].prop1);
            Assert.AreEqual("F", result[4].prop1);
        }

        [Test]
        public void ShouldFindByNot()
        {
            // Given
            var storage = MongoTestHelpers.GetEmptyStorageProvider<SimpleEntity>(nameof(ShouldFindByNot));

            // When

            storage.InsertMany(new[]
                               {
                                   new SimpleEntity { _id = 1, prop1 = "A" },
                                   new SimpleEntity { _id = 2, prop1 = "B" },
                                   new SimpleEntity { _id = 3, prop1 = "C" },
                                   new SimpleEntity { _id = 4, prop1 = "D" },
                                   new SimpleEntity { _id = 5, prop1 = "F" }
                               });

            var result = storage.Find(i => i.prop1 != "F").ToList();

            // Then

            Assert.AreEqual(4, result.Count);
            Assert.AreEqual(1, result[0]._id);
            Assert.AreEqual(2, result[1]._id);
            Assert.AreEqual(3, result[2]._id);
            Assert.AreEqual(4, result[3]._id);
            Assert.AreEqual("A", result[0].prop1);
            Assert.AreEqual("B", result[1].prop1);
            Assert.AreEqual("C", result[2].prop1);
            Assert.AreEqual("D", result[3].prop1);
        }

        [Test]
        public void ShouldFindByOr()
        {
            // Given
            var storage = MongoTestHelpers.GetEmptyStorageProvider<SimpleEntity>(nameof(ShouldFindByOr));

            // When

            storage.InsertMany(new[]
                               {
                                   new SimpleEntity { _id = 1, prop2 = 5 },
                                   new SimpleEntity { _id = 2, prop2 = 4 },
                                   new SimpleEntity { _id = 3, prop2 = 3 },
                                   new SimpleEntity { _id = 4, prop2 = 2 },
                                   new SimpleEntity { _id = 5, prop2 = 1 }
                               });

            var result = storage.Find(i => i.prop2 < 2 || i.prop2 > 4).ToList();

            // Then

            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(1, result[0]._id);
            Assert.AreEqual(5, result[1]._id);
            Assert.AreEqual(5, result[0].prop2);
            Assert.AreEqual(1, result[1].prop2);
        }

        [Test]
        public void ShouldFindByAnd()
        {
            // Given
            var storage = MongoTestHelpers.GetEmptyStorageProvider<SimpleEntity>(nameof(ShouldFindByAnd));

            // When

            storage.InsertMany(new[]
                               {
                                   new SimpleEntity { _id = 1, prop2 = 5 },
                                   new SimpleEntity { _id = 2, prop2 = 4 },
                                   new SimpleEntity { _id = 3, prop2 = 3 },
                                   new SimpleEntity { _id = 4, prop2 = 2 },
                                   new SimpleEntity { _id = 5, prop2 = 1 }
                               });

            var result = storage.Find(i => i.prop2 >= 2 && i.prop2 <= 4).ToList();

            // Then

            Assert.AreEqual(3, result.Count);
            Assert.AreEqual(2, result[0]._id);
            Assert.AreEqual(3, result[1]._id);
            Assert.AreEqual(4, result[2]._id);
            Assert.AreEqual(4, result[0].prop2);
            Assert.AreEqual(3, result[1].prop2);
            Assert.AreEqual(2, result[2].prop2);
        }

        [Test]
        public void ShouldFindByIn()
        {
            // Given
            var storage = MongoTestHelpers.GetEmptyStorageProvider<SimpleEntity>(nameof(ShouldFindByIn));

            // When

            storage.InsertMany(new[]
                               {
                                   new SimpleEntity { _id = 1, prop2 = 55 },
                                   new SimpleEntity { _id = 2, prop2 = 44 },
                                   new SimpleEntity { _id = 3, prop2 = 33 },
                                   new SimpleEntity { _id = 4, prop2 = 22 },
                                   new SimpleEntity { _id = 5, prop2 = null }
                               });

            var inResult = storage.Find(i => new[] { 11, 33, 55 }.Contains(i.prop2.Value)).ToList();
            var notInResult = storage.Find(i => !new[] { 11, 33, 55 }.Contains(i.prop2.Value)).ToList();

            // Then

            Assert.AreEqual(2, inResult.Count);
            Assert.AreEqual(1, inResult[0]._id);
            Assert.AreEqual(3, inResult[1]._id);

            Assert.AreEqual(3, notInResult.Count);
            Assert.AreEqual(2, notInResult[0]._id);
            Assert.AreEqual(4, notInResult[1]._id);
            Assert.AreEqual(5, notInResult[2]._id);
        }

        [Test]
        public void ShouldFindByEq()
        {
            // Given
            var storage = MongoTestHelpers.GetEmptyStorageProvider<SimpleEntity>(nameof(ShouldFindByEq));

            // When

            storage.InsertMany(new[]
                               {
                                   new SimpleEntity { _id = 1, prop2 = 11 },
                                   new SimpleEntity { _id = 2, prop2 = 22 },
                                   new SimpleEntity { _id = 3, prop2 = 11 },
                                   new SimpleEntity { _id = 4, prop2 = 22 },
                                   new SimpleEntity { _id = 5, prop2 = 11 }
                               });

            var eqResult = storage.Find(i => i.prop2 == 11).ToList();
            var notEqResult = storage.Find(i => i.prop2 != 11).ToList();

            // Then

            Assert.AreEqual(3, eqResult.Count);
            Assert.AreEqual(1, eqResult[0]._id);
            Assert.AreEqual(3, eqResult[1]._id);
            Assert.AreEqual(5, eqResult[2]._id);

            Assert.AreEqual(2, notEqResult.Count);
            Assert.AreEqual(2, notEqResult[0]._id);
            Assert.AreEqual(4, notEqResult[1]._id);
        }

        [Test]
        public void ShouldFindByCompareNumbers()
        {
            // Given
            var storage = MongoTestHelpers.GetEmptyStorageProvider<RangeEntity>(nameof(ShouldFindByCompareNumbers));

            // When

            storage.InsertMany(new[]
                               {
                                   new RangeEntity { _id = 1, number = 1 },
                                   new RangeEntity { _id = 2, number = 1.5 },
                                   new RangeEntity { _id = 3, number = 2 },
                                   new RangeEntity { _id = 4, number = 2.5 },
                                   new RangeEntity { _id = 5, number = 3 },
                                   new RangeEntity { _id = 6, number = 3.5 },
                                   new RangeEntity { _id = 7, number = 4 }
                               });

            var gtResult = storage.Find(i => i.number > 3).ToList();
            var gteResult = storage.Find(i => i.number >= 3.5).ToList();

            var ltResult = storage.Find(i => i.number < 2).ToList();
            var lteResult = storage.Find(i => i.number <= 1.5).ToList();

            var betweenResult = storage.Find(i => i.number > 1.5 && i.number < 3.5).ToList();

            // Then

            Assert.AreEqual(2, gtResult.Count);
            Assert.AreEqual(6, gtResult[0]._id);
            Assert.AreEqual(7, gtResult[1]._id);

            Assert.AreEqual(2, gteResult.Count);
            Assert.AreEqual(6, gteResult[0]._id);
            Assert.AreEqual(7, gteResult[1]._id);

            Assert.AreEqual(2, ltResult.Count);
            Assert.AreEqual(1, ltResult[0]._id);
            Assert.AreEqual(2, ltResult[1]._id);

            Assert.AreEqual(2, lteResult.Count);
            Assert.AreEqual(1, lteResult[0]._id);
            Assert.AreEqual(2, lteResult[1]._id);

            Assert.AreEqual(3, betweenResult.Count);
            Assert.AreEqual(3, betweenResult[0]._id);
            Assert.AreEqual(4, betweenResult[1]._id);
            Assert.AreEqual(5, betweenResult[2]._id);
        }

        [Test]
        public void ShouldFindByCompareDateTimes()
        {
            // Given
            var storage = MongoTestHelpers.GetEmptyStorageProvider<RangeEntity>(nameof(ShouldFindByCompareDateTimes));

            // When

            storage.InsertMany(new[]
                               {
                                   new RangeEntity { _id = 1, datetime = DateTime.Today.AddHours(1) },
                                   new RangeEntity { _id = 2, datetime = DateTime.Today.AddHours(1.5) },
                                   new RangeEntity { _id = 3, datetime = DateTime.Today.AddHours(2) },
                                   new RangeEntity { _id = 4, datetime = DateTime.Today.AddHours(2.5) },
                                   new RangeEntity { _id = 5, datetime = DateTime.Today.AddHours(3) },
                                   new RangeEntity { _id = 6, datetime = DateTime.Today.AddHours(3.5) },
                                   new RangeEntity { _id = 7, datetime = DateTime.Today.AddHours(4) }
                               });

            var gtResult = storage.Find(i => i.datetime > DateTime.Today.AddHours(3)).ToList();
            var gteResult = storage.Find(i => i.datetime >= DateTime.Today.AddHours(3.5)).ToList();

            var ltResult = storage.Find(i => i.datetime < DateTime.Today.AddHours(2)).ToList();
            var lteResult = storage.Find(i => i.datetime <= DateTime.Today.AddHours(1.5)).ToList();

            var betweenResult = storage.Find(i => i.datetime > DateTime.Today.AddHours(1.5) && i.datetime < DateTime.Today.AddHours(3.5)).ToList();

            // Then

            Assert.AreEqual(2, gtResult.Count);
            Assert.AreEqual(6, gtResult[0]._id);
            Assert.AreEqual(7, gtResult[1]._id);

            Assert.AreEqual(2, gteResult.Count);
            Assert.AreEqual(6, gteResult[0]._id);
            Assert.AreEqual(7, gteResult[1]._id);

            Assert.AreEqual(2, ltResult.Count);
            Assert.AreEqual(1, ltResult[0]._id);
            Assert.AreEqual(2, ltResult[1]._id);

            Assert.AreEqual(2, lteResult.Count);
            Assert.AreEqual(1, lteResult[0]._id);
            Assert.AreEqual(2, lteResult[1]._id);

            Assert.AreEqual(3, betweenResult.Count);
            Assert.AreEqual(3, betweenResult[0]._id);
            Assert.AreEqual(4, betweenResult[1]._id);
            Assert.AreEqual(5, betweenResult[2]._id);
        }

        [Test]
        public void ShouldFindByRegex()
        {
            // Given
            var storage = MongoTestHelpers.GetEmptyStorageProvider<Product>(nameof(ShouldFindByRegex));

            // When

            storage.InsertMany(new[]
                               {
                                   new Product { _id = 100, sku = "abc123", description = "Single line description." },
                                   new Product { _id = 101, sku = "abc789", description = "First line\nSecond line" },
                                   new Product { _id = 102, sku = "xyz456", description = "Many spaces before     line" },
                                   new Product { _id = 103, sku = "xyz789", description = "Multiple\nline description" }
                               });

            var caseInsensitiveRegexResult = storage.Find(i => Regex.IsMatch(i.sku, "^ABC", RegexOptions.IgnoreCase)).ToList();
            var multilineMatchRegexResult = storage.Find(i => Regex.IsMatch(i.description, "^S", RegexOptions.Multiline)).ToList();
            var ignoreNewLineRegexResult = storage.Find(i => Regex.IsMatch(i.description, "m.*line", RegexOptions.Singleline | RegexOptions.IgnoreCase)).ToList();
            var ignoreWhiteSpacesRegexResult = storage.Find(i => Regex.IsMatch(i.description, "\\S+\\s+line$", RegexOptions.Singleline)).ToList();
            var startsWithResult = storage.Find(i => i.sku.StartsWith("ab")).ToList();
            var endsWithResult = storage.Find(i => i.sku.EndsWith("89")).ToList();
            var containsResult = storage.Find(i => i.sku.Contains("7")).ToList();

            // Then

            Assert.AreEqual(2, caseInsensitiveRegexResult.Count);
            Assert.AreEqual(100, caseInsensitiveRegexResult[0]._id);
            Assert.AreEqual(101, caseInsensitiveRegexResult[1]._id);

            Assert.AreEqual(2, multilineMatchRegexResult.Count);
            Assert.AreEqual(100, caseInsensitiveRegexResult[0]._id);
            Assert.AreEqual(101, caseInsensitiveRegexResult[1]._id);

            Assert.AreEqual(2, ignoreNewLineRegexResult.Count);
            Assert.AreEqual(102, ignoreNewLineRegexResult[0]._id);
            Assert.AreEqual(103, ignoreNewLineRegexResult[1]._id);

            Assert.AreEqual(2, ignoreWhiteSpacesRegexResult.Count);
            Assert.AreEqual(101, ignoreWhiteSpacesRegexResult[0]._id);
            Assert.AreEqual(102, ignoreWhiteSpacesRegexResult[1]._id);

            Assert.AreEqual(2, startsWithResult.Count);
            Assert.AreEqual(100, startsWithResult[0]._id);
            Assert.AreEqual(101, startsWithResult[1]._id);

            Assert.AreEqual(2, endsWithResult.Count);
            Assert.AreEqual(101, endsWithResult[0]._id);
            Assert.AreEqual(103, endsWithResult[1]._id);

            Assert.AreEqual(2, containsResult.Count);
            Assert.AreEqual(101, containsResult[0]._id);
            Assert.AreEqual(103, containsResult[1]._id);
        }

        [Test]
        public void ShouldFindByStartsWith()
        {
            // Given
            var storage = MongoTestHelpers.GetEmptyStorageProvider<SimpleEntity>(nameof(ShouldFindByStartsWith));

            // When

            storage.InsertMany(new[]
                               {
                                   new SimpleEntity { _id = 1, prop1 = "It starts with some text." },
                                   new SimpleEntity { _id=  2, prop1 = "it starts with some text." },
                                   new SimpleEntity { _id=  3, prop1 = "Does it start with some text?" }
                               });

            var caseInsensitiveResult = storage.Find(i => Regex.IsMatch(i.prop1, "^It", RegexOptions.IgnoreCase)).ToList();
            var caseSensitiveResult = storage.Find(i => Regex.IsMatch(i.prop1, "^It")).ToList();

            // Then

            Assert.AreEqual(2, caseInsensitiveResult.Count);
            Assert.AreEqual(1, caseInsensitiveResult[0]._id);
            Assert.AreEqual(2, caseInsensitiveResult[1]._id);

            Assert.AreEqual(1, caseSensitiveResult.Count);
            Assert.AreEqual(1, caseSensitiveResult[0]._id);
        }

        [Test]
        public void ShouldFindByEndsWith()
        {
            // Given
            var storage = MongoTestHelpers.GetEmptyStorageProvider<SimpleEntity>(nameof(ShouldFindByEndsWith));

            // When

            storage.InsertMany(new[]
                               {
                                   new SimpleEntity { _id = 1, prop1 = "It ends with some Text." },
                                   new SimpleEntity { _id = 2, prop1 = "It ends with some text." },
                                   new SimpleEntity { _id = 3, prop1 = "Does it end with some text?" }
                               });

            var caseInsensitiveResult = storage.Find(i => Regex.IsMatch(i.prop1, @"Text\.", RegexOptions.IgnoreCase)).ToList();
            var caseSensitiveResult = storage.Find(i => Regex.IsMatch(i.prop1, @"Text\.")).ToList();

            // Then

            Assert.AreEqual(2, caseInsensitiveResult.Count);
            Assert.AreEqual(1, caseInsensitiveResult[0]._id);
            Assert.AreEqual(2, caseInsensitiveResult[1]._id);

            Assert.AreEqual(1, caseSensitiveResult.Count);
            Assert.AreEqual(1, caseSensitiveResult[0]._id);
        }

        [Test]
        public void ShouldFindByContains()
        {
            // Given
            var storage = MongoTestHelpers.GetEmptyStorageProvider<SimpleEntity>(nameof(ShouldFindByContains));

            // When

            storage.InsertMany(new[]
                               {
                                   new SimpleEntity { _id = 1, prop1 = "It Contains some text." },
                                   new SimpleEntity { _id = 2, prop1 = "It contains some text." },
                                   new SimpleEntity { _id = 3, prop1 = "Does it contain some text?" }
                               });

            var caseInsensitiveResult = storage.Find(i => Regex.IsMatch(i.prop1, @"Contains", RegexOptions.IgnoreCase)).ToList();
            var caseSensitiveResult = storage.Find(i => Regex.IsMatch(i.prop1, @"Contains")).ToList();

            // Then

            Assert.AreEqual(2, caseInsensitiveResult.Count);
            Assert.AreEqual(1, caseInsensitiveResult[0]._id);
            Assert.AreEqual(2, caseInsensitiveResult[1]._id);

            Assert.AreEqual(1, caseSensitiveResult.Count);
            Assert.AreEqual(1, caseSensitiveResult[0]._id);
        }

        [Test]
        public void ShouldFindByMatch()
        {
            // Given
            var storage = MongoTestHelpers.GetEmptyStorageProvider<Survey>(nameof(ShouldFindByMatch));

            // When

            storage.InsertMany(new[]
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
                               });

            var elemMatchResult = storage.Find(i => i.results.Any(r => r.product == "xyz" && r.score >= 8)).ToList();

            // Then

            Assert.AreEqual(1, elemMatchResult.Count);
            Assert.AreEqual(3, elemMatchResult[0]._id);
        }

        [Test]
        public void ShouldFindByAll()
        {
            // Given
            var storage = MongoTestHelpers.GetEmptyStorageProvider<Thing>(nameof(ShouldFindByAll));

            // When

            storage.InsertMany(new[]
                               {
                                   new Thing
                                   {
                                       _id = 1,
                                       code = "xyz",
                                       tags = new[] { "school", "book", "bag", "headphone", "appliance" },
                                       qty = new[]
                                             {
                                                 new ThingItem { size = "S", num = 10, color = "blue" },
                                                 new ThingItem { size = "M", num = 45, color = "blue" },
                                                 new ThingItem { size = "L", num = 100, color = "green" }
                                             }
                                   },
                                   new Thing
                                   {
                                       _id = 2,
                                       code = "abc",
                                       tags = new[] { "appliance", "school", "book" },
                                       qty = new[]
                                             {
                                                 new ThingItem { size = "6", num = 100, color = "green" },
                                                 new ThingItem { size = "6", num = 50, color = "blue" },
                                                 new ThingItem { size = "8", num = 100, color = "brown" }
                                             }
                                   },
                                   new Thing
                                   {
                                       _id = 3,
                                       code = "efg",
                                       tags = new[] { "school", "book" },
                                       qty = new[]
                                             {
                                                 new ThingItem { size = "S", num = 10, color = "blue" },
                                                 new ThingItem { size = "M", num = 100, color = "blue" },
                                                 new ThingItem { size = "L", num = 100, color = "green" }
                                             }
                                   },
                                   new Thing
                                   {
                                       _id = 4,
                                       code = "ijk",
                                       tags = new[] { "electronics", "school" },
                                       qty = new[]
                                             {
                                                 new ThingItem { size = "M", num = 100, color = "green" }
                                             }
                                   }
                               });

            var allResult = storage.Find(i => new[] { "appliance", "school", "book" }.All(t => i.tags.Contains(t))).ToList();

            // Then
            Assert.AreEqual(2, allResult.Count);
            Assert.AreEqual(1, allResult[0]._id);
            Assert.AreEqual(2, allResult[1]._id);
        }

        [Test]
        public void ShouldFindByAnyIn()
        {
            // Given
            var storage = MongoTestHelpers.GetEmptyStorageProvider<CollectionEntity<int>>(nameof(ShouldFindByAnyIn));

            // When

            storage.InsertMany(new[]
                               {
                                   new CollectionEntity<int> { _id = 1, items = new[] { 1, 2, 3 } },
                                   new CollectionEntity<int> { _id = 2, items = new[] { 2, 3, 4 } },
                                   new CollectionEntity<int> { _id = 3, items = new[] { 3, 4, 5 } },
                                   new CollectionEntity<int> { _id = 4, items = new[] { 4, 5, 6 } },
                                   new CollectionEntity<int> { _id = 5, items = new[] { 5, 6, 7 } }
                               });

            var anyInResult = storage.Find(i => new[] { 3, 4 }.Any(ii => i.items.Contains(ii))).ToList();
            var anyNotInResult = storage.Find(i => !new[] { 3, 4 }.Any(ii => i.items.Contains(ii))).ToList();

            // Then

            Assert.AreEqual(4, anyInResult.Count);
            Assert.AreEqual(1, anyInResult[0]._id);
            Assert.AreEqual(2, anyInResult[1]._id);
            Assert.AreEqual(3, anyInResult[2]._id);
            Assert.AreEqual(4, anyInResult[3]._id);

            Assert.AreEqual(1, anyNotInResult.Count);
            Assert.AreEqual(5, anyNotInResult[0]._id);
        }

        [Test]
        public void ShouldFindByAnyEq()
        {
            // Given
            var storage = MongoTestHelpers.GetEmptyStorageProvider<CollectionEntity<int>>(nameof(ShouldFindByAnyEq));

            // When

            storage.InsertMany(new[]
                               {
                                   new CollectionEntity<int> { _id = 1, items = new[] { 1, 2, 3 } },
                                   new CollectionEntity<int> { _id = 2, items = new[] { 2, 3, 4 } },
                                   new CollectionEntity<int> { _id = 3, items = new[] { 3, 4, 5 } },
                                   new CollectionEntity<int> { _id = 4, items = new[] { 4, 5, 6 } },
                                   new CollectionEntity<int> { _id = 5, items = new[] { 5, 6, 7 } }
                               });

            var anyEqResult = storage.Find(i => i.items.Any(ii => ii == 4)).ToList();
            var anyNotEqResult = storage.Find(i => !i.items.Any(ii => ii == 4)).ToList();

            // Then

            Assert.AreEqual(3, anyEqResult.Count);
            Assert.AreEqual(2, anyEqResult[0]._id);
            Assert.AreEqual(3, anyEqResult[1]._id);
            Assert.AreEqual(4, anyEqResult[2]._id);

            Assert.AreEqual(2, anyNotEqResult.Count);
            Assert.AreEqual(1, anyNotEqResult[0]._id);
            Assert.AreEqual(5, anyNotEqResult[1]._id);
        }

        [Test]
        public void ShouldFindByAnyCompareNumbers()
        {
            // Given
            var storage = MongoTestHelpers.GetEmptyStorageProvider<CollectionEntity<int>>(nameof(ShouldFindByAnyCompareNumbers));

            // When

            storage.InsertMany(new[]
                               {
                                   new CollectionEntity<int> { _id = 1, items = new[] { 1, 2, 3 } },
                                   new CollectionEntity<int> { _id = 2, items = new[] { 2, 3, 4 } },
                                   new CollectionEntity<int> { _id = 3, items = new[] { 3, 4, 5 } },
                                   new CollectionEntity<int> { _id = 4, items = new[] { 4, 5, 6 } },
                                   new CollectionEntity<int> { _id = 5, items = new[] { 5, 6, 7 } }
                               });

            var anyGtResult = storage.Find(i => i.items.Any(ii => ii > 4)).ToList();
            var anyGteResult = storage.Find(i => i.items.Any(ii => ii >= 6)).ToList();
            var anyLtResult = storage.Find(i => i.items.Any(ii => ii < 3)).ToList();
            var anyLteResult = storage.Find(i => i.items.Any(ii => ii <= 4)).ToList();

            // Then

            Assert.AreEqual(3, anyGtResult.Count);
            Assert.AreEqual(3, anyGtResult[0]._id);
            Assert.AreEqual(4, anyGtResult[1]._id);
            Assert.AreEqual(5, anyGtResult[2]._id);

            Assert.AreEqual(2, anyGteResult.Count);
            Assert.AreEqual(4, anyGteResult[0]._id);
            Assert.AreEqual(5, anyGteResult[1]._id);

            Assert.AreEqual(2, anyLtResult.Count);
            Assert.AreEqual(1, anyLtResult[0]._id);
            Assert.AreEqual(2, anyLtResult[1]._id);

            Assert.AreEqual(4, anyLteResult.Count);
            Assert.AreEqual(1, anyLteResult[0]._id);
            Assert.AreEqual(2, anyLteResult[1]._id);
            Assert.AreEqual(3, anyLteResult[2]._id);
            Assert.AreEqual(4, anyLteResult[3]._id);
        }

        [Test]
        public void ShouldFindByAnyCompareDateTimes()
        {
            // Given
            var storage = MongoTestHelpers.GetEmptyStorageProvider<CollectionEntity<DateTime>>(nameof(ShouldFindByAnyCompareDateTimes));

            // When

            var today = DateTime.Today;

            storage.InsertMany(new[]
                               {
                                   new CollectionEntity<DateTime> { _id = 1, items = new[] { today.AddHours(1), today.AddHours(2), today.AddHours(3) } },
                                   new CollectionEntity<DateTime> { _id = 2, items = new[] { today.AddHours(2), today.AddHours(3), today.AddHours(4) } },
                                   new CollectionEntity<DateTime> { _id = 3, items = new[] { today.AddHours(3), today.AddHours(4), today.AddHours(5) } },
                                   new CollectionEntity<DateTime> { _id = 4, items = new[] { today.AddHours(4), today.AddHours(5), today.AddHours(6) } },
                                   new CollectionEntity<DateTime> { _id = 5, items = new[] { today.AddHours(5), today.AddHours(6), today.AddHours(7) } }
                               });

            var anyGtResult = storage.Find(i => i.items.Any(ii => ii > today.AddHours(4))).ToList();
            var anyGteResult = storage.Find(i => i.items.Any(ii => ii >= today.AddHours(6))).ToList();
            var anyLtResult = storage.Find(i => i.items.Any(ii => ii < today.AddHours(3))).ToList();
            var anyLteResult = storage.Find(i => i.items.Any(ii => ii <= today.AddHours(4))).ToList();

            // Then

            Assert.AreEqual(3, anyGtResult.Count);
            Assert.AreEqual(3, anyGtResult[0]._id);
            Assert.AreEqual(4, anyGtResult[1]._id);
            Assert.AreEqual(5, anyGtResult[2]._id);

            Assert.AreEqual(2, anyGteResult.Count);
            Assert.AreEqual(4, anyGteResult[0]._id);
            Assert.AreEqual(5, anyGteResult[1]._id);

            Assert.AreEqual(2, anyLtResult.Count);
            Assert.AreEqual(1, anyLtResult[0]._id);
            Assert.AreEqual(2, anyLtResult[1]._id);

            Assert.AreEqual(4, anyLteResult.Count);
            Assert.AreEqual(1, anyLteResult[0]._id);
            Assert.AreEqual(2, anyLteResult[1]._id);
            Assert.AreEqual(3, anyLteResult[2]._id);
            Assert.AreEqual(4, anyLteResult[3]._id);
        }

        [Test]
        public void ShouldFindBySize()
        {
            // Given
            var storage = MongoTestHelpers.GetEmptyStorageProvider<CollectionEntity<int>>(nameof(ShouldFindBySize));

            // When

            storage.InsertMany(new[]
                               {
                                   new CollectionEntity<int> { _id=1 },
                                   new CollectionEntity<int> { _id=2, items=new int[] { } },
                                   new CollectionEntity<int> { _id=3, items=new[] { 1 } },
                                   new CollectionEntity<int> { _id=4, items=new[] { 1, 2 } },
                                   new CollectionEntity<int> { _id=5, items=new[] { 1, 2, 3 } },
                                   new CollectionEntity<int> { _id=6, items=new[] { 1, 2, 3, 4 } },
                                   new CollectionEntity<int> { _id=7, items=new[] { 1, 2, 3, 4, 5 } }
                               });

            var sizeEq0Result = storage.Find(i => i.items.Count() == 0).ToList();
            var sizeEq1Result = storage.Find(i => i.items.Count() == 1).ToList();
            var sizeEq2Result = storage.Find(i => i.items.Count() == 2).ToList();

            var sizeGtResult = storage.Find(i => i.items.Count() > 3).ToList();
            var sizeGteResult = storage.Find(i => i.items.Count() >= 3).ToList();

            var sizeLtResult = storage.Find(i => i.items.Count() < 2).ToList();
            var sizeLteResult = storage.Find(i => i.items.Count() <= 2).ToList();

            // Then

            Assert.AreEqual(1, sizeEq0Result.Count);
            Assert.AreEqual(2, sizeEq0Result[0]._id);

            Assert.AreEqual(1, sizeEq1Result.Count);
            Assert.AreEqual(3, sizeEq1Result[0]._id);

            Assert.AreEqual(1, sizeEq2Result.Count);
            Assert.AreEqual(4, sizeEq2Result[0]._id);

            Assert.AreEqual(2, sizeGtResult.Count);
            Assert.AreEqual(6, sizeGtResult[0]._id);
            Assert.AreEqual(7, sizeGtResult[1]._id);

            Assert.AreEqual(3, sizeGteResult.Count);
            Assert.AreEqual(5, sizeGteResult[0]._id);
            Assert.AreEqual(6, sizeGteResult[1]._id);
            Assert.AreEqual(7, sizeGteResult[2]._id);

            Assert.AreEqual(3, sizeLtResult.Count);
            Assert.AreEqual(1, sizeLtResult[0]._id);
            Assert.AreEqual(2, sizeLtResult[1]._id);
            Assert.AreEqual(3, sizeLtResult[2]._id);

            Assert.AreEqual(4, sizeLteResult.Count);
            Assert.AreEqual(1, sizeLteResult[0]._id);
            Assert.AreEqual(2, sizeLteResult[1]._id);
            Assert.AreEqual(3, sizeLteResult[2]._id);
            Assert.AreEqual(4, sizeLteResult[3]._id);
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

            var storage = MongoTestHelpers.GetEmptyStorageProvider<Article>(nameof(ShouldFindByText), textIndex);

            // When

            storage.InsertMany(new[]
                               {
                                   new Article { _id = 1, subject = "coffee", author = "xyz", views = 50 },
                                   new Article { _id = 2, subject = "Coffee Shopping", author = "efg", views = 5 },
                                   new Article { _id = 3, subject = "Baking a cake", author = "abc", views = 90 },
                                   new Article { _id = 4, subject = "baking", author = "xyz", views = 100 },
                                   new Article { _id = 5, subject = "Café Con Leche", author = "abc", views = 200 },
                                   new Article { _id = 6, subject = "Сырники", author = "jkl", views = 80 },
                                   new Article { _id = 7, subject = "coffee and cream", author = "efg", views = 10 },
                                   new Article { _id = 8, subject = "Cafe con Leche", author = "xyz", views = 10 }
                               });

            var searchSingleWordResult = storage.FindText("coffee").ToList();
            var searchWithoutTermResult = storage.FindText("coffee -shop").ToList();
            var searchWithLanguageResult = storage.FindText("leche", "es").ToList();
            var diacriticInsensitiveSearchResult = storage.FindText("сы́рники CAFÉS").ToList();
            var caseSensitiveSearchForTermResult = storage.FindText("Coffee", caseSensitive: true).ToList();
            var caseSensitiveSearchForPhraseResult = storage.FindText("\"Café Con Leche\"", caseSensitive: true).ToList();
            var caseSensitiveSearchWithNegatedTermResult = storage.FindText("Coffee -shop", caseSensitive: true).ToList();
            var diacriticSensitiveSearchForTermResult = storage.FindText("CAFÉ", diacriticSensitive: true).ToList();
            var diacriticSensitiveSearchWithNegatedTermResult = storage.FindText("leches -cafés", diacriticSensitive: true).ToList();
            var searchWithTextScoreResult = storage.FindText("coffee").SortByTextScore(i => i.score).ToList();
            var searchWithTextScoreAndProjectionResult = storage.FindText("coffee").Project(i => new ArticleProjection { _id = i._id, subject = i.subject }).SortByTextScore(i => i.score).ToList();

            // Then

            Assert.AreEqual(3, searchSingleWordResult.Count, nameof(searchSingleWordResult));
            CollectionAssert.AreEquivalent(new[] { 2, 7, 1 }, new[] { searchSingleWordResult[0]._id, searchSingleWordResult[1]._id, searchSingleWordResult[2]._id });

            Assert.AreEqual(2, searchWithoutTermResult.Count, nameof(searchWithoutTermResult));
            CollectionAssert.AreEquivalent(new[] { 7, 1 }, new[] { searchWithoutTermResult[0]._id, searchWithoutTermResult[1]._id });

            Assert.AreEqual(2, searchWithLanguageResult.Count, nameof(searchWithLanguageResult));
            CollectionAssert.AreEquivalent(new[] { 5, 8 }, new[] { searchWithLanguageResult[0]._id, searchWithLanguageResult[1]._id });

            Assert.AreEqual(3, diacriticInsensitiveSearchResult.Count, nameof(diacriticInsensitiveSearchResult));
            CollectionAssert.AreEquivalent(new[] { 6, 5, 8 }, new[] { diacriticInsensitiveSearchResult[0]._id, diacriticInsensitiveSearchResult[1]._id, diacriticInsensitiveSearchResult[2]._id });

            Assert.AreEqual(1, caseSensitiveSearchForTermResult.Count, nameof(caseSensitiveSearchForTermResult));
            Assert.AreEqual(2, caseSensitiveSearchForTermResult[0]._id, nameof(caseSensitiveSearchForTermResult));

            Assert.AreEqual(1, caseSensitiveSearchForPhraseResult.Count, nameof(caseSensitiveSearchForPhraseResult));
            Assert.AreEqual(5, caseSensitiveSearchForPhraseResult[0]._id, nameof(caseSensitiveSearchForPhraseResult));

            Assert.AreEqual(1, caseSensitiveSearchWithNegatedTermResult.Count, nameof(caseSensitiveSearchWithNegatedTermResult));
            Assert.AreEqual(2, caseSensitiveSearchWithNegatedTermResult[0]._id, nameof(caseSensitiveSearchWithNegatedTermResult));

            Assert.AreEqual(1, diacriticSensitiveSearchForTermResult.Count, nameof(diacriticSensitiveSearchForTermResult));
            Assert.AreEqual(5, diacriticSensitiveSearchForTermResult[0]._id, nameof(diacriticSensitiveSearchForTermResult));

            Assert.AreEqual(1, diacriticSensitiveSearchWithNegatedTermResult.Count, nameof(diacriticSensitiveSearchWithNegatedTermResult));
            Assert.AreEqual(8, diacriticSensitiveSearchWithNegatedTermResult[0]._id, nameof(diacriticSensitiveSearchWithNegatedTermResult));

            Assert.AreEqual(3, searchWithTextScoreResult.Count, nameof(searchWithTextScoreResult));
            Assert.AreEqual(1, searchWithTextScoreResult[0]._id, nameof(searchWithTextScoreResult));
            Assert.AreEqual(2, searchWithTextScoreResult[1]._id, nameof(searchWithTextScoreResult));
            Assert.AreEqual(7, searchWithTextScoreResult[2]._id, nameof(searchWithTextScoreResult));

            Assert.AreEqual(3, searchWithTextScoreAndProjectionResult.Count, nameof(searchWithTextScoreAndProjectionResult));
            Assert.AreEqual(1, searchWithTextScoreAndProjectionResult[0]._id, nameof(searchWithTextScoreAndProjectionResult));
            Assert.AreEqual(2, searchWithTextScoreAndProjectionResult[1]._id, nameof(searchWithTextScoreAndProjectionResult));
            Assert.AreEqual(7, searchWithTextScoreAndProjectionResult[2]._id, nameof(searchWithTextScoreAndProjectionResult));
        }

        [Test]
        public void ShouldFindByWhere()
        {
            // Given
            var storage = MongoTestHelpers.GetEmptyStorageProvider<SimpleEntity>(nameof(ShouldFindWithSort));

            // When

            storage.InsertMany(new[]
                               {
                                   new SimpleEntity { _id = 1, prop1 = "A", prop2 = 11 },
                                   new SimpleEntity { _id = 2, prop1 = "A", prop2 = 11 },
                                   new SimpleEntity { _id = 3, prop1 = "A", prop2 = 12 },
                                   new SimpleEntity { _id = 4, prop1 = "B", prop2 = 11 },
                                   new SimpleEntity { _id = 5, prop1 = "B", prop2 = 11 },
                                   new SimpleEntity { _id = 6, prop1 = "B", prop2 = 12 }
                               });

            var resultA11 = storage.Find()
                                   .Where(i => i.prop1 == "A")
                                   .Where(i => i.prop2 == 11)
                                   .ToList();

            var resultA12 = storage.Find()
                                   .Where(i => i.prop1 == "A")
                                   .Where(i => i.prop2 == 12)
                                   .ToList();

            // Then

            Assert.AreEqual(2, resultA11.Count);
            Assert.AreEqual(1, resultA11[0]._id);
            Assert.AreEqual(2, resultA11[1]._id);

            Assert.AreEqual(1, resultA12.Count);
            Assert.AreEqual(3, resultA12[0]._id);
        }

        [Test]
        public void ShouldFindWithSort()
        {
            // Given
            var storage = MongoTestHelpers.GetEmptyStorageProvider<Order>(nameof(ShouldFindWithSort));

            // When

            storage.InsertMany(new[]
                               {
                                   new Order { _id = 1, item = new OrderItem { category = "cake", type = "chiffon" }, amount = 10 },
                                   new Order { _id = 2, item = new OrderItem { category = "cookies", type = "chocolate chip" }, amount = 50 },
                                   new Order { _id = 3, item = new OrderItem { category = "cookies", type = "chocolate chip" }, amount = 15 },
                                   new Order { _id = 4, item = new OrderItem { category = "cake", type = "lemon" }, amount = 30 },
                                   new Order { _id = 5, item = new OrderItem { category = "cake", type = "carrot" }, amount = 20 },
                                   new Order { _id = 6, item = new OrderItem { category = "brownies", type = "blondie" }, amount = 10 }
                               });

            var sortAscByOneFieldResult = storage.Find().SortBy(i => i.amount).ToList();
            var sortDescByOneFieldResult = storage.Find().SortByDescending(i => i.amount).ToList();
            var sortAscByTwoFieldsResult = storage.Find().SortBy(i => i.item.category).ThenBy(i => i.item.type).ToList();
            var sortAscDescByTwoFieldsResult = storage.Find().SortBy(i => i.item.category).ThenByDescending(i => i.item.type).ToList();

            // Then

            Assert.AreEqual(6, sortAscByOneFieldResult.Count);
            Assert.AreEqual(1, sortAscByOneFieldResult[0]._id);
            Assert.AreEqual(6, sortAscByOneFieldResult[1]._id);
            Assert.AreEqual(3, sortAscByOneFieldResult[2]._id);
            Assert.AreEqual(5, sortAscByOneFieldResult[3]._id);
            Assert.AreEqual(4, sortAscByOneFieldResult[4]._id);
            Assert.AreEqual(2, sortAscByOneFieldResult[5]._id);

            Assert.AreEqual(6, sortDescByOneFieldResult.Count);
            Assert.AreEqual(2, sortDescByOneFieldResult[0]._id);
            Assert.AreEqual(4, sortDescByOneFieldResult[1]._id);
            Assert.AreEqual(5, sortDescByOneFieldResult[2]._id);
            Assert.AreEqual(3, sortDescByOneFieldResult[3]._id);
            Assert.AreEqual(1, sortDescByOneFieldResult[4]._id);
            Assert.AreEqual(6, sortDescByOneFieldResult[5]._id);

            Assert.AreEqual(6, sortAscByTwoFieldsResult.Count);
            Assert.AreEqual(6, sortAscByTwoFieldsResult[0]._id);
            Assert.AreEqual(5, sortAscByTwoFieldsResult[1]._id);
            Assert.AreEqual(1, sortAscByTwoFieldsResult[2]._id);
            Assert.AreEqual(4, sortAscByTwoFieldsResult[3]._id);
            Assert.AreEqual(2, sortAscByTwoFieldsResult[4]._id);
            Assert.AreEqual(3, sortAscByTwoFieldsResult[5]._id);

            Assert.AreEqual(6, sortAscDescByTwoFieldsResult.Count);
            Assert.AreEqual(6, sortAscDescByTwoFieldsResult[0]._id);
            Assert.AreEqual(4, sortAscDescByTwoFieldsResult[1]._id);
            Assert.AreEqual(1, sortAscDescByTwoFieldsResult[2]._id);
            Assert.AreEqual(5, sortAscDescByTwoFieldsResult[3]._id);
            Assert.AreEqual(2, sortAscDescByTwoFieldsResult[4]._id);
            Assert.AreEqual(3, sortAscDescByTwoFieldsResult[5]._id);
        }

        [Test]
        public void ShouldFindWithSkipAndLimit()
        {
            // Given
            var storage = MongoTestHelpers.GetEmptyStorageProvider<SimpleEntity>(nameof(ShouldFindWithSkipAndLimit));

            // When

            storage.InsertMany(new[]
                               {
                                   new SimpleEntity { _id = 1 },
                                   new SimpleEntity { _id = 2 },
                                   new SimpleEntity { _id = 3 },
                                   new SimpleEntity { _id = 4 },
                                   new SimpleEntity { _id = 5 },
                                   new SimpleEntity { _id = 6 },
                                   new SimpleEntity { _id = 7 },
                                   new SimpleEntity { _id = 8 },
                                   new SimpleEntity { _id = 9 }
                               });

            var skipResult = storage.Find().Skip(7).ToList();
            var sortAndSkipResult = storage.Find().SortByDescending(i => i._id).Skip(7).ToList();
            var limitResult = storage.Find().Limit(3).ToList();
            var sortAndLimitResult = storage.Find().SortByDescending(i => i._id).Limit(3).ToList();
            var skipAndLimitResult = storage.Find().Skip(3).Limit(3).ToList();
            var sortSkipAndLimitResult = storage.Find().SortByDescending(i => i._id).Skip(3).Limit(3).ToList();

            // Then

            Assert.AreEqual(2, skipResult.Count);
            Assert.AreEqual(8, skipResult[0]._id);
            Assert.AreEqual(9, skipResult[1]._id);

            Assert.AreEqual(2, sortAndSkipResult.Count);
            Assert.AreEqual(2, sortAndSkipResult[0]._id);
            Assert.AreEqual(1, sortAndSkipResult[1]._id);

            Assert.AreEqual(3, limitResult.Count);
            Assert.AreEqual(1, limitResult[0]._id);
            Assert.AreEqual(2, limitResult[1]._id);
            Assert.AreEqual(3, limitResult[2]._id);

            Assert.AreEqual(3, sortAndLimitResult.Count);
            Assert.AreEqual(9, sortAndLimitResult[0]._id);
            Assert.AreEqual(8, sortAndLimitResult[1]._id);
            Assert.AreEqual(7, sortAndLimitResult[2]._id);

            Assert.AreEqual(3, skipAndLimitResult.Count);
            Assert.AreEqual(4, skipAndLimitResult[0]._id);
            Assert.AreEqual(5, skipAndLimitResult[1]._id);
            Assert.AreEqual(6, skipAndLimitResult[2]._id);

            Assert.AreEqual(3, sortSkipAndLimitResult.Count);
            Assert.AreEqual(6, sortSkipAndLimitResult[0]._id);
            Assert.AreEqual(5, sortSkipAndLimitResult[1]._id);
            Assert.AreEqual(4, sortSkipAndLimitResult[2]._id);
        }

        [Test]
        public void ShouldFindWithProjection()
        {
            // Given
            var storage = MongoTestHelpers.GetEmptyStorageProvider<ComplexInventory>(nameof(ShouldFindWithProjection));

            // When

            storage.InsertOne(new ComplexInventory
            {
                _id = 1,
                type = "food",
                item = "Super Dark Chocolate",
                ratings = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 },
                classification = new ComplexInventoryClassification
                {
                    dept = "grocery",
                    category = "chocolate"
                },
                vendor = new ComplexInventoryVendor
                {
                    primary = new ComplexInventoryVendorInfo
                    {
                        name = "Marsupial Vending Co",
                        address = "Wallaby Rd",
                        delivery = new[] { "M", "W", "F" }
                    },
                    secondary = new ComplexInventoryVendorInfo
                    {
                        name = "Intl. Chocolatiers",
                        address = "Cocoa Plaza",
                        delivery = new[] { "Sa" }
                    }
                }
            });

            var specifiedFieldsWithIdResult = storage.Find().Project(i => new { i._id, i.type, i.item }).ToList();
            var specifiedFieldsWithoutIdResult = storage.Find().Project(i => new { i.type, i.item }).ToList();
            var specificFieldsInEmbeddedDocumentsResult = storage.Find().Project(i => new { i.classification.category }).ToList();
            var sliceFirstCountArrayItemsResult = storage.Find().Project(i => new { ratings = i.ratings.Take(2) }).ToList();
            var sliceLastCountArrayItemsResult = storage.Find().Project(i => new { ratings = i.ratings.Skip(i.ratings.Count() - 2) }).ToList();
            var sliceFirstArrayItemsResult = storage.Find().Project(i => new { ratings = i.ratings.Skip(5).Take(2) }).ToList();
            var sliceLastArrayItemsResult = storage.Find().Project(i => new { ratings = i.ratings.Skip(i.ratings.Count() - 5).Take(2) }).ToList();
            var sliceFirstCountWithOutOfRangeResult = storage.Find().Project(i => new { ratings = i.ratings.Take(100) }).ToList();
            var sliceLastCountWithOutOfRangeResult = storage.Find().Project(i => new { ratings = i.ratings.Skip(i.ratings.Count() - 100) }).ToList();
            var sliceFirstWithOutOfRangeResult = storage.Find().Project(i => new { ratings = i.ratings.Skip(5).Take(100) }).ToList();
            var sliceLastWithOutOfRangeResult = storage.Find().Project(i => new { ratings = i.ratings.Skip(i.ratings.Count() - 5).Take(100) }).ToList();

            // Then

            Assert.AreEqual(1, specifiedFieldsWithIdResult.Count);
            Assert.AreEqual(1, specifiedFieldsWithIdResult[0]._id);
            Assert.AreEqual("food", specifiedFieldsWithIdResult[0].type);
            Assert.AreEqual("Super Dark Chocolate", specifiedFieldsWithIdResult[0].item);

            Assert.AreEqual(1, specifiedFieldsWithoutIdResult.Count);
            Assert.AreEqual("food", specifiedFieldsWithoutIdResult[0].type);
            Assert.AreEqual("Super Dark Chocolate", specifiedFieldsWithoutIdResult[0].item);

            Assert.AreEqual(1, specificFieldsInEmbeddedDocumentsResult.Count);
            Assert.AreEqual("chocolate", specificFieldsInEmbeddedDocumentsResult[0].category);

            Assert.AreEqual(1, sliceFirstCountArrayItemsResult.Count);
            CollectionAssert.AreEqual(new[] { 1, 2 }, sliceFirstCountArrayItemsResult[0].ratings);

            Assert.AreEqual(1, sliceLastCountArrayItemsResult.Count);
            CollectionAssert.AreEqual(new[] { 8, 9 }, sliceLastCountArrayItemsResult[0].ratings);

            Assert.AreEqual(1, sliceFirstArrayItemsResult.Count);
            CollectionAssert.AreEqual(new[] { 6, 7 }, sliceFirstArrayItemsResult[0].ratings);

            Assert.AreEqual(1, sliceLastArrayItemsResult.Count);
            CollectionAssert.AreEqual(new[] { 5, 6 }, sliceLastArrayItemsResult[0].ratings);

            Assert.AreEqual(1, sliceFirstCountWithOutOfRangeResult.Count);
            CollectionAssert.AreEqual(new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 }, sliceFirstCountWithOutOfRangeResult[0].ratings);

            Assert.AreEqual(1, sliceLastCountWithOutOfRangeResult.Count);
            CollectionAssert.AreEqual(new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 }, sliceLastCountWithOutOfRangeResult[0].ratings);

            Assert.AreEqual(1, sliceFirstWithOutOfRangeResult.Count);
            CollectionAssert.AreEqual(new[] { 6, 7, 8, 9 }, sliceFirstWithOutOfRangeResult[0].ratings);

            Assert.AreEqual(1, sliceLastWithOutOfRangeResult.Count);
            CollectionAssert.AreEqual(new[] { 5, 6, 7, 8, 9 }, sliceLastWithOutOfRangeResult[0].ratings);
        }

        [Test]
        public void ShouldFindWithProjectionWithMatchArray()
        {
            // Given
            var storage = MongoTestHelpers.GetEmptyStorageProvider<School>(nameof(ShouldFindWithProjectionWithMatchArray));

            // When

            storage.InsertMany(new[]
                               {
                                   new School
                                   {
                                       _id = 1,
                                       zipcode = "63109",
                                       students = new[]
                                                  {
                                                      new SchoolStudent { name = "john", school = 102, age = 10 },
                                                      new SchoolStudent { name = "jess", school = 102, age = 11 },
                                                      new SchoolStudent { name = "jeff", school = 108, age = 15 }
                                                  }
                                   },
                                   new School
                                   {
                                       _id = 2,
                                       zipcode = "63110",
                                       students = new[]
                                                  {
                                                      new SchoolStudent { name = "ajax", school = 100, age = 7 },
                                                      new SchoolStudent { name = "achilles", school = 100, age = 8 }
                                                  }
                                   },
                                   new School
                                   {
                                       _id = 3,
                                       zipcode = "63110",
                                       students = new[]
                                                  {
                                                      new SchoolStudent { name = "ajax", school = 100, age = 7 },
                                                      new SchoolStudent { name = "achilles", school = 100, age = 8 }
                                                  }
                                   },
                                   new School
                                   {
                                       _id = 4,
                                       zipcode = "63109",
                                       students = new[]
                                                  {
                                                      new SchoolStudent { name = "barney", school = 102, age = 7 },
                                                      new SchoolStudent { name = "ruth", school = 102, age = 16 }
                                                  }
                                   }
                               });

            // Note: MongoDB.Driver 2.2.3 при использовании делает фильтрацию на клиенте, если проекцию записывать в виде lambda-выражения!
            var simpleMatchResult = storage.Find(i => i.zipcode == "63109").Project(i => new { i._id, students = i.students.Where(s => s.school == 102) }).ToList();
            var complexMatchResult = storage.Find(i => i.zipcode == "63109").Project(i => new { i._id, students = i.students.Where(s => s.school == 102 && s.age > 10) }).ToList();

            // Then

            Assert.AreEqual(2, simpleMatchResult.Count);
            Assert.AreEqual(1, simpleMatchResult[0]._id);
            Assert.AreNotEqual(null, simpleMatchResult[0].students);
            Assert.AreEqual(2, simpleMatchResult[0].students.Count()); // Note: Должен быть 1 элемент! Проблема в MongoDB.Driver 2.2.3
            Assert.AreEqual("john", simpleMatchResult[0].students.ElementAt(0).name);
            Assert.AreEqual(102, simpleMatchResult[0].students.ElementAt(0).school);
            Assert.AreEqual(10, simpleMatchResult[0].students.ElementAt(0).age);
            Assert.AreEqual(4, simpleMatchResult[1]._id);
            Assert.AreNotEqual(null, simpleMatchResult[1].students);
            Assert.AreEqual(2, simpleMatchResult[1].students.Count()); // Note: Должен быть 1 элемент! Проблема в MongoDB.Driver 2.2.3
            Assert.AreEqual("barney", simpleMatchResult[1].students.ElementAt(0).name);
            Assert.AreEqual(102, simpleMatchResult[1].students.ElementAt(0).school);
            Assert.AreEqual(7, simpleMatchResult[1].students.ElementAt(0).age);

            Assert.AreEqual(2, complexMatchResult.Count);
            Assert.AreEqual(1, complexMatchResult[0]._id);
            Assert.AreNotEqual(null, complexMatchResult[0].students);
            Assert.AreEqual(1, complexMatchResult[0].students.Count());
            Assert.AreEqual("jess", complexMatchResult[0].students.ElementAt(0).name);
            Assert.AreEqual(102, complexMatchResult[0].students.ElementAt(0).school);
            Assert.AreEqual(11, complexMatchResult[0].students.ElementAt(0).age);
            Assert.AreEqual(4, complexMatchResult[1]._id);
            Assert.AreNotEqual(null, complexMatchResult[1].students);
            Assert.AreEqual(1, complexMatchResult[1].students.Count());
            Assert.AreEqual("ruth", complexMatchResult[1].students.ElementAt(0).name);
            Assert.AreEqual(102, complexMatchResult[1].students.ElementAt(0).school);
            Assert.AreEqual(16, complexMatchResult[1].students.ElementAt(0).age);
        }

        [Test]
        public void ShouldInsertOne()
        {
            // Given
            var storage = MongoTestHelpers.GetEmptyStorageProvider<SimpleEntity>(nameof(ShouldInsertOne));

            // When
            storage.InsertOne(new SimpleEntity { _id = 1 });
            var result = storage.Count();

            // Then

            Assert.AreEqual(1, result);
        }

        [Test]
        public async Task ShouldInsertOneAsync()
        {
            // Given
            var storage = MongoTestHelpers.GetEmptyStorageProvider<SimpleEntity>(nameof(ShouldInsertOne));

            // When
            await storage.InsertOneAsync(new SimpleEntity { _id = 1 });
            var result = storage.Count();

            // Then

            Assert.AreEqual(1, result);
        }

        [Test]
        public void ShouldInsertMany()
        {
            // Given
            var storage = MongoTestHelpers.GetEmptyStorageProvider<SimpleEntity>(nameof(ShouldInsertMany));

            // When
            storage.InsertMany(new[] { new SimpleEntity { _id = 1 }, new SimpleEntity { _id = 2 } });
            var result = storage.Count();

            // Then

            Assert.AreEqual(2, result);
        }

        [Test]
        public async Task ShouldInsertManyAsync()
        {
            // Given
            var storage = MongoTestHelpers.GetEmptyStorageProvider<SimpleEntity>(nameof(ShouldInsertManyAsync));

            // When
            await storage.InsertManyAsync(new[] { new SimpleEntity { _id = 1 }, new SimpleEntity { _id = 2 } });
            var result = storage.Count();

            // Then

            Assert.AreEqual(2, result);
        }

        [Test]
        public void ShouldUpdateOne()
        {
            // Given
            var storage = MongoTestHelpers.GetEmptyStorageProvider<SimpleEntity>(nameof(ShouldUpdateOne));

            // When
            storage.InsertMany(new[]
                               {
                                   new SimpleEntity { _id = 1, prop1 = "11" },
                                   new SimpleEntity { _id = 2, prop1 = "22" },
                                   new SimpleEntity { _id = 3, prop1 = "33" }
                               });

            var updateResult1 = storage.UpdateOne(u => u.Set(i => i.prop1, "44"));
            var updateResult2 = storage.UpdateOne(u => u.Set(i => i.prop1, "55"), i => i._id == (object)3);
            var updateResult3 = storage.UpdateOne(u => u.Set(i => i.prop1, "66"), i => i._id == (object)4);
            var updateResult4 = storage.UpdateOne(u => u.Set(i => i.prop1, "77"), i => i._id == (object)5, true);

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
            Assert.AreEqual("44", documents[0].prop1);
            Assert.AreEqual("22", documents[1].prop1);
            Assert.AreEqual("55", documents[2].prop1);
            Assert.AreEqual(5, documents[3]._id);
            Assert.AreEqual("77", documents[3].prop1);
        }

        [Test]
        public async Task ShouldUpdateOneAsync()
        {
            // Given
            var storage = MongoTestHelpers.GetEmptyStorageProvider<SimpleEntity>(nameof(ShouldUpdateOneAsync));

            // When
            storage.InsertMany(new[]
                               {
                                   new SimpleEntity { _id = 1, prop1 = "11" },
                                   new SimpleEntity { _id = 2, prop1 = "22" },
                                   new SimpleEntity { _id = 3, prop1 = "33" }
                               });

            var updateResult1 = await storage.UpdateOneAsync(u => u.Set(i => i.prop1, "44"));
            var updateResult2 = await storage.UpdateOneAsync(u => u.Set(i => i.prop1, "55"), i => i._id == (object)3);
            var updateResult3 = await storage.UpdateOneAsync(u => u.Set(i => i.prop1, "66"), i => i._id == (object)4);
            var updateResult4 = await storage.UpdateOneAsync(u => u.Set(i => i.prop1, "77"), i => i._id == (object)5, true);

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
            Assert.AreEqual("44", documents[0].prop1);
            Assert.AreEqual("22", documents[1].prop1);
            Assert.AreEqual("55", documents[2].prop1);
            Assert.AreEqual(5, documents[3]._id);
            Assert.AreEqual("77", documents[3].prop1);
        }

        [Test]
        public void ShoudlUpdateMany()
        {
            // Given
            var storage = MongoTestHelpers.GetEmptyStorageProvider<SimpleEntity>(nameof(ShoudlUpdateMany));

            // When
            storage.InsertMany(new[]
                               {
                                   new SimpleEntity { _id = 1, prop1 = "11", prop2 = 111 },
                                   new SimpleEntity { _id = 2, prop1 = "22", prop2 = 111 },
                                   new SimpleEntity { _id = 3, prop1 = "33", prop2 = 222 }
                               });

            var updateResult1 = storage.UpdateMany(u => u.Set(i => i.prop1, "44"));
            var updateResult2 = storage.UpdateMany(u => u.Set(i => i.prop2, 333), i => i.prop2 == 111);
            var updateResult3 = storage.UpdateMany(u => u.Set(i => i.prop2, 444), i => i._id == (object)4);
            var updateResult4 = storage.UpdateMany(u => u.Set(i => i.prop2, 555), i => i._id == (object)5 && i.prop1 == "55", true);

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

            Assert.AreEqual(1, documents[0]._id);
            Assert.AreEqual("44", documents[0].prop1);
            Assert.AreEqual(333, documents[0].prop2);

            Assert.AreEqual(2, documents[1]._id);
            Assert.AreEqual("44", documents[1].prop1);
            Assert.AreEqual(333, documents[1].prop2);

            Assert.AreEqual(3, documents[2]._id);
            Assert.AreEqual("44", documents[2].prop1);
            Assert.AreEqual(222, documents[2].prop2);

            Assert.AreEqual(5, documents[3]._id);
            Assert.AreEqual("55", documents[3].prop1);
            Assert.AreEqual(555, documents[3].prop2);
        }

        [Test]
        public async Task ShoudlUpdateManyAsync()
        {
            // Given
            var storage = MongoTestHelpers.GetEmptyStorageProvider<SimpleEntity>(nameof(ShoudlUpdateManyAsync));

            // When
            storage.InsertMany(new[]
                               {
                                   new SimpleEntity { _id = 1, prop1 = "11", prop2 = 111 },
                                   new SimpleEntity { _id = 2, prop1 = "22", prop2 = 111 },
                                   new SimpleEntity { _id = 3, prop1 = "33", prop2 = 222 }
                               });

            var updateResult1 = await storage.UpdateManyAsync(u => u.Set(i => i.prop1, "44"));
            var updateResult2 = await storage.UpdateManyAsync(u => u.Set(i => i.prop2, 333), i => i.prop2 == 111);
            var updateResult3 = await storage.UpdateManyAsync(u => u.Set(i => i.prop2, 444), i => i._id == (object)4);
            var updateResult4 = await storage.UpdateManyAsync(u => u.Set(i => i.prop2, 555), i => i._id == (object)5 && i.prop1 == "55", true);

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

            Assert.AreEqual(1, documents[0]._id);
            Assert.AreEqual("44", documents[0].prop1);
            Assert.AreEqual(333, documents[0].prop2);

            Assert.AreEqual(2, documents[1]._id);
            Assert.AreEqual("44", documents[1].prop1);
            Assert.AreEqual(333, documents[1].prop2);

            Assert.AreEqual(3, documents[2]._id);
            Assert.AreEqual("44", documents[2].prop1);
            Assert.AreEqual(222, documents[2].prop2);

            Assert.AreEqual(5, documents[3]._id);
            Assert.AreEqual("55", documents[3].prop1);
            Assert.AreEqual(555, documents[3].prop2);
        }

        [Test]
        public void ShouldUpdateWithRename()
        {
            // Given
            var storage = MongoTestHelpers.GetEmptyStorageProvider<RenameAndRemoveEntity>(nameof(ShouldUpdateWithRename));

            // When

            storage.InsertOne(new RenameAndRemoveEntity { prop1 = 123 });
            storage.UpdateOne(u => u.Rename(i => i.prop1, "prop2"));
            dynamic document = storage.Find().First();

            // Then
            Assert.AreEqual(123, document.prop2);
        }

        [Test]
        public void ShouldUpdateWithRemove()
        {
            // Given
            var storage = MongoTestHelpers.GetEmptyStorageProvider<RenameAndRemoveEntity>(nameof(ShouldUpdateWithRemove));

            // When

            storage.InsertOne(new RenameAndRemoveEntity { prop1 = 123 });
            storage.UpdateOne(u => u.Remove(i => i.prop1));
            dynamic document = storage.Find().First();

            // Then
            Assert.AreEqual(null, document.prop1);
        }

        [Test]
        public void ShouldUpdateWithSet()
        {
            // Given
            var storage = MongoTestHelpers.GetEmptyStorageProvider<SimpleEntity>(nameof(ShouldUpdateWithSet));

            // When

            storage.InsertOne(new SimpleEntity { _id = 1 });
            storage.UpdateOne(u => u.Set(i => i.prop1, "123"));
            dynamic document = storage.Find().First();

            // Then
            Assert.AreEqual("123", document.prop1);
        }

        [Test]
        public void ShouldUpdateWithInc()
        {
            // Given
            var storage = MongoTestHelpers.GetEmptyStorageProvider<MathEntity>(nameof(ShouldUpdateWithInc));

            // When

            storage.InsertOne(new MathEntity { intProp1 = 1, intProp2 = 2, doubleProp1 = 1.5, doubleProp2 = 2.5 });
            storage.UpdateOne(u => u.Inc(i => i.intProp1, 3).Inc(i => i.intProp2, -3).Inc(i => i.doubleProp1, 3.3).Inc(i => i.doubleProp2, -3.3));
            var document = storage.Find().First();

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
            var storage = MongoTestHelpers.GetEmptyStorageProvider<MathEntity>(nameof(ShouldUpdateWithMul));

            // When

            storage.InsertOne(new MathEntity { intProp1 = 2, intProp2 = 3, doubleProp1 = 1.5, doubleProp2 = 2.5 });
            storage.UpdateOne(u => u.Mul(i => i.intProp1, 4).Mul(i => i.intProp2, -5).Mul(i => i.doubleProp1, 3.3).Mul(i => i.doubleProp2, -3.3));
            var document = storage.Find().First();

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
            var storage = MongoTestHelpers.GetEmptyStorageProvider<MathEntity>(nameof(ShouldUpdateWithMin));

            // When

            storage.InsertOne(new MathEntity { intProp1 = 2, intProp2 = 2, doubleProp1 = 1.5, doubleProp2 = 1.5 });
            storage.UpdateOne(u => u.Min(i => i.intProp1, 1).Min(i => i.intProp2, 3).Min(i => i.doubleProp1, 1.3).Min(i => i.doubleProp2, 1.7));
            var document = storage.Find().First();

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
            var storage = MongoTestHelpers.GetEmptyStorageProvider<MathEntity>(nameof(ShouldUpdateWithMul));

            // When

            storage.InsertOne(new MathEntity { intProp1 = 2, intProp2 = 2, doubleProp1 = 1.5, doubleProp2 = 1.5 });
            storage.UpdateOne(u => u.Max(i => i.intProp1, 1).Max(i => i.intProp2, 3).Max(i => i.doubleProp1, 1.3).Max(i => i.doubleProp2, 1.7));
            var document = storage.Find().First();

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
            var storage = MongoTestHelpers.GetEmptyStorageProvider<MathEntity>(nameof(ShouldUpdateWithBitwiseOperations));

            // When

            storage.InsertOne(new MathEntity { intProp1 = 0x1101, intProp2 = 0x0011, intProp3 = 0x0001 });
            storage.UpdateOne(u => u.BitwiseAnd(i => i.intProp1, 0x1010).BitwiseOr(i => i.intProp2, 0x0101).BitwiseXor(i => i.intProp3, 0x0101));
            var document = storage.Find().First();

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
            var storage = MongoTestHelpers.GetEmptyStorageProvider<SimpleEntity>(nameof(ShouldUpdateWithCurrentDate));

            // When
            storage.InsertOne(new SimpleEntity { _id = 1 });
            storage.UpdateOne(u => u.CurrentDate(i => i.date));
            var document = storage.Find().First();

            // Then
            Assert.IsNotNull(document.date);
            Assert.IsTrue(Math.Abs((nowDate - document.date.Value).TotalSeconds) < 30);
        }

        [Test]
        public void ShouldUpdateWithPush()
        {
            // Given
            var storage = MongoTestHelpers.GetEmptyStorageProvider<PushAndPullEntity>(nameof(ShouldUpdateWithPush));

            // When

            storage.InsertOne(new PushAndPullEntity { _id = 1, propPush = new int[0], propPushAll = new int[0], propPushUnique = new int[0], propPushAllUnique = new int[0] });

            storage.UpdateOne(u => u.Push(i => i.propPush, 1).PushAll(i => i.propPushAll, new[] { 11, 22 }).PushUnique(i => i.propPushUnique, 111).PushAllUnique(i => i.propPushAllUnique, new[] { 1111, 2222 }));
            storage.UpdateOne(u => u.Push(i => i.propPush, 1).PushAll(i => i.propPushAll, new[] { 22, 33 }).PushUnique(i => i.propPushUnique, 111).PushAllUnique(i => i.propPushAllUnique, new[] { 2222, 3333 }));
            storage.UpdateOne(u => u.Push(i => i.propPush, 2).PushAll(i => i.propPushAll, new[] { 33, 44 }).PushUnique(i => i.propPushUnique, 222).PushAllUnique(i => i.propPushAllUnique, new[] { 3333, 4444 }));
            storage.UpdateOne(u => u.Push(i => i.propPush, 2).PushAll(i => i.propPushAll, new[] { 44, 55 }).PushUnique(i => i.propPushUnique, 222).PushAllUnique(i => i.propPushAllUnique, new[] { 4444, 5555 }));

            var document = storage.Find().First();

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
            var storage = MongoTestHelpers.GetEmptyStorageProvider<PushAndPullEntity>(nameof(ShouldUpdateWithPopAndPull));

            // When

            storage.InsertOne(new PushAndPullEntity
            {
                propPopFirst = new[] { 1, 2, 3, 4 },
                propPopLast = new[] { 5, 6, 7, 8 },
                propPull = new[] { 11, 22, 33 },
                propPullAll = new[] { 44, 55, 66 },
                propPullFilter = new[] { new PushAndPullEntity.Item { value = 111 }, new PushAndPullEntity.Item { value = 111 }, new PushAndPullEntity.Item { value = 222 } }
            });

            storage.UpdateOne(u => u.PopFirst(i => i.propPopFirst)
                                    .PopLast(i => i.propPopLast)
                                    .Pull(i => i.propPull, 22)
                                    .PullAll(i => i.propPullAll, new[] { 44, 66 })
                                    .PullFilter(i => i.propPullFilter, i => i.value == 111)
                                    );

            var document = storage.Find().First();

            // Then
            CollectionAssert.AreEqual(new[] { 2, 3, 4 }, document.propPopFirst);
            CollectionAssert.AreEqual(new[] { 5, 6, 7 }, document.propPopLast);
            CollectionAssert.AreEqual(new[] { 11, 33 }, document.propPull);
            CollectionAssert.AreEqual(new[] { 55 }, document.propPullAll);
            Assert.AreEqual(1, document.propPullFilter.Count());
            Assert.AreEqual(222, document.propPullFilter.ElementAt(0).value);
        }

        [Test]
        public void ShouldReplaceOne()
        {
            // Given
            var storage = MongoTestHelpers.GetEmptyStorageProvider<ReplaceEntity>(nameof(ShouldReplaceOne));

            // When

            storage.InsertMany(new[]
                               {
                                   new ReplaceEntity { _id = 1 },
                                   new ReplaceEntity { _id = 2, prop1 = 22, prop2 = "A" },
                                   new ReplaceEntity { _id = 3, prop1 = 33, prop2 = "A" }
                               });

            var replaceResult1 = storage.ReplaceOne(new ReplaceEntity { _id = 1, prop1 = 11 });
            var replaceResult2 = storage.ReplaceOne(new ReplaceEntity { _id = 2, prop1 = 22, prop2 = "B" }, i => i.prop2 == "A");
            var replaceResult3 = storage.ReplaceOne(new ReplaceEntity { prop1 = 44, prop2 = "C" }, i => i._id == (object)4);
            var replaceResult4 = storage.ReplaceOne(new ReplaceEntity { _id = 5, prop1 = 55, prop2 = "D" }, i => i._id == (object)5, true);

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

            Assert.AreEqual(1, documents[0]._id);
            Assert.AreEqual(11, documents[0].prop1);
            Assert.AreEqual(null, documents[0].prop2);

            Assert.AreEqual(2, documents[1]._id);
            Assert.AreEqual(22, documents[1].prop1);
            Assert.AreEqual("B", documents[1].prop2);

            Assert.AreEqual(3, documents[2]._id);
            Assert.AreEqual(33, documents[2].prop1);
            Assert.AreEqual("A", documents[2].prop2);

            Assert.AreEqual(5, documents[3]._id);
            Assert.AreEqual(55, documents[3].prop1);
            Assert.AreEqual("D", documents[3].prop2);
        }

        [Test]
        public async Task ShouldReplaceOneAsync()
        {
            // Given
            var storage = MongoTestHelpers.GetEmptyStorageProvider<ReplaceEntity>(nameof(ShouldReplaceOneAsync));

            // When

            storage.InsertMany(new[]
                               {
                                   new ReplaceEntity { _id = 1 },
                                   new ReplaceEntity { _id = 2, prop1 = 22, prop2 = "A" },
                                   new ReplaceEntity { _id = 3, prop1 = 33, prop2 = "A" }
                               });

            var replaceResult1 = await storage.ReplaceOneAsync(new ReplaceEntity { _id = 1, prop1 = 11 });
            var replaceResult2 = await storage.ReplaceOneAsync(new ReplaceEntity { _id = 2, prop1 = 22, prop2 = "B" }, i => i.prop2 == "A");
            var replaceResult3 = await storage.ReplaceOneAsync(new ReplaceEntity { prop1 = 44, prop2 = "C" }, i => i._id == (object)4);
            var replaceResult4 = await storage.ReplaceOneAsync(new ReplaceEntity { _id = 5, prop1 = 55, prop2 = "D" }, i => i._id == (object)5, true);

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

            Assert.AreEqual(1, documents[0]._id);
            Assert.AreEqual(11, documents[0].prop1);
            Assert.AreEqual(null, documents[0].prop2);

            Assert.AreEqual(2, documents[1]._id);
            Assert.AreEqual(22, documents[1].prop1);
            Assert.AreEqual("B", documents[1].prop2);

            Assert.AreEqual(3, documents[2]._id);
            Assert.AreEqual(33, documents[2].prop1);
            Assert.AreEqual("A", documents[2].prop2);

            Assert.AreEqual(5, documents[3]._id);
            Assert.AreEqual(55, documents[3].prop1);
            Assert.AreEqual("D", documents[3].prop2);
        }

        [Test]
        public void ShouldDeleteOne()
        {
            // Given
            var storage = MongoTestHelpers.GetEmptyStorageProvider<SimpleEntity>(nameof(ShouldDeleteOne));

            // When

            storage.InsertMany(new[]
                               {
                                   new SimpleEntity { _id = 1, prop1 = "11" }, // first delete
                                   new SimpleEntity { _id = 2, prop1 = "11" },
                                   new SimpleEntity { _id = 3, prop1 = "22" }, // second delete
                                   new SimpleEntity { _id = 4, prop1 = "22" },
                                   new SimpleEntity { _id = 5, prop1 = "33" }
                               });

            var deleteResult1 = storage.DeleteOne();
            var deleteResult2 = storage.DeleteOne(i => i.prop1 == "22");
            var deleteResult3 = storage.DeleteOne(i => i.prop1 == "44");

            var documents = storage.Find().ToList();

            // Then

            Assert.AreEqual(1, deleteResult1);
            Assert.AreEqual(1, deleteResult2);
            Assert.AreEqual(0, deleteResult3);

            Assert.AreEqual(3, documents.Count);
            Assert.AreEqual(2, documents[0]._id);
            Assert.AreEqual(4, documents[1]._id);
            Assert.AreEqual(5, documents[2]._id);
        }

        [Test]
        public async Task ShouldDeleteOneAsync()
        {
            // Given
            var storage = MongoTestHelpers.GetEmptyStorageProvider<SimpleEntity>(nameof(ShouldDeleteOneAsync));

            // When

            storage.InsertMany(new[]
                               {
                                   new SimpleEntity { _id = 1, prop1 = "11" }, // first delete
                                   new SimpleEntity { _id = 2, prop1 = "11" },
                                   new SimpleEntity { _id = 3, prop1 = "22" }, // second delete
                                   new SimpleEntity { _id = 4, prop1 = "22" },
                                   new SimpleEntity { _id = 5, prop1 = "33" }
                               });

            var deleteResult1 = await storage.DeleteOneAsync();
            var deleteResult2 = await storage.DeleteOneAsync(i => i.prop1 == "22");
            var deleteResult3 = await storage.DeleteOneAsync(i => i.prop1 == "44");

            var documents = storage.Find().ToList();

            // Then

            Assert.AreEqual(1, deleteResult1);
            Assert.AreEqual(1, deleteResult2);
            Assert.AreEqual(0, deleteResult3);

            Assert.AreEqual(3, documents.Count);
            Assert.AreEqual(2, documents[0]._id);
            Assert.AreEqual(4, documents[1]._id);
            Assert.AreEqual(5, documents[2]._id);
        }

        [Test]
        public void ShouldDeleteMany()
        {
            // Given
            var storage = MongoTestHelpers.GetEmptyStorageProvider<SimpleEntity>(nameof(ShouldDeleteMany));

            // When

            storage.InsertMany(new[]
                               {
                                   new SimpleEntity { _id = 1, prop1 = "11" },
                                   new SimpleEntity { _id = 2, prop1 = "11" },
                                   new SimpleEntity { _id = 3, prop1 = "22" },
                                   new SimpleEntity { _id = 4, prop1 = "22" },
                                   new SimpleEntity { _id = 5, prop1 = "33" }
                               });

            var deleteResult1 = storage.DeleteMany();

            storage.InsertMany(new[]
                               {
                                   new SimpleEntity { _id = 1, prop1 = "11" }, // second delete
                                   new SimpleEntity { _id = 2, prop1 = "11" }, // second delete
                                   new SimpleEntity { _id = 3, prop1 = "22" }, // third delete
                                   new SimpleEntity { _id = 4, prop1 = "22" }, // third delete
                                   new SimpleEntity { _id = 5, prop1 = "33" }
                               });

            var deleteResult2 = storage.DeleteMany(i => i.prop1 == "11");
            var deleteResult3 = storage.DeleteMany(i => i.prop1 == "22");
            var deleteResult4 = storage.DeleteMany(i => i.prop1 == "44");

            var documents = storage.Find().ToList();

            // Then

            Assert.AreEqual(5, deleteResult1);
            Assert.AreEqual(2, deleteResult2);
            Assert.AreEqual(2, deleteResult3);
            Assert.AreEqual(0, deleteResult4);

            Assert.AreEqual(1, documents.Count);
            Assert.AreEqual(5, documents[0]._id);
        }

        [Test]
        public async Task ShouldDeleteManyAsync()
        {
            // Given
            var storage = MongoTestHelpers.GetEmptyStorageProvider<SimpleEntity>(nameof(ShouldDeleteManyAsync));

            // When

            storage.InsertMany(new[]
                               {
                                   new SimpleEntity { _id = 1, prop1 = "11" },
                                   new SimpleEntity { _id = 2, prop1 = "11" },
                                   new SimpleEntity { _id = 3, prop1 = "22" },
                                   new SimpleEntity { _id = 4, prop1 = "22" },
                                   new SimpleEntity { _id = 5, prop1 = "33" }
                               });

            var deleteResult1 = await storage.DeleteManyAsync();

            storage.InsertMany(new[]
                               {
                                   new SimpleEntity { _id = 1, prop1 = "11" }, // second delete
                                   new SimpleEntity { _id = 2, prop1 = "11" }, // second delete
                                   new SimpleEntity { _id = 3, prop1 = "22" }, // third delete
                                   new SimpleEntity { _id = 4, prop1 = "22" }, // third delete
                                   new SimpleEntity { _id = 5, prop1 = "33" }
                               });

            var deleteResult2 = await storage.DeleteManyAsync(i => i.prop1 == "11");
            var deleteResult3 = await storage.DeleteManyAsync(i => i.prop1 == "22");
            var deleteResult4 = await storage.DeleteManyAsync(i => i.prop1 == "44");

            var documents = storage.Find().ToList();

            // Then

            Assert.AreEqual(5, deleteResult1);
            Assert.AreEqual(2, deleteResult2);
            Assert.AreEqual(2, deleteResult3);
            Assert.AreEqual(0, deleteResult4);

            Assert.AreEqual(1, documents.Count);
            Assert.AreEqual(5, documents[0]._id);
        }

        [Test]
        public void ShouldAggregateWithGroup()
        {
            // Given
            var storage = MongoTestHelpers.GetEmptyStorageProvider<Sale>(nameof(ShouldAggregateWithGroup));

            // When

            storage.InsertMany(new[]
                               {
                                   new Sale { _id = 1, item = "abc", price = 10, quantity = 2, date = DateTime.Parse("2016-03-01T08:00:00Z") },
                                   new Sale { _id = 2, item = "jkl", price = 20, quantity = 1, date = DateTime.Parse("2016-03-01T09:00:00Z") },
                                   new Sale { _id = 3, item = "xyz", price = 5, quantity = 10, date = DateTime.Parse("2016-03-15T09:00:00Z") },
                                   new Sale { _id = 4, item = "xyz", price = 5, quantity = 20, date = DateTime.Parse("2016-04-04T11:21:39.736Z") },
                                   new Sale { _id = 5, item = "abc", price = 10, quantity = 10, date = DateTime.Parse("2016-04-04T21:23:13.331Z") }
                               });

            var groupByMonthDayYearResult = storage.Aggregate()
                                                   .Group(i => new { month = i.date.Month, day = i.date.Day, year = i.date.Year },
                                                       g => new
                                                       {
                                                           _id = g.Key,
                                                           totalPrice = g.Sum(i => i.price * i.quantity),
                                                           averageQuantity = g.Average(i => i.quantity),
                                                           count = g.Count()
                                                       })
                                                   .ToList();

            // Then

            Assert.AreEqual(3, groupByMonthDayYearResult.Count);

            var item1 = groupByMonthDayYearResult.FirstOrDefault(i => i._id != null && i._id.month == 3 && i._id.day == 15 && i._id.year == 2016);
            Assert.IsNotNull(item1);
            Assert.AreEqual(3, item1._id.month);
            Assert.AreEqual(15, item1._id.day);
            Assert.AreEqual(2016, item1._id.year);
            Assert.AreEqual(50, item1.totalPrice);
            Assert.AreEqual(10, item1.averageQuantity);
            Assert.AreEqual(1, item1.count);

            var item2 = groupByMonthDayYearResult.FirstOrDefault(i => i._id != null && i._id.month == 4 && i._id.day == 4 && i._id.year == 2016);
            Assert.IsNotNull(item2);
            Assert.AreEqual(4, item2._id.month);
            Assert.AreEqual(4, item2._id.day);
            Assert.AreEqual(2016, item2._id.year);
            Assert.AreEqual(200, item2.totalPrice);
            Assert.AreEqual(15, item2.averageQuantity);
            Assert.AreEqual(2, item2.count);

            var item3 = groupByMonthDayYearResult.FirstOrDefault(i => i._id != null && i._id.month == 3 && i._id.day == 1 && i._id.year == 2016);
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
            var storage = MongoTestHelpers.GetEmptyStorageProvider<SimpleEntity>(nameof(ShouldBulk));

            // When

            storage.InsertMany(new[]
                               {
                                   new SimpleEntity { _id = 1, prop1 = "1" },
                                   new SimpleEntity { _id = 2, prop1 = "1" },
                                   new SimpleEntity { _id = 3, prop1 = "2" },
                                   new SimpleEntity { _id = 4, prop1 = "2" },
                                   new SimpleEntity { _id = 5, prop1 = "3" }
                               });

            var result = storage.Bulk(b => b.InsertOne(new SimpleEntity { _id = 6, prop1 = "6" })
                                            .UpdateOne(u => u.Set(i => i.prop1, "11"), i => i.prop1 == "1")
                                            .UpdateMany(u => u.Set(i => i.prop1, "22"), i => i.prop1 == "2")
                                            .ReplaceOne(new SimpleEntity { _id = 7, prop1 = "7" }, i => i._id == (object)7, true)
                                            .DeleteOne(i => i.prop1 == "3"));

            var documents = storage.Find().ToList();

            // Then

            Assert.IsNotNull(result);
            Assert.AreEqual(5, result.RequestCount);
            Assert.AreEqual(3, result.ModifiedCount);
            Assert.AreEqual(1, result.InsertedCount);
            Assert.AreEqual(1, result.DeletedCount);

            Assert.AreEqual(6, documents.Count);
            Assert.AreEqual(1, documents[0]._id);
            Assert.AreEqual("11", documents[0].prop1);
            Assert.AreEqual(2, documents[1]._id);
            Assert.AreEqual("1", documents[1].prop1);
            Assert.AreEqual(3, documents[2]._id);
            Assert.AreEqual("22", documents[2].prop1);
            Assert.AreEqual(4, documents[3]._id);
            Assert.AreEqual("22", documents[3].prop1);
            Assert.AreEqual(6, documents[4]._id);
            Assert.AreEqual("6", documents[4].prop1);
            Assert.AreEqual(7, documents[5]._id);
            Assert.AreEqual("7", documents[5].prop1);
        }

        [Test]
        public void ShouldUseDocumentTypeAttribute()
        {
            // Given

            var value1 = Guid.NewGuid().ToString();
            var value2 = Guid.NewGuid().ToString();

            var storageWithSpecifiedName = MongoTestHelpers.GetEmptyStorageProvider<FakeDocument>("FakeDocumentCollection");
            var storageWithDefaultName = MongoTestHelpers.GetStorageProvider<FakeDocument>();

            // When

            storageWithSpecifiedName.InsertOne(new FakeDocument { _id = 1, prop2 = value1 });
            storageWithDefaultName.InsertOne(new FakeDocument { _id = 2, prop2 = value2 });

            var storageWithSpecifiedNameDocuments = storageWithDefaultName.Find().ToList();
            var storageWithDefaultNameDocuments = storageWithDefaultName.Find().ToList();

            // Then

            Assert.AreEqual(2, storageWithSpecifiedNameDocuments.Count);
            Assert.AreEqual(1, storageWithSpecifiedNameDocuments[0]._id);
            Assert.AreEqual(value1, storageWithSpecifiedNameDocuments[0].prop2);
            Assert.AreEqual(2, storageWithSpecifiedNameDocuments[1]._id);
            Assert.AreEqual(value2, storageWithSpecifiedNameDocuments[1].prop2);

            Assert.AreEqual(2, storageWithDefaultNameDocuments.Count);
            Assert.AreEqual(1, storageWithDefaultNameDocuments[0]._id);
            Assert.AreEqual(value1, storageWithDefaultNameDocuments[0].prop2);
            Assert.AreEqual(2, storageWithDefaultNameDocuments[1]._id);
            Assert.AreEqual(value2, storageWithDefaultNameDocuments[1].prop2);
        }

        [Test]
        public void ShouldApplyDocumentIgnorePropertyAttribute()
        {
            // Given
            var document1 = new DocumentWithIgnore { _id = 1, Property1 = 11, Property2Ignore = 12, Property3 = 13, Property4Ignore = 14 };
            var document2 = new DocumentWithIgnore { _id = 2, Property1 = 21, Property2Ignore = 22, Property3 = 23, Property4Ignore = 24 };
            var storage = MongoTestHelpers.GetEmptyStorageProvider<DocumentWithIgnore>(nameof(ShouldApplyDocumentIgnorePropertyAttribute));

            // When
            storage.InsertMany(new[] { document1, document2 });
            var actualDocument1 = storage.Find(i => i._id == document1._id).FirstOrDefault();
            var actualDocument2 = storage.Find(i => i._id == document2._id).FirstOrDefault();

            // Then

            Assert.IsNotNull(actualDocument1);
            Assert.AreEqual(document1._id, actualDocument1._id);
            Assert.AreEqual(document1.Property1, actualDocument1.Property1);
            Assert.AreEqual(default(int), actualDocument1.Property2Ignore);
            Assert.AreEqual(document1.Property3, actualDocument1.Property3);
            Assert.AreEqual(default(int), actualDocument1.Property4Ignore);

            Assert.IsNotNull(actualDocument2);
            Assert.AreEqual(document2._id, actualDocument2._id);
            Assert.AreEqual(document2.Property1, actualDocument2.Property1);
            Assert.AreEqual(default(int), actualDocument2.Property2Ignore);
            Assert.AreEqual(document2.Property3, actualDocument2.Property3);
            Assert.AreEqual(default(int), actualDocument2.Property4Ignore);
        }
    }
}