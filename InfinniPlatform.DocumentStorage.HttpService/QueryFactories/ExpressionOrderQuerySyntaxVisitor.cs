﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using InfinniPlatform.DocumentStorage.QuerySyntax;

namespace InfinniPlatform.DocumentStorage.QueryFactories
{
    /// <summary>
    /// Выполняет синтаксический разбор для получения правила сортировки документов <see cref="DocumentGetQuery{TDocument}.Order"/>.
    /// </summary>
    public class ExpressionOrderQuerySyntaxVisitor : ExpressionBaseQuerySyntaxVisitor
    {
        private ExpressionOrderQuerySyntaxVisitor(Type type) : base(type)
        {
        }


        /// <summary>
        /// Creates order expression.
        /// </summary>
        /// <param name="type">Type.</param>
        /// <param name="node">Invocation query node.</param>
        public static IEnumerable<KeyValuePair<Expression, DocumentSortOrder>> CreateOrderExpression(Type type, InvocationQuerySyntaxNode node)
        {
            var visitor = new ExpressionOrderQuerySyntaxVisitor(type);
            var orderProperties = (ConstantExpression)visitor.Visit(node);
            return (IEnumerable<KeyValuePair<Expression, DocumentSortOrder>>)orderProperties.Value;
        }


        // Overrides

        /// <inheritdoc />
        public override Expression VisitInvocationExpression(InvocationQuerySyntaxNode node)
        {
            if (string.Equals(node.Name, QuerySyntaxHelper.AscMethodName, StringComparison.OrdinalIgnoreCase))
            {
                var ascProperties = node.Arguments.Select(Visit)
                                        .OfType<MemberExpression>()
                                        .Select(i => new KeyValuePair<Expression, DocumentSortOrder>(Expression.Lambda(QuerySyntaxHelper.Convert(i, typeof(object)), Parameter), DocumentSortOrder.Asc))
                                        .ToArray();

                return Expression.Constant(ascProperties);
            }

            if (string.Equals(node.Name, QuerySyntaxHelper.DescMethodName, StringComparison.OrdinalIgnoreCase))
            {
                var descProperties = node.Arguments.Select(Visit)
                                         .OfType<MemberExpression>()
                                         .Select(i => new KeyValuePair<Expression, DocumentSortOrder>(Expression.Lambda(QuerySyntaxHelper.Convert(i, typeof(object)), Parameter), DocumentSortOrder.Desc))
                                         .ToArray();

                return Expression.Constant(descProperties);
            }

            return base.VisitInvocationExpression(node);
        }
    }
}