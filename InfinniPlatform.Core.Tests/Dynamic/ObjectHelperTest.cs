using System;
using System.Collections.Generic;
using System.Dynamic;

using Microsoft.CSharp.RuntimeBinder;

using NUnit.Framework;

namespace InfinniPlatform.Dynamic
{
    [TestFixture]
    [Category(TestCategories.UnitTest)]
    public sealed class ObjectHelperTest
    {
        // GetProperty


        // AddItem

        private class SomeClass1
        {
            public object Property1 { get; set; }
            public SomeClass2 Property2 { get; set; }
        }

        private class SomeClass2
        {
            public object NestedProperty1 { get; set; }
        }

        [Test]
        public void AddItemShouldThrowExceptionWhenCollectionIsNull()
        {
            // When
            TestDelegate test = () => ObjectHelper.AddItem(null, new object());

            // Then
            Assert.Throws<ArgumentNullException>(test);
        }

        [Test]
        public void AddItemShouldThrowExceptionWhenNotCollection()
        {
            // Given
            var target = new object();

            // When
            TestDelegate test = () => target.AddItem(new object());

            // Then
            Assert.Throws<NotSupportedException>(test);
        }

        [Test]
        public void AddItemShouldThrowExceptionWhenReadOnlyCollection()
        {
            // Given
            var target = new int[] {};

            // When
            TestDelegate test = () => target.AddItem(11);

            // Then
            Assert.Throws<NotSupportedException>(test);
        }

        [Test]
        public void GetPropertyShouldReturnCollectionItemWhenTargetIsCollection()
        {
            // Given
            var target = new List<int> {11, 22, 33};

            // When
            var result = target.GetProperty("1");

            // Then
            Assert.AreEqual(22, result);
        }

        [Test]
        public void GetPropertyShouldReturnNullWhenPropertyIsNotExists()
        {
            // Given
            var target = new object();

            // When
            var result = target.GetProperty("NotExistsProperty");

            // Then
            Assert.IsNull(result);
        }

        [Test]
        public void GetPropertyShouldReturnNullWhenTargetIsNull()
        {
            // When
            var result = ObjectHelper.GetProperty(null, "Property1");

            // Then
            Assert.IsNull(result);
        }


        // InsertItem

        [Test]
        public void InsertItemShouldThrowExceptionWhenCollectionIsNull()
        {
            // When
            TestDelegate test = () => ObjectHelper.InsertItem(null, 0, new object());

            // Then
            Assert.Throws<ArgumentNullException>(test);
        }

        [Test]
        public void InsertItemShouldThrowExceptionWhenNotCollection()
        {
            // Given
            var target = new object();

            // When
            TestDelegate test = () => target.InsertItem(0, new object());

            // Then
            Assert.Throws<NotSupportedException>(test);
        }

        [Test]
        public void InsertItemShouldThrowExceptionWhenReadOnlyCollection()
        {
            // Given
            var target = new int[] {};

            // When
            TestDelegate test = () => target.InsertItem(0, 11);

            // Then
            Assert.Throws<NotSupportedException>(test);
        }


        // RemoveItemAt

        [Test]
        public void RemoveItemAtShouldThrowExceptionWhenCollectionIsNull()
        {
            // When
            TestDelegate test = () => ObjectHelper.RemoveItemAt(null, 0);

            // Then
            Assert.Throws<ArgumentNullException>(test);
        }

        [Test]
        public void RemoveItemAtShouldThrowExceptionWhenNotCollection()
        {
            // Given
            var target = new object();

            // When
            TestDelegate test = () => target.RemoveItemAt(0);

            // Then
            Assert.Throws<NotSupportedException>(test);
        }

        [Test]
        public void RemoveItemAtShouldThrowExceptionWhenReadOnlyCollection()
        {
            // Given
            var target = new[] {11};

            // When
            TestDelegate test = () => target.RemoveItemAt(0);

            // Then
            Assert.Throws<NotSupportedException>(test);
        }

        [Test]
        public void RemoveItemShouldThrowExceptionWhenCollectionIsNull()
        {
            // When
            TestDelegate test = () => ObjectHelper.RemoveItem(null, new object());

            // Then
            Assert.Throws<ArgumentNullException>(test);
        }

        [Test]
        public void RemoveItemShouldThrowExceptionWhenReadOnlyCollection()
        {
            // Given
            var target = new int[] {};

            // When
            TestDelegate test = () => target.RemoveItem(11);

            // Then
            Assert.Throws<NotSupportedException>(test);
        }

        [Test]
        public void RemoveItemThrowExceptionWhenNotCollection()
        {
            // Given
            var target = new object();

            // When
            TestDelegate test = () => target.RemoveItem(new object());

            // Then
            Assert.Throws<NotSupportedException>(test);
        }

        [Test]
        public void SetItemShouldThrowExceptionWhenCollectionIsNull()
        {
            // When
            TestDelegate test = () => ObjectHelper.SetItem(null, 0, new object());

            // Then
            Assert.Throws<ArgumentNullException>(test);
        }

