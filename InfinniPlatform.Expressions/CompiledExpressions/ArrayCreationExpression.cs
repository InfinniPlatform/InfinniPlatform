using System;
using System.Collections.Generic;
using System.Linq;

namespace InfinniPlatform.Expressions.CompiledExpressions
{
    internal sealed class ArrayCreationExpression : ICompiledExpression
    {
        private readonly Type _elementType;
        private readonly IDictionary<int[], ICompiledExpression> _initializers;
        private readonly IEnumerable<IEnumerable<ICompiledExpression>> _rankSpecifiers;

        public ArrayCreationExpression(Type elementType, IEnumerable<IEnumerable<ICompiledExpression>> rankSpecifiers,
            IDictionary<int[], ICompiledExpression> initializers)
        {
            _elementType = elementType;
            _rankSpecifiers = rankSpecifiers;
            _initializers = initializers;
        }

        public object Execute(object dataContext, ExpressionScope scope)
        {
            var arrayType = _elementType;
            var arraySizes = new List<object>();

            var rankSpecifierIndex = _rankSpecifiers.Count();

            foreach (var rankSpecifier in _rankSpecifiers.Reverse())
            {
                var rank = 0;

                foreach (var size in rankSpecifier)
                {
                    if (size != null)
                    {
                        var sizeValue = size.Execute(dataContext, scope);

                        if (sizeValue != null)
                        {
                            arraySizes.Add(sizeValue);
                        }
                    }
                    else if (rankSpecifierIndex == 1 && _initializers != null && _initializers.Count > 0)
                    {
                        var sizeValue = 0;

                        foreach (var i in _initializers)
                        {
                            if (rank < i.Key.Length)
                            {
                                if (sizeValue < i.Key[rank])
                                {
                                    sizeValue = i.Key[rank];
                                }
                            }
                            else
                            {
                                break;
                            }
                        }

                        arraySizes.Add(sizeValue + 1);
                    }

                    ++rank;
                }

                arrayType = arrayType.MakeArrayType(rank);

                --rankSpecifierIndex;
            }

            if (arraySizes.Count == 0)
            {
                arraySizes.Add(0);
            }

            var array = (Array) Activator.CreateInstance(arrayType, arraySizes.ToArray());

            if (_initializers != null)
            {
                foreach (var initializer in _initializers)
                {
                    var elementIndexes = initializer.Key;
                    var elementValue = initializer.Value.Execute(dataContext, scope);
                    array.SetValue(elementValue, elementIndexes);
                }
            }

            return array;
        }
    }
}