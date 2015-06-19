using System.Collections.Generic;
using System.Linq;
using InfinniPlatform.Sdk.Application.Dynamic;

namespace InfinniPlatform.Expressions.CompiledExpressions
{
    internal sealed class ElementAccessExpression : ICompiledExpression
    {
        private readonly ICompiledExpression _expression;
        private readonly IEnumerable<ICompiledExpression> _indexes;

        public ElementAccessExpression(ICompiledExpression expression, IEnumerable<ICompiledExpression> indexes)
        {
            _expression = expression;
            _indexes = indexes;
        }

        public object Execute(object dataContext, ExpressionScope scope)
        {
            var expression = _expression.Execute(dataContext, scope);
            var indexes = _indexes.Select(i => i.Execute(dataContext, scope));

            return ObjectHelper.GetIndexProperty(expression, indexes);
        }
    }
}