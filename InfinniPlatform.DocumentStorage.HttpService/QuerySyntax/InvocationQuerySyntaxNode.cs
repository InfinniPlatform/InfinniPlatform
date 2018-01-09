using System.Collections.Generic;
using System.Linq;

namespace InfinniPlatform.DocumentStorage.QuerySyntax
{
    /// <summary>
    /// Method invocation node name.
    /// </summary>
    public class InvocationQuerySyntaxNode : IQuerySyntaxNode
    {
        /// <summary>
        /// Initializes a new instance of <see cref="InvocationQuerySyntaxNode" />.
        /// </summary>
        /// <param name="name">Method name.</param>
        public InvocationQuerySyntaxNode(string name)
        {
            Name = name;
            Arguments = new List<IQuerySyntaxNode>();
        }


        /// <summary>
        /// Method name.
        /// </summary>
        public readonly string Name;

        /// <summary>
        /// Method arguments list.
        /// </summary>
        public readonly List<IQuerySyntaxNode> Arguments;


        /// <inheritdoc />
        public TResult Accept<TResult>(QuerySyntaxVisitor<TResult> visitor)
        {
            return visitor.VisitInvocationExpression(this);
        }


        /// <inheritdoc />
        public override string ToString()
        {
            return $"{Name}({string.Join(",", Arguments.Select(i => i.ToString()))})";
        }
    }
}