using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using InfinniPlatform.DocumentStorage.Services.QueryBuilders;
using InfinniPlatform.DocumentStorage.Services.QuerySyntax;
using InfinniPlatform.DocumentStorage.Tests.TestEntities;
using InfinniPlatform.Sdk.Documents.Services;

using NUnit.Framework;

namespace InfinniPlatform.DocumentStorage.Tests.Services.QueryBuilders
{
    [TestFixture]
    [Category(TestCategories.UnitTest)]
    public class DocumentQueryBuilderTest
    {
        private static readonly IQuerySyntaxTreeParser SyntaxTreeParser = new QuerySyntaxTreeParser();


        [Test]
        public void ShouldBuildQuery()
        {
            // Given

            var queryBuilder = new DocumentQueryBuilder<SimpleEntity>(SyntaxTreeParser);

            var query = new Dictionary<string, object>
                        {
                            { "search", "Full text search" },
                            { "filter", "eq(prop1,'abc'),gt(prop2,15)" },
                            { "select", "prop2" },
                            { "order", "desc(prop2),asc(date)" },
                            { "count", "true" },
                            { "skip", "1" },
                            { "take", "20" }
                        };

            // When
            var documentQuery = queryBuilder.BuildGetQuery(query);

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
    }
}