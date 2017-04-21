using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace InfinniPlatform.PrintView.Expressions.CompiledExpressions
{
    internal class CollectionInitializer : IInstanceInitializer
    {
        private readonly IEnumerable<ICompiledExpression> _items;

        public CollectionInitializer(IEnumerable<ICompiledExpression> items)
        {
            _items = items;
        }

        public void Initialize(object instance, object dataContext, ExpressionScope scope)
        {
            if (instance != null)
            {
                if (instance is IList)
                {
                    var list = (IList) instance;

                    foreach (var item in _items)
                    {
                        var itemValue = item.Execute(dataContext, scope);
                        list.Add(itemValue);
                    }
                }
                else
                {
                    var collectionType =
                        instance.GetType()
                                .GetTypeInfo()
                                .GetInterfaces()
                                .FirstOrDefault(i => i.GetTypeInfo().IsGenericType && i.GetGenericTypeDefinition() == typeof(ICollection<>))
                                ?.GetTypeInfo();

                    if (collectionType != null)
                    {
                        var addMethod = collectionType.GetMethod("Add");
                        var elementType = collectionType.GetGenericArguments().FirstOrDefault();

                        if (addMethod != null && elementType != null)
                        {
                            var elementTypeInfo = elementType.GetTypeInfo();

                            if (elementTypeInfo != null && (elementTypeInfo.IsClass || elementTypeInfo.IsValueType))
                            {
                                foreach (var item in _items)
                                {
                                    var itemValue = Activator.CreateInstance(elementType, item.Execute(dataContext, scope) as object[]);

                                    addMethod.Invoke(instance, new[] { itemValue });
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}