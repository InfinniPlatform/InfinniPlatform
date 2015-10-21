using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using InfinniPlatform.Sdk.Dynamic;

using Microsoft.CSharp.RuntimeBinder;

using Newtonsoft.Json;

using NUnit.Framework;

namespace InfinniPlatform.Api.Tests.Dynamic
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
        public void ShouldAddArrayObjectItems()
        {
            // Given
            dynamic dynamicWrapper = new List<dynamic>();
            dynamic i1 = new DynamicWrapper();
            i1.Value = 1;

            dynamic i2 = new DynamicWrapper();
            i2.Value = 2;

            dynamic i3 = new DynamicWrapper();
            i3.Value = 3;
            // When
            dynamicWrapper.Add(i1);
            dynamicWrapper.Add(i2);
            dynamicWrapper.Add(i3);
            // Then
            IEnumerable<dynamic> items = DynamicWrapperExtensions.ToEnumerable(dynamicWrapper);
            Assert.AreEqual(3, items.Count());
            Assert.AreEqual(1, items.First().Value);
            Assert.AreEqual(2, items.Skip(1).First().Value);
            Assert.AreEqual(3, items.Skip(2).First().Value);
        }

        [Test]
        public void ShouldAddArraySimpleValueItems()
        {
            // Given
            dynamic dynamicWrapper = new List<dynamic>();
            // When
            dynamicWrapper.Add("1");
            dynamicWrapper.Add("2");
            dynamicWrapper.Add("3");
            // Then
            IEnumerable<dynamic> items = DynamicWrapperExtensions.ToEnumerable(dynamicWrapper);
            Assert.AreEqual(3, items.Count());
            Assert.AreEqual("1", items.First());
            Assert.AreEqual("2", items.Skip(1).First());
            Assert.AreEqual("3", items.Skip(2).First());
        }

        [Test]
        public void ShouldAddArraySimpleValueItemsProperty()
        {
            // Given
            dynamic dynamicWrapper = new List<dynamic>();

            dynamic someInstance = new DynamicWrapper();
            someInstance.InnerProperty = dynamicWrapper;

            // When
            someInstance.InnerProperty.Add("1");
            someInstance.InnerProperty.Add("2");
            someInstance.InnerProperty.Add("3");
            // Then
            IEnumerable<dynamic> items = DynamicWrapperExtensions.ToEnumerable(someInstance.InnerProperty);
            Assert.AreEqual(3, items.Count());
            Assert.AreEqual("1", items.First());
            Assert.AreEqual("2", items.Skip(1).First());
            Assert.AreEqual("3", items.Skip(2).First());
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
        public void ShouldConvertDictionaryToJObject()
        {
            var dict = new Dictionary<string, object>();

            dynamic instance1 = new DynamicWrapper();
            instance1.TestProperty = 1;

            dynamic instance2 = new DynamicWrapper();
            instance2.TestProperty = 2;

            dict.Add("1", instance1);
            dict.Add("2", instance2);

            var result = dict.ToDynamic();
            Assert.AreEqual(string.Format("{{{0}  \"1\": {{{0}    \"TestProperty\": 1{0}  }},{0}  \"2\": {{{0}    \"TestProperty\": 2{0}  }}{0}}}", Environment.NewLine), result.ToString());
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
        public void ShouldConvertToByteArray()
        {
            var items = new byte[] { 10, 100, 200, 30 }.ToDynamic();

            Assert.AreEqual(items.Length, 4);
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
        public void ShouldCreateInstanceFromAnonimousObject()
        {
            var obj = new
                      {
                          TestObjectField = new
                                            {
                                                TestProperty = 1
                                            },
                          TestPropertyField = "1234"
                      };

            dynamic instance = obj.ToDynamic();

            Assert.IsNotNull(instance.TestObjectField.TestProperty);
            Assert.IsNotNull(instance.TestPropertyField);
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
        public void ShouldFastAsJObjectNative()
        {
            var stopWatch = Stopwatch.StartNew();

            dynamic dynamicWrapper = new DynamicWrapper();
            var attachments = new List<string>
                              {
                                  "1",
                                  "2",
                                  "3"
                              };

            dynamicWrapper.Attachments = attachments;

            Console.WriteLine(dynamicWrapper.Attachments[0]);
            Console.WriteLine(dynamicWrapper.Attachments[1]);
            Console.WriteLine(dynamicWrapper.Attachments[2]);

            stopWatch.Stop();
            Console.WriteLine(stopWatch.ElapsedMilliseconds);


            stopWatch = Stopwatch.StartNew();

            dynamic jobject = new DynamicWrapper();
            dynamic jattachments = new[]
                                   {
                                       "1",
                                       "2",
                                       "3"
                                   };

            jobject.Attachments = jattachments;

            Console.WriteLine(jobject.Attachments[0]);
            Console.WriteLine(jobject.Attachments[1]);
            Console.WriteLine(jobject.Attachments[2]);

            stopWatch.Stop();
            Console.WriteLine(stopWatch.ElapsedMilliseconds);
        }

        [Test]
        public void ShouldGetArraySimpleItem()
        {
            dynamic dynamicWrapper = new DynamicWrapper();
            var jarray = new[] { "1", "2", "3" };

            dynamicWrapper.SomeArray = jarray.ToDynamic();

            Assert.AreEqual("1", dynamicWrapper.SomeArray[0]);
            Assert.AreEqual("2", dynamicWrapper.SomeArray[1]);
            Assert.AreEqual("3", dynamicWrapper.SomeArray[2]);
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

            dynamic jarray = new List<dynamic>();
            jarray.Add("3");

            // When
            dynamicWrapper.SomeObj = obj;
            dynamicWrapper.SomeValue = 2;
            dynamicWrapper.SomeArray = ((object)jarray).ToDynamic();

            // Then
            Assert.AreEqual(1, dynamicWrapper.SomeObj.SomeValue);
            dynamic arr = dynamicWrapper.SomeArray;
            Assert.AreEqual("3", arr[0]);
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
        public void ShouldInvokeLinqMethodsForObjectsCollections()
        {
            // Given
            dynamic dynamicWrapper = new DynamicWrapper();

            dynamic at1 = new DynamicWrapper();
            at1.a = "1";

            dynamic at2 = new DynamicWrapper();
            at2.a = "2";

            dynamic at3 = new DynamicWrapper();
            at3.a = "3";

            var attachments = new List<DynamicWrapper>
                              {
                                  at1,
                                  at2,
                                  at3
                              };
            dynamicWrapper.Attachments = attachments;
            // When
            IEnumerable<dynamic> enumerable = DynamicWrapperExtensions.ToEnumerable(dynamicWrapper.Attachments);
            // Then
            Assert.AreEqual(3, enumerable.Count());
            dynamic instance = enumerable.FirstOrDefault(e => e.a == "3");

            Assert.IsNotNull(instance);
            Assert.AreEqual("3", instance.a);
        }

        [Test]
        public void ShouldInvokeLinqMethodsForSimpleValues()
        {
            // Given
            dynamic dynamicWrapper = new DynamicWrapper();
            var attachments = new List<string>
                              {
                                  "1",
                                  "2",
                                  "3"
                              };

            dynamicWrapper.Attachments = attachments;
            // When
            IEnumerable<dynamic> enumerable = DynamicWrapperExtensions.ToEnumerable(dynamicWrapper.Attachments);
            dynamic item = enumerable.FirstOrDefault(a => a == "1");
            // Then
            Assert.AreEqual("1", item);
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
        public void ShouldParseDynamicWrapperFieldToJObject()
        {
            dynamic someInstance = new DynamicWrapper();
            someInstance.TestProperty = "1";
            someInstance.TestProperty2 = 2;

            var anonimousObject = new
                                  {
                                      TestField = 1,
                                      TestInnerInstance = someInstance
                                  };

            dynamic jobject = anonimousObject.ToDynamic();

            Assert.IsNotNull(jobject.TestInnerInstance.TestProperty2);
        }

        [Test]
        public void ShouldRemoveArrayObjectItems()
        {
            // Given
            dynamic dynamicWrapper = new DynamicWrapper();
            dynamic i1 = new DynamicWrapper();
            i1.Value = 1;

            dynamic i2 = new DynamicWrapper();
            i2.Value = 2;

            dynamic i3 = new DynamicWrapper();
            i3.Value = 3;

            dynamic arr = new List<DynamicWrapper>
                          {
                              i1,
                              i2,
                              i3
                          };

            dynamicWrapper.Arr = arr;
            // When
            dynamicWrapper.Arr.Remove(i2);
            // Then
            IEnumerable<dynamic> items = DynamicWrapperExtensions.ToEnumerable(dynamicWrapper.Arr);
            Assert.AreEqual(2, items.Count());
            Assert.AreEqual(1, items.First().Value);
            Assert.AreEqual(3, items.Skip(1).First().Value);
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