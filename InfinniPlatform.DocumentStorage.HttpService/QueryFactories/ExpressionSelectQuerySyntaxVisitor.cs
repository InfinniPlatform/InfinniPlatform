using System;
using System.Linq;
using System.Linq.Expressions;

using InfinniPlatform.DocumentStorage.QuerySyntax;

namespace InfinniPlatform.DocumentStorage.QueryFactories
{
    /// <summary>
    /// Parse document projection rules from select query.
    /// </summary>
    public class ExpressionSelectQuerySyntaxVisitor : ExpressionBaseQuerySyntaxVisitor
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

        /// <inheritdoc />
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