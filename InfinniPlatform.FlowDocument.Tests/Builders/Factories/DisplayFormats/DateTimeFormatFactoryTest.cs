using System;

using InfinniPlatform.Api.Dynamic;

using NUnit.Framework;

namespace InfinniPlatform.FlowDocument.Tests.Builders.Factories.DisplayFormats
{
	[TestFixture]
	[Category(TestCategories.UnitTest)]
	public sealed class DateTimeFormatFactoryTest
	{
		[Test]
		public void ShouldReturnNullWhenNotDateTimeValue()
		{
			// Given
			dynamic elementMetadata = new DynamicWrapper();

			// When
			Func<object, string> format = BuildTestHelper.BuildDateTimeFormat(elementMetadata);
			var result1 = format(null);
			var result2 = format(new object());

			// Then
			Assert.IsNull(result1);
			Assert.IsNull(result2);
		}

		[Test]
		public void ShouldFormatWithDefaultSettings()
		{
			// Given
			var value = DateTime.Now;
			dynamic elementMetadata = new DynamicWrapper();

			// When
			Func<object, string> format = BuildTestHelper.BuildDateTimeFormat(elementMetadata);
			var result = format(value);

			// Then
			Assert.AreEqual(value.ToString("G"), result);
		}

		[Test]
		public void ShouldFormatWithSettings()
		{
			// Given
			var value = new DateTime(2014, 10, 15);
			dynamic elementMetadata = new DynamicWrapper();
			elementMetadata.Format = "yyyy'.'MM'.'dd";

			// When
			Func<object, string> format = BuildTestHelper.BuildDateTimeFormat(elementMetadata);
			var result = format(value);

			// Then
			Assert.AreEqual("2014.10.15", result);
		}
	}
}