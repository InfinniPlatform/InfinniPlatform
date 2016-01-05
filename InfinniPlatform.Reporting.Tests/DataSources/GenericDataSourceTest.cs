using System.Collections.Generic;

using InfinniPlatform.Core.Schema;
using InfinniPlatform.FastReport.Templates.Data;
using InfinniPlatform.Reporting.DataSources;

using Newtonsoft.Json.Linq;

using NUnit.Framework;

namespace InfinniPlatform.Reporting.Tests.DataSources
{
    [TestFixture]
    [Category(TestCategories.UnitTest)]
    public sealed class GenericDataSourceTest
    {
        [Test]
        public void ShouldInvokeRegisterDataSource()
        {
            // Given

            var dataSourceInfo = new DataSourceInfo
                                 {
                                     Name = "DataSource1",
                                     Provider = new DataProviderInfoStub(),
                                     Schema = new DataSchema()
                                 };

            var parameterInfos = new List<ParameterInfo>();
            var parameterValues = new Dictionary<string, object>();

            var data = new JArray();
            var dataSource = new DataSourceStub(data);

            var target = new GenericDataSource();
            target.RegisterDataSource(dataSource);

            // When
            var result = target.GetData(dataSourceInfo, parameterInfos, parameterValues);

            // Then
            Assert.AreEqual(data, result);
        }
    }
}