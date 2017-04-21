using System;
using System.Collections.Generic;
using System.Linq;

using InfinniPlatform.DocumentStorage.Metadata;
using InfinniPlatform.DocumentStorage.MongoDB;
using InfinniPlatform.DocumentStorage.QueryFactories;
using InfinniPlatform.DocumentStorage.QuerySyntax;
using InfinniPlatform.Dynamic;
using InfinniPlatform.Tests;

using NUnit.Framework;

namespace InfinniPlatform.DocumentStorage.HttpService.QueryFactories
{
    [TestFixture]
    [Category(TestCategories.IntegrationTest)]
    public class FuncSelectQuerySyntaxVisitorTest
    {
        private static readonly IQuerySyntaxTreeParser SyntaxTreeParser = new QuerySyntaxTreeParser();


        [Test]
        public void ShouldInclude()
        {
            // Given

            const string selectExpression1 = "include(prop1)";
            const string selectExpression2 = "include(prop1, prop2)";

            Action<IDocumentProjectionBuilder> expectedSelectExpression1 = p => p.Include("prop1");
            Action<IDocumentProjectionBuilder> expectedSelectExpression2 = p => p.Include("prop1").Include("prop2");

            var item = new DynamicWrapper
                       {
                           { "prop1", "Item1" },
                           { "prop2", 12345 },
                           { "date", DateTime.Today }
                       };

            // When
            var selectActualExpression1 = ParseSelect(selectExpression1);
            var selectActualExpression2 = ParseSelect(selectExpression2);

            // Then
            AssertSelect(nameof(ShouldInclude), item, expectedSelectExpression1, selectActualExpression1);
            AssertSelect(nameof(ShouldInclude), item, expectedSelectExpression2, selectActualExpression2);
        }

        [Test]
        public void ShouldExclude()
        {
            // Given

            const string selectExpression1 = "exclude(prop1)";
            const string selectExpression2 = "exclude(prop1, prop2)";

            Action<IDocumentProjectionBuilder> expectedSelectExpression1 = p => p.Exclude("prop1");
            Action<IDocumentProjectionBuilder> expectedSelectExpression2 = p => p.Exclude("prop1").Exclude("prop2");

            var item = new DynamicWrapper
                       {
                           { "prop1", "Item1" },
                           { "prop2", 12345 },
                           { "date", DateTime.Today }
                       };

            // When
            var selectActualExpression1 = ParseSelect(selectExpression1);
            var selectActualExpression2 = ParseSelect(selectExpression2);

            // Then
            AssertSelect(nameof(ShouldInclude), item, expectedSelectExpression1, selectActualExpression1);
            AssertSelect(nameof(ShouldInclude), item, expectedSelectExpression2, selectActualExpression2);
        }

        [Test]
        public void ShouldIncludeTextScore()
        {
            // Given

            const string selectExpression1 = "textScore()";
            const string selectExpression2 = "textScore(myTextScore)";

            Action<IDocumentProjectionBuilder> expectedSelectExpression1 = p => p.IncludeTextScore();
            Action<IDocumentProjectionBuilder> expectedSelectExpression2 = p => p.IncludeTextScore("myTextScore");

            var item = new DynamicWrapper
                       {
                           { "prop1", "Item1" },
                           { "prop2", 12345 },
                           { "date", DateTime.Today }
                       };

            // When
            var selectActualExpression1 = ParseSelect(selectExpression1);
            var selectActualExpression2 = ParseSelect(selectExpression2);

            // Then

            var textIndex = new DocumentIndex { Key = new Dictionary<string, DocumentIndexKeyType> { { "prop1", DocumentIndexKeyType.Text } } };
            var documentStorage = DocumentStorageTestHelpers.GetEmptyStorage(nameof(ShouldIncludeTextScore), textIndex);
            documentStorage.InsertOne(item);

            var expectedValue1 = documentStorage.FindText("Item1").Project(expectedSelectExpression1).FirstOrDefault();
            var actualValue1 = documentStorage.FindText("Item1").Project(selectActualExpression1).FirstOrDefault();
            AreEqualDynamicWrapper(expectedValue1, actualValue1);

            var expectedValue2 = documentStorage.FindText("Item1").Project(expectedSelectExpression2).FirstOrDefault();
            var actualValue2 = documentStorage.FindText("Item1").Project(selectActualExpression2).FirstOrDefault();
            AreEqualDynamicWrapper(expectedValue2, actualValue2);
        }


        private static void AssertSelect(string documentType, DynamicWrapper item, Action<IDocumentProjectionBuilder> expectedSelect, Action<IDocumentProjectionBuilder> actualSelect)
        {
            var documentStorage = DocumentStorageTestHelpers.GetEmptyStorage(documentType);
            documentStorage.InsertOne(item);

            var expectedValue = documentStorage.Find().Project(expectedSelect).FirstOrDefault();
            var actualValue = documentStorage.Find().Project(actualSelect).FirstOrDefault();

            AreEqualDynamicWrapper(expectedValue, actualValue);
        }

        private static void AreEqualDynamicWrapper(DynamicWrapper expected, DynamicWrapper actual)
        {
            Assert.IsNotNull(expected);
            Assert.IsNotNull(actual);

            var expectedProperties = expected.Cast<KeyValuePair<string, object>>().Where(i => i.Key != "_header").Select(i => new Tuple<string, object>(i.Key, i.Value));
            var actualProperties = actual.Cast<KeyValuePair<string, object>>().Where(i => i.Key != "_header").Select(i => new Tuple<string, object>(i.Key, i.Value));

            CollectionAssert.AreEqual(expectedProperties, actualProperties);
        }


        private static Action<IDocumentProjectionBuilder> ParseSelect(string selectExpression)
        {
            var selectMethods = SyntaxTreeParser.Parse(selectExpression);

            Assert.IsNotNull(selectMethods);
            Assert.AreEqual(1, selectMethods.Count);
            Assert.IsInstanceOf<InvocationQuerySyntaxNode>(selectMethods[0]);

            return FuncSelectQuerySyntaxVisitor.CreateSelectExpression((InvocationQuerySyntaxNode)selectMethods[0]);
        }
    }
}