﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using InfinniPlatform.Api.Index;
using InfinniPlatform.Api.Index.SearchOptions;
using InfinniPlatform.Api.RestApi.Auth;
using InfinniPlatform.Api.SearchOptions;
using InfinniPlatform.Index.ElasticSearch.Factories;
using InfinniPlatform.Index.ElasticSearch.Implementation.ElasticProviders;
using InfinniPlatform.Index.ElasticSearch.Tests.Builders;
using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.Sdk.Environment.Index;
using InfinniPlatform.SystemConfig.RoutingFactory;
using NUnit.Framework;

namespace InfinniPlatform.Index.ElasticSearch.Tests.ElasticWrappers
{
    [TestFixture]
    [NUnit.Framework.Category(TestCategories.IntegrationTest)]
    public class ElasticSearchAggregationExecutorTests
    {
        [SetUp]
        public void InitializeElasticSearch()
        {
            _indexStateProvider = new ElasticFactory(new RoutingFactoryBase()).BuildIndexStateProvider();
            _indexStateProvider.CreateIndexType(IndexName, IndexName);
            _elasticSearchProvider = new ElasticFactory(new RoutingFactoryBase()).BuildCrudOperationProvider(IndexName,
                                                                                                             IndexName,
                                                                                                             AuthorizationStorageExtensions
                                                                                                                 .AnonimousUser,
                                                                                                             null);

            foreach (School school in SchoolsFactory.CreateSchoolsForFacetsTesting())
            {
                // Преобразование школы в объект типа dynamic
                IDictionary<string, object> expando = new ExpandoObject();
                foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(school.GetType()))
                    expando.Add(property.Name, property.GetValue(school));

                dynamic dynSchool = expando as ExpandoObject;
                dynSchool.Id = Guid.NewGuid().ToString().ToLowerInvariant();

                _elasticSearchProvider.Set(dynSchool);
            }

            _elasticSearchProvider.Refresh();
        }

        [TearDown]
        public void DeleteIndex()
        {
            _indexStateProvider.DeleteIndexType(IndexName, IndexName);
        }

        private const string IndexName = "aggrunittestindex";

        private ICrudOperationProvider _elasticSearchProvider;
        private IIndexStateProvider _indexStateProvider;

