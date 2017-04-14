using System.Collections.Generic;
using System.Linq;

namespace InfinniPlatform.DocumentStorage.HttpService.QuerySyntax
{
    /// <summary>
    /// Вызов метода.
    /// </summary>
    public class InvocationQuerySyntaxNode : IQuerySyntaxNode
    {
        public InvocationQuerySyntaxNode(string name)
        {
            Name = name;
            Arguments = new List<IQuerySyntaxNode>();
        }


        /// <summary>
        /// Имя метода.
        /// </summary>
        public readonly string Name;

        /// <summary>
        /// Список аргументов метода.
        /// </summary>
        public readonly List<IQuerySyntaxNode> Arguments;


        public TResult Accept<TResult>(QuerySyntaxVisitor<TResult> visitor)
        {
            return visitor.VisitInvocationExpression(this);
        }


        public override string ToString()
        {
            return $"{Name}({string.Join(",", Arguments.Select(i => i.ToString()))})";
        }
    }
}