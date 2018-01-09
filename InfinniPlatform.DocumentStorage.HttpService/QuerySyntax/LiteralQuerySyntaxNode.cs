using System;
using System.Globalization;

namespace InfinniPlatform.DocumentStorage.QuerySyntax
{
    /// <summary>
    /// Literal node.
    /// </summary>
    public class LiteralQuerySyntaxNode : IQuerySyntaxNode
    {
        /// <summary>
        /// Initializes a new instance of <see cref="LiteralQuerySyntaxNode" />.
        /// </summary>
        /// <param name="value">Literal value.</param>
        public LiteralQuerySyntaxNode(object value)
        {
            Value = value;
        }


        /// <summary>
        /// Literal value.
        /// </summary>
        public readonly object Value;


        /// <inheritdoc />
        public TResult Accept<TResult>(QuerySyntaxVisitor<TResult> visitor)
        {
            return visitor.VisitLiteral(this);
        }


        /// <inheritdoc />
        public override string ToString()
        {
            switch (Value)
            {
                case null:
                    return "null";
                case false:
                    return "false";
                case true:
                    return "true";
                case string _:
                    return $"'{Value}'";
            }

            return Convert.ToString(Value, CultureInfo.InvariantCulture);
        }
    }
}