        [Test]
        [Ignore("Ручной тест для оценки быстродействия агрегаций")]
        /// Тест выдаёт следующие результаты измерений
        /// Aggregation time 1 term (10^6 documents) 8 ms.
        /// Aggregation time 2 terms (10^6 documents) 9 ms.
        /// Aggregation time 3 terms (10^6 documents) 83 ms.
        /// Aggregation time by range (10^6 documents) 19 ms.
        /// Search by query time (10^6 documents) 39 ms.
        public void AggregationPerformanceVsSearchPerformance()
        {
            const string aggrindex = "aggrperfomancetest";

            IIndexStateProvider indexStateProvider =
                new ElasticFactory(new RoutingFactoryBase()).BuildIndexStateProvider();

            if (indexStateProvider.GetIndexStatus(aggrindex, aggrindex) == IndexStatus.NotExists)
            {
                indexStateProvider.CreateIndexType(aggrindex, aggrindex, false);
                ICrudOperationProvider elasticSearchProvider =
                    new ElasticFactory(new RoutingFactoryBase()).BuildCrudOperationProvider(aggrindex, aggrindex,
                                                                                            AuthorizationStorageExtensions
                                                                                                .AnonimousUser, null);

                foreach (School school in SchoolsFactory.CreateRandomSchools(300000))
                {
                    dynamic dynSchool = school.ToDynamic();
                    dynSchool.Id = Guid.NewGuid().ToString().ToLowerInvariant();
                    elasticSearchProvider.Set(dynSchool);
                }

                elasticSearchProvider.Refresh();
            }

            var executor = new ElasticSearchAggregationProvider(aggrindex, aggrindex,
                                                                AuthorizationStorageExtensions.AnonimousUser);
            var queryWrapper = new IndexQueryExecutor(aggrindex, aggrindex, AuthorizationStorageExtensions.AnonimousUser);

            IEnumerable<AggregationResult> r = executor.ExecuteTermAggregation(
                new string[0],
                AggregationType.Avg,
                "Rating");

            Stopwatch aggrTime = Stopwatch.StartNew();
            long integralResult = 0;

            for (int i = 0; i < 10; i++)
            {
                aggrTime = Stopwatch.StartNew();

                executor.ExecuteTermAggregation(
                    new[] {"Principal.Grade"},
                    AggregationType.Avg,
                    "Rating");
                aggrTime.Stop();

                integralResult += aggrTime.ElapsedMilliseconds;
            }

            Console.WriteLine("Aggregation time 1 term (10^6 documents) {0} ms.", integralResult/10);

            integralResult = 0;

            for (int i = 0; i < 10; i++)
            {
                aggrTime = Stopwatch.StartNew();

                executor.ExecuteTermAggregation(
                    new[] {"Principal.Grade", "HouseNumber"},
                    AggregationType.Avg,
                    "Rating");
                aggrTime.Stop();

                integralResult += aggrTime.ElapsedMilliseconds;
            }

            Console.WriteLine("Aggregation time 2 terms (10^6 documents) {0} ms.", integralResult/10);

            integralResult = 0;

            for (int i = 0; i < 10; i++)
            {
                aggrTime = Stopwatch.StartNew();

                executor.ExecuteTermAggregation(
                    new[] {"Principal.Grade", "HouseNumber", "Name"},
                    AggregationType.Avg,
                    "Rating");
                aggrTime.Stop();

                integralResult += aggrTime.ElapsedMilliseconds;
            }

            Console.WriteLine("Aggregation time 3 terms (10^6 documents) {0} ms.", integralResult/10);

            integralResult = 0;

            for (int i = 0; i < 10; i++)
            {
                aggrTime = Stopwatch.StartNew();

                executor.ExecuteAggregation(
                    new[]
                        {
                            new
                                {
                                    Label = "rating_1",
                                    FieldName = "FoundationDate",
                                    DimensionType = DimensionType.DateRange,
                                    ValueSet = new
                                        {
                                            Property = "Rating",
                                            CriteriaType = CriteriaType.ValueSet,
                                            Value =
                        string.Join("\n", new DateTime(1980, 1, 1), new DateTime(1990, 1, 1))
                                        }
                                }.ToDynamic(),
                            new
                                {
                                    Label = "type_term",
                                    FieldName = "Principal.Grade",
                                    DimensionType = DimensionType.Term
                                }.ToDynamic()
                        },
                    new[] {AggregationType.Avg},
                    new[] {"Rating"});
                aggrTime.Stop();

                integralResult += aggrTime.ElapsedMilliseconds;
            }

            Console.WriteLine("Aggregation time by range (10^6 documents) {0} ms.", integralResult/10);

            Stopwatch searchTime = Stopwatch.StartNew();

            queryWrapper.Query(new SearchModel()).Items.FirstOrDefault();

            searchTime.Stop();

            Console.WriteLine("Search by query time (10^6 documents) {0} ms.", searchTime.ElapsedMilliseconds);
        }

        [Test]
        public void AvgTest()
        {
            IAggregationProvider target =
                new ElasticFactory(new RoutingFactoryBase()).BuildAggregationProvider(IndexName, IndexName,
                                                                                      AuthorizationStorageExtensions
                                                                                          .AnonimousUser);

            IEnumerable<AggregationResult> avg = target.ExecuteTermAggregation(new[] {"Street"}, AggregationType.Avg,
                                                                               "Rating");

            Assert.AreEqual(5, avg.Count());
            Assert.AreEqual(4.15, avg.First(b => b.Name == "leninaavenue").Values[0]);
        }

        [Test]
        public void CompleteAggregationBehavior()
        {
            var executor = new ElasticSearchAggregationProvider(IndexName, IndexName,
                                                                AuthorizationStorageExtensions.AnonimousUser);

            IEnumerable<AggregationResult> result = executor.ExecuteTermAggregation(
                new[] {"Name", "Principal.LastName"},
                AggregationType.Avg,
                "Rating");

            // Количество школ с имененм badschool
            long? count = result.First(s => s.Name == "badschool").DocCount;

            Assert.IsTrue(count > 0);

            // Получение списка имен школ
            List<string> names = result.Select(school => school.Name).ToList();

            var ratings = new List<string>();

            // Проход по рейтингам всех школ
            foreach (AggregationResult school in result)
            {
                foreach (AggregationResult principal in school.Buckets)
                {
                    ratings.Add(
                        string.Format("Школа {0} - Фамилия директора {1} - Рейтинг {2}",
                                      school.Name, principal.Name, principal.Values[0]));
                }
            }

            dynamic points = new DynamicWrapper();
            points.Property = "Rating";
            points.CriteriaType = CriteriaType.ValueSet;
            points.Value = "2.5\n4.5";

            // Пример агрегации по диапазонам
            var rangeDim = new
                {
                    Label = "grouping_by_rating",
                    FieldName = "Rating",
                    DimensionType = DimensionType.Range,
                    ValueSet = points
                }.ToDynamic();

            result = executor.ExecuteAggregation(new[] {rangeDim}, new[] {AggregationType.Min, AggregationType.Max},
                                                 new[] {"Principal.Grade", "Principal.KnowledgeRating"});

            // Пример агрегации по временным диапазонам
            dynamic datapoint1 = new DynamicWrapper();
            datapoint1.Property = "FoundationDate";
            datapoint1.CriteriaType = CriteriaType.ValueSet;
            datapoint1.Value = string.Join("\n", new DateTime(1980, 1, 1), new DateTime(1990, 1, 1));

            var dateRangeDim = new
                {
                    Label = "grouping_by_foundationDate",
                    FieldName = "FoundationDate",
                    DimensionType = DimensionType.DateRange,
                    DateTimeFormattingPattern = "yyyy-MM-dd",
                    ValueSet = datapoint1
                }.ToDynamic();

            result = executor.ExecuteAggregation(new object[] {rangeDim, dateRangeDim}, new[] {AggregationType.Count},
                                                 new string[0]);

            var dateHistogramDim = new
                {
                    Label = "grouping_by_foundationDate",
                    FieldName = "FoundationDate",
                    DimensionType = DimensionType.DateHistogram,
                    Interval = "month"
                }.ToDynamic();

            result = executor.ExecuteAggregation(new object[] {dateHistogramDim}, new[] {AggregationType.Count},
                                                 new string[0]);
        }

