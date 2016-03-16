using System;
using System.Linq;
using System.Linq.Expressions;

using InfinniPlatform.DocumentStorage.Properties;
using InfinniPlatform.DocumentStorage.Services.QuerySyntax;

namespace InfinniPlatform.DocumentStorage.Services.QueryFactories
{
    internal abstract class ExpressionBaseQuerySyntaxVisitor : QuerySyntaxVisitor<Expression>
    {
        protected ExpressionBaseQuerySyntaxVisitor(Type type)
        {
            Type = type;
            Parameter = Expression.Parameter(type);
        }


        protected readonly Type Type;
        protected readonly ParameterExpression Parameter;


        public override Expression VisitInvocationExpression(InvocationQuerySyntaxNode node)
        {
            throw new NotSupportedException(string.Format(Resources.FunctionIsNotSupported, node.Name));
        }

        public override Expression VisitIdentifierName(IdentifierNameQuerySyntaxNode node)
        {
            var memberPath = node.Identifier.Split('.');

            return memberPath.Aggregate((Expression)Parameter, Expression.PropertyOrField);
        }


        public override Expression VisitLiteral(LiteralQuerySyntaxNode node)
        {
            return Expression.Constant(node.Value);
        }
    }
}