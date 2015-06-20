using System.Collections.Generic;

namespace InfinniPlatform.UserInterface.ViewBuilders.DataElements.CodeEditor
{
    /// <summary>
    ///     Интерфейс для форматирования текста.
    /// </summary>
    public sealed class TextObjectFormatter : IObjectFormatter
    {
        public string ConvertToString(object objectValue, out IEnumerable<CodeEditorError> errors)
        {
            string result = null;

            errors = null;

            if (objectValue != null)
            {
                if (objectValue is string)
                {
                    result = (string) objectValue;
                }
                else
                {
                    result = objectValue.ToString();
                }
            }

            return result;
        }

        public object ConvertFromString(string stringValue, out IEnumerable<CodeEditorError> errors)
        {
            errors = null;

            return stringValue;
        }
    }
}