        [Test]
        public void CountTest()
        {
            IAggregationProvider target =
                new ElasticFactory(new RoutingFactoryBase()).BuildAggregationProvider(IndexName, IndexName,
                                                                                      AuthorizationStorageExtensions
                                                                                          .AnonimousUser);

            IEnumerable<AggregationResult> count = target.ExecuteTermAggregation(new[] {"Street"}, AggregationType.Count,
                                                                                 "");

            Assert.AreEqual(5, count.Count());
            Assert.AreEqual(2, count.First(b => b.Name == "leninaavenue").Values[0]);
        }

        [Test]
        public void MaxTest()
        {
            IAggregationProvider target =
                new ElasticFactory(new RoutingFactoryBase()).BuildAggregationProvider(IndexName, IndexName,
                                                                                      AuthorizationStorageExtensions
                                                                                          .AnonimousUser);

            IEnumerable<AggregationResult> max = target.ExecuteTermAggregation(new[] {"Street"}, AggregationType.Max,
                                                                               "Rating");

            Assert.AreEqual(5, max.Count());
            Assert.AreEqual(4.2, max.First(b => b.Name == "leninaavenue").Values[0]);
        }

        [Test]
        public void MinTest()
        {
            IAggregationProvider target =
                new ElasticFactory(new RoutingFactoryBase()).BuildAggregationProvider(IndexName, IndexName,
                                                                                      AuthorizationStorageExtensions
                                                                                          .AnonimousUser);

            IEnumerable<AggregationResult> min = target.ExecuteTermAggregation(new[] {"Street"}, AggregationType.Min,
                                                                               "Rating");

            Assert.AreEqual(5, min.Count());
            Assert.AreEqual(4.1, min.First(b => b.Name == "leninaavenue").Values[0]);
        }

        [Test]
        public void MutiDimTest()
        {
            IAggregationProvider target =
                new ElasticFactory(new RoutingFactoryBase()).BuildAggregationProvider(IndexName, IndexName,
                                                                                      AuthorizationStorageExtensions
                                                                                          .AnonimousUser);


            IEnumerable<AggregationResult> result = target.ExecuteTermAggregation(
                new[] {"Street", "Name"},
                AggregationType.Sum, "Rating");

            Assert.AreEqual(5, result.Count());
            Assert.AreEqual(4.2,
                            result.First(b => b.Name == "leninaavenue")
                                  .Buckets.First(b => b.Name == "simpleschool")
                                  .Values[0]);

            //Assert.AreEqual(4.1, min["leninaavenue"].Value);

            //var target = new ElasticFactory(new RoutingFactoryBase()).BuildAggregationProvider(IndexName, IndexName);

            //var cube = target.ExecuteAggregation(null, new[] { "Street", "HouseNumber", "Name" }, new[] { new AggregationDescriptor("Rating", AggregationType.Sum) });
            //var slice = cube.GetSlice(new Dictionary<string, string> { { "Name", "niceschool" } });
            //var value = slice.GetValue(new Dictionary<string, string> { { "Street", "kirova" }, { "HouseNumber", "31" } });

            //Assert.AreEqual(new[] { "Street", "HouseNumber", "Name" }, cube.Dimensions);
            //Assert.AreEqual(6, cube.Count);
            //Assert.AreEqual(18.7, cube.Sum.First(x => x.Aggregation.AggregationType == AggregationType.Sum).Value);

            //Assert.AreEqual(new[] { "Street", "HouseNumber" }, slice.Dimensions);
            //Assert.AreEqual(2, slice.Count);
            //Assert.AreEqual(6.2, slice.Sum.First(x => x.Aggregation.AggregationType == AggregationType.Sum).Value);
            //Assert.AreEqual(3.1, slice.Average.First(x => x.Aggregation.AggregationType == AggregationType.Sum).Value);

            //Assert.AreEqual(3.1, value.First(x => x.Aggregation.AggregationType == AggregationType.Sum).Value);
        }

