﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using InfinniPlatform.DocumentStorage.Services.QueryFactories;
using InfinniPlatform.DocumentStorage.Services.QuerySyntax;
using InfinniPlatform.DocumentStorage.Tests.TestEntities;
using InfinniPlatform.Sdk.Dynamic;

using NUnit.Framework;

namespace InfinniPlatform.DocumentStorage.Tests.Services.QueryFactories
{
    [TestFixture]
    [Category(TestCategories.UnitTest)]
    public class ExpressionSelectQuerySyntaxVisitorTest
    {
        private static readonly IQuerySyntaxTreeParser SyntaxTreeParser = new QuerySyntaxTreeParser();


        [Test]
        public void ShouldSelectSpecifiedProperties()
        {
            // Given

            const string selectExpression1 = "include(prop1)";
            const string selectExpression2 = "include(prop1, prop2)";

            Expression<Func<SimpleEntity, object>> selectExpectedExpression1 = i => new DynamicWrapper { { "prop1", i.prop1 } };
            Expression<Func<SimpleEntity, object>> selectExpectedExpression2 = i => new DynamicWrapper { { "prop1", i.prop1 }, { "prop2", i.prop2 } };

            var item = new SimpleEntity
            {
                prop1 = "Item1",
                prop2 = 12345,
                date = DateTime.Today
            };

            // When
            var selectActualExpression1 = ParseSelect<SimpleEntity>(selectExpression1);
            var selectActualExpression2 = ParseSelect<SimpleEntity>(selectExpression2);

            // Then
            AssertSelect(item, selectExpectedExpression1, selectActualExpression1);
            AssertSelect(item, selectExpectedExpression2, selectActualExpression2);
        }


        private static void AssertSelect<T>(T item, Expression<Func<T, object>> expectedSelect, Expression<Func<T, object>> actualSelect)
        {
            var expectedValue = (DynamicWrapper)expectedSelect.Compile().Invoke(item);
            var actualValue = (DynamicWrapper)actualSelect.Compile().Invoke(item);

            Assert.IsNotNull(expectedValue);
            Assert.IsNotNull(actualValue);

            var expectedProperties = expectedValue.Cast<KeyValuePair<string, object>>().Select(i => new Tuple<string, object>(i.Key, i.Value));
            var actualProperties = actualValue.Cast<KeyValuePair<string, object>>().Select(i => new Tuple<string, object>(i.Key, i.Value));

            CollectionAssert.AreEqual(expectedProperties, actualProperties);
        }


        private static Expression<Func<T, object>> ParseSelect<T>(string selectExpression)
        {
            var selectMethods = SyntaxTreeParser.Parse(selectExpression);

            Assert.IsNotNull(selectMethods);
            Assert.AreEqual(1, selectMethods.Count);
            Assert.IsInstanceOf<InvocationQuerySyntaxNode>(selectMethods[0]);

            return (Expression<Func<T, object>>)ExpressionSelectQuerySyntaxVisitor.CreateSelectExpression(typeof(T), (InvocationQuerySyntaxNode)selectMethods[0]);
        }
    }
}