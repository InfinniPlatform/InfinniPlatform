using System;
using System.Collections.Generic;
using System.Linq;
using InfinniPlatform.Api.Metadata.Validation;
using InfinniPlatform.Sdk.Application.Dynamic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;

namespace InfinniPlatform.UserInterface.ViewBuilders.DataElements.CodeEditor
{
    /// <summary>
    ///     Интерфейс для форматирования JSON-объекта.
    /// </summary>
    public sealed class JsonObjectFormatter : IObjectFormatter
    {
        private readonly Action<object> _shemaValidator;

        public JsonObjectFormatter(Action<object> shemaValidator = null)
        {
            _shemaValidator = shemaValidator;
        }

        public string ConvertToString(object objectValue, out IEnumerable<CodeEditorError> errors)
        {
            string result = null;

            var errorList = new List<CodeEditorError>();

            if (objectValue != null)
            {
                if (objectValue is string)
                {
                    result = (string) objectValue;
                }
                else
                {
                    // Создание JSON-представления исходного объекта
                    var jsonValue = InvokeSilent(() => JObject.FromObject(objectValue), errorList);

                    if (NoErrors(errorList))
                    {
                        // Создание строкового представления исходного объекта
                        var stringValue = InvokeSilent(() => jsonValue.ToString(Formatting.Indented), errorList);

                        if (NoErrors(errorList))
                        {
                            result = stringValue;
                        }
                    }
                }
            }

            errors = errorList;

            return result;
        }

        public object ConvertFromString(string stringValue, out IEnumerable<CodeEditorError> errors)
        {
            object result = null;

            var errorList = new List<CodeEditorError>();

            if (string.IsNullOrEmpty(stringValue) == false)
            {
                // Преобразование исходной строки в JSON-представление
                var jsonValue = InvokeSilent(() => JObject.Parse(stringValue), errorList);

                if (NoErrors(errorList))
                {
                    // Создание динамического объекта из JSON-представления
                    var objectValue = InvokeSilent(jsonValue.ToObject<DynamicWrapper>, errorList);

                    if (NoErrors(errorList))
                    {
                        // Проверка схемы созданного динамического объекта
                        InvokeSilent(() => ValidateSchema(objectValue), errorList);

                        if (NoErrors(errorList))
                        {
                            result = objectValue;
                        }
                    }
                }
            }

            errors = errorList;

            return result;
        }

        private static TResult InvokeSilent<TResult>(Func<TResult> action, List<CodeEditorError> errors)
            where TResult : class
        {
            TResult result = null;

            try
            {
                result = action();
            }
            catch (JsonReaderException error)
            {
                // Синтаксические ошибки
                errors.Add(new CodeEditorError(CodeEditorErrorCategory.Error, error.Message, error.Path,
                    error.LineNumber, error.LinePosition));
            }
            catch (JsonSchemaException error)
            {
                // Синтаксические ошибки
                errors.Add(new CodeEditorError(CodeEditorErrorCategory.Error, error.Message, error.Path,
                    error.LineNumber, error.LinePosition));
            }
            catch (MetadataSchemaException error)
            {
                // Ошибки в схеме объекта
                errors.AddRange(
                    error.Errors.Select(
                        e =>
                            new CodeEditorError(CodeEditorErrorCategory.Warning, e.Message, e.Path, e.LineNumber,
                                e.LineColumn)));
            }
            catch (Exception error)
            {
                // Неизвестные ошибки
                errors.Add(new CodeEditorError(CodeEditorErrorCategory.Error, error.Message, null, 0, 0));
            }

            return result;
        }

        private static bool NoErrors(List<CodeEditorError> errors)
        {
            return (errors == null) || errors.All(i => i.Category != CodeEditorErrorCategory.Error);
        }

        private object ValidateSchema(object value)
        {
            if (_shemaValidator != null)
            {
                _shemaValidator(value);
            }

            return null;
        }
    }
}