using System;
using System.Collections.Generic;
using System.Linq;

using InfinniPlatform.Tests;

using Microsoft.CSharp.RuntimeBinder;

using Newtonsoft.Json;

using NUnit.Framework;

namespace InfinniPlatform.Dynamic
{
    [TestFixture]
    [Category(TestCategories.UnitTest)]
    public sealed class DynamicWrapperBehavior
    {
        [TestCase(1)]
        [TestCase("test")]
        [TestCase(true)]
        [TestCase(1.232)]
        [TestCase(null)]
        public void ShouldSetSimpleValue(object value)
        {
            dynamic dynamicWrapper = new DynamicWrapper();
            dynamicWrapper.Property = value;

            Assert.AreEqual(dynamicWrapper.Property, value);
        }


        private class TestClass
        {
            public string SomeProperty { get; set; }
        }


        [Test]
        public void ShouldAddProperty()
        {
            // Given
            dynamic dynamicWrapper = new DynamicWrapper();
            // When

            dynamic obj = new DynamicWrapper();
            obj.SomeValue = 2;

            dynamic arr = new List<dynamic>();
            arr.Add("3");


            dynamicWrapper["SomeValue"] = 1;
            dynamicWrapper["SomeObject"] = obj;
            dynamicWrapper.SomeArray = arr;
            // Then

            Assert.AreEqual(dynamicWrapper.SomeValue, 1);
            Assert.AreEqual(dynamicWrapper.SomeObject.SomeValue, 2);
            Assert.AreEqual(dynamicWrapper.SomeArray[0], "3");
        }

        [Test]
        public void ShouldBindMethodOnDynamicWrapper()
        {
            dynamic dynamicWrapper = new DynamicWrapper();
            dynamicWrapper.SomeValue = 1;

            Assert.AreEqual(1, dynamicWrapper.SomeValue);
        }

        [Test]
        public void ShouldConvertDynamicWrapperProperty()
        {
            dynamic someInstance = new DynamicWrapper();

            dynamic dynamicWrapper = new DynamicWrapper();

            dynamicWrapper.Property1 = "test";

            someInstance.WrapperProperty = dynamicWrapper;

            Assert.IsNotNull(someInstance.WrapperProperty.Property1);
            Assert.AreEqual(someInstance.WrapperProperty.Property1, "test");
        }

        [Test]
        public void ShouldConvertJObjectToJObject()
        {
            // Given

            dynamic obj = new DynamicWrapper();
            obj.SomeValue = 1;

            // When
            dynamic result = JsonConvert.SerializeObject(obj);
            // Then
            Assert.AreEqual("{\"SomeValue\":1}", result);
        }

        [Test]
        public void ShouldConvertOwnedArrayToJObject()
        {
            var ownedArray = new List<dynamic>();
            dynamic instance1 = new DynamicWrapper();
            instance1.TestProperty = 1;

            dynamic instance2 = new DynamicWrapper();
            instance2.TestProperty = 2;

            ownedArray.Add(instance1);
            ownedArray.Add(instance2);

            dynamic hostInstance = new DynamicWrapper();

            hostInstance.Result = ownedArray;
            Assert.AreEqual(string.Format("{{{0}  \"Result\": [{0}    {{{0}      \"TestProperty\": 1{0}    }},{0}    {{{0}      \"TestProperty\": 2{0}    }}{0}  ]{0}}}", Environment.NewLine), hostInstance.ToString());
        }

        [Test]
        public void ShouldConvertSimpleObjectToJObject()
        {
            // Given

            var obj = new TestClass { SomeProperty = "1" };

            // When
            var result = JsonConvert.SerializeObject(obj);

            // Then
            Assert.AreEqual("{\"SomeProperty\":\"1\"}", result);
        }

        [Test]
        public void ShouldConvertToJObject()
        {
            // Given
            dynamic dynamicWrapper = new DynamicWrapper();
            dynamic obj = new DynamicWrapper();
            obj.SomeValue = 1;

            dynamic jarray = new[] { "3" };

            dynamicWrapper.SomeObj = obj;
            dynamicWrapper.SomeValue = 2;
            dynamicWrapper.SomeArray = jarray;

            // When
            dynamic jobject = JsonConvert.SerializeObject(dynamicWrapper);

            // Then
            Assert.AreEqual("{\"SomeObj\":{\"SomeValue\":1},\"SomeValue\":2,\"SomeArray\":[\"3\"]}", jobject);
        }

