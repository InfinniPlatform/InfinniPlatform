﻿using System;
using System.Globalization;

namespace InfinniPlatform.DocumentStorage.Services.QuerySyntax
{
    /// <summary>
    /// Литерал.
    /// </summary>
    public sealed class LiteralQuerySyntaxNode : IQuerySyntaxNode
    {
        public LiteralQuerySyntaxNode(object value)
        {
            Value = value;
        }


        /// <summary>
        /// Значение литерала.
        /// </summary>
        public readonly object Value;


        public TResult Accept<TResult>(QuerySyntaxVisitor<TResult> visitor)
        {
            return visitor.VisitLiteral(this);
        }


        public override string ToString()
        {
            if (Value == null)
            {
                return "null";
            }

            if (Value.Equals(false))
            {
                return "false";
            }

            if (Value.Equals(true))
            {
                return "true";
            }

            if (Value is string)
            {
                return $"'{Value}'";
            }

            return Convert.ToString(Value, CultureInfo.InvariantCulture);
        }
    }
}