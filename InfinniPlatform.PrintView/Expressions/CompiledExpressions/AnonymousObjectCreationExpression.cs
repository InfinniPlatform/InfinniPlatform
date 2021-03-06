﻿using InfinniPlatform.Dynamic;

namespace InfinniPlatform.PrintView.Expressions.CompiledExpressions
{
    internal class AnonymousObjectCreationExpression : ICompiledExpression
    {
        private readonly ObjectInitializer _initializer;

        public AnonymousObjectCreationExpression(ObjectInitializer initializer)
        {
            _initializer = initializer;
        }

        public object Execute(object dataContext, ExpressionScope scope)
        {
            var result = new DynamicDocument();

            if (_initializer != null)
            {
                _initializer.Initialize(result, dataContext, scope);
            }

            return result;
        }
    }
}