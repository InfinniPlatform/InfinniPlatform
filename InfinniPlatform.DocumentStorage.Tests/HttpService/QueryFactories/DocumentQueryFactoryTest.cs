using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using InfinniPlatform.DocumentStorage.HttpService;
using InfinniPlatform.DocumentStorage.HttpService.QueryFactories;
using InfinniPlatform.DocumentStorage.HttpService.QuerySyntax;
using InfinniPlatform.DocumentStorage.Tests.TestEntities;
using InfinniPlatform.Sdk.Http.Services;

using Moq;

using NUnit.Framework;

namespace InfinniPlatform.DocumentStorage.Tests.HttpService.QueryFactories
{
    [TestFixture]
    [Category(TestCategories.UnitTest)]
    public class DocumentQueryFactoryTest
    {
        private static readonly IQuerySyntaxTreeParser SyntaxTreeParser = new QuerySyntaxTreeParser();


        [Test]
        public void ShouldBuildTypedQuery()
        {
            // Given

            var queryBuilder = new DocumentQueryFactory<SimpleEntity>(SyntaxTreeParser, null);

            var request = CreateGetRequest(new Dictionary<string, object>
                                           {
                                               { "id", null },
                                               { "search", "Full text search" },
                                               { "filter", "eq(prop1,'abc'),gt(prop2,15)" },
                                               { "select", "prop2" },
                                               { "order", "desc(prop2),asc(date)" },
                                               { "count", "true" },
                                               { "skip", "1" },
                                               { "take", "20" }
                                           });

            // When
            var documentQuery = queryBuilder.CreateGetQuery(request);

            // Then
            Assert.IsNotNull(documentQuery);
            Assert.AreEqual("Full text search", documentQuery.Search);
            Assert.IsNotNull(documentQuery.Filter);
            Assert.AreEqual(ExpressionType.AndAlso, documentQuery.Filter.Body.NodeType);
            Assert.IsNotNull(documentQuery.Select);
            Assert.IsNotNull(documentQuery.Order);
            Assert.AreEqual(2, documentQuery.Order.Count);
            Assert.AreEqual(DocumentSortOrder.Desc, documentQuery.Order.Values.ElementAt(0));
            Assert.AreEqual(DocumentSortOrder.Asc, documentQuery.Order.Values.ElementAt(1));
            Assert.AreEqual(true, documentQuery.Count);
            Assert.AreEqual(1, documentQuery.Skip);
            Assert.AreEqual(20, documentQuery.Take);
        }

        [Test]
        public void ShouldBuildDynamicQuery()
        {
            // Given

            var queryBuilder = new DocumentQueryFactory(SyntaxTreeParser, null);

            var request = CreateGetRequest(new Dictionary<string, object>
                                           {
                                               { "id", null },
                                               { "search", "Full text search" },
                                               { "filter", "eq(prop1,'abc'),gt(prop2,15)" },
                                               { "select", "prop2" },
                                               { "order", "desc(prop2),asc(date)" },
                                               { "count", "true" },
                                               { "skip", "1" },
                                               { "take", "20" }
                                           });

            // When
            var documentQuery = queryBuilder.CreateGetQuery(request);

            // Then
            Assert.IsNotNull(documentQuery);
            Assert.AreEqual("Full text search", documentQuery.Search);
            Assert.IsNotNull(documentQuery.Filter);
            Assert.IsNotNull(documentQuery.Select);
            Assert.IsNotNull(documentQuery.Order);
            Assert.AreEqual(2, documentQuery.Order.Count);
            Assert.AreEqual(DocumentSortOrder.Desc, documentQuery.Order.Values.ElementAt(0));
            Assert.AreEqual(DocumentSortOrder.Asc, documentQuery.Order.Values.ElementAt(1));
            Assert.AreEqual(true, documentQuery.Count);
            Assert.AreEqual(1, documentQuery.Skip);
            Assert.AreEqual(20, documentQuery.Take);
        }


        private static IHttpRequest CreateGetRequest(IDictionary<string, object> query)
        {
            var request = new Mock<IHttpRequest>();
            request.SetupGet(i => i.Method).Returns("GET");
            request.SetupGet(i => i.Query).Returns(query);
            return request.Object;
        }
    }
}