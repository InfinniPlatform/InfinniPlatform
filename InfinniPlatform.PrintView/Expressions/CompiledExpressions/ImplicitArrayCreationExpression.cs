using System;
using System.Collections.Generic;

namespace InfinniPlatform.PrintView.Expressions.CompiledExpressions
{
    internal class ImplicitArrayCreationExpression : ICompiledExpression
    {
        private readonly IDictionary<int[], ICompiledExpression> _initializers;

        public ImplicitArrayCreationExpression(IDictionary<int[], ICompiledExpression> initializers)
        {
            _initializers = initializers;
        }

        public object Execute(object dataContext, ExpressionScope scope)
        {
            var elementType = typeof (object);
            var arraySizes = new List<object>();
            var initializers = new Dictionary<int[], object>();

            if (_initializers != null)
            {
                Type type = null;
                Type prevType = null;
                var hasDiffTypes = false;
                var hasNullItems = false;

                foreach (var initializer in _initializers)
                {
                    // Вычисление размеров массива

                    var elementIndexes = initializer.Key;

                    for (var s = arraySizes.Count; s < initializer.Key.Length; ++s)
                    {
                        arraySizes.Add(0);
                    }

                    var rank = 0;

                    foreach (var index in elementIndexes)
                    {
                        arraySizes[rank] = Math.Max((int) arraySizes[rank], index + 1);

                        ++rank;
                    }

                    // Определение типа элементов массива

                    var elementValue = initializer.Value.Execute(dataContext, scope);

                    if (elementValue != null)
                    {
                        type = elementValue.GetType();

                        if (prevType == null)
                        {
                            prevType = type;
                        }
                        else if (prevType != type)
                        {
                            hasDiffTypes = true;
                        }
                    }
                    else
                    {
                        hasNullItems = true;
                    }

                    // Вычисление значения элемента массива

                    initializers.Add(elementIndexes, elementValue);
                }

                if (!hasDiffTypes && type != null && (!hasNullItems || !type.IsValueType))
                {
                    elementType = type;
                }
            }

            if (arraySizes.Count == 0)
            {
                arraySizes.Add(0);
            }

            var array =
                (Array) Activator.CreateInstance(elementType.MakeArrayType(arraySizes.Count), arraySizes.ToArray());

            foreach (var initializer in initializers)
            {
                var elementIndexes = initializer.Key;
                var elementValue = initializer.Value;

                array.SetValue(elementValue, elementIndexes);
            }

            return array;
        }
    }
}