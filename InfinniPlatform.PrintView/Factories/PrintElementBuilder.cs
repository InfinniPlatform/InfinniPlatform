using System;
using System.Collections;
using System.Collections.Generic;

using InfinniPlatform.Dynamic;
using InfinniPlatform.PrintView.Defaults;
using InfinniPlatform.PrintView.Expressions;
using InfinniPlatform.PrintView.Expressions.Parser;

namespace InfinniPlatform.PrintView.Factories
{
    /// <summary>
    /// Универсальная фабрика для создания элементов.
    /// </summary>
    internal class PrintElementBuilder
    {
        private readonly Dictionary<Type, IPrintElementFactory> _factories
            = new Dictionary<Type, IPrintElementFactory>();


        public void Register<T>(PrintElementFactoryBase<T> factory)
        {
            _factories.Add(typeof(T), factory);
        }


        public object BuildElement(PrintElementFactoryContext context, object template)
        {
            if (template != null)
            {
                IPrintElementFactory factory;

                if (_factories.TryGetValue(template.GetType(), out factory) && CanCreateElement(context, template))
                {
                    var element = factory.Create(context, template);
                    context.MapElement(element, template);
                    return element;
                }
            }

            return null;
        }

        public IEnumerable BuildElements(PrintElementFactoryContext context, IEnumerable templates)
        {
            if (templates != null)
            {
                var elements = new List<object>();

                foreach (var template in templates)
                {
                    var elementContext = context.Clone();

                    var element = BuildElement(elementContext, template);

                    if (element != null)
                    {
                        elements.Add(element);
                    }
                }

                return elements;
            }

            return null;
        }


        private static bool CanCreateElement(PrintElementFactoryContext context, object template)
        {
            // Определение настроек видимости элемента

            var elementTemplate = template as PrintElement;

            if (elementTemplate == null)
            {
                return true;
            }

            var visibility = elementTemplate.Visibility ?? PrintVisibility.Source;

            if (visibility == PrintVisibility.Never)
            {
                return false;
            }

            var sourceProperty = elementTemplate.Source;
            var sourceExpression = elementTemplate.Expression;

            var hasDataSource = !string.IsNullOrWhiteSpace(sourceProperty) || !string.IsNullOrWhiteSpace(sourceExpression);

            // Если у элемента указан источник данных
            if (hasDataSource)
            {
                // Установка данных элемента по источнику
                SetElementData(context, sourceProperty, sourceExpression);

                // Если элемент печатается только при наличии данных
                if (!context.IsDesignMode && (visibility == PrintVisibility.Source) &&
                    ConvertHelper.ObjectIsNullOrEmpty(context.ElementSourceValue))
                {
                    return false;
                }
            }

            // Установка стиля элемента по имени
            context.ElementStyle = context.FindStyle(elementTemplate.Style);

            return true;
        }

        private static void SetElementData(PrintElementFactoryContext context, string sourceProperty, string sourceExpression)
        {
            context.ElementSourceProperty = sourceProperty;
            context.ElementSourceExpression = sourceExpression;

            var elementSourceValue = context.ElementSourceValue;

            // Если указано свойство данных
            if (!string.IsNullOrWhiteSpace(sourceProperty))
            {
                if (sourceProperty != PrintViewDefaults.RootSource)
                {
                    elementSourceValue = sourceProperty.StartsWith(PrintViewDefaults.RootSource + ".")
                        ? DynamicObjectExtensions.TryGetPropertyValueByPath(context.Source, sourceProperty.Substring(2))
                        : DynamicObjectExtensions.TryGetPropertyValueByPath(context.ElementSourceValue, sourceProperty);
                }
                else
                {
                    elementSourceValue = context.Source;
                }
            }

            // Если указано выражение над данными
            if (!string.IsNullOrWhiteSpace(sourceExpression))
            {
                elementSourceValue = ExpressionExecutor.Execute(sourceExpression, elementSourceValue);
            }

            context.ElementSourceValue = elementSourceValue;
        }
    }
}