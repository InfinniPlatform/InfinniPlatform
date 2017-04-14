using System;

using InfinniPlatform.DocumentStorage.HttpService.Properties;
using InfinniPlatform.DocumentStorage.HttpService.QuerySyntax;

namespace InfinniPlatform.DocumentStorage.HttpService.QueryFactories
{
    public abstract class FuncBaseQuerySyntaxVisitor<TResult> : QuerySyntaxVisitor<TResult>
    {
        public override TResult VisitInvocationExpression(InvocationQuerySyntaxNode node)
        {
            throw new NotSupportedException(string.Format(Resources.FunctionIsNotSupported, node.Name));
        }
    }
}