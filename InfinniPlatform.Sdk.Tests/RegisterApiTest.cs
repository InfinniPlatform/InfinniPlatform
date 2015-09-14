using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfinniPlatform.Sdk.Api;
using InfinniPlatform.Sdk.Environment.Index;
using NUnit.Framework;

namespace InfinniPlatform.Sdk.Tests
{
    [TestFixture("Для запуска теста необходимо наличие конфигурации и запущенного сервера.")]
    [Ignore]
    public class RegisterApiTest
    {
        private const string InfinniSessionPort = "9900";
        private const string InfinniSessionServer = "localhost";
        private const string Route = "1";

        private InfinniRegisterApi _api;

        [TestFixtureSetUp]
        public void SetupApi()
        {
            _api = new InfinniRegisterApi(InfinniSessionServer, InfinniSessionPort, Route);
        }

        [Test]
        public void ShouldGetValuesByDate()
        {
            var values = _api.GetValuesByDate("PTA", "FundsMovement",
                                         DateTime.Now,
                                         new List<string> { "Category" },
                                         new[] { "Price" },
                                         new[] { AggregationType.Sum },
                                         null);
            Assert.IsNotNull(values);
        }

        [Test]
        public void ShouldGetRegisterEntries()
        {
            var values = _api.GetRegisterEntries("PTA", "FundsMovement",
                                                        f =>
                                                        {
                                                            f.AddCriteria(c => c.Property("DocumentDate").IsMoreThanOrEquals(DateTime.Now));
                                                            f.AddCriteria(c => c.Property("DocumentDate").IsLessThanOrEquals(DateTime.Now));
                                                        },
                                                        0, 1000)
                                    .ToArray();
            Assert.IsNotNull(values);
        }

        [Test]
        public void ShouldGetValuesBetweenDates()
        {
            IEnumerable<dynamic> parentDebtSum = _api.GetValuesBetweenDates("PTA",
                                                                                   "FundsMovement",
                                                                                   DateTime.Now.AddDays(-20),
                                                                                   DateTime.Now,
                                                                                   new List<string> { "Category" },
                                                                                   new[] { "Price" },
                                                                                   new[] { AggregationType.Sum },
                                                                                   new Action<FilterBuilder>(
                                                                                       f => f.AddCriteria(c => c.Property("Category").IsEquals("ParentDebt"))));
            Assert.IsNotNull(parentDebtSum);
        }
    }
}
