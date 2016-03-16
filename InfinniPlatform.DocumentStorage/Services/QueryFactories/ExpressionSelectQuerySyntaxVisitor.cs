using System;
using System.Linq;
using System.Linq.Expressions;

using InfinniPlatform.DocumentStorage.Services.QuerySyntax;
using InfinniPlatform.Sdk.Documents.Services;

namespace InfinniPlatform.DocumentStorage.Services.QueryFactories
{
    /// <summary>
    /// Выполняет синтаксический разбор для получения правила отображения документа <see cref="DocumentGetQuery{TDocument}.Select"/>.
    /// </summary>
    internal sealed class ExpressionSelectQuerySyntaxVisitor : ExpressionBaseQuerySyntaxVisitor
    {
        private ExpressionSelectQuerySyntaxVisitor(Type type) : base(type)
        {
        }


        public static Expression CreateSelectExpression(Type type, InvocationQuerySyntaxNode node)
        {
            var visitor = new ExpressionSelectQuerySyntaxVisitor(type);
            var selectBody = visitor.Visit(node);
            return Expression.Lambda(selectBody, visitor.Parameter);
        }


        // Overrides

        public override Expression VisitInvocationExpression(InvocationQuerySyntaxNode node)
        {
            if (!string.Equals(node.Name, QuerySyntaxHelper.IncludeMethodName, StringComparison.OrdinalIgnoreCase))
            {
                return base.VisitInvocationExpression(node);
            }

            return QuerySyntaxHelper.Projection(node.Arguments.Select(Visit));
        }
    }
}