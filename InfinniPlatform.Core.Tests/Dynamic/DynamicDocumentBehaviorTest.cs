using System;
using System.Collections.Generic;
using System.Linq;

using InfinniPlatform.Tests;

using Microsoft.CSharp.RuntimeBinder;

using NUnit.Framework;

namespace InfinniPlatform.Dynamic
{
    [TestFixture]
    [Category(TestCategories.UnitTest)]
    public class DynamicDocumentBehaviorTest
    {
        [TestCase(1)]
        [TestCase("test")]
        [TestCase(true)]
        [TestCase(1.232)]
        [TestCase(null)]
        public void ShouldSetSimpleValue(object value)
        {
            dynamic dynamicDocument = new DynamicDocument();
            dynamicDocument.Property = value;

            Assert.AreEqual(dynamicDocument.Property, value);
        }

        [Test]
        public void ShouldAddProperty()
        {
            // Given
            dynamic dynamicDocument = new DynamicDocument();
            // When

            dynamic obj = new DynamicDocument();
            obj.SomeValue = 2;

            dynamic arr = new List<dynamic>();
            arr.Add("3");


            dynamicDocument["SomeValue"] = 1;
            dynamicDocument["SomeObject"] = obj;
            dynamicDocument.SomeArray = arr;
            // Then

            Assert.AreEqual(dynamicDocument.SomeValue, 1);
            Assert.AreEqual(dynamicDocument.SomeObject.SomeValue, 2);
            Assert.AreEqual(dynamicDocument.SomeArray[0], "3");
        }

        [Test]
        public void ShouldBindMethodOnDynamicDocument()
        {
            dynamic dynamicDocument = new DynamicDocument();
            dynamicDocument.SomeValue = 1;

            Assert.AreEqual(1, dynamicDocument.SomeValue);
        }

        [Test]
        public void ShouldConvertDynamicDocumentProperty()
        {
            dynamic someInstance = new DynamicDocument();

            dynamic dynamicDocument = new DynamicDocument();

            dynamicDocument.Property1 = "test";

            someInstance.DocumentProperty = dynamicDocument;

            Assert.IsNotNull(someInstance.DocumentProperty.Property1);
            Assert.AreEqual(someInstance.DocumentProperty.Property1, "test");
        }

        [Test]
        public void ShouldConvertToStringArray()
        {
            var stringArray = new List<dynamic> { "f", "a" };

            dynamic i = new DynamicDocument();
            i.StringArray = stringArray;

            dynamic items = i.StringArray.ToArray();

            Assert.AreEqual(items.Length, 2);
            Assert.AreEqual("f", items[0]);
            Assert.AreEqual("a", items[1]);
        }

        [Test]
        public void ShouldEnumerateDynamicDocumentProperties()
        {
            dynamic dynamicDocument = new DynamicDocument();
            dynamicDocument.Property1 = 1;
            dynamicDocument.Property2 = "testproperty";

            var props = new List<object>();
            foreach (var property in dynamicDocument)
            {
                props.Add(property);
            }

            Assert.AreEqual(2, props.Count);
            var pair = (KeyValuePair<string, dynamic>)props.ToArray().First();
            Assert.AreEqual(pair.Key, "Property1");
            pair = (KeyValuePair<string, dynamic>)props.ToArray().Skip(1).First();
            Assert.AreEqual(pair.Key, "Property2");
        }

        [Test]
        public void ShouldFailToAccessNonExistingProperty()
        {
            dynamic dynamicDocument = new DynamicDocument();
            dynamic obj = new DynamicDocument();
            obj.SomeValue = 1;

            dynamicDocument.SomeObj = obj;

            var result = dynamicDocument.SomeObj.SomeValue111;

            Assert.IsNull(result);
        }

        [Test]
        public void ShouldFailToAccessPropertyForSimpleValue()
        {
            dynamic dynamicDocument = new DynamicDocument();
            dynamic obj = new DynamicDocument();
            obj.SomeValue = 1;

            dynamicDocument.SomeObj = obj;

            TestDelegate accessPropertyForSimpleValue = () => { var result = dynamicDocument.SomeObj.SomeValue.FailedProperty; };
            Assert.Throws<RuntimeBinderException>(accessPropertyForSimpleValue);
        }

