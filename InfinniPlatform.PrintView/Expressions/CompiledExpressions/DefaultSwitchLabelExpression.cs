﻿namespace InfinniPlatform.PrintView.Expressions.CompiledExpressions
{
    internal class DefaultSwitchLabelExpression : ICompiledExpression
    {
        public object Execute(object dataContext, ExpressionScope scope)
        {
            return this;
        }

        public override int GetHashCode()
        {
            return 0;
        }

        public override bool Equals(object obj)
        {
            return true;
        }
    }
}