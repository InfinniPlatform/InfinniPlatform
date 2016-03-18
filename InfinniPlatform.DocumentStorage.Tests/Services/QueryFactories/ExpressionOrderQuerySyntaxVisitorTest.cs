using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using InfinniPlatform.DocumentStorage.Services.QueryFactories;
using InfinniPlatform.DocumentStorage.Services.QuerySyntax;
using InfinniPlatform.DocumentStorage.Tests.TestEntities;
using InfinniPlatform.Sdk.Documents.Services;

using NUnit.Framework;

namespace InfinniPlatform.DocumentStorage.Tests.Services.QueryFactories
{
    [TestFixture]
    [Category(TestCategories.UnitTest)]
    public class ExpressionOrderQuerySyntaxVisitorTest
    {
        private static readonly IQuerySyntaxTreeParser SyntaxTreeParser = new QuerySyntaxTreeParser();


        [Test]
        public void ShouldSelectSpecifiedProperties()
        {
            // Given

            const string ascExpression1 = "asc(prop1)";
            const string ascExpression2 = "asc(prop1, prop2)";
            const string ascExpression3 = "asc(item.category)";
            const string descExpression1 = "desc(prop1)";
            const string descExpression2 = "desc(prop1, prop2)";
            const string descExpression3 = "desc(item.category)";

            var ascExpectedExpression1 = new Dictionary<string, DocumentSortOrder>
                                         {
                                             { "prop1", DocumentSortOrder.Asc }
                                         };

            var ascExpectedExpression2 = new Dictionary<string, DocumentSortOrder>
                                         {
                                             { "prop1", DocumentSortOrder.Asc },
                                             { "prop2", DocumentSortOrder.Asc }
                                         };

            var ascExpectedExpression3 = new Dictionary<string, DocumentSortOrder>
                                         {
                                             { "category", DocumentSortOrder.Asc }
                                         };

            var descExpectedExpression1 = new Dictionary<string, DocumentSortOrder>
                                          {
                                              { "prop1", DocumentSortOrder.Desc }
                                          };

            var descExpectedExpression2 = new Dictionary<string, DocumentSortOrder>
                                          {
                                              { "prop1", DocumentSortOrder.Desc },
                                              { "prop2", DocumentSortOrder.Desc }
                                          };

            var descExpectedExpression3 = new Dictionary<string, DocumentSortOrder>
                                          {
                                              { "category", DocumentSortOrder.Desc }
                                          };

            // When
            var ascActualExpression1 = ParseOrder<SimpleEntity>(ascExpression1);
            var ascActualExpression2 = ParseOrder<SimpleEntity>(ascExpression2);
            var ascActualExpression3 = ParseOrder<Order>(ascExpression3);
            var descActualExpression1 = ParseOrder<SimpleEntity>(descExpression1);
            var descActualExpression2 = ParseOrder<SimpleEntity>(descExpression2);
            var descActualExpression3 = ParseOrder<Order>(descExpression3);

            // Then
            AssertOrder(ascExpectedExpression1, ascActualExpression1);
            AssertOrder(ascExpectedExpression2, ascActualExpression2);
            AssertOrder(ascExpectedExpression3, ascActualExpression3);
            AssertOrder(descExpectedExpression1, descActualExpression1);
            AssertOrder(descExpectedExpression2, descActualExpression2);
            AssertOrder(descExpectedExpression3, descActualExpression3);
        }


        private static void AssertOrder<T>(IDictionary<string, DocumentSortOrder> expectedOrder, IDictionary<Expression<Func<T, object>>, DocumentSortOrder> actualOrder)
        {
            var expectedProperties = expectedOrder.Select(i => new Tuple<string, DocumentSortOrder>(i.Key, i.Value));
            var actualProperties = actualOrder.Select(i => new Tuple<string, DocumentSortOrder>(((MemberExpression)((UnaryExpression)i.Key.Body).Operand).Member.Name, i.Value));

            CollectionAssert.AreEqual(expectedProperties, actualProperties);
        }


        private static IDictionary<Expression<Func<T, object>>, DocumentSortOrder> ParseOrder<T>(string orderExpression)
        {
            var orderMethods = SyntaxTreeParser.Parse(orderExpression);

            Assert.IsNotNull(orderMethods);
            Assert.AreEqual(1, orderMethods.Count);
            Assert.IsInstanceOf<InvocationQuerySyntaxNode>(orderMethods[0]);

            return ExpressionOrderQuerySyntaxVisitor.CreateOrderExpression(typeof(T), (InvocationQuerySyntaxNode)orderMethods[0])
                                                    .ToDictionary(i => (Expression<Func<T, object>>)i.Key, i => i.Value);
        }
    }
}