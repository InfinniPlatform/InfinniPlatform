using System;
using System.Collections.Generic;
using System.Linq;

using InfinniPlatform.Core.RestQuery.EventObjects;

using NUnit.Framework;
using Newtonsoft.Json;

namespace InfinniPlatform.Core.Tests.Events
{
    [TestFixture]
	[Category(TestCategories.UnitTest)]
    class EventBuildersTests
    {
        private object _testObject;

        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            _testObject =
                new
                    {
                        TestContainer =
                            new
                                {
                                    TestField = false,
                                    FieldToExclude = false
                                },
                        ConteinerToExclude =
                            new
                                {
                                    TestField = false
                                },
                        TestCollection =
                            new List<object>
                                {
                                    new {TestField = false},
                                    new {TestField = false},
                                    new {TestField = false},
                                    new {TestField = false}
                                },
                        StringAttributeCollection =
                            new List<string>
                                {
                                    "1",
                                    "2",
                                    "3",
                                    "4"
                                },
                        IntAttributeCollection =
                            new List<int>
                                {
                                    1,
                                    2,
                                    3,
                                    4
                                }
                    };
        }

        [Test]
        public void Exclusions()
        {
            const string leadString = "{\"Property\":\"";
            var errors = new List<string>();

            var events = _testObject
                .ToEventListAsObject("TestData")
                .Exclude("TestData.ConteinerToExclude")
                .GetSerializedEvents();
            if (events.Any(x => x.StartsWith(String.Format("{0}TestData.ConteinerToExclude", leadString))))
                errors.Add("Container");

            events = _testObject
                .ToEventListAsObject("TestData")
                .Exclude("TestData.TestContainer.FieldToExclude")
                .GetSerializedEvents();
            if (events.Any(x => x.StartsWith(String.Format("{0}TestData.TestContainer.FieldToExclude", leadString))))
                errors.Add("Field");


            events = _testObject
                .ToEventListAsObject("TestData")
                .Exclude("TestData.TestCollection")
                .GetSerializedEvents();
            if (events.Any(x => x.StartsWith(String.Format("{0}TestData.TestCollection", leadString))))
                errors.Add("Collection");

            events = _testObject
                .ToEventListAsObject("TestData")
                .Exclude("TestData.TestCollection.([0-9]+)")
                .GetSerializedEvents();
            if (events.Any(x => x.StartsWith(String.Format("{0}TestData.TestCollection.", leadString))))
                errors.Add("Collection items");

            if (errors.Any())
            {
                var message = "Exclusion failer for: " + String.Join("; ", errors);
                Assert.Fail(message);
            }
        }
    }
}
