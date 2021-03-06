﻿using System;
using System.Text;

using InfinniPlatform.Dynamic;
using InfinniPlatform.PrintView.Format;

namespace InfinniPlatform.PrintView.Factories.Format
{
    internal class ObjectFormatFactory : PrintElementFactoryBase<ObjectFormat>
    {
        public override object Create(PrintElementFactoryContext context, ObjectFormat template)
        {
            return FormatFunc(template.Format);
        }

        private static Func<object, string> FormatFunc(string format)
        {
            return value =>
                   {
                       if (string.IsNullOrEmpty(format))
                       {
                           return value?.ToString();
                       }

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
                   };
        }

        private static string FormatProperty(object value, string propertyName, string propertyFormat)
        {
            string propertyString = null;

            // Получение значения свойства
            var propertyValue = string.IsNullOrEmpty(propertyName) ? value : value?.TryGetPropertyValueByPath(propertyName);

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