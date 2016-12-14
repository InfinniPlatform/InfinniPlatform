using System;
using System.Collections.Generic;
using System.Linq;

using InfinniPlatform.DocumentStorage.Contract;
using InfinniPlatform.DocumentStorage.Contract.Services;
using InfinniPlatform.DocumentStorage.Services.QueryFactories;
using InfinniPlatform.DocumentStorage.Services.QuerySyntax;

using NUnit.Framework;

namespace InfinniPlatform.DocumentStorage.Tests.Services.QueryFactories
{
    [TestFixture]
    [Category(TestCategories.IntegrationTest)]
    public class FuncOrderQuerySyntaxVisitorTest
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
            const string textScoreExpression1 = "textScore()";
            const string textScoreExpression2 = "textScore(myTextScore)";

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
                                             { "item.category", DocumentSortOrder.Asc }
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
                                              { "item.category", DocumentSortOrder.Desc }
                                          };

            var textScoreExpectedExpression1 = new Dictionary<string, DocumentSortOrder>
                                               {
                                                   { DocumentStorageExtensions.DefaultTextScoreProperty, DocumentSortOrder.TextScore }
                                               };

            var textScoreExpectedExpression2 = new Dictionary<string, DocumentSortOrder>
                                               {
                                                   { "myTextScore", DocumentSortOrder.TextScore }
                                               };

            // When
            var ascActualExpression1 = ParseOrder(ascExpression1);
            var ascActualExpression2 = ParseOrder(ascExpression2);
            var ascActualExpression3 = ParseOrder(ascExpression3);
            var descActualExpression1 = ParseOrder(descExpression1);
            var descActualExpression2 = ParseOrder(descExpression2);
            var descActualExpression3 = ParseOrder(descExpression3);
            var textScoreActualExpression1 = ParseOrder(textScoreExpression1);
            var textScoreActualExpression2 = ParseOrder(textScoreExpression2);

            // Then
            AssertOrder(ascExpectedExpression1, ascActualExpression1);
            AssertOrder(ascExpectedExpression2, ascActualExpression2);
            AssertOrder(ascExpectedExpression3, ascActualExpression3);
            AssertOrder(descExpectedExpression1, descActualExpression1);
            AssertOrder(descExpectedExpression2, descActualExpression2);
            AssertOrder(descExpectedExpression3, descActualExpression3);
            AssertOrder(textScoreExpectedExpression1, textScoreActualExpression1);
            AssertOrder(textScoreExpectedExpression2, textScoreActualExpression2);
        }


        private static void AssertOrder(IDictionary<string, DocumentSortOrder> expectedOrder, IDictionary<string, DocumentSortOrder> actualOrder)
        {
            var expectedProperties = expectedOrder.Select(i => new Tuple<string, DocumentSortOrder>(i.Key, i.Value));
            var actualProperties = actualOrder.Select(i => new Tuple<string, DocumentSortOrder>(i.Key, i.Value));

            CollectionAssert.AreEqual(expectedProperties, actualProperties);
        }


        private static IDictionary<string, DocumentSortOrder> ParseOrder(string orderExpression)
        {
            var orderMethods = SyntaxTreeParser.Parse(orderExpression);

            Assert.IsNotNull(orderMethods);
            Assert.AreEqual(1, orderMethods.Count);
            Assert.IsInstanceOf<InvocationQuerySyntaxNode>(orderMethods[0]);

            return FuncOrderQuerySyntaxVisitor.CreateOrderExpression((InvocationQuerySyntaxNode)orderMethods[0]).ToDictionary(i => i.Key, i => i.Value);
        }
    }
}