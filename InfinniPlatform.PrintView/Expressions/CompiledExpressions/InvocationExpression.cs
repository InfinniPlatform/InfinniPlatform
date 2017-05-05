using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;

using InfinniPlatform.Dynamic;

namespace InfinniPlatform.PrintView.Expressions.CompiledExpressions
{
    internal class InvocationExpression : ICompiledExpression
    {
        private readonly Type[] _genericArguments;
        private readonly IEnumerable<ICompiledExpression> _invokeArguments;
        private readonly ICompiledExpression _invokeTarget;
        private readonly string _methodName;

        public InvocationExpression(string methodName, Type[] genericArguments, ICompiledExpression invokeTarget,
            IEnumerable<ICompiledExpression> invokeArguments)
        {
            _methodName = methodName;
            _genericArguments = genericArguments;
            _invokeTarget = invokeTarget;
            _invokeArguments = invokeArguments;
        }

        public object Execute(object dataContext, ExpressionScope scope)
        {
            object result;

            object invokeTarget = null;
            object[] invokeArguments = null;

            if (_invokeTarget != null)
            {
                invokeTarget = _invokeTarget.Execute(dataContext, scope);
            }

            if (_invokeArguments != null)
            {
                invokeArguments =
                    _invokeArguments.Select(i => i?.Execute(dataContext, scope)).ToArray();
            }

            // Вызов статического метода
            if (invokeTarget is Type)
            {
                ReflectionExtensions.InvokeMember((Type)invokeTarget, _methodName, invokeArguments, out result, _genericArguments);
            }
            // Вызов метода динамического объекта
            else if (invokeTarget is IDynamicMetaObjectProvider)
            {
                // Получение свойства динамического объекта
                var methodDelegate = DynamicObjectExtensions.TryGetPropertyValueByPath(invokeTarget, _methodName) as Delegate;

                if (methodDelegate != null)
                {
                    result = ReflectionExtensions.FastDynamicInvoke(methodDelegate, invokeArguments);
                }
                else
                {
                    ReflectionExtensions.InvokeMember(invokeTarget, _methodName, invokeArguments, out result, _genericArguments);
                }
            }
            // Вызов метода строготипизированного обычного объекта
            else
            {
                ReflectionExtensions.InvokeMember(invokeTarget, _methodName, invokeArguments, out result, _genericArguments);
            }

            return result;
        }
    }
}