        [Test]
        public void SetItemShouldThrowExceptionWhenNotCollection()
        {
            // Given
            var target = new object();

            // When
            TestDelegate test = () => target.SetItem(0, new object());

            // Then
            Assert.Throws<NotSupportedException>(test);
        }

        [Test]
        public void SetPropertyShouldCreateAllNotExistsPropertiesInPath()
        {
            // Given
            var value = new object();
            dynamic target = new DynamicWrapper();

            // When
            ObjectHelper.SetProperty(target, "Property1.Property2.Property3", value);

            // Then
            Assert.AreEqual(value, target.Property1.Property2.Property3);
        }

        [Test]
        public void SetPropertyShouldReplaceCollectionItemWhenTargetIsCollection()
        {
            // Given
            var target = new List<int> {11, 00, 33};

            // When
            target.SetProperty("1", 22);

            // Then
            Assert.AreEqual(new[] {11, 22, 33}, target);
        }

        [Test]
        public void SetPropertyShouldThrowExceptionWhenPropertyIsNotExists()
        {
            // Given
            var target = new object();

            // When
            TestDelegate test = () => target.SetProperty("NotExistsProperty", new object());

            // Then
            Assert.Throws<RuntimeBinderException>(test);
        }

        [Test]
        public void SetPropertyShouldThrowExceptionWhenPropertyIsNull()
        {
            // When
            TestDelegate test1 = () => ObjectHelper.SetProperty(null, null, new object());
            TestDelegate test2 = () => ObjectHelper.SetProperty(null, string.Empty, new object());

            // Then
            Assert.Throws<ArgumentNullException>(test1);
            Assert.Throws<ArgumentNullException>(test2);
        }

        [Test]
        public void SetPropertyShouldThrowExceptionWhenTargetIsNull()
        {
            // When
            TestDelegate test = () => ObjectHelper.SetProperty(null, "Property1", new object());

            // Then
            Assert.Throws<ArgumentNullException>(test);
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
            CollectionAssert.AreEqual(new[] {11, 22, 33}, target);
        }

        [Test]
        public void SholdInsertItemToList()
        {
            // Given
            var target = new List<int> {11, 33};

            // When
            target.InsertItem(1, 22);

            // Then
            CollectionAssert.AreEqual(new[] {11, 22, 33}, target);
        }

        [Test]
        public void SholdRemoveItemAtFromList()
        {
            // Given
            var target = new List<int> {11, 22, 33};

            // When
            target.RemoveAt(1);

            // Then
            CollectionAssert.AreEqual(new[] {11, 33}, target);
        }

        [Test]
        public void SholdRemoveItemFromList()
        {
            // Given
            var target = new List<int> {11, 22, 33};

            // When
            target.RemoveItem(22);

            // Then
            CollectionAssert.AreEqual(new[] {11, 33}, target);
        }

