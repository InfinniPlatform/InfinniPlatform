using System;
using System.Reflection;

namespace InfinniPlatform.PrintView.Expressions.CompiledExpressions
{
    internal class AsExpression : BinaryExpression
    {
        public AsExpression(ICompiledExpression left, ICompiledExpression right)
            : base(left, right)
        {
        }

        public override object Execute(object dataContext, ExpressionScope scope)
        {
            var left = Left.Execute(dataContext, scope);
            var right = (Type) Right.Execute(dataContext, scope);

            return right.GetTypeInfo().IsInstanceOfType(left) ? left : null;
        }
    }
}