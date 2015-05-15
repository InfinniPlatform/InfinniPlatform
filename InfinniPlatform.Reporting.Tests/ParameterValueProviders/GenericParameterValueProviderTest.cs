using System.Collections.Generic;

using InfinniPlatform.FastReport.Templates.Data;
using InfinniPlatform.Reporting.ParameterValueProviders;

using NUnit.Framework;

namespace InfinniPlatform.Reporting.Tests.ParameterValueProviders
{
	[TestFixture]
	[Category(TestCategories.UnitTest)]
	public sealed class GenericParameterValueProviderTest
	{
		[Test]
		public void ShouldInvokeRegisterProvider()
		{
			// Given

			var providerInfo = new ParameterValueProviderInfoStub();
			var dataSources = new List<DataSourceInfo>();
			var values = new Dictionary<string, object>();

			var provider = new ParameterValueProviderStub(values);
			var target = new GenericParameterValueProvider();
			target.RegisterProvider<ParameterValueProviderInfoStub>(provider);

			// When
			var result = target.GetParameterValues(providerInfo, dataSources);

			// Then
			Assert.AreEqual(values, result);
		}


		class ParameterValueProviderStub : IParameterValueProvider
		{
			private readonly IDictionary<string, object> _values;

			public ParameterValueProviderStub(IDictionary<string, object> values)
			{
				_values = values;
			}

			public IDictionary<string, object> GetParameterValues(IParameterValueProviderInfo providerInfo, IEnumerable<DataSourceInfo> dataSources)
			{
				return _values;
			}
		}


		class ParameterValueProviderInfoStub : IParameterValueProviderInfo
		{
		}
	}
}