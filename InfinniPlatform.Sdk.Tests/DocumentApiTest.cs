using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace InfinniPlatform.Sdk.Tests
{
    [TestFixture]
    public class DocumentApiTest
    {
        [TestFixtureSetUp]
        public void TestFixtureSetup()
        {
            InfinniSession.Port = "9900";
            InfinniSession.Server = "localhost";
            InfinniSession.Version = "1";
        }

        [Test]
        public void ShouldGetDocument()
        {
            var result = InfinniDocumentApi.GetDocument("gameshop", "catalogue",
                f => f.AddCriteria(cr => cr.Property("Name").IsContains("gta")), 0, 1,
                s => s.AddSorting("Price", "descending"));
            Assert.False(result.Any());
        }

        [Test]
        public void ShouldSetDocument()
        {
            var documentObject = new
            {
                Name = "gta vice city",
                Price = 100.50
            };

            var result = InfinniDocumentApi.SetDocument("gameshop", "catalogue", documentObject);
            Assert.True(!string.IsNullOrEmpty(result));
        }
    }
}
