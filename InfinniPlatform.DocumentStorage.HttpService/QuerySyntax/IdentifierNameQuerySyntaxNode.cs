namespace InfinniPlatform.DocumentStorage.QuerySyntax
{
    /// <summary>
    /// Имя идентификатора.
    /// </summary>
    public class IdentifierNameQuerySyntaxNode : IQuerySyntaxNode
    {
        public IdentifierNameQuerySyntaxNode(string identifier)
        {
            Identifier = identifier;
        }


        /// <summary>
        /// Имя идентификатора.
        /// </summary>
        public readonly string Identifier;


        public TResult Accept<TResult>(QuerySyntaxVisitor<TResult> visitor)
        {
            return visitor.VisitIdentifierName(this);
        }


        public override string ToString()
        {
            return Identifier;
        }
    }
}