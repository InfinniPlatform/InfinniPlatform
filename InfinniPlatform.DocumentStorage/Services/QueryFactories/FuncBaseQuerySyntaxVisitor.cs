using System;

using InfinniPlatform.DocumentStorage.Properties;
using InfinniPlatform.DocumentStorage.Services.QuerySyntax;
using InfinniPlatform.Sdk.Documents;

namespace InfinniPlatform.DocumentStorage.Services.QueryFactories
{
    internal abstract class FuncBaseQuerySyntaxVisitor : QuerySyntaxVisitor<Func<IDocumentFilterBuilder, object>>
    {
        public override Func<IDocumentFilterBuilder, object> VisitInvocationExpression(InvocationQuerySyntaxNode node)
        {
            throw new NotSupportedException(string.Format(Resources.FunctionIsNotSupported, node.Name));
        }

        public override Func<IDocumentFilterBuilder, object> VisitIdentifierName(IdentifierNameQuerySyntaxNode node)
        {
            return f => node.Identifier;
        }


        public override Func<IDocumentFilterBuilder, object> VisitLiteral(LiteralQuerySyntaxNode node)
        {
            return f => node.Value;
        }
    }
}