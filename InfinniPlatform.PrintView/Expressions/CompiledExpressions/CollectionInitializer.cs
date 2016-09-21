using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

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
                            .GetInterfaces()
                            .FirstOrDefault(
                                i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof (ICollection<>));

                    if (collectionType != null)
                    {
                        var addMethod = collectionType.GetMethod("Add");
                        var elementType = collectionType.GetGenericArguments().FirstOrDefault();

                        if (addMethod != null && elementType != null && (elementType.IsClass || elementType.IsValueType))
                        {
                            foreach (var item in _items)
                            {
                                var itemValue = Activator.CreateInstance(elementType,
                                    item.Execute(dataContext, scope) as object[]);
                                addMethod.Invoke(instance, new[] {itemValue});
                            }
                        }
                    }
                }
            }
        }
    }
}