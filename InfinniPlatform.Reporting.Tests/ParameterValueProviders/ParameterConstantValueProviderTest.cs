using System.Collections.Generic;

using InfinniPlatform.FastReport.Templates.Data;
using InfinniPlatform.Reporting.ParameterValueProviders;

using NUnit.Framework;

namespace InfinniPlatform.Reporting.Tests.ParameterValueProviders
{
	[TestFixture]
	[Category(TestCategories.UnitTest)]
	public sealed class ParameterConstantValueProviderTest
	{
		[Test]
		public void ShouldGetParameterValues()
		{
			// Given

			var valueProviderInfo = new ParameterConstantValueProviderInfo
										{
											Items = new Dictionary<string, IDataBind>
						                                {
							                                { "Label1", new ConstantBind { Value = "Value1" } },
							                                { "Label2", new ConstantBind { Value = "Value2" } },
							                                { "Label3", new ConstantBind { Value = "Value3" } }
						                                }
										};

			// When
			var target = new ParameterConstantValueProvider();
			var result = target.GetParameterValues(valueProviderInfo, null);

			// Then
			Assert.IsNotNull(result);
			Assert.AreEqual(3, result.Count);
			Assert.AreEqual("Value1", result["Label1"]);
			Assert.AreEqual("Value2", result["Label2"]);
			Assert.AreEqual("Value3", result["Label3"]);
		}
	}
}