using System.Dynamic;
using System.Linq;

using InfinniPlatform.Reporting.DataProviders;
using InfinniPlatform.Sdk.Dynamic;
using Newtonsoft.Json.Linq;

using NUnit.Framework;

namespace InfinniPlatform.Reporting.Tests.DataProviders
{
	[TestFixture]
	[Category(TestCategories.UnitTest)]
	public sealed class DynamicDataProviderTest
	{
		[Test]
		public void ShouldEnumerateData()
		{
			// Given
			var array = new dynamic[] { new object(), new object(), new object() };
			var target = new DynamicDataProvider(array);

			// When
			var result = target.Cast<dynamic>().ToArray();

			// Then
			CollectionAssert.AreEqual(array, result);
		}

		[Test]
		public void ShouldEnumerateDataWhenSourceDataIsNull()
		{
			// Given
			var target = new DynamicDataProvider(null);

			// When
			var result = target.Cast<dynamic>().ToArray();

			// Then
			Assert.AreEqual(0, result.Length);
		}

		[Test]
		public void ShouldReturnNullWhenPropertyIsNotExists()
		{
			// Given
			var target = new DynamicDataProvider(null);
			dynamic obj = new object();

			// When
			var result = target.GetPropertyValue(obj, "NotExistsProperty");

			// Then
			Assert.IsNull(result);
		}

		[Test]
		public void SholdReturnPropertyValueForDynamicWrapper()
		{
			// Given
			var target = new DynamicDataProvider(null);
			dynamic obj = new DynamicWrapper();
			obj.Property1 = 12345;

			// When
			var result = target.GetPropertyValue(obj, "Property1");

			// Then
			Assert.AreEqual(12345, result);
		}

		[Test]
		public void SholdReturnPropertyValueForJsonObject()
		{
			// Given
			var target = new DynamicDataProvider(null);
			dynamic obj = new JObject();
			obj.Property1 = 12345;

			// When
			var result = target.GetPropertyValue(obj, "Property1");

			// Then
			Assert.AreEqual(12345, result);
		}

		[Test]
		public void SholdReturnPropertyValueForDictionaryObject()
		{
			// Given
			var target = new DynamicDataProvider(null);
			dynamic obj = new ExpandoObject();
			obj.Property1 = 12345;

			// When
			var result = target.GetPropertyValue(obj, "Property1");

			// Then
			Assert.AreEqual(12345, result);
		}

		[Test]
		public void SholdReturnPropertyValueForClrObject()
		{
			// Given
			var target = new DynamicDataProvider(null);
			dynamic obj = new SomeClass { Property1 = 12345, Field1 = 67890 };

			// When
			var result1 = target.GetPropertyValue(obj, "Property1");
			var result2 = target.GetPropertyValue(obj, "Field1");

			// Then
			Assert.AreEqual(12345, result1);
			Assert.AreEqual(67890, result2);
		}


		// ReSharper disable NotAccessedField.Local
		// ReSharper disable UnusedAutoPropertyAccessor.Local

		private class SomeClass
		{
			public int Property1 { get; set; }
			public int Field1;
		}

		// ReSharper restore UnusedAutoPropertyAccessor.Local
		// ReSharper restore NotAccessedField.Local
	}
}