        [Test]
        public void ShouldGetItemFromArray()
        {
            // Given
            var target = new[] {11, 22, 33};

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
            var target = new List<int> {11, 22, 33};

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
        public void ShouldGetNestedPropertyFromInstanceOfAnonymousType()
        {
            // Given
            var value = new object();
            var target = new {Property2 = new {NestedProperty1 = value}};

            // When
            var result = target.GetProperty("Property2.NestedProperty1");

            // Then
            Assert.AreEqual(value, result);
        }

        [Test]
        public void ShouldGetNestedPropertyFromInstanceOfDynamicWrapper()
        {
            // Given
            var value = new object();
            dynamic target = new DynamicWrapper();
            dynamic nested = new DynamicWrapper();
            target.Property2 = nested;
            nested.NestedProperty1 = value;

            // When
            var result = ObjectHelper.GetProperty(target, "Property2.NestedProperty1");

            // Then
            Assert.AreEqual(value, result);
        }

        [Test]
        public void ShouldGetNestedPropertyFromInstanceOfExpandoObject()
        {
            // Given
            var value = new object();
            dynamic target = new ExpandoObject();
            dynamic nested = new ExpandoObject();
            target.Property2 = nested;
            nested.NestedProperty1 = value;

            // When
            var result = ObjectHelper.GetProperty(target, "Property2.NestedProperty1");

            // Then
            Assert.AreEqual(value, result);
        }

        [Test]
        public void ShouldGetNestedPropertyFromInstanceOfSpecifiedType()
        {
            // Given
            var value = new object();
            var target = new SomeClass1 {Property2 = new SomeClass2 {NestedProperty1 = value}};

            // When
            var result = target.GetProperty("Property2.NestedProperty1");

            // Then
            Assert.AreEqual(value, result);
        }

        [Test]
        public void ShouldGetPropertyFromInstanceOfAnonymousType()
        {
            // Given
            var value = new object();
            var target = new {Property1 = value};

            // When
            var result = target.GetProperty("Property1");

            // Then
            Assert.AreEqual(value, result);
        }

        [Test]
        public void ShouldGetPropertyFromInstanceOfDynamicWrapper()
        {
            // Given
            var value = new object();
            dynamic target = new DynamicWrapper();
            target.Property1 = value;

            // When
            var result = ObjectHelper.GetProperty(target, "Property1");

            // Then
            Assert.AreEqual(value, result);
        }

        [Test]
        public void ShouldGetPropertyFromInstanceOfExpandoObject()
        {
            // Given
            var value = new object();
            dynamic target = new ExpandoObject();
            target.Property1 = value;

            // When
            var result = ObjectHelper.GetProperty(target, "Property1");

            // Then
            Assert.AreEqual(value, result);
        }

        [Test]
        public void ShouldGetPropertyFromInstanceOfSpecifiedType()
        {
            // Given
            var value = new object();
            var target = new SomeClass1 {Property1 = value};

            // When
            var result = target.GetProperty("Property1");

            // Then
            Assert.AreEqual(value, result);
        }

        [Test]
        public void ShouldGetPropertyOfSpecifiedCollectionItem()
        {
            // Given

            dynamic target = new DynamicWrapper();

            dynamic item0 = new DynamicWrapper();
            item0.ItemProperty1 = 11;
            dynamic item1 = new DynamicWrapper();
            item1.ItemProperty1 = 22;
            dynamic item2 = new DynamicWrapper();
            item2.ItemProperty1 = 33;

            target.Collection1 = new List<object>
                {
                    item0,
                    item1,
                    item2,
                };

            // When
            var result = ObjectHelper.GetProperty(target, "Collection1.1.ItemProperty1");

            // Then
            Assert.AreEqual(22, result);
        }

        [Test]
        public void ShouldSetItemItemToArray()
        {
            // Given
            var target = new[] {11, 00, 33};

            // When
            target.SetItem(1, 22);

            // Then
            CollectionAssert.AreEqual(new[] {11, 22, 33}, target);
        }

        [Test]
        public void ShouldSetItemItemToList()
        {
            // Given
            var target = new List<int> {11, 00, 33};

            // When
            target.SetItem(1, 22);

            // Then
            CollectionAssert.AreEqual(new[] {11, 22, 33}, target);
        }

        [Test]
        public void ShouldSetNestedPropertyForInstanceOfDynamicWrapper()
        {
            // Given
            var value = new object();
            dynamic target = new DynamicWrapper();
            dynamic nested = new DynamicWrapper();
            target.Property2 = nested;

            // When
            ObjectHelper.SetProperty(target, "Property2.NestedProperty1", value);

            // Then
            Assert.AreEqual(value, nested.NestedProperty1);
        }

        [Test]
        public void ShouldSetNestedPropertyForInstanceOfExpandoObject()
        {
            // Given
            var value = new object();
            dynamic target = new ExpandoObject();
            dynamic nested = new ExpandoObject();
            target.Property2 = nested;

            // When
            ObjectHelper.SetProperty(target, "Property2.NestedProperty1", value);

            // Then
            Assert.AreEqual(value, nested.NestedProperty1);
        }

        [Test]
        public void ShouldSetNestedPropertyForInstanceOfSpecifiedType()
        {
            // Given
            var value = new object();
            var target = new SomeClass1 {Property2 = new SomeClass2()};

            // When
            target.SetProperty("Property2.NestedProperty1", value);

            // Then
            Assert.AreEqual(value, target.Property2.NestedProperty1);
        }

        [Test]
        public void ShouldSetPropertyForInstanceOfDynamicWrapper()
        {
            // Given
            var value = new object();
            dynamic target = new DynamicWrapper();

            // When
            ObjectHelper.SetProperty(target, "Property1", value);

            // Then
            Assert.AreEqual(value, target.Property1);
        }

        [Test]
        public void ShouldSetPropertyForInstanceOfExpandoObject()
        {
            // Given
            var value = new object();
            dynamic target = new ExpandoObject();

            // When
            ObjectHelper.SetProperty(target, "Property1", value);

            // Then
            Assert.AreEqual(value, target.Property1);
        }

        [Test]
        public void ShouldSetPropertyForInstanceOfSpecifiedType()
        {
            // Given
            var value = new object();
            var target = new SomeClass1();

            // When
            target.SetProperty("Property1", value);

            // Then
            Assert.AreEqual(value, target.Property1);
        }

        [Test]
        public void ShouldSetPropertyOfSpecifiedCollectionItem()
        {
            // Given

            dynamic target = new DynamicWrapper();

            dynamic item0 = new DynamicWrapper();
            item0.ItemProperty1 = 11;
            dynamic item1 = new DynamicWrapper();
            item1.ItemProperty1 = 00;
            dynamic item2 = new DynamicWrapper();
            item2.ItemProperty1 = 33;

            target.Collection1 = new List<object>
                {
                    item0,
                    item1,
                    item2,
                };

            // When
            ObjectHelper.SetProperty(target, "Collection1.1.ItemProperty1", 22);

            // Then
            Assert.AreEqual(22, item1.ItemProperty1);
        }
    }
}