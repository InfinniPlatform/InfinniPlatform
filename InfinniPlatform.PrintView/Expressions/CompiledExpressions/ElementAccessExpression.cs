using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace InfinniPlatform.PrintView.Expressions.CompiledExpressions
{
    internal class ElementAccessExpression : ICompiledExpression
    {
        public ElementAccessExpression(ICompiledExpression expression, IEnumerable<ICompiledExpression> indexes)
        {
            _expression = expression;
            _indexes = indexes;
        }


        private readonly ICompiledExpression _expression;
        private readonly IEnumerable<ICompiledExpression> _indexes;


        public object Execute(object dataContext, ExpressionScope scope)
        {
            var expression = _expression.Execute(dataContext, scope);
            var indexes = _indexes.Select(i => i.Execute(dataContext, scope));

            return GetIndexProperty(expression, indexes);
        }


        private static object GetIndexProperty(object target, IEnumerable<object> indexes)
        {
            object result = null;

            if (target != null && indexes != null)
            {
                var arrayIndexes = indexes.ToArray();
                var length = arrayIndexes.Length;

                if (length > 0)
                {
                    if (target is Array)
                    {
                        result = ((Array)target).GetValue(arrayIndexes.Cast<int>().ToArray());
                    }
                    else if (target is IEnumerable)
                    {
                        return ((IEnumerable)target).Cast<object>().ElementAt((int)arrayIndexes[0]);
                    }
                    else
                    {
                        var properties = target.GetType().GetTypeInfo().GetProperties();

                        foreach (var property in properties)
                        {
                            if (property.GetIndexParameters().Length == length)
                            {
                                try
                                {
                                    return property.GetValue(target, arrayIndexes);
                                }
                                catch (TargetInvocationException error)
                                {
                                    if (error.InnerException != null)
                                    {
                                        throw error.InnerException;
                                    }

                                    throw;
                                }
                            }
                        }
                    }
                }
            }

            return result;
        }
    }
}