        [Test]
        public void SumTest()
        {
            IAggregationProvider target =
                new ElasticFactory(new RoutingFactoryBase()).BuildAggregationProvider(IndexName, IndexName,
                                                                                      AuthorizationStorageExtensions
                                                                                          .AnonimousUser);

            IEnumerable<AggregationResult> sum = target.ExecuteTermAggregation(new[] {"Street"}, AggregationType.Sum,
                                                                               "Rating");

            Assert.AreEqual(5, sum.Count());
            Assert.AreEqual(8.3, sum.First(b => b.Name == "leninaavenue").Values[0]);
        }

        //[Test]
        //public void MultiFieldAggregation()
        //{
        //    var target = new ElasticFactory(new RoutingFactoryBase()).BuildAggregationProvider(IndexName, IndexName);
        //    var cube = target.ExecuteAggregation
        //        (
        //            null,
        //            new[] {"Street"},
        //            new[]
        //                {
        //                    new AggregationDescriptor("Rating", AggregationType.Avg),
        //                    new AggregationDescriptor("Rating", AggregationType.Max),
        //                    new AggregationDescriptor(null, AggregationType.Count),
        //                    new AggregationDescriptor("Rating", AggregationType.Min),
        //                    new AggregationDescriptor("HouseNumber", AggregationType.Max),
        //                    new AggregationDescriptor("HouseNumber", AggregationType.Min)
        //                }
        //        );
        //    var value = cube.GetValue(new Dictionary<string, string> { { "Street", "leninaavenue" } });

        //    Assert.AreEqual(2, value.First(x => x.Aggregation.AggregationType == AggregationType.Count).Value);
        //    Assert.AreEqual(4.15, value.First(x => x.Aggregation.AggregationType == AggregationType.Avg && x.Aggregation.FieldName == "Rating").Value);
        //    Assert.AreEqual(4.2, value.First(x => x.Aggregation.AggregationType == AggregationType.Max && x.Aggregation.FieldName == "Rating").Value);
        //    Assert.AreEqual(4.1, value.First(x => x.Aggregation.AggregationType == AggregationType.Min && x.Aggregation.FieldName == "Rating").Value);
        //    Assert.AreEqual(41, value.First(x => x.Aggregation.AggregationType == AggregationType.Max && x.Aggregation.FieldName == "HouseNumber").Value);
        //    Assert.AreEqual(41, value.First(x => x.Aggregation.AggregationType == AggregationType.Min && x.Aggregation.FieldName == "HouseNumber").Value);
        //}

        //[Test]
        //public void ScriptAggregation()
        //{
        //    var target = new ElasticFactory(new RoutingFactoryBase()).BuildAggregationProvider(IndexName, IndexName);
        //    var cube = target.ExecuteAggregation
        //        (
        //            null,
        //            new Dictionary<string, ICalculatedField>
        //                {
        //                    {
        //                        "FullName",
        //                        _calculatedFieldFactory.Field("Street")
        //                                               .Add(_calculatedFieldFactory.RawString("\" \""))
        //                                               .Add(_calculatedFieldFactory.Field("HouseNumber"))
        //                                               .Add(_calculatedFieldFactory.RawString("\" \""))
        //                                               .Add(_calculatedFieldFactory.Field("Name"))
        //                    }
        //                },
        //            new[]
        //                {
        //                    new AggregationDescriptor("Rating", AggregationType.Avg),
        //                    new AggregationDescriptor("Rating", AggregationType.Max),
        //                    new AggregationDescriptor("Rating", AggregationType.Min),
        //                    new AggregationDescriptor("Rating", AggregationType.Sum),
        //                    new AggregationDescriptor(null, AggregationType.Count),
        //                    new AggregationDescriptor("HouseNumber", AggregationType.Max),
        //                    new AggregationDescriptor("HouseNumber", AggregationType.Min)
        //                }
        //        );

        //    Assert.AreEqual(new[] { "FullName" }, cube.Dimensions);
        //    Assert.AreEqual(6, cube.Count);
        //    Assert.AreEqual(18.7, cube.Sum.First(x => x.Aggregation.AggregationType == AggregationType.Sum).Value);
        //}
    }
}