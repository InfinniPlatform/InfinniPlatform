using System;
using System.Globalization;
using System.Threading;
using InfinniPlatform.Api.Schema;
using InfinniPlatform.FastReport.Templates.Data;

using NUnit.Framework;

namespace InfinniPlatform.FastReport.Tests.Templates.Data
{
	[TestFixture]
	[Category(TestCategories.UnitTest)]
	public sealed class DataExtensionsTest
	{
		private static readonly CultureInfo Culture = new CultureInfo("en-US");

		[TestFixtureSetUp]
		public void SetUp()
		{
			Thread.CurrentThread.CurrentCulture = Culture;
		}

		[Test]
		[TestCaseSource("ConvertToStringSource")]
		public void ConvertToString(object value, string expected)
		{
			var result = (string)value.ConvertTo(SchemaDataType.String);

			Assert.AreEqual(expected, result);
		}

		[Test]
		[TestCaseSource("ConvertToFloatSource")]
		public void ConvertToFloat(object value, double expected)
		{
			var result = (double)value.ConvertTo(SchemaDataType.Float);

			Assert.AreEqual(expected, result, 0.00001);
		}

		[Test]
		[TestCaseSource("ConvertToIntegerSource")]
		public void ConvertToInteger(object value, int expected)
		{
			var result = (int)value.ConvertTo(SchemaDataType.Integer);

			Assert.AreEqual(expected, result);
		}

		[Test]
		[TestCaseSource("ConvertToBooleanSource")]
		public void ConvertToBoolean(object value, bool expected)
		{
			var result = (bool)value.ConvertTo(SchemaDataType.Boolean);

			Assert.AreEqual(expected, result);
		}

		[Test]
		[TestCaseSource("ConvertToDateTimeSource")]
		public void ConvertToDateTime(object value, DateTime expected)
		{
			var result = (DateTime)value.ConvertTo(SchemaDataType.DateTime);

			Assert.AreEqual(expected, result);
		}


		// ReSharper disable RedundantArrayCreationExpression
		// ReSharper disable RedundantCast

		private static readonly object[] ConvertToStringSource
			= new object[]
			  {
				  new object[] { null, null },
				  new object[] { "String", "String" },
				  new object[] { true, "True" },
				  new object[] { (byte)123, "123" },
				  new object[] { (sbyte)-123, "-123" },
				  new object[] { (short)12345, "12345" },
				  new object[] { (ushort)12345, "12345" },
				  new object[] { (int)12345, "12345" },
				  new object[] { (uint)12345, "12345" },
				  new object[] { (long)12345, "12345" },
				  new object[] { (ulong)12345, "12345" },
				  new object[] { (float)123.45, "123.45" },
				  new object[] { (double)123.45, "123.45" },
				  new object[] { (decimal)123.45, "123.45" },
				  new object[] { new Guid("48562f32-a491-4377-93c6-c9d67fbc24ee"), new Guid("48562f32-a491-4377-93c6-c9d67fbc24ee").ToString() },
				  new object[] { DateTime.Today, DateTime.Today.ToString(Culture) }
			  };

		private static readonly object[] ConvertToFloatSource
			= new object[]
			  {
				  new object[] { null, 0d },
				  new object[] { true, 1d },
				  new object[] { (byte)123, 123d },
				  new object[] { (sbyte)-123, -123d },
				  new object[] { (short)12345, 12345d },
				  new object[] { (ushort)12345, 12345d },
				  new object[] { (int)12345, 12345d },
				  new object[] { (uint)12345, 12345d },
				  new object[] { (long)12345, 12345d },
				  new object[] { (ulong)12345, 12345d },
				  new object[] { (float)123.45, 123.45d },
				  new object[] { (double)123.45, 123.45d },
				  new object[] { (decimal)123.45, 123.45d }
			  };

		private static readonly object[] ConvertToIntegerSource
			= new object[]
			  {
				  new object[] { null, 0 },
				  new object[] { true, 1 },
				  new object[] { (byte)123, 123 },
				  new object[] { (sbyte)-123, -123 },
				  new object[] { (short)12345, 12345 },
				  new object[] { (ushort)12345, 12345 },
				  new object[] { (int)12345, 12345 },
				  new object[] { (uint)12345, 12345 },
				  new object[] { (long)12345, 12345 },
				  new object[] { (ulong)12345, 12345 },
				  new object[] { (float)123.45, 123 },
				  new object[] { (double)123.45, 123 },
				  new object[] { (decimal)123.45, 123 }
			  };

		private static readonly object[] ConvertToBooleanSource
			= new object[]
			  {
				  new object[] { null, false },
				  new object[] { true, true },
				  new object[] { false, false },
				  new object[] { "true", true },
				  new object[] { "True", true },
				  new object[] { "false", false },
				  new object[] { "False", false },
				  new object[] { (byte)0, false },
				  new object[] { (byte)1, true },
				  new object[] { (sbyte)0, false },
				  new object[] { (sbyte)1, true },
				  new object[] { (short)0, false },
				  new object[] { (short)1, true },
				  new object[] { (ushort)0, false },
				  new object[] { (ushort)1, true },
				  new object[] { (int)0, false },
				  new object[] { (int)1, true },
				  new object[] { (uint)0, false },
				  new object[] { (uint)1, true },
				  new object[] { (long)0, false },
				  new object[] { (long)1, true },
				  new object[] { (ulong)0, false },
				  new object[] { (ulong)1, true },
				  new object[] { (float)0, false },
				  new object[] { (float)1, true },
				  new object[] { (double)0, false },
				  new object[] { (double)1, true },
				  new object[] { (decimal)0, false },
				  new object[] { (decimal)1, true }
			  };

		private static readonly object[] ConvertToDateTimeSource
			= new object[]
			  {
				  new object[] { null, default(DateTime) },
				  new object[] { "2014-01-02 03:04:05", new DateTime(2014, 1, 2, 3, 4, 5) } // ISO 8601
			  };

		// ReSharper restore RedundantCast
		// ReSharper restore RedundantArrayCreationExpression
	}
}