using System.Collections.Generic;
using System.Linq;

using InfinniPlatform.Reporting.DataProviders;

using NUnit.Framework;

namespace InfinniPlatform.Reporting.Tests.DataProviders
{
	[TestFixture]
	[Category(TestCategories.UnitTest)]
	public sealed class KeyValueDataProviderTest
	{
		[Test]
		public void ShouldEnumerateData()
		{
			// Given

			var dataReader = new[]
				                 {
					                 new Dictionary<string, object>
						                 {
							                 { "Column1", 1 },
							                 { "Column2", "Value1" },
						                 },
					                 new Dictionary<string, object>
						                 {
							                 { "Column1", 2 },
							                 { "Column2", "Value2" },
						                 }
				                 };

			var target = new KeyValueDataProvider(dataReader);

			// When
			var result = target.Cast<object>().ToArray();

			// Then
			Assert.AreEqual(2, result.Length);
		}

		[Test]
		public void ShouldEnumerateDataWhenSourceDataIsNull()
		{
			// Given
			var target = new KeyValueDataProvider(null);

			// When
			var result = target.Cast<object>().ToArray();

			// Then
			Assert.AreEqual(0, result.Length);
		}

		[Test]
		public void ShouldReturnNullWhenPropertyIsNotExists()
		{
			var dataReader = new[]
				                 {
					                 new Dictionary<string, object>
						                 {
							                 { "Column1", 1 },
							                 { "Column2", "Value1" },
						                 },
					                 new Dictionary<string, object>
						                 {
							                 { "Column1", 2 },
							                 { "Column2", "Value2" },
						                 }
				                 };

			var target = new KeyValueDataProvider(dataReader);

			// When

			var targetEnumerator = target.GetEnumerator();
			targetEnumerator.MoveNext();

			var result = target.GetPropertyValue(targetEnumerator.Current, "NotExistsProperty");

			// Then
			Assert.IsNull(result);
		}

		[Test]
		public void ShouldGetPropertyValue()
		{
			var dataReader = new[]
				                 {
					                 new Dictionary<string, object>
						                 {
							                 { "Column1", 1 },
							                 { "Column2", "Value1" },
						                 },
					                 new Dictionary<string, object>
						                 {
							                 { "Column1", 2 },
							                 { "Column2", "Value2" },
						                 }
				                 };

			var target = new KeyValueDataProvider(dataReader);

			// When

			var targetEnumerator = target.GetEnumerator();

			targetEnumerator.MoveNext();
			var row0Column1 = target.GetPropertyValue(targetEnumerator.Current, "Column1");
			var row0Column2 = target.GetPropertyValue(targetEnumerator.Current, "Column2");

			targetEnumerator.MoveNext();
			var row1Column1 = target.GetPropertyValue(targetEnumerator.Current, "Column1");
			var row1Column2 = target.GetPropertyValue(targetEnumerator.Current, "Column2");

			// Then
			Assert.AreEqual(1, row0Column1);
			Assert.AreEqual("Value1", row0Column2);
			Assert.AreEqual(2, row1Column1);
			Assert.AreEqual("Value2", row1Column2);
		}
	}
}