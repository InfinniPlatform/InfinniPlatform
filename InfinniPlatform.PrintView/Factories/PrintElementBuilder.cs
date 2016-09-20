using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;

using InfinniPlatform.Expressions.Parser;
using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.Sdk.Serialization;

namespace InfinniPlatform.PrintView.Factories
{
    /// <summary>
    /// Построитель элементов печатного представления.
    /// </summary>
    internal class PrintElementBuilder
    {
        private readonly Dictionary<string, IPrintElementFactory> _factories
            = new Dictionary<string, IPrintElementFactory>(StringComparer.OrdinalIgnoreCase);


        public void Register(string elementType, IPrintElementFactory elementFactory)
        {
            if (string.IsNullOrEmpty(elementType))
            {
                throw new ArgumentNullException(nameof(elementType));
            }

            if (elementFactory == null)
            {
                throw new ArgumentNullException(nameof(elementFactory));
            }

            _factories.Add(elementType, elementFactory);
        }


        public object BuildElement(PrintElementBuildContext buildContext, dynamic elementMetadata)
        {
            if (elementMetadata is IDynamicMetaObjectProvider)
            {
                foreach (var property in elementMetadata)
                {
                    return BuildElement(buildContext, property.Value, property.Key);
                }
            }

            return null;
        }

        public object BuildElement(PrintElementBuildContext buildContext, dynamic elementMetadata, string elementType)
        {
            if (elementMetadata != null)
            {
                IPrintElementFactory elementFactory;

                if (_factories.TryGetValue(elementType, out elementFactory) &&
                    CanCreateElement(buildContext, elementMetadata))
                {
                    var element = elementFactory.Create(buildContext, elementMetadata);
                    buildContext.MapElement(element, elementMetadata);
                    return element;
                }
            }

            return null;
        }


        public IEnumerable BuildElements(PrintElementBuildContext buildContext, IEnumerable elementMetadata)
        {
            if (elementMetadata != null)
            {
                var items = new List<object>();

                foreach (var itemMetadata in elementMetadata)
                {
                    var item = BuildElement(buildContext.Clone(), JsonObjectSerializer.Default.ConvertFromDynamic<DynamicWrapper>(itemMetadata));

                    if (item != null)
                    {
                        items.Add(item);
                    }
                }

                return items;
            }

            return null;
        }

        public IEnumerable BuildElements(PrintElementBuildContext buildContext, IEnumerable elementMetadata,
            string elementType)
        {
            if (elementMetadata != null)
            {
                var items = new List<object>();

                foreach (var itemMetadata in elementMetadata)
                {
                    var item = BuildElement(buildContext.Clone(), itemMetadata, elementType);

                    if (item != null)
                    {
                        items.Add(item);
                    }
                }

                return items;
            }

            return null;
        }


        private static bool CanCreateElement(PrintElementBuildContext buildContext, dynamic elementMetadata)
        {
            string visibility;

            if (!ConvertHelper.TryToNormString(elementMetadata.Visibility, out visibility))
            {
                visibility = "source";
            }

            if (visibility == "never")
            {
                return false;
            }

            string sourceProperty;
            string sourceExpression = null;

            var hasDataSource = ConvertHelper.TryToString(elementMetadata.Source, out sourceProperty)
                                || ConvertHelper.TryToString(elementMetadata.Expression, out sourceExpression);

            // Если у элемента указан источник данных
            if (hasDataSource)
            {
                // Установка данных элемента по источнику
                SetElementData(buildContext, sourceProperty, sourceExpression);

                // Если элемент печатается только при наличии данных
                if (!buildContext.IsDesignMode && (visibility == "source") &&
                    ConvertHelper.ObjectIsNullOrEmpty(buildContext.ElementSourceValue))
                {
                    return false;
                }
            }

            // Установка стиля элемента по имени
            SetElementStyle(buildContext, elementMetadata.Style);

            return true;
        }

        private static void SetElementData(PrintElementBuildContext buildContext, string sourceProperty, string sourceExpression)
        {
            buildContext.ElementSourceProperty = sourceProperty;
            buildContext.ElementSourceExpression = sourceExpression;

            var elementSourceValue = buildContext.ElementSourceValue;

            // Если указано свойство данных
            if (!string.IsNullOrEmpty(sourceProperty))
            {
                if (sourceProperty != "$")
                {
                    elementSourceValue = sourceProperty.StartsWith("$.")
                        ? buildContext.PrintViewSource.GetProperty(sourceProperty.Substring(2))
                        : buildContext.ElementSourceValue.GetProperty(sourceProperty);
                }
                else
                {
                    elementSourceValue = buildContext.PrintViewSource;
                }
            }

            // Если указано выражение над данными
            if (!string.IsNullOrEmpty(sourceExpression))
            {
                elementSourceValue = ExpressionExecutor.Execute(sourceExpression, elementSourceValue);
            }

            buildContext.ElementSourceValue = elementSourceValue;
        }

        private static void SetElementStyle(PrintElementBuildContext buildContext, object elementStyleName)
        {
            buildContext.ElementStyle = buildContext.FindStyle(elementStyleName);
        }
    }
}