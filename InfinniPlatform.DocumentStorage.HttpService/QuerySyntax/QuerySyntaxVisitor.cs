namespace InfinniPlatform.DocumentStorage.QuerySyntax
{
    /// <summary>
    /// Provides interface for request string syntax analysis.
    /// </summary>
    /// <typeparam name="TResult">Syntax analysis result type.</typeparam>
    public abstract class QuerySyntaxVisitor<TResult>
    {
        /// <summary>
        /// Processes method invocation.
        /// </summary>
        public virtual TResult VisitInvocationExpression(InvocationQuerySyntaxNode node)
        {
            return DefaultVisit(node);
        }

        /// <summary>
        /// Processes identifier name.
        /// </summary>
        public virtual TResult VisitIdentifierName(IdentifierNameQuerySyntaxNode node)
        {
            return DefaultVisit(node);
        }

        /// <summary>
        /// Processes literal.
        /// </summary>
        public virtual TResult VisitLiteral(LiteralQuerySyntaxNode node)
        {
            return DefaultVisit(node);
        }


        /// <summary>
        /// Default node handler.
        /// </summary>
        public virtual TResult DefaultVisit(IQuerySyntaxNode node)
        {
            return default(TResult);
        }


        /// <summary>
        /// Node handler.
        /// </summary>
        public virtual TResult Visit(IQuerySyntaxNode node)
        {
            if (node != null)
            {
                return node.Accept(this);
            }

            return default(TResult);
        }
    }
}