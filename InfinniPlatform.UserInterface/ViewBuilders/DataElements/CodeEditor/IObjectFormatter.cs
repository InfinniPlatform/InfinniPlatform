using System.Collections.Generic;

namespace InfinniPlatform.UserInterface.ViewBuilders.DataElements.CodeEditor
{
    /// <summary>
    ///     Интерфейс для форматирования объекта.
    /// </summary>
    public interface IObjectFormatter
    {
        string ConvertToString(object objectValue, out IEnumerable<CodeEditorError> errors);
        object ConvertFromString(string stringValue, out IEnumerable<CodeEditorError> errors);
    }
}