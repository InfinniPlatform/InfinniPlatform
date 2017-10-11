using System;
using System.Collections.Generic;

using InfinniPlatform.Tests;

using NUnit.Framework;

namespace InfinniPlatform.Dynamic
{
    [TestFixture]
    [Category(TestCategories.UnitTest)]
    public class DynamicCollectionExtensionsTest
    {
        [Test]
        public void AddItemShouldThrowExceptionWhenCollectionIsNull()
        {
            // When
            void Test() => DynamicCollectionExtensions.AddItem(null, new object());

            // Then
            Assert.Throws<ArgumentNullException>(Test);
        }

        [Test]
        public void AddItemShouldThrowExceptionWhenTargetIsNotCollection()
        {
            // Given
            var target = new object();

            // When
            void Test() => target.AddItem(new object());

            // Then
            Assert.Throws<NotSupportedException>(Test);
        }

        [Test]
        public void AddItemShouldThrowExceptionWhenReadOnlyCollection()
        {
            // Given
            var target = new int[] { };

            // When
            void Test() => target.AddItem(11);

            // Then
            Assert.Throws<NotSupportedException>(Test);
        }

        [Test]
        public void InsertItemShouldThrowExceptionWhenCollectionIsNull()
        {
            // When
            void Test() => DynamicCollectionExtensions.InsertItem(null, 0, new object());

            // Then
            Assert.Throws<ArgumentNullException>(Test);
        }

        [Test]
        public void InsertItemShouldThrowExceptionWhenTargetIsNotCollection()
        {
            // Given
            var target = new object();

            // When
            void Test() => target.InsertItem(0, new object());

            // Then
            Assert.Throws<NotSupportedException>(Test);
        }

        [Test]
        public void InsertItemShouldThrowExceptionWhenReadOnlyCollection()
        {
            // Given
            var target = new int[] { };

            // When
            void Test() => target.InsertItem(0, 11);

            // Then
            Assert.Throws<NotSupportedException>(Test);
        }

        [Test]
        public void RemoveItemAtShouldThrowExceptionWhenCollectionIsNull()
        {
            // When
            void Test() => DynamicCollectionExtensions.RemoveItemAt(null, 0);

            // Then
            Assert.Throws<ArgumentNullException>(Test);
        }

        [Test]
        public void RemoveItemAtShouldThrowExceptionWhenTargetIsNotCollection()
        {
            // Given
            var target = new object();

            // When
            void Test() => target.RemoveItemAt(0);

            // Then
            Assert.Throws<NotSupportedException>(Test);
        }

        [Test]
        public void RemoveItemAtShouldThrowExceptionWhenReadOnlyCollection()
        {
            // Given
            var target = new[] { 11 };

            // When
            void Test() => target.RemoveItemAt(0);

            // Then
            Assert.Throws<NotSupportedException>(Test);
        }

        [Test]
        public void RemoveItemShouldThrowExceptionWhenCollectionIsNull()
        {
            // When
            void Test() => DynamicCollectionExtensions.RemoveItem(null, new object());

            // Then
            Assert.Throws<ArgumentNullException>(Test);
        }

        [Test]
        public void RemoveItemShouldThrowExceptionWhenReadOnlyCollection()
        {
            // Given
            var target = new int[] { };

            // When
            void Test() => target.RemoveItem(11);

            // Then
            Assert.Throws<NotSupportedException>(Test);
        }

        [Test]
        public void RemoveItemThrowExceptionWhenTargetIsNotCollection()
        {
            // Given
            var target = new object();

            // When
            void Test() => target.RemoveItem(new object());

            // Then
            Assert.Throws<NotSupportedException>(Test);
        }

        [Test]
        public void SetItemShouldThrowExceptionWhenCollectionIsNull()
        {
            // When
            void Test() => DynamicCollectionExtensions.SetItem(null, 0, new object());

            // Then
            Assert.Throws<ArgumentNullException>(Test);
        }

