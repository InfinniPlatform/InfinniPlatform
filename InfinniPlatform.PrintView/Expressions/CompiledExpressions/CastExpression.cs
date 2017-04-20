using System;
using System.Reflection;

using InfinniPlatform.Dynamic;

namespace InfinniPlatform.PrintView.Expressions.CompiledExpressions
{
    internal class CastExpression : ICompiledExpression
    {
        private readonly ICompiledExpression _expression;
        private readonly Type _type;

        public CastExpression(Type type, ICompiledExpression expression)
        {
            _type = type;
            _expression = expression;
        }

        public object Execute(object dataContext, ExpressionScope scope)
        {
            var expression = _expression.Execute(dataContext, scope);

            if (expression == null)
            {
                if (_type.GetTypeInfo().IsValueType)
                {
                    expression = ReflectionExtensions.GetDefaultValue(_type);
                }
            }
            else
            {
                expression = Convert.ChangeType(expression, _type);
            }

            return expression;
        }
    }
}