using System;
using System.Collections;
using System.Collections.Generic;

using InfinniPlatform.Sdk.Dynamic;

using Newtonsoft.Json.Linq;

using NUnit.Framework;

namespace InfinniPlatform.Core.Tests.Dynamic
{
    [TestFixture]
    [Category(TestCategories.UnitTest)]
    public sealed class DynamicWrapperJsonConverterTest
    {
        private static JObject ConvertToJson(DynamicWrapper value)
        {
            return JObject.FromObject(value);
        }

        private static DynamicWrapper ConvertFromJson(JObject value)
        {
            return value.ToObject<DynamicWrapper>();
        }


        // DYNAMIC -> JSON


        // DYNAMIC TEST DATA

        private static DynamicWrapper CreateSimpleObject()
        {
            var result = new DynamicWrapper();
            result["Bool"] = true;
            result["Int32"] = 12345;
            result["Float"] = 6.789;
            result["String"] = "Some string";
            result["DateTime"] = DateTime.Today;
            result["SimpleArray"] = new[] {1, 2, 3};

            return result;
        }

        private static DynamicWrapper CreateComplexObject()
        {
            var result = CreateSimpleObject();
            result["NestedObject"] = CreateSimpleObject();
            result["ComplexArray"] = new[] {CreateSimpleObject(), CreateSimpleObject(), CreateSimpleObject()};

            return result;
        }

        private static void AssertSimpleObject(DynamicWrapper value)
        {
            Assert.IsNotNull(value);
            Assert.AreEqual(true, value["Bool"]);
            Assert.AreEqual(12345, value["Int32"]);
            Assert.AreEqual(6.789, value["Float"]);
            Assert.AreEqual("Some string", value["String"]);
            Assert.AreEqual(DateTime.Today, value["DateTime"]);
            CollectionAssert.AreEquivalent(new[] {1, 2, 3}, (IEnumerable) value["SimpleArray"]);
        }

        private static void AssertComplexObject(DynamicWrapper value)
        {
            AssertSimpleObject(value);
            AssertSimpleObject((DynamicWrapper) value["NestedObject"]);

            var complexArray = ((IList<object>) value["ComplexArray"]);
            Assert.AreEqual(3, complexArray.Count);
            AssertSimpleObject((DynamicWrapper) complexArray[0]);
            AssertSimpleObject((DynamicWrapper) complexArray[1]);
            AssertSimpleObject((DynamicWrapper) complexArray[2]);
        }


        // JSON TEST DATA

        private static JObject CreateSimpleJson()
        {
            var result = new JObject();
            result["Bool"] = true;
            result["Int32"] = 12345;
            result["Float"] = 6.789;
            result["String"] = "Some string";
            result["DateTime"] = DateTime.Today;
            result["SimpleArray"] = new JArray(1, 2, 3);

            return result;
        }

        private static JObject CreateComplexJson()
        {
            JObject result = CreateSimpleJson();
            result["NestedObject"] = CreateSimpleJson();
            result["ComplexArray"] = new JArray(CreateSimpleJson(), CreateSimpleJson(), CreateSimpleJson());

            return result;
        }

        private static void AssertSimpleJson(JObject value)
        {
            Assert.IsNotNull(value);
            Assert.AreEqual(true, ((JValue) value["Bool"]).Value);
            Assert.AreEqual(12345, ((JValue) value["Int32"]).Value);
            Assert.AreEqual(6.789, ((JValue) value["Float"]).Value);
            Assert.AreEqual("Some string", ((JValue) value["String"]).Value);
            Assert.AreEqual(DateTime.Today, ((JValue) value["DateTime"]).Value);
            CollectionAssert.AreEquivalent(new[] {1, 2, 3}, ((JArray) value["SimpleArray"]).Values<int>());
        }

        private static void AssertComplexJson(JObject value)
        {
            AssertSimpleJson(value);
            AssertSimpleJson((JObject) value["NestedObject"]);

            var complexArray = (JArray) value["ComplexArray"];
            Assert.AreEqual(3, complexArray.Count);
            AssertSimpleJson((JObject) complexArray[0]);
            AssertSimpleJson((JObject) complexArray[1]);
            AssertSimpleJson((JObject) complexArray[2]);
        }

        [Test]
        public void ShouldConvertFromJsonWhenComplexJson()
        {
            // Given
            JObject value = CreateComplexJson();

            // When
            var result = ConvertFromJson(value);

            // Then
            AssertComplexObject(result);
        }

        [Test]
        public void ShouldConvertFromJsonWhenSimpleJson()
        {
            // Given
            JObject value = CreateSimpleJson();

            // When
            var result = ConvertFromJson(value);

            // Then
            AssertSimpleObject(result);
        }

        [Test]
        public void ShouldConvertToJsonWhenComplexObject()
        {
            // Given
            var value = CreateComplexObject();

            // When
            JObject result = ConvertToJson(value);

            // Then
            AssertComplexJson(result);
        }

        [Test]
        public void ShouldConvertToJsonWhenSimpleObject()
        {
            // Given
            var value = CreateSimpleObject();

            // When
            JObject result = ConvertToJson(value);

            // Then
            AssertSimpleJson(result);
        }
    }
}