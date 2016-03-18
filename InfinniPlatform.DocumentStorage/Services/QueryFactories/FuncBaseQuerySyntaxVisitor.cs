using System;

using InfinniPlatform.DocumentStorage.Properties;
using InfinniPlatform.DocumentStorage.Services.QuerySyntax;

namespace InfinniPlatform.DocumentStorage.Services.QueryFactories
{
    internal abstract class FuncBaseQuerySyntaxVisitor<TResult> : QuerySyntaxVisitor<TResult>
    {
        public override TResult VisitInvocationExpression(InvocationQuerySyntaxNode node)
        {
            throw new NotSupportedException(string.Format(Resources.FunctionIsNotSupported, node.Name));
        }
    }
}