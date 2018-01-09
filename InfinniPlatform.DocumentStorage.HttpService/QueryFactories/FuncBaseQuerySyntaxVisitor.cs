using System;

using InfinniPlatform.DocumentStorage.Properties;
using InfinniPlatform.DocumentStorage.QuerySyntax;

namespace InfinniPlatform.DocumentStorage.QueryFactories
{
    /// <inheritdoc />
    /// <typeparam name="TResult"></typeparam>
    public abstract class FuncBaseQuerySyntaxVisitor<TResult> : QuerySyntaxVisitor<TResult>
    {
        /// <inheritdoc />
        public override TResult VisitInvocationExpression(InvocationQuerySyntaxNode node)
        {
            throw new NotSupportedException(string.Format(Resources.FunctionIsNotSupported, node.Name));
        }
    }
}