        [Test]
        public void ShouldConvertToStringArray()
        {
            var stringArray = new List<dynamic> { "f", "a" };

            dynamic i = new DynamicWrapper();
            i.StringArray = stringArray;

            dynamic items = i.StringArray.ToArray();

            Assert.AreEqual(items.Length, 2);
            Assert.AreEqual("f", items[0]);
            Assert.AreEqual("a", items[1]);
        }

        [Test]
        public void ShouldEnumerateDynamicWrapperProperties()
        {
            dynamic dynamicWrapper = new DynamicWrapper();
            dynamicWrapper.Property1 = 1;
            dynamicWrapper.Property2 = "testproperty";

            var props = new List<object>();
            foreach (var property in dynamicWrapper)
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
            dynamic dynamicWrapper = new DynamicWrapper();
            dynamic obj = new DynamicWrapper();
            obj.SomeValue = 1;

            dynamicWrapper.SomeObj = obj;

            var result = dynamicWrapper.SomeObj.SomeValue111;

            Assert.IsNull(result);
        }

        [Test]
        public void ShouldFailToAccessPropertyForSimpleValue()
        {
            dynamic dynamicWrapper = new DynamicWrapper();
            dynamic obj = new DynamicWrapper();
            obj.SomeValue = 1;

            dynamicWrapper.SomeObj = obj;

            TestDelegate accessPropertyForSimpleValue = () => { var result = dynamicWrapper.SomeObj.SomeValue.FailedProperty; };
            Assert.Throws<RuntimeBinderException>(accessPropertyForSimpleValue);
        }

        [Test]
        public void ShouldGetPropertiesList()
        {
            dynamic dynamicWrapper = new DynamicWrapper();
            dynamic obj = new DynamicWrapper();
            obj.SomeValue = 1;

            dynamic jarray = new[] { "3" };


            dynamicWrapper.SomeObj = obj;
            dynamicWrapper.SomeValue = 2;
            dynamicWrapper.SomeArray = jarray;

            var propsCount = 0;
            var propList = new List<string>();

            foreach (var instance in dynamicWrapper)
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
            dynamic dynamicWrapper = new DynamicWrapper();
            dynamic obj = new DynamicWrapper();
            obj.SomeValue = 1;

            // When
            dynamicWrapper.SomeObj = obj;
            dynamicWrapper.SomeValue = 2;

            // Then
            Assert.AreEqual(1, dynamicWrapper.SomeObj.SomeValue);
            Assert.AreEqual(2, dynamicWrapper.SomeValue);
            Assert.AreEqual(null, dynamicWrapper["NonExistingProperty"]);
        }

        [Test]
        public void ShouldHasProperty()
        {
            dynamic dynamicWrapper = new DynamicWrapper();
            dynamic obj = new DynamicWrapper();
            dynamicWrapper.SomeValue = obj;

            dynamic hasProp = dynamicWrapper.SomeValue != null;
            dynamic notHasProp = dynamicWrapper.NotHasSomeValue != null;

            Assert.True(hasProp);
            Assert.False(notHasProp);
        }

        [Test]
        public void ShouldNotThrowOnSetNotExistingProperty()
        {
            // Given
            dynamic dynamicWrapper = new DynamicWrapper();

            // When

            dynamicWrapper.SomeValue = 2;

            Assert.AreEqual(dynamicWrapper["SomeValue"], 2);
        }

        [Test]
        public void ShouldRemoveProperty()
        {
            // Given
            var dynamicWrapper = new DynamicWrapper();
            dynamicWrapper["SomeValue"] = 1;

            // When
            dynamicWrapper["SomeValue"] = null;

            // Then
            Assert.AreEqual(null, dynamicWrapper["SomeValue"]);

            var hasProps = false;

            foreach (var elements in dynamicWrapper)
            {
                hasProps = true;
            }


            Assert.IsFalse(hasProps);
        }

        [Test]
        public void ShouldReturnsNullAsArrayItem()
        {
            dynamic dynamicWrapper = new DynamicWrapper();

            dynamic arr = new object[] { null };
            dynamicWrapper.ArrayProperty = arr;

            Assert.IsNull(dynamicWrapper.ArrayProperty[0]);
        }

        [Test]
        public void ShouldReturnsNullAsPropertyValue()
        {
            dynamic dynamicWrapper = new DynamicWrapper();
            dynamic obj = new DynamicWrapper();
            obj.SomeValue = null;

            dynamicWrapper.SomeObj = obj;

            Assert.AreEqual(null, dynamicWrapper.SomeObj.SomeValue);
        }

