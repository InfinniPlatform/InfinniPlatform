using System.Linq;

using InfinniPlatform.Reporting.DataProviders;

using NUnit.Framework;

namespace InfinniPlatform.Reporting.Tests.DataProviders
{
	[TestFixture]
	[Category(TestCategories.UnitTest)]
	public sealed class ObjectDataProviderTest
	{
		[Test]
		public void ShouldEnumerateData()
		{
			// Given
			var array = new[] { new object(), new object(), new object() };
			var target = new ObjectDataProvider(array);

			// When
			var result = target.Cast<object>().ToArray();

			// Then
			CollectionAssert.AreEqual(array, result);
		}

		[Test]
		public void ShouldEnumerateDataWhenSourceDataIsNull()
		{
			// Given
			var target = new ObjectDataProvider(null);

			// When
			var result = target.Cast<object>().ToArray();

			// Then
			Assert.AreEqual(0, result.Length);
		}

		[Test]
		public void ShouldReturnNullWhenPropertyIsNotExists()
		{
			// Given
			var obj = new object();
			var target = new ObjectDataProvider(new[] { obj });

			// When
			var result = target.GetPropertyValue(obj, "NotExistsProperty");

			// Then
			Assert.IsNull(result);
		}

		[Test]
		public void ShouldGetPropertyValue()
		{
			// Given
			var obj = new TestEntity { Property1 = "12345" };

			// When
			var target = new ObjectDataProvider(new[] { obj });
			var result = target.GetPropertyValue(obj, "Property1");

			// Then
			Assert.AreEqual("12345", result);
		}

		[Test]
		public void ShouldGetFieldValue()
		{
			// Given
			var obj = new TestEntity { Field1 = 123 };

			// When
			var target = new ObjectDataProvider(new[] { obj });
			var result = target.GetPropertyValue(obj, "Field1");

			// Then
			Assert.AreEqual(123, result);
		}


		// ReSharper disable NotAccessedField.Local
		// ReSharper disable UnusedAutoPropertyAccessor.Local

		class TestEntity
		{
			public int Field1;

			public string Property1 { get; set; }
		}

		// ReSharper restore UnusedAutoPropertyAccessor.Local
		// ReSharper restore NotAccessedField.Local
	}
}