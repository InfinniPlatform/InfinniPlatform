namespace InfinniPlatform.DocumentStorage.QuerySyntax
{
    /// <summary>
    /// Identifier name node.
    /// </summary>
    public class IdentifierNameQuerySyntaxNode : IQuerySyntaxNode
    {
        /// <summary>
        /// Initializes a new instance of <see cref="IdentifierNameQuerySyntaxNode" />.
        /// </summary>
        /// <param name="identifier">Identifier name.</param>
        public IdentifierNameQuerySyntaxNode(string identifier)
        {
            Identifier = identifier;
        }


        /// <summary>
        /// Identifier name.
        /// </summary>
        public readonly string Identifier;


        /// <inheritdoc />
        public TResult Accept<TResult>(QuerySyntaxVisitor<TResult> visitor)
        {
            return visitor.VisitIdentifierName(this);
        }


        /// <inheritdoc />
        public override string ToString()
        {
            return Identifier;
        }
    }
}