        [Test]
        public void ShouldSetArrayObjectItemsAndTrackChanges()
        {
            //проверяем ссылки на один и тот же массив
            dynamic dynamicWrapper1 = new DynamicWrapper();
            dynamicWrapper1.ArrayProperty = new List<dynamic>();

            dynamic dynamicWrapper2 = new DynamicWrapper();
            dynamicWrapper2.ArrayProperty1 = dynamicWrapper1.ArrayProperty;

            dynamic dynamicWrapper3 = new DynamicWrapper();
            dynamicWrapper3["ArrayProperty2"] = dynamicWrapper1.ArrayProperty;

            dynamic checkInstance = new DynamicWrapper();
            checkInstance.SimpleField = 1;

            dynamicWrapper1.ArrayProperty.Add(checkInstance);

            Assert.AreEqual(1, dynamicWrapper1.ArrayProperty.Count);
            Assert.AreEqual(1, dynamicWrapper2.ArrayProperty1.Count);
            Assert.AreEqual(1, dynamicWrapper3.ArrayProperty2.Count);

            //проверяем изменение при использовании SetProperty
            dynamicWrapper3.ArrayProperty2 = new List<dynamic>();

            Assert.AreEqual(0, dynamicWrapper3.ArrayProperty2.Count);

            //проверяем, что ссылка удаляется при присвоении другого массива

            dynamicWrapper2.ArrayProperty1 = new List<dynamic>();

            Assert.AreEqual(0, dynamicWrapper2.ArrayProperty1.Count);
            Assert.AreEqual(1, dynamicWrapper1.ArrayProperty.Count);

            //проверяем что после смены ссылки на массив отслеживание изменений прекратилось
            dynamic checkInstance2 = new DynamicWrapper();
            checkInstance2.SimpleField = 2;

            dynamicWrapper1.ArrayProperty.Add(checkInstance2);

            Assert.AreEqual(2, dynamicWrapper1.ArrayProperty.Count);
            Assert.AreEqual(1, dynamicWrapper1.ArrayProperty[0].SimpleField);
            Assert.AreEqual(2, dynamicWrapper1.ArrayProperty[1].SimpleField);
            Assert.AreEqual(0, dynamicWrapper2.ArrayProperty1.Count);

            //проверяем что в старом массиве ничего не изменяется при добавлении в новый
            dynamic checkInstance3 = new DynamicWrapper();
            checkInstance3.SimpleField = 3;

            dynamicWrapper2.ArrayProperty1.Add(checkInstance3);

            Assert.AreEqual(2, dynamicWrapper1.ArrayProperty.Count);
            Assert.AreEqual(1, dynamicWrapper2.ArrayProperty1.Count);

            Assert.AreEqual(3, dynamicWrapper2.ArrayProperty1[0].SimpleField);

            //проверяем удаление из массива
            dynamicWrapper2.ArrayProperty1.Remove(checkInstance3);
            Assert.AreEqual(2, dynamicWrapper1.ArrayProperty.Count);
            Assert.AreEqual(0, dynamicWrapper2.ArrayProperty1.Count);

            //проверяем изменение ссылки на существующий массив
            dynamicWrapper2.ArrayProperty1 = dynamicWrapper1.ArrayProperty;
            Assert.AreEqual(2, dynamicWrapper2.ArrayProperty1.Count);
            Assert.AreEqual(2, dynamicWrapper1.ArrayProperty.Count);
        }

        [Test]
        public void ShouldSetArrayProperty()
        {
            dynamic dynamicWrapper = new DynamicWrapper();
            dynamic obj = new DynamicWrapper();
            obj.SomeValue = 1;

            dynamic arr = new[] { obj };
            dynamicWrapper.ArrayProperty = arr;

            Assert.AreEqual(dynamicWrapper.ArrayProperty[0].SomeValue, 1);
        }

        [Test]
        public void ShouldSetArrayPropertyWithSomeElements1()
        {
            dynamic dynamicWrapper = new DynamicWrapper();
            dynamic obj1 = new DynamicWrapper();
            obj1.SomeValue = 1;
            dynamic obj2 = new DynamicWrapper();
            obj2.SomeValue = 2;

            dynamic arr = new[] { obj1, obj2 };
            dynamicWrapper.ArrayProperty = arr;

            Assert.AreEqual(dynamicWrapper.ArrayProperty[1].SomeValue, 2);
        }

