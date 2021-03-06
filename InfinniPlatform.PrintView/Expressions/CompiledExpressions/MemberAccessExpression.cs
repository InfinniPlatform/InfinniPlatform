﻿using System;

using InfinniPlatform.Dynamic;

namespace InfinniPlatform.PrintView.Expressions.CompiledExpressions
{
    internal class MemberAccessExpression : ICompiledExpression
    {
        private readonly ICompiledExpression _expression;
        private readonly string _memberName;

        public MemberAccessExpression(ICompiledExpression expression, string memberName)
        {
            _expression = expression;
            _memberName = memberName;
        }

        public object Execute(object dataContext, ExpressionScope scope)
        {
            object result;

            var expression = _expression.Execute(dataContext, scope);

            if (expression is Type)
            {
                result = ReflectionExtensions.GetMemberValue((Type) expression, _memberName);
            }
            else
            {
                result = DynamicObjectExtensions.TryGetPropertyValueByPath(expression, _memberName);
            }

            return result;
        }
    }
}