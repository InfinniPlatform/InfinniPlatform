using System;
using System.Linq;
using System.Linq.Expressions;

using InfinniPlatform.DocumentStorage.Properties;
using InfinniPlatform.DocumentStorage.QuerySyntax;

namespace InfinniPlatform.DocumentStorage.QueryFactories
{
    /// <inheritdoc />
    public abstract class ExpressionBaseQuerySyntaxVisitor : QuerySyntaxVisitor<Expression>
    {
        /// <summary>
        /// Initializes a new instance of <see cref="ExpressionBaseQuerySyntaxVisitor" />.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="level"></param>
        protected ExpressionBaseQuerySyntaxVisitor(Type type, int level = 0)
        {
            Type = type;
            Level = level;
            Parameter = Expression.Parameter(type, $"i_{level}");
        }


        protected readonly Type Type;
        protected readonly int Level;
        protected readonly ParameterExpression Parameter;


        /// <inheritdoc />
        public override Expression VisitInvocationExpression(InvocationQuerySyntaxNode node)
        {
            throw new NotSupportedException(string.Format(Resources.FunctionIsNotSupported, node.Name));
        }

        /// <inheritdoc />
        public override Expression VisitIdentifierName(IdentifierNameQuerySyntaxNode node)
        {
            var memberPath = node.Identifier.Split('.');

            return memberPath.Aggregate((Expression)Parameter, Expression.PropertyOrField);
        }


        /// <inheritdoc />
        public override Expression VisitLiteral(LiteralQuerySyntaxNode node)
        {
            return Expression.Constant(node.Value);
        }
    }
}