namespace InfinniPlatform.UserInterface.ViewBuilders.DataElements.CodeEditor
{
	/// <summary>
	/// Ошибка редактора программного кода.
	/// </summary>
	public sealed class CodeEditorError
	{
		public CodeEditorError(CodeEditorErrorCategory category, string message, string path, int lineNumber, int lineColumn)
		{
			Category = category;
			Message = message;
			Path = path;
			LineNumber = lineNumber;
			LineColumn = lineColumn;
		}


		public CodeEditorErrorCategory Category { get; private set; }
		public string Message { get; private set; }
		public string Path { get; private set; }
		public int LineNumber { get; private set; }
		public int LineColumn { get; private set; }
	}
}