        [Test]
        public void ShouldGetPropertiesList()
        {
            dynamic dynamicDocument = new DynamicDocument();
            dynamic obj = new DynamicDocument();
            obj.SomeValue = 1;

            dynamic jarray = new[] { "3" };


            dynamicDocument.SomeObj = obj;
            dynamicDocument.SomeValue = 2;
            dynamicDocument.SomeArray = jarray;

            var propsCount = 0;
            var propList = new List<string>();

            foreach (var instance in dynamicDocument)
            {
                propsCount++;
                propList.Add(instance.Key);
            }

            Assert.AreEqual(3, propsCount);
            Assert.AreEqual(propList.ToArray().First(), "SomeObj");
            Assert.AreEqual(propList.ToArray().Skip(1).First(), "SomeValue");
            Assert.AreEqual(propList.ToArray().Skip(2).First(), "SomeArray");
        }

        [Test]
        public void ShouldGetProperty()
        {
            // Given
            dynamic dynamicDocument = new DynamicDocument();
            dynamic obj = new DynamicDocument();
            obj.SomeValue = 1;

            // When
            dynamicDocument.SomeObj = obj;
            dynamicDocument.SomeValue = 2;

            // Then
            Assert.AreEqual(1, dynamicDocument.SomeObj.SomeValue);
            Assert.AreEqual(2, dynamicDocument.SomeValue);
            Assert.AreEqual(null, dynamicDocument["NonExistingProperty"]);
        }

        [Test]
        public void ShouldHasProperty()
        {
            dynamic dynamicDocument = new DynamicDocument();
            dynamic obj = new DynamicDocument();
            dynamicDocument.SomeValue = obj;

            dynamic hasProp = dynamicDocument.SomeValue != null;
            dynamic notHasProp = dynamicDocument.NotHasSomeValue != null;

            Assert.True(hasProp);
            Assert.False(notHasProp);
        }

        [Test]
        public void ShouldNotThrowOnSetNotExistingProperty()
        {
            // Given
            dynamic dynamicDocument = new DynamicDocument();

            // When

            dynamicDocument.SomeValue = 2;

            Assert.AreEqual(dynamicDocument["SomeValue"], 2);
        }

        [Test]
        public void ShouldRemoveProperty()
        {
            // Given
            var dynamicDocument = new DynamicDocument();
            dynamicDocument["SomeValue"] = 1;

            // When
            dynamicDocument["SomeValue"] = null;

            // Then
            Assert.AreEqual(null, dynamicDocument["SomeValue"]);

            var hasProps = false;

            foreach (var elements in dynamicDocument)
            {
                hasProps = true;
            }


            Assert.IsFalse(hasProps);
        }

        [Test]
        public void ShouldReturnsNullAsArrayItem()
        {
            dynamic dynamicDocument = new DynamicDocument();

            dynamic arr = new object[] { null };
            dynamicDocument.ArrayProperty = arr;

            Assert.IsNull(dynamicDocument.ArrayProperty[0]);
        }

        [Test]
        public void ShouldReturnsNullAsPropertyValue()
        {
            dynamic dynamicDocument = new DynamicDocument();
            dynamic obj = new DynamicDocument();
            obj.SomeValue = null;

            dynamicDocument.SomeObj = obj;

            Assert.AreEqual(null, dynamicDocument.SomeObj.SomeValue);
        }

