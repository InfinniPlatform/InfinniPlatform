using System;
using System.Text;
using InfinniPlatform.Sdk.Dynamic;

namespace InfinniPlatform.FlowDocument.Builders.Factories.DisplayFormats
{
    internal sealed class ObjectFormatFactory : IPrintElementFactory
    {
        public object Create(PrintElementBuildContext buildContext, dynamic elementMetadata)
        {
            return FormatFunc(elementMetadata.Format);
        }

        private static Func<object, string> FormatFunc(object format)
        {
            var formatString = format as string;

            return value => FormatObject(value, formatString);
        }

        private static string FormatObject(object value, string format)
        {
            if (!string.IsNullOrEmpty(format))
            {
                var brackets = 0;

                var isProperty = false;
                var isPropertyFormat = false;

                var resultBuilder = new StringBuilder();
                var propertyNameBuilder = new StringBuilder();
                var propertyFormatBuilder = new StringBuilder();

                foreach (var c in format)
                {
                    if (c == '{')
                    {
                        ++brackets;

                        // Начало выражения форматирования свойства
                        if (brackets == 1)
                        {
                            isProperty = true;
                            isPropertyFormat = false;

                            propertyNameBuilder.Clear();
                            propertyFormatBuilder.Clear();

                            continue;
                        }
                    }
                    else if (c == ':')
                    {
                        // Начало настроек форматирования свойства
                        if (isProperty)
                        {
                            isProperty = false;
                            isPropertyFormat = true;

                            continue;
                        }
                    }
                    else if (c == '}')
                    {
                        --brackets;

                        // Конец выражения форматирования свойства
                        if (brackets == 0)
                        {
                            var propertyName = propertyNameBuilder.ToString();
                            var propertyFormat = isPropertyFormat ? propertyFormatBuilder.ToString() : null;

                            var propertyString = FormatProperty(value, propertyName, propertyFormat);
                            resultBuilder.Append(propertyString);

                            isProperty = false;
                            isPropertyFormat = false;

                            continue;
                        }
                    }

                    if (isProperty)
                    {
                        propertyNameBuilder.Append(c);
                    }
                    else if (isPropertyFormat)
                    {
                        propertyFormatBuilder.Append(c);
                    }
                    else
                    {
                        resultBuilder.Append(c);
                    }
                }

                return resultBuilder.ToString();
            }

            return (value != null) ? value.ToString() : null;
        }

        private static string FormatProperty(object value, string propertyName, string propertyFormat)
        {
            string propertyString = null;

            // Получение значения свойства
            var propertyValue = value.GetProperty(propertyName);

            if (propertyValue != null)
            {
                // Если правила форматирования не заданы
                if (string.IsNullOrEmpty(propertyFormat))
                {
                    propertyString = propertyValue.ToString();
                }
                else
                {
                    // Если значением является строка и ее хотят отформатировать
                    if (propertyValue is string)
                    {
                        DateTime propertyValueAsDateTime;
                        double propertyValueAsNumber;

                        // Попытка преобразовать строку в DateTime (наиболее вероятно)
                        if (ConvertHelper.TryToDateTime(propertyValue, out propertyValueAsDateTime))
                        {
                            propertyValue = propertyValueAsDateTime;
                        }
                        // Попытка преобразовать строку в Double (наименее вероятно)
                        else if (ConvertHelper.TryToDouble(propertyValue, out propertyValueAsNumber))
                        {
                            propertyValue = propertyValueAsNumber;
                        }
                    }

                    propertyFormat = "{0:" + propertyFormat + "}";
                    propertyString = string.Format(propertyFormat, propertyValue);
                }
            }

            return propertyString;
        }
    }
}