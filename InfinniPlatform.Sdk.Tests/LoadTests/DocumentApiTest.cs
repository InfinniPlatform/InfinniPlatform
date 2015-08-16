using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfinniPlatform.Sdk.Api;
using InfinniPlatform.Sdk.Dynamic;
using NUnit.Framework;

namespace InfinniPlatform.Sdk.Tests.LoadTests
{
    //[Ignore("Необходимо наличие запущенного сервера InfinniPlatform для данного теста")]
    [TestFixture]
    public class DocumentApiTest
    {
        private const string InfinniSessionPort = "9900";
        private const string InfinniSessionServer = "localhost";
        private const string Route = "1";

        [Test]
        public void ShouldGetDocument()
        {
            var api = new InfinniDocumentApi(InfinniSessionServer, InfinniSessionPort, Route);

            Action action = () => api.GetDocument("gameshop", "catalogue",
                 f => f.AddCriteria(cr => cr.Property("Name").IsContains("gta")), 0, 100,
                 s => s.AddSorting("Price", "descending"));

            var actions = Enumerable.Repeat(action, 15000).ToArray();
            var watch = Stopwatch.StartNew();
            Parallel.Invoke(actions);
            watch.Stop();

            Console.WriteLine(watch.ElapsedMilliseconds);
        }

        [Test]
        public void ShouldSetDocument()
        {
            var api = new InfinniDocumentApi(InfinniSessionServer, InfinniSessionPort, Route);

            var random = new Random(1);
            Action action = () =>
            {
                var document = new
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "TestGameName" + Guid.NewGuid().ToString(),
                    Price = random.Next(100, 2400),
                    Availability = new
                    {
                        Available = false,
                        SaleStartDate = DateTime.Now
                    }
                };
                
                api.SetDocument("gameshop", "catalogue", document.Id, document);
            };

            var actions = Enumerable.Repeat(action, 15000).ToArray();
            var watch = Stopwatch.StartNew();
            Parallel.Invoke(actions);
            watch.Stop();

            Console.WriteLine(watch.ElapsedMilliseconds);
        }

    }
}
