using System;
using System.Linq;
using System.Linq.Expressions;

using InfinniPlatform.DocumentStorage.HttpService.Properties;
using InfinniPlatform.DocumentStorage.HttpService.QuerySyntax;

namespace InfinniPlatform.DocumentStorage.HttpService.QueryFactories
{
    public abstract class ExpressionBaseQuerySyntaxVisitor : QuerySyntaxVisitor<Expression>
    {
        protected ExpressionBaseQuerySyntaxVisitor(Type type, int level = 0)
        {
            Type = type;
            Level = level;
            Parameter = Expression.Parameter(type, $"i_{level}");
        }


        protected readonly Type Type;
        protected readonly int Level;
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