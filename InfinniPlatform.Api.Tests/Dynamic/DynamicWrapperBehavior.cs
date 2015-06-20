using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using InfinniPlatform.Sdk.Application.Dynamic;
using NUnit.Framework;
using Newtonsoft.Json;

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
            dynamic DynamicWrapper = new DynamicWrapper();
            DynamicWrapper.Property = value;

            Assert.AreEqual(DynamicWrapper.Property, value);
        }

        private class TestClass
        {
            public string SomeProperty { get; set; }
        }

        [Test]
        public void LinqIteratingTest()
        {
            dynamic instance = new List<dynamic>();

            instance.Add(1);
            instance.Add(2);
            instance.Add(3);

            instance.ToString();

            Stopwatch stopWatch = Stopwatch.StartNew();

            dynamic r1 = instance.ToArray()[0];

            stopWatch.Stop();
            Console.WriteLine("Вызов First " + stopWatch.ElapsedMilliseconds);

            Assert.AreEqual("1", r1.ToString());

            stopWatch = Stopwatch.StartNew();

            dynamic r2 = instance.ToArray()[1];

            stopWatch.Stop();
            Console.WriteLine("Вызов Skip " + stopWatch.ElapsedMilliseconds);

            Assert.AreEqual(2, r2);
        }

        [Test]
        public void ShouldAddArrayObjectItems()
        {
            //given
            dynamic DynamicWrapper = new List<dynamic>();
            dynamic i1 = new DynamicWrapper();
            i1.Value = 1;

            dynamic i2 = new DynamicWrapper();
            i2.Value = 2;

            dynamic i3 = new DynamicWrapper();
            i3.Value = 3;
            //when
            DynamicWrapper.Add(i1);
            DynamicWrapper.Add(i2);
            DynamicWrapper.Add(i3);
            //then
            IEnumerable<dynamic> items = DynamicWrapperExtensions.ToEnumerable(DynamicWrapper);
            Assert.AreEqual(3, items.Count());
            Assert.AreEqual(1, items.First().Value);
            Assert.AreEqual(2, items.Skip(1).First().Value);
            Assert.AreEqual(3, items.Skip(2).First().Value);
        }

        [Test]
        public void ShouldAddArraySimpleValueItems()
        {
            //given
            dynamic DynamicWrapper = new List<dynamic>();
            //when
            DynamicWrapper.Add("1");
            DynamicWrapper.Add("2");
            DynamicWrapper.Add("3");
            //then
            IEnumerable<dynamic> items = DynamicWrapperExtensions.ToEnumerable(DynamicWrapper);
            Assert.AreEqual(3, items.Count());
            Assert.AreEqual("1", items.First());
            Assert.AreEqual("2", items.Skip(1).First());
            Assert.AreEqual("3", items.Skip(2).First());
        }

        [Test]
        public void ShouldAddArraySimpleValueItemsProperty()
        {
            //given
            dynamic DynamicWrapper = new List<dynamic>();

            dynamic someInstance = new DynamicWrapper();
            someInstance.InnerProperty = DynamicWrapper;

            //when
            someInstance.InnerProperty.Add("1");
            someInstance.InnerProperty.Add("2");
            someInstance.InnerProperty.Add("3");
            //then
            IEnumerable<dynamic> items = DynamicWrapperExtensions.ToEnumerable(someInstance.InnerProperty);
            Assert.AreEqual(3, items.Count());
            Assert.AreEqual("1", items.First());
            Assert.AreEqual("2", items.Skip(1).First());
            Assert.AreEqual("3", items.Skip(2).First());
        }

        [Test]
        public void ShouldAddProperty()
        {
            //given
            dynamic DynamicWrapper = new DynamicWrapper();
            //when

            dynamic obj = new DynamicWrapper();
            obj.SomeValue = 2;

            dynamic arr = new List<dynamic>();
            arr.Add("3");


            DynamicWrapper["SomeValue"] = 1;
            DynamicWrapper["SomeObject"] = obj;
            DynamicWrapper.SomeArray = arr;
            //then

            Assert.AreEqual(DynamicWrapper.SomeValue, 1);
            Assert.AreEqual(DynamicWrapper.SomeObject.SomeValue, 2);
            Assert.AreEqual(DynamicWrapper.SomeArray[0], "3");
        }

        [Test]
        public void ShouldBindMethodOnDynamicWrapper()
        {
            dynamic DynamicWrapper = new DynamicWrapper();
            DynamicWrapper.SomeValue = 1;

            Assert.AreEqual(1, DynamicWrapper.SomeValue);
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
            Assert.AreEqual(
                "{\r\n  \"1\": {\r\n    \"TestProperty\": 1\r\n  },\r\n  \"2\": {\r\n    \"TestProperty\": 2\r\n  }\r\n}",
                result.ToString());
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
            //given

            dynamic obj = new DynamicWrapper();
            obj.SomeValue = 1;

            //when
            dynamic result = JsonConvert.SerializeObject(obj);
            //then
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
            Assert.AreEqual(
                "{\r\n  \"Result\": [\r\n    {\r\n      \"TestProperty\": 1\r\n    },\r\n    {\r\n      \"TestProperty\": 2\r\n    }\r\n  ]\r\n}",
                hostInstance.ToString());
        }

        [Test]
        public void ShouldConvertSimpleObjectToJObject()
        {
            //given

            var obj = new TestClass();
            obj.SomeProperty = "1";

            //when
            string result = JsonConvert.SerializeObject(obj);
            //then
            Assert.AreEqual("{\"SomeProperty\":\"1\"}", result);
        }

        [Test]
        public void ShouldConvertToByteArray()
        {
            var items = new Byte[] {10, 100, 200, 30}.ToDynamic();

            Assert.AreEqual(items.Length, 4);
        }

        [Test]
        public void ShouldConvertToJObject()
        {
            //given
            dynamic DynamicWrapper = new DynamicWrapper();
            dynamic obj = new DynamicWrapper();
            obj.SomeValue = 1;

            dynamic jarray = new[] {"3"};

            DynamicWrapper.SomeObj = obj;
            DynamicWrapper.SomeValue = 2;
            DynamicWrapper.SomeArray = jarray;

            //when
            dynamic jobject = JsonConvert.SerializeObject(DynamicWrapper);

            //then
            Assert.AreEqual("{\"SomeObj\":{\"SomeValue\":1},\"SomeValue\":2,\"SomeArray\":[\"3\"]}", jobject);
        }

        [Test]
        public void ShouldConvertToStringArray()
        {
            var stringArray = new List<dynamic> {"f", "a"};

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

            dynamic instance = DynamicWrapperExtensions.ToDynamic(obj);

            Assert.IsNotNull(instance.TestObjectField.TestProperty);
            Assert.IsNotNull(instance.TestPropertyField);
        }

        [Test]
        public void ShouldEnumerateDynamicWrapperProperties()
        {
            dynamic DynamicWrapper = new DynamicWrapper();
            DynamicWrapper.Property1 = 1;
            DynamicWrapper.Property2 = "testproperty";

            var props = new List<object>();
            foreach (dynamic property in DynamicWrapper)
            {
                props.Add(property);
            }

            Assert.AreEqual(2, props.Count);
            var pair = (KeyValuePair<string, dynamic>) props.ToArray().First();
            Assert.AreEqual(pair.Key, "Property1");
            pair = (KeyValuePair<string, dynamic>) props.ToArray().Skip(1).First();
            Assert.AreEqual(pair.Key, "Property2");
        }


        [Test]
        [Ignore]
        public void ShouldFailToAccessNonExistingProperty()
        {
            dynamic DynamicWrapper = new DynamicWrapper();
            dynamic obj = new DynamicWrapper();
            obj.SomeValue = 1;

            DynamicWrapper.SomeObj = obj;

            try
            {
                dynamic tryproperty = DynamicWrapper.SomeObj.SomeValue111;
            }
            catch
            {
                Assert.Pass();
                return;
            }
            Assert.Fail();
        }

        [Test]
        public void ShouldFailToAccessPropertyForSimpleValue()
        {
            dynamic DynamicWrapper = new DynamicWrapper();
            dynamic obj = new DynamicWrapper();
            obj.SomeValue = 1;

            DynamicWrapper.SomeObj = obj;

            try
            {
                dynamic tryproperty = DynamicWrapper.SomeObj.SomeValue.FailedProperty;
            }
            catch
            {
                Assert.Pass();
                return;
            }
            Assert.Fail();
        }

        [Test]
        public void ShouldFastAsJObjectNative()
        {
            Stopwatch stopWatch = Stopwatch.StartNew();

            dynamic DynamicWrapper = new DynamicWrapper();
            var attachments = new List<string>
                {
                    "1",
                    "2",
                    "3"
                };

            DynamicWrapper.Attachments = attachments;

            Console.WriteLine(DynamicWrapper.Attachments[0]);
            Console.WriteLine(DynamicWrapper.Attachments[1]);
            Console.WriteLine(DynamicWrapper.Attachments[2]);

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
            dynamic DynamicWrapper = new DynamicWrapper();
            var jarray = new[] {"1", "2", "3"};

            DynamicWrapper.SomeArray = jarray.ToDynamic();

            Assert.AreEqual("1", DynamicWrapper.SomeArray[0]);
            Assert.AreEqual("2", DynamicWrapper.SomeArray[1]);
            Assert.AreEqual("3", DynamicWrapper.SomeArray[2]);
        }


        [Test]
        public void ShouldGetPropertiesList()
        {
            dynamic DynamicWrapper = new DynamicWrapper();
            dynamic obj = new DynamicWrapper();
            obj.SomeValue = 1;

            dynamic jarray = new[] {"3"};


            DynamicWrapper.SomeObj = obj;
            DynamicWrapper.SomeValue = 2;
            DynamicWrapper.SomeArray = jarray;

            int propsCount = 0;
            var propList = new List<string>();

            foreach (dynamic instance in DynamicWrapper)
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
            //given
            dynamic DynamicWrapper = new DynamicWrapper();
            dynamic obj = new DynamicWrapper();
            obj.SomeValue = 1;

            dynamic jarray = new List<dynamic>();
            jarray.Add("3");

            //when
            DynamicWrapper.SomeObj = obj;
            DynamicWrapper.SomeValue = 2;
            DynamicWrapper.SomeArray = DynamicWrapperExtensions.ToDynamic((object) jarray);

            //then
            Assert.AreEqual(1, DynamicWrapper.SomeObj.SomeValue);
            dynamic arr = DynamicWrapper.SomeArray;
            Assert.AreEqual("3", arr[0]);
            Assert.AreEqual(2, DynamicWrapper.SomeValue);
            Assert.AreEqual(null, DynamicWrapper["NonExistingProperty"]);
        }

        [Test]
        public void ShouldHasProperty()
        {
            dynamic DynamicWrapper = new DynamicWrapper();
            dynamic obj = new DynamicWrapper();
            DynamicWrapper.SomeValue = obj;

            dynamic hasProp = DynamicWrapper.SomeValue != null;
            dynamic notHasProp = DynamicWrapper.NotHasSomeValue != null;

            Assert.True(hasProp);
            Assert.False(notHasProp);
        }

        [Test]
        public void ShouldInvokeLinqMethodsForObjectsCollections()
        {
            //given
            dynamic DynamicWrapper = new DynamicWrapper();

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
            DynamicWrapper.Attachments = attachments;
            //when
            IEnumerable<dynamic> enumerable = DynamicWrapperExtensions.ToEnumerable(DynamicWrapper.Attachments);
            //then
            Assert.AreEqual(3, enumerable.Count());
            dynamic instance = enumerable.FirstOrDefault(e => e.a == "3");
            Assert.AreEqual("3", instance.a);
        }

        [Test]
        public void ShouldInvokeLinqMethodsForSimpleValues()
        {
            //given
            dynamic DynamicWrapper = new DynamicWrapper();
            var attachments = new List<string>
                {
                    "1",
                    "2",
                    "3"
                };

            DynamicWrapper.Attachments = attachments;
            //when
            IEnumerable<dynamic> enumerable = DynamicWrapperExtensions.ToEnumerable(DynamicWrapper.Attachments);
            dynamic item = enumerable.FirstOrDefault(a => a == "1");
            //then
            Assert.AreEqual("1", item);
        }

        [Test]
        public void ShouldNotThrowOnSetNotExistingProperty()
        {
            //given
            dynamic DynamicWrapper = new DynamicWrapper();

            //when

            DynamicWrapper.SomeValue = 2;

            Assert.AreEqual(DynamicWrapper["SomeValue"], 2);
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
            //given
            dynamic DynamicWrapper = new DynamicWrapper();
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

            DynamicWrapper.Arr = arr;
            //when
            DynamicWrapper.Arr.Remove(i2);
            //then
            IEnumerable<dynamic> items = DynamicWrapperExtensions.ToEnumerable(DynamicWrapper.Arr);
            Assert.AreEqual(2, items.Count());
            Assert.AreEqual(1, items.First().Value);
            Assert.AreEqual(3, items.Skip(1).First().Value);
        }

        [Test]
        public void ShouldRemoveProperty()
        {
            //given
            var DynamicWrapper = new DynamicWrapper();
            DynamicWrapper["SomeValue"] = 1;

            //when
            DynamicWrapper["SomeValue"] = null;
            //then			
            Assert.AreEqual(null, DynamicWrapper["SomeValue"]);

            bool hasProps = false;

            foreach (var elements in DynamicWrapper)
            {
                hasProps = true;
            }


            Assert.IsFalse(hasProps);
        }

        [Test]
        [Ignore]
        public void ShouldReturnsNullAsArrayItem()
        {
            dynamic DynamicWrapper = new DynamicWrapper();

            dynamic arr = new object[] {null};
            DynamicWrapper.ArrayProperty = arr;

            Assert.IsNull(DynamicWrapper.ArrayProperty[0]);
        }

        [Test]
        public void ShouldReturnsNullAsPropertyValue()
        {
            dynamic DynamicWrapper = new DynamicWrapper();
            dynamic obj = new DynamicWrapper();
            obj.SomeValue = null;

            DynamicWrapper.SomeObj = obj;

            Assert.AreEqual(null, DynamicWrapper.SomeObj.SomeValue);
        }

        [Test]
        public void ShouldSetArrayObjectItemsAndTrackChanges()
        {
            //проверяем ссылки на один и тот же массив
            dynamic DynamicWrapper1 = new DynamicWrapper();
            DynamicWrapper1.ArrayProperty = new List<dynamic>();

            dynamic DynamicWrapper2 = new DynamicWrapper();
            DynamicWrapper2.ArrayProperty1 = DynamicWrapper1.ArrayProperty;

            dynamic DynamicWrapper3 = new DynamicWrapper();
            DynamicWrapper3["ArrayProperty2"] = DynamicWrapper1.ArrayProperty;

            dynamic checkInstance = new DynamicWrapper();
            checkInstance.SimpleField = 1;

            DynamicWrapper1.ArrayProperty.Add(checkInstance);

            Assert.AreEqual(1, DynamicWrapper1.ArrayProperty.Count);
            Assert.AreEqual(1, DynamicWrapper2.ArrayProperty1.Count);
            Assert.AreEqual(1, DynamicWrapper3.ArrayProperty2.Count);

            //проверяем изменение при использовании SetProperty
            DynamicWrapper3.ArrayProperty2 = new List<dynamic>();

            Assert.AreEqual(0, DynamicWrapper3.ArrayProperty2.Count);

            //проверяем, что ссылка удаляется при присвоении другого массива

            DynamicWrapper2.ArrayProperty1 = new List<dynamic>();

            Assert.AreEqual(0, DynamicWrapper2.ArrayProperty1.Count);
            Assert.AreEqual(1, DynamicWrapper1.ArrayProperty.Count);

            //проверяем что после смены ссылки на массив отслеживание изменений прекратилось
            dynamic checkInstance2 = new DynamicWrapper();
            checkInstance2.SimpleField = 2;

            DynamicWrapper1.ArrayProperty.Add(checkInstance2);

            Assert.AreEqual(2, DynamicWrapper1.ArrayProperty.Count);
            Assert.AreEqual(1, DynamicWrapper1.ArrayProperty[0].SimpleField);
            Assert.AreEqual(2, DynamicWrapper1.ArrayProperty[1].SimpleField);
            Assert.AreEqual(0, DynamicWrapper2.ArrayProperty1.Count);

            //проверяем что в старом массиве ничего не изменяется при добавлении в новый
            dynamic checkInstance3 = new DynamicWrapper();
            checkInstance3.SimpleField = 3;

            DynamicWrapper2.ArrayProperty1.Add(checkInstance3);

            Assert.AreEqual(2, DynamicWrapper1.ArrayProperty.Count);
            Assert.AreEqual(1, DynamicWrapper2.ArrayProperty1.Count);

            Assert.AreEqual(3, DynamicWrapper2.ArrayProperty1[0].SimpleField);

            //проверяем удаление из массива
            DynamicWrapper2.ArrayProperty1.Remove(checkInstance3);
            Assert.AreEqual(2, DynamicWrapper1.ArrayProperty.Count);
            Assert.AreEqual(0, DynamicWrapper2.ArrayProperty1.Count);

            //проверяем изменение ссылки на существующий массив
            DynamicWrapper2.ArrayProperty1 = DynamicWrapper1.ArrayProperty;
            Assert.AreEqual(2, DynamicWrapper2.ArrayProperty1.Count);
            Assert.AreEqual(2, DynamicWrapper1.ArrayProperty.Count);
        }

        [Test]
        public void ShouldSetArrayProperty()
        {
            dynamic DynamicWrapper = new DynamicWrapper();
            dynamic obj = new DynamicWrapper();
            obj.SomeValue = 1;

            dynamic arr = new[] {obj};
            DynamicWrapper.ArrayProperty = arr;

            Assert.AreEqual(DynamicWrapper.ArrayProperty[0].SomeValue, 1);
        }

        [Test]
        public void ShouldSetArrayPropertyWithSomeElements1()
        {
            dynamic DynamicWrapper = new DynamicWrapper();
            dynamic obj1 = new DynamicWrapper();
            obj1.SomeValue = 1;
            dynamic obj2 = new DynamicWrapper();
            obj2.SomeValue = 2;

            dynamic arr = new[] {obj1, obj2};
            DynamicWrapper.ArrayProperty = arr;

            Assert.AreEqual(DynamicWrapper.ArrayProperty[1].SomeValue, 2);
        }


        [Test]
        public void ShouldSetArrayPropertyWithSomeElements2()
        {
            dynamic DynamicWrapper = new DynamicWrapper();
            dynamic obj1 = new DynamicWrapper();
            obj1.SomeValue = 1;
            dynamic obj2 = new DynamicWrapper();
            obj2.SomeValue = 2;

            dynamic arr = new[] {obj1, obj2};
            DynamicWrapper.ArrayProperty = arr;

            Assert.AreEqual(DynamicWrapper.ArrayProperty[1].SomeValue, 2);
        }


        [Test]
        public void ShouldSetArrayPropertyWithSomeElements3()
        {
            dynamic DynamicWrapper = new DynamicWrapper();
            dynamic obj1 = new DynamicWrapper();
            obj1.SomeValue = 1;
            dynamic obj2 = new DynamicWrapper();
            obj2.SomeValue = 2;

            dynamic arr = new List<DynamicWrapper> {obj1, obj2};
            DynamicWrapper.ArrayProperty = arr;

            Assert.AreEqual(DynamicWrapper.ArrayProperty[1].SomeValue, 2);
        }

        [Test]
        public void ShouldSetArrayValue()
        {
            dynamic DynamicWrapper = new DynamicWrapper();
            var obj = new List<dynamic>();
            DynamicWrapper.Property = obj;

            Assert.IsNotNull(DynamicWrapper.Property);
        }

        [Test]
        public void ShouldSetCompositProperty()
        {
            dynamic DynamicWrapper = new DynamicWrapper();
            dynamic obj = new DynamicWrapper();
            obj.SomeValue = 1;

            DynamicWrapper.SomeObj = obj;

            Assert.AreEqual(1, DynamicWrapper.SomeObj.SomeValue);
        }

        [Test]
        public void ShouldSetDateTimeValue()
        {
            dynamic DynamicWrapper = new DynamicWrapper();
            DynamicWrapper.Property = new DateTime(2011, 01, 01);

            Assert.AreEqual(DynamicWrapper.Property, new DateTime(2011, 01, 01));
        }

        [Test]
        public void ShouldSetObjectValue()
        {
            dynamic DynamicWrapper = new DynamicWrapper();
            var obj = new DynamicWrapper();
            DynamicWrapper.Property = obj;

            Assert.IsNotNull(DynamicWrapper.Property);
        }

        [Test]
        public void ShouldSetPropertiesWithoutTransformation()
        {
            //given
            dynamic DynamicWrapper = new DynamicWrapper();

            //when
            var attachments = new List<string>
                {
                    "1",
                    "2",
                    "3"
                };

            DynamicWrapper.Attachments = attachments;
            //then
            Assert.AreEqual("1", DynamicWrapper.Attachments[0]);
            Assert.AreEqual("2", DynamicWrapper.Attachments[1]);
            Assert.AreEqual("3", DynamicWrapper.Attachments[2]);
        }

        [Test]
        public void ShouldSetPropertyValue()
        {
            //given
            dynamic DynamicWrapper = new DynamicWrapper();

            dynamic obj = new DynamicWrapper();
            obj.SomeValue = 2;

            dynamic arr = new List<dynamic>();
            arr.Add("3");

            DynamicWrapper.SomeValue = 0;
            DynamicWrapper.SomeObject = null;
            DynamicWrapper.SomeArray = null;

            //when

            DynamicWrapper.SomeValue = 2;
            DynamicWrapper.SomeObject = obj;
            DynamicWrapper.SomeArray = arr;


            //then

            Assert.AreEqual(DynamicWrapper.SomeValue, 2);
            Assert.AreEqual(DynamicWrapper.SomeObject.SomeValue, 2);
            Assert.AreEqual(DynamicWrapper.SomeArray[0], "3");
        }

        [Test]
        public void ShouldThrowIfIndexIncorrect()
        {
            dynamic DynamicWrapper = new DynamicWrapper();
            dynamic obj = new DynamicWrapper();
            obj.SomeValue = 1;

            dynamic arr = new[] {obj};
            DynamicWrapper.ArrayProperty = arr;

            try
            {
                dynamic tryarrayitem = DynamicWrapper.ArrayProperty[1];
            }
            catch
            {
                Assert.Pass();
                return;
            }
            Assert.Fail();
        }

        [Test]
        public void ShouldThrowOnAccessNonExistingObject()
        {
            dynamic DynamicWrapper = new DynamicWrapper();

            dynamic arr = new object[] {null};
            DynamicWrapper.ArrayProperty = arr;

            try
            {
                dynamic tryget = DynamicWrapper.ArrayProperty[0].SomeValue;
            }
            catch
            {
                Assert.Pass();
                return;
            }
            Assert.Fail();
        }
    }
}