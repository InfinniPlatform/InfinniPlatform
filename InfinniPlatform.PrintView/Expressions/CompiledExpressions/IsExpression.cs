using System;
using System.Reflection;

namespace InfinniPlatform.PrintView.Expressions.CompiledExpressions
{
    internal class IsExpression : BinaryExpression
    {
        public IsExpression(ICompiledExpression left, ICompiledExpression right)
            : base(left, right)
        {
        }

        public override object Execute(object dataContext, ExpressionScope scope)
        {
            var left = Left.Execute(dataContext, scope);
            var right = (Type) Right.Execute(dataContext, scope);

            return right.GetTypeInfo().IsInstanceOfType(left);
        }
    }
}