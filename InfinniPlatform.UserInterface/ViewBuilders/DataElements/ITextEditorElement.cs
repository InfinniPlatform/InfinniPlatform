namespace InfinniPlatform.UserInterface.ViewBuilders.DataElements
{
	/// <summary>
	/// Общий интерфейс для редакторов текста.
	/// </summary>
	public interface ITextEditorElement
	{
		/// <summary>
		/// Возвращает текст.
		/// </summary>
		string GetText();

		/// <summary>
		/// Устанавливает текст.
		/// </summary>
		void SetText(string value);


		/// <summary>
		/// Возвращает позицию корретки.
		/// </summary>
		int GetCaretOffset();

		/// <summary>
		/// Устанавливает позицию корретки.
		/// </summary>
		void SetCaretOffset(int value);


		/// <summary>
		/// Возвращает позицию начала выделения.
		/// </summary>
		int GetSelectionStart();

		/// <summary>
		/// Возвращает выделенный текст.
		/// </summary>
		string GetSelectedText();


		/// <summary>
		/// Выделяет указанную часть текста.
		/// </summary>
		void SelectText(int start, int length);

		/// <summary>
		/// Вставляет указанный текст в текущую позицию.
		/// </summary>
		void InsertText(string text);
	}
}