        [Test]
        public void ShouldSetArrayPropertyWithSomeElements2()
        {
            dynamic dynamicWrapper = new DynamicWrapper();
            dynamic obj1 = new DynamicWrapper();
            obj1.SomeValue = 1;
            dynamic obj2 = new DynamicWrapper();
            obj2.SomeValue = 2;

            dynamic arr = new[] { obj1, obj2 };
            dynamicWrapper.ArrayProperty = arr;

            Assert.AreEqual(dynamicWrapper.ArrayProperty[1].SomeValue, 2);
        }

        [Test]
        public void ShouldSetArrayPropertyWithSomeElements3()
        {
            dynamic dynamicWrapper = new DynamicWrapper();
            dynamic obj1 = new DynamicWrapper();
            obj1.SomeValue = 1;
            dynamic obj2 = new DynamicWrapper();
            obj2.SomeValue = 2;

            dynamic arr = new List<DynamicWrapper> { obj1, obj2 };
            dynamicWrapper.ArrayProperty = arr;

            Assert.AreEqual(dynamicWrapper.ArrayProperty[1].SomeValue, 2);
        }

        [Test]
        public void ShouldSetArrayValue()
        {
            dynamic dynamicWrapper = new DynamicWrapper();
            var obj = new List<dynamic>();
            dynamicWrapper.Property = obj;

            Assert.IsNotNull(dynamicWrapper.Property);
        }

        [Test]
        public void ShouldSetCompositProperty()
        {
            dynamic dynamicWrapper = new DynamicWrapper();
            dynamic obj = new DynamicWrapper();
            obj.SomeValue = 1;

            dynamicWrapper.SomeObj = obj;

            Assert.AreEqual(1, dynamicWrapper.SomeObj.SomeValue);
        }

        [Test]
        public void ShouldSetDateTimeValue()
        {
            dynamic dynamicWrapper = new DynamicWrapper();
            dynamicWrapper.Property = new DateTime(2011, 01, 01);

            Assert.AreEqual(dynamicWrapper.Property, new DateTime(2011, 01, 01));
        }

        [Test]
        public void ShouldSetObjectValue()
        {
            dynamic dynamicWrapper = new DynamicWrapper();
            var obj = new DynamicWrapper();
            dynamicWrapper.Property = obj;

            Assert.IsNotNull(dynamicWrapper.Property);
        }

        [Test]
        public void ShouldSetPropertiesWithoutTransformation()
        {
            // Given
            dynamic dynamicWrapper = new DynamicWrapper();

            // When
            var attachments = new List<string>
                              {
                                  "1",
                                  "2",
                                  "3"
                              };

            dynamicWrapper.Attachments = attachments;
            // Then
            Assert.AreEqual("1", dynamicWrapper.Attachments[0]);
            Assert.AreEqual("2", dynamicWrapper.Attachments[1]);
            Assert.AreEqual("3", dynamicWrapper.Attachments[2]);
        }

        [Test]
        public void ShouldSetPropertyValue()
        {
            // Given
            dynamic dynamicWrapper = new DynamicWrapper();

            dynamic obj = new DynamicWrapper();
            obj.SomeValue = 2;

            dynamic arr = new List<dynamic>();
            arr.Add("3");

            dynamicWrapper.SomeValue = 0;
            dynamicWrapper.SomeObject = null;
            dynamicWrapper.SomeArray = null;

            // When

            dynamicWrapper.SomeValue = 2;
            dynamicWrapper.SomeObject = obj;
            dynamicWrapper.SomeArray = arr;


            // Then

            Assert.AreEqual(dynamicWrapper.SomeValue, 2);
            Assert.AreEqual(dynamicWrapper.SomeObject.SomeValue, 2);
            Assert.AreEqual(dynamicWrapper.SomeArray[0], "3");
        }

        [Test]
        public void ShouldThrowIfIndexIncorrect()
        {
            dynamic dynamicWrapper = new DynamicWrapper();
            dynamic obj = new DynamicWrapper();
            obj.SomeValue = 1;

            dynamic arr = new[] { obj };
            dynamicWrapper.ArrayProperty = arr;

            TestDelegate indexIncorrect = () => { var result = dynamicWrapper.ArrayProperty[1]; };
            Assert.Throws<IndexOutOfRangeException>(indexIncorrect);
        }

        [Test]
        public void ShouldThrowOnAccessNonExistingObject()
        {
            dynamic dynamicWrapper = new DynamicWrapper();

            dynamic arr = new object[] { null };
            dynamicWrapper.ArrayProperty = arr;

            TestDelegate accessNonExistingObject = () => { var result = dynamicWrapper.ArrayProperty[0].SomeValue; };
            Assert.Throws<RuntimeBinderException>(accessNonExistingObject);
        }
    }
}