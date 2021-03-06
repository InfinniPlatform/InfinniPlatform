﻿namespace InfinniPlatform.PrintView.Expressions.CompiledExpressions
{
    internal class CoalesceExpression : BinaryExpression
    {
        public CoalesceExpression(ICompiledExpression left, ICompiledExpression right)
            : base(left, right)
        {
        }

        public override object Execute(object dataContext, ExpressionScope scope)
        {
            dynamic left = Left.Execute(dataContext, scope);
            dynamic right = Right.Execute(dataContext, scope);
            return left ?? right;
        }
    }
}