        [Test]
        public void SetItemShouldThrowExceptionWhenTargetIsNotCollection()
        {
            // Given
            var target = new object();

            // When
            void Test() => target.SetItem(0, new object());

            // Then
            Assert.Throws<NotSupportedException>(Test);
        }

        [Test]
        public void SetPropertyShouldThrowExceptionWhenPropertyIsNull()
        {
            // When
            void Test1() => DynamicObjectExtensions.TrySetPropertyValueByPath(null, null, new object());
            void Test2() => DynamicObjectExtensions.TrySetPropertyValueByPath(null, string.Empty, new object());

            // Then
            Assert.Throws<ArgumentNullException>(Test1);
            Assert.Throws<ArgumentNullException>(Test2);
        }

        [Test]
        public void SetPropertyShouldThrowExceptionWhenTargetIsNull()
        {
            // When
            void Test() => DynamicObjectExtensions.TrySetPropertyValueByPath(null, "Property1", new object());

            // Then
            Assert.Throws<ArgumentNullException>(Test);
        }

        [Test]
        public void SholdAddItemToList()
        {
            // Given
            var target = new List<int>();

            // When
            target.AddItem(11);
            target.AddItem(22);
            target.AddItem(33);

            // Then
            CollectionAssert.AreEqual(new[] { 11, 22, 33 }, target);
        }

        [Test]
        public void SholdInsertItemToList()
        {
            // Given
            var target = new List<int> { 11, 33 };

            // When
            target.InsertItem(1, 22);

            // Then
            CollectionAssert.AreEqual(new[] { 11, 22, 33 }, target);
        }

        [Test]
        public void SholdRemoveItemAtFromList()
        {
            // Given
            var target = new List<int> { 11, 22, 33 };

            // When
            target.RemoveAt(1);

            // Then
            CollectionAssert.AreEqual(new[] { 11, 33 }, target);
        }

        [Test]
        public void SholdRemoveItemFromList()
        {
            // Given
            var target = new List<int> { 11, 22, 33 };

            // When
            target.RemoveItem(22);

            // Then
            CollectionAssert.AreEqual(new[] { 11, 33 }, target);
        }

        [Test]
        public void ShouldGetItemFromArray()
        {
            // Given
            var target = new[] { 11, 22, 33 };

            // When
            var item0 = target.GetItem(0);
            var item1 = target.GetItem(1);
            var item2 = target.GetItem(2);

            // Then
            Assert.AreEqual(11, item0);
            Assert.AreEqual(22, item1);
            Assert.AreEqual(33, item2);
        }

        [Test]
        public void ShouldGetItemFromList()
        {
            // Given
            var target = new List<int> { 11, 22, 33 };

            // When
            var item0 = target.GetItem(0);
            var item1 = target.GetItem(1);
            var item2 = target.GetItem(2);

            // Then
            Assert.AreEqual(11, item0);
            Assert.AreEqual(22, item1);
            Assert.AreEqual(33, item2);
        }

        [Test]
        public void ShouldSetItemItemToArray()
        {
            // Given
            var target = new[] { 11, 00, 33 };

            // When
            target.SetItem(1, 22);

            // Then
            CollectionAssert.AreEqual(new[] { 11, 22, 33 }, target);
        }

        [Test]
        public void ShouldSetItemItemToList()
        {
            // Given
            var target = new List<int> { 11, 00, 33 };

            // When
            target.SetItem(1, 22);

            // Then
            CollectionAssert.AreEqual(new[] { 11, 22, 33 }, target);
        }

        [Test]
        public void TrySetPropertyValueByPathShouldSetItemsInCollections()
        {
            // Given
            var foo = new { Files = new object[] { null, null } };

            // When
            foo.TrySetPropertyValueByPath("Files.0", 0);
            foo.TrySetPropertyValueByPath("Files.1", 1);

            // Then
            Assert.AreEqual(0, foo.Files[0]);
            Assert.AreEqual(1, foo.Files[1]);
        }
    }
}