using System;
using System.Diagnostics;
using System.IO;
using InfinniPlatform.Index.QueryLanguage.Implementation;
using NUnit.Framework;
using Newtonsoft.Json.Linq;

namespace InfinniPlatform.Index.QueryLanguage.Tests
{
    [TestFixture]
    [Category(TestCategories.UnitTest)]
    public sealed class JsonDenormalizerBehavior
    {
        [Test]
        public void DenormalizationPerformance()
        {
            JArray testData = JArray.Parse(File.ReadAllText("IqlResult.json"));

            Stopwatch stopWatch = Stopwatch.StartNew();

            for (int i = 0; i < 10; i++)
            {
                new JsonDenormalizer().ProcessIqlResult(testData);
            }

            stopWatch.Stop();

            Console.WriteLine("Denormalization time: " + stopWatch.ElapsedMilliseconds/10);

            Assert.Less(stopWatch.ElapsedMilliseconds/10, 5, "Время денормализации должно быть не больше 5 мс");
        }

        [Test]
        public void ShouldPerformDenormalization()
        {
            JArray testData = JArray.Parse(File.ReadAllText("IqlResult.json"));

            JArray result = new JsonDenormalizer().ProcessIqlResult(testData);

            Assert.IsNotNull(result);

            Assert.AreEqual(8, ((JArray) result[0]).Count);

            Assert.AreEqual("123", result[0][0]["PoliciesNumber"].Value<string>());
            Assert.AreEqual("89226567890", result[0][0]["Phones"].Value<string>());

            Assert.AreEqual("012", result[0][7]["PoliciesNumber"].Value<string>());
            Assert.AreEqual("89121234567", result[0][7]["Phones"].Value<string>());
        }

        [Test]
        public void ShouldPerformDenormalizationOfComplexDocuments()
        {
            JArray testData = JArray.Parse(File.ReadAllText("IqlBigResult.json"));

            JArray result = new JsonDenormalizer().ProcessIqlResult(testData);

            Assert.IsNotNull(result);

            Assert.AreEqual(256, ((JArray) result[0]).Count);
            Assert.AreEqual(8, ((JArray) result[1]).Count);

            Assert.AreEqual("123", result[1][0]["PoliciesNumber"].Value<string>());
            Assert.AreEqual("89226567890", result[1][0]["Phones"].Value<string>());

            Assert.AreEqual("012", result[1][7]["PoliciesNumber"].Value<string>());
            Assert.AreEqual("89121234567", result[1][7]["Phones"].Value<string>());
        }
    }
}