        [Test]
        public void ShouldSetArrayObjectItemsAndTrackChanges()
        {
            //проверяем ссылки на один и тот же массив
            dynamic dynamicDocument1 = new DynamicDocument();
            dynamicDocument1.ArrayProperty = new List<dynamic>();

            dynamic dynamicDocument2 = new DynamicDocument();
            dynamicDocument2.ArrayProperty1 = dynamicDocument1.ArrayProperty;

            dynamic dynamicDocument3 = new DynamicDocument();
            dynamicDocument3["ArrayProperty2"] = dynamicDocument1.ArrayProperty;

            dynamic checkInstance = new DynamicDocument();
            checkInstance.SimpleField = 1;

            dynamicDocument1.ArrayProperty.Add(checkInstance);

            Assert.AreEqual(1, dynamicDocument1.ArrayProperty.Count);
            Assert.AreEqual(1, dynamicDocument2.ArrayProperty1.Count);
            Assert.AreEqual(1, dynamicDocument3.ArrayProperty2.Count);

            //проверяем изменение при использовании SetProperty
            dynamicDocument3.ArrayProperty2 = new List<dynamic>();

            Assert.AreEqual(0, dynamicDocument3.ArrayProperty2.Count);

            //проверяем, что ссылка удаляется при присвоении другого массива

            dynamicDocument2.ArrayProperty1 = new List<dynamic>();

            Assert.AreEqual(0, dynamicDocument2.ArrayProperty1.Count);
            Assert.AreEqual(1, dynamicDocument1.ArrayProperty.Count);

            //проверяем что после смены ссылки на массив отслеживание изменений прекратилось
            dynamic checkInstance2 = new DynamicDocument();
            checkInstance2.SimpleField = 2;

            dynamicDocument1.ArrayProperty.Add(checkInstance2);

            Assert.AreEqual(2, dynamicDocument1.ArrayProperty.Count);
            Assert.AreEqual(1, dynamicDocument1.ArrayProperty[0].SimpleField);
            Assert.AreEqual(2, dynamicDocument1.ArrayProperty[1].SimpleField);
            Assert.AreEqual(0, dynamicDocument2.ArrayProperty1.Count);

            //проверяем что в старом массиве ничего не изменяется при добавлении в новый
            dynamic checkInstance3 = new DynamicDocument();
            checkInstance3.SimpleField = 3;

            dynamicDocument2.ArrayProperty1.Add(checkInstance3);

            Assert.AreEqual(2, dynamicDocument1.ArrayProperty.Count);
            Assert.AreEqual(1, dynamicDocument2.ArrayProperty1.Count);

            Assert.AreEqual(3, dynamicDocument2.ArrayProperty1[0].SimpleField);

            //проверяем удаление из массива
            dynamicDocument2.ArrayProperty1.Remove(checkInstance3);
            Assert.AreEqual(2, dynamicDocument1.ArrayProperty.Count);
            Assert.AreEqual(0, dynamicDocument2.ArrayProperty1.Count);

            //проверяем изменение ссылки на существующий массив
            dynamicDocument2.ArrayProperty1 = dynamicDocument1.ArrayProperty;
            Assert.AreEqual(2, dynamicDocument2.ArrayProperty1.Count);
            Assert.AreEqual(2, dynamicDocument1.ArrayProperty.Count);
        }

        [Test]
        public void ShouldSetArrayProperty()
        {
            dynamic dynamicDocument = new DynamicDocument();
            dynamic obj = new DynamicDocument();
            obj.SomeValue = 1;

            dynamic arr = new[] { obj };
            dynamicDocument.ArrayProperty = arr;

            Assert.AreEqual(dynamicDocument.ArrayProperty[0].SomeValue, 1);
        }

        [Test]
        public void ShouldSetArrayPropertyWithSomeElements1()
        {
            dynamic dynamicDocument = new DynamicDocument();
            dynamic obj1 = new DynamicDocument();
            obj1.SomeValue = 1;
            dynamic obj2 = new DynamicDocument();
            obj2.SomeValue = 2;

            dynamic arr = new[] { obj1, obj2 };
            dynamicDocument.ArrayProperty = arr;

            Assert.AreEqual(dynamicDocument.ArrayProperty[1].SomeValue, 2);
        }

        [Test]
        public void ShouldSetArrayPropertyWithSomeElements2()
        {
            dynamic dynamicDocument = new DynamicDocument();
            dynamic obj1 = new DynamicDocument();
            obj1.SomeValue = 1;
            dynamic obj2 = new DynamicDocument();
            obj2.SomeValue = 2;

            dynamic arr = new[] { obj1, obj2 };
            dynamicDocument.ArrayProperty = arr;

            Assert.AreEqual(dynamicDocument.ArrayProperty[1].SomeValue, 2);
        }

