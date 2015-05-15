using System.Linq;

using InfinniPlatform.Reporting.DataProviders;

using Newtonsoft.Json.Linq;

using NUnit.Framework;

namespace InfinniPlatform.Reporting.Tests.DataProviders
{
	[TestFixture]
	[Category(TestCategories.UnitTest)]
	public sealed class JsonDataProviderTest
	{
		[Test]
		public void ShouldEnumerateData()
		{
			// Given
			var array = new[] { new JObject(), new JObject(), new JObject() };
			var target = new JsonDataProvider(array);

			// When
			var result = target.Cast<object>().ToArray();

			// Then
			CollectionAssert.AreEqual(array, result);
		}

		[Test]
		public void ShouldEnumerateDataWhenSourceDataIsNull()
		{
			// Given
			var target = new JsonDataProvider(null);

			// When
			var result = target.Cast<object>().ToArray();

			// Then
			Assert.AreEqual(0, result.Length);
		}

		[Test]
		public void ShouldReturnNullWhenPropertyIsNotExists()
		{
			// Given
			var jObject = new JObject();
			var target = new JsonDataProvider(new[] { jObject });

			// When
			var result = target.GetPropertyValue(jObject, "NotExistsProperty");

			// Then
			Assert.IsNull(result);
		}

		[Test]
		public void ShouldGetObjectPropertyValue()
		{
			// Given
			var jObject = new JObject();
			var jSubObject = new JObject();
			jObject["Property1"] = jSubObject;

			// When
			var target = new JsonDataProvider(new[] { jObject });
			var result = target.GetPropertyValue(jObject, "Property1");

			// Then
			Assert.AreEqual(jSubObject, result);
		}

		[Test]
		public void ShouldGetArrayPropertyValue()
		{
			// Given
			var jObject = new JObject();
			var jArray = new JArray(1, 2, 3);
			jObject["Property1"] = jArray;

			// When
			var target = new JsonDataProvider(new[] { jObject });
			var result = target.GetPropertyValue(jObject, "Property1");

			// Then
			Assert.AreEqual(jArray, result);
		}

		[Test]
		public void ShouldGetScalarPropertyValue()
		{
			// Given
			var jObject = JObject.Parse("{ 'Property1': 123 }");

			// When
			var target = new JsonDataProvider(new[] { jObject });
			var result = target.GetPropertyValue(jObject, "Property1");

			// Then
			Assert.AreEqual(123, result);
		}
	}
}