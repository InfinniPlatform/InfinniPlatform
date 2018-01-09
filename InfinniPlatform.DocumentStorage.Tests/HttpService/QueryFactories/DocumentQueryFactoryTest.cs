using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using InfinniPlatform.DocumentStorage.QueryFactories;
using InfinniPlatform.DocumentStorage.QuerySyntax;
using InfinniPlatform.DocumentStorage.TestEntities;
using InfinniPlatform.Tests;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Primitives;

using Moq;

using NUnit.Framework;

namespace InfinniPlatform.DocumentStorage.HttpService.QueryFactories
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

            var request = CreateGetRequest(new Dictionary<string, string>
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
            var documentQuery = queryBuilder.CreateGetQuery(request, new RouteData());

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

            var request = CreateGetRequest(new Dictionary<string, string>
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
            var documentQuery = queryBuilder.CreateGetQuery(request,new RouteData());

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


        private static HttpRequest CreateGetRequest(IDictionary<string, string> query)
        {
            var queryCollection = new QueryCollection(query.ToDictionary(q => q.Key,
                                                                         q => new StringValues(q.Value)));

            var request = new Mock<HttpRequest>();
            request.SetupGet(i => i.Method).Returns("GET");
            request.SetupGet(i => i.Query).Returns(queryCollection);
            return request.Object;
        }
    }
}