        [Test]
        public void ShouldSetArrayPropertyWithSomeElements3()
        {
            dynamic dynamicDocument = new DynamicDocument();
            dynamic obj1 = new DynamicDocument();
            obj1.SomeValue = 1;
            dynamic obj2 = new DynamicDocument();
            obj2.SomeValue = 2;

            dynamic arr = new List<DynamicDocument> { obj1, obj2 };
            dynamicDocument.ArrayProperty = arr;

            Assert.AreEqual(dynamicDocument.ArrayProperty[1].SomeValue, 2);
        }

        [Test]
        public void ShouldSetArrayValue()
        {
            dynamic dynamicDocument = new DynamicDocument();
            var obj = new List<dynamic>();
            dynamicDocument.Property = obj;

            Assert.IsNotNull(dynamicDocument.Property);
        }

        [Test]
        public void ShouldSetCompositProperty()
        {
            dynamic dynamicDocument = new DynamicDocument();
            dynamic obj = new DynamicDocument();
            obj.SomeValue = 1;

            dynamicDocument.SomeObj = obj;

            Assert.AreEqual(1, dynamicDocument.SomeObj.SomeValue);
        }

        [Test]
        public void ShouldSetDateTimeValue()
        {
            dynamic dynamicDocument = new DynamicDocument();
            dynamicDocument.Property = new DateTime(2011, 01, 01);

            Assert.AreEqual(dynamicDocument.Property, new DateTime(2011, 01, 01));
        }

        [Test]
        public void ShouldSetObjectValue()
        {
            dynamic dynamicDocument = new DynamicDocument();
            var obj = new DynamicDocument();
            dynamicDocument.Property = obj;

            Assert.IsNotNull(dynamicDocument.Property);
        }

        [Test]
        public void ShouldSetPropertiesWithoutTransformation()
        {
            // Given
            dynamic dynamicDocument = new DynamicDocument();

            // When
            var attachments = new List<string>
                              {
                                  "1",
                                  "2",
                                  "3"
                              };

            dynamicDocument.Attachments = attachments;
            // Then
            Assert.AreEqual("1", dynamicDocument.Attachments[0]);
            Assert.AreEqual("2", dynamicDocument.Attachments[1]);
            Assert.AreEqual("3", dynamicDocument.Attachments[2]);
        }

        [Test]
        public void ShouldSetPropertyValue()
        {
            // Given
            dynamic dynamicDocument = new DynamicDocument();

            dynamic obj = new DynamicDocument();
            obj.SomeValue = 2;

            dynamic arr = new List<dynamic>();
            arr.Add("3");

            dynamicDocument.SomeValue = 0;
            dynamicDocument.SomeObject = null;
            dynamicDocument.SomeArray = null;

            // When

            dynamicDocument.SomeValue = 2;
            dynamicDocument.SomeObject = obj;
            dynamicDocument.SomeArray = arr;


            // Then

            Assert.AreEqual(dynamicDocument.SomeValue, 2);
            Assert.AreEqual(dynamicDocument.SomeObject.SomeValue, 2);
            Assert.AreEqual(dynamicDocument.SomeArray[0], "3");
        }

        [Test]
        public void ShouldThrowIfIndexIncorrect()
        {
            dynamic dynamicDocument = new DynamicDocument();
            dynamic obj = new DynamicDocument();
            obj.SomeValue = 1;

            dynamic arr = new[] { obj };
            dynamicDocument.ArrayProperty = arr;

            TestDelegate indexIncorrect = () => { var result = dynamicDocument.ArrayProperty[1]; };
            Assert.Throws<IndexOutOfRangeException>(indexIncorrect);
        }

        [Test]
        public void ShouldThrowOnAccessNonExistingObject()
        {
            dynamic dynamicDocument = new DynamicDocument();

            dynamic arr = new object[] { null };
            dynamicDocument.ArrayProperty = arr;

            TestDelegate accessNonExistingObject = () => { var result = dynamicDocument.ArrayProperty[0].SomeValue; };
            Assert.Throws<RuntimeBinderException>(accessNonExistingObject);
        }
    }
}