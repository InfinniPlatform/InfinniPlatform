using System;

namespace InfinniPlatform.PrintView.Expressions.CompiledExpressions
{
    internal class FunctionExpression : ICompiledExpression
    {
        private readonly Func<object, ExpressionScope, object> _function;

        public FunctionExpression(Func<object, ExpressionScope, object> function)
        {
            _function = function;
        }

        public object Execute(object dataContext, ExpressionScope scope)
        {
            return _function(dataContext, scope);
        }
    }
}