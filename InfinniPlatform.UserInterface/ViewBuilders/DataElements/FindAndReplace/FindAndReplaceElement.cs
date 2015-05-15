using System;
using System.Windows.Controls;
using System.Windows.Input;

using InfinniPlatform.Api.Extensions;
using InfinniPlatform.UserInterface.Properties;
using InfinniPlatform.UserInterface.ViewBuilders.ActionElements.ToolBar;
using InfinniPlatform.UserInterface.ViewBuilders.DataElements.CheckBox;
using InfinniPlatform.UserInterface.ViewBuilders.DataElements.TextBox;
using InfinniPlatform.UserInterface.ViewBuilders.Elements;
using InfinniPlatform.UserInterface.ViewBuilders.LayoutPanels.GridPanel;
using InfinniPlatform.UserInterface.ViewBuilders.Scripts;
using InfinniPlatform.UserInterface.ViewBuilders.Views;

namespace InfinniPlatform.UserInterface.ViewBuilders.DataElements.FindAndReplace
{
	/// <summary>
	/// Элемент представления для поиска и замены текста.
	/// </summary>
	public sealed class FindAndReplaceElement : BaseElement<UserControl>
	{
		public FindAndReplaceElement(View view)
			: base(view)
		{
			// Main Panel
			var mainPanel = new GridPanelElement(view);
			mainPanel.SetColumns(12);

			// Rows
			var findWhatRow = mainPanel.AddRow();
			var replaceWithRow = mainPanel.AddRow();
			var settingsRow = mainPanel.AddRow();

			// Find What
			var findWhatTextCell = findWhatRow.AddCell(10);
			var findWhatText = new TextBoxElement(view);
			findWhatText.SetPlaceholder(Resources.FindAndReplaceElementFindWhat);
			findWhatText.OnKeyDown += OnFindWhatTextKeyDownHandler;
			findWhatTextCell.AddItem(findWhatText);

			// Find Menu
			var findWhatMenuCell = findWhatRow.AddCell(2);
			var findWhatMenu = new ToolBarElement(view);
			findWhatMenuCell.AddItem(findWhatMenu);

			// Find Previous
			var findPreviousButton = new ToolBarButtonItem(view);
			findPreviousButton.SetImage("Actions/FindPrevious_16x16");
			findPreviousButton.SetToolTip(Resources.FindAndReplaceElementFindPreviousButtonToolTip);
			findPreviousButton.SetHotkey("Shift+F3");
			findPreviousButton.OnClick += OnFindPreviousHandler;
			findWhatMenu.AddItem(findPreviousButton);

			// Find Next
			var findNextButton = new ToolBarButtonItem(view);
			findNextButton.SetImage("Actions/FindNext_16x16");
			findNextButton.SetToolTip(Resources.FindAndReplaceElementFindNextButtonToolTip);
			findNextButton.SetHotkey("F3");
			findNextButton.OnClick += OnFindNextHandler;
			findWhatMenu.AddItem(findNextButton);

			// Replace With
			var replaceWithTextCell = replaceWithRow.AddCell(10);
			var replaceWithText = new TextBoxElement(view);
			replaceWithText.SetPlaceholder(Resources.FindAndReplaceElementReplaceWith);
			replaceWithTextCell.AddItem(replaceWithText);

			// Replace Menu
			var replaceWithMenuCell = replaceWithRow.AddCell(2);
			var replaceWithMenu = new ToolBarElement(view);
			replaceWithMenuCell.AddItem(replaceWithMenu);

			// Replace
			var replaceButton = new ToolBarButtonItem(view);
			replaceButton.SetImage("Actions/Replace_16x16");
			replaceButton.SetToolTip(Resources.FindAndReplaceElementReplaceButtonToolTip);
			replaceButton.SetHotkey("Alt+R");
			replaceButton.OnClick += OnReplaceHandler;
			replaceWithMenu.AddItem(replaceButton);

			// Replace All
			var replaceAllButton = new ToolBarButtonItem(view);
			replaceAllButton.SetImage("Actions/ReplaceAll_16x16");
			replaceAllButton.SetToolTip(Resources.FindAndReplaceElementReplaceAllButtonToolTip);
			replaceAllButton.SetHotkey("Alt+A");
			replaceAllButton.OnClick += OnReplaceAllHandler;
			replaceWithMenu.AddItem(replaceAllButton);

			// Match Case
			var matchCaseCell = settingsRow.AddCell(2);
			var matchCaseCheck = new CheckBoxElement(view);
			matchCaseCheck.SetText(Resources.FindAndReplaceElementMatchCase);
			matchCaseCheck.SetHorizontalAlignment(ElementHorizontalAlignment.Left);
			matchCaseCell.AddItem(matchCaseCheck);

			// Whole Word
			var wholeWordCell = settingsRow.AddCell(2);
			var wholeWordCheck = new CheckBoxElement(view);
			wholeWordCheck.SetText(Resources.FindAndReplaceElementWholeWord);
			wholeWordCheck.SetHorizontalAlignment(ElementHorizontalAlignment.Left);
			wholeWordCell.AddItem(wholeWordCheck);

			_setReplaceMode = replaceMode =>
							  {
								  replaceWithText.SetVisible(replaceMode);
								  replaceButton.SetVisible(replaceMode);
								  replaceAllButton.SetVisible(replaceMode);
							  };

			_findWhatText = findWhatText;
			_replaceWithText = replaceWithText;
			_matchCaseCheck = matchCaseCheck;
			_wholeWordCheck = wholeWordCheck;

			Control.Content = mainPanel.GetControl();
		}


		// ReplaceMode

		private bool _replaceMode;
		private readonly Action<bool> _setReplaceMode;

		/// <summary>
		/// Возвращает значение, определяющее, находится ли элемет в режиме замены.
		/// </summary>
		public bool GetReplaceMode()
		{
			return _replaceMode;
		}

		/// <summary>
		/// Устанавливает значение, определяющее, находится ли элемет в режиме замены.
		/// </summary>
		public void SetReplaceMode(bool value)
		{
			_replaceMode = value;
			_setReplaceMode(value);
		}


		// FindWhat

		private readonly TextBoxElement _findWhatText;

		/// <summary>
		/// Возвращает искомый текст.
		/// </summary>
		public string GetFindWhat()
		{
			return (string)_findWhatText.GetValue();
		}

		/// <summary>
		/// Устанавливает искомый текст.
		/// </summary>
		public void SetFindWhat(string value)
		{
			_findWhatText.SetValue(value);
		}


		// ReplaceWith

		private readonly TextBoxElement _replaceWithText;

		/// <summary>
		/// Возвращает текст для замены.
		/// </summary>
		public string GetReplaceWith()
		{
			return (string)_replaceWithText.GetValue();
		}

		/// <summary>
		/// Устанавливает текст для замены.
		/// </summary>
		public void SetReplaceWith(string value)
		{
			_replaceWithText.SetValue(value);
		}


		// MatchCase

		private readonly CheckBoxElement _matchCaseCheck;

		/// <summary>
		/// Возвращает значение, определяющее, нужно ли производить поиск с учетом регистра.
		/// </summary>
		public bool GetMatchCase()
		{
			return Equals(_matchCaseCheck.GetValue(), true);
		}

		/// <summary>
		/// Устанавливает значение, определяющее, нужно ли производить поиск с учетом регистра.
		/// </summary>
		public void SetMatchCase(bool value)
		{
			_matchCaseCheck.SetValue(value);
		}


		// WholeWord

		private readonly CheckBoxElement _wholeWordCheck;

		/// <summary>
		/// Возвращает значение, определяющее, нужно ли производить поиск слова целиком с учетом регистра.
		/// </summary>
		public bool GetWholeWord()
		{
			return Equals(_wholeWordCheck.GetValue(), true);
		}

		/// <summary>
		/// Устанавливает значение, определяющее, нужно ли производить поиск слова целиком с учетом регистра.
		/// </summary>
		public void SetWholeWord(bool value)
		{
			_wholeWordCheck.SetValue(value);
		}


		// Events

		/// <summary>
		/// Возвращает или устанавливает обработчик события поиска предыдущего совпадения.
		/// </summary>
		public ScriptDelegate OnFindPrevious { get; set; }

		/// <summary>
		/// Возвращает или устанавливает обработчик события поиска следующего совпадения.
		/// </summary>
		public ScriptDelegate OnFindNext { get; set; }

		/// <summary>
		/// Возвращает или устанавливает обработчик события замены текущего совпадения.
		/// </summary>
		public ScriptDelegate OnReplace { get; set; }

		/// <summary>
		/// Возвращает или устанавливает обработчик события замены всех совпадений.
		/// </summary>
		public ScriptDelegate OnReplaceAll { get; set; }


		// Handlers

		private void OnFindWhatTextKeyDownHandler(object context, dynamic arguments)
		{
			if (arguments.Key == Key.Enter)
			{
				OnFindNextHandler(context, arguments);
			}
		}

		private void OnFindPreviousHandler(object context, object arguments)
		{
			InvokeFindAndReplaceEvent(OnFindPrevious);
		}

		private void OnFindNextHandler(dynamic context, dynamic arguments)
		{
			InvokeFindAndReplaceEvent(OnFindNext);
		}

		private void OnReplaceHandler(object context, object arguments)
		{
			InvokeFindAndReplaceEvent(OnReplace);
		}

		private void OnReplaceAllHandler(dynamic context, dynamic arguments)
		{
			InvokeFindAndReplaceEvent(OnReplaceAll);
		}

		private void InvokeFindAndReplaceEvent(ScriptDelegate eventDelegate)
		{
			this.InvokeScript(eventDelegate, a =>
											 {
												 a.FindWhat = GetFindWhat();
												 a.ReplaceWith = GetReplaceWith();
												 a.MatchCase = GetMatchCase();
												 a.WholeWord = GetWholeWord();
											 });
		}


		// Methods

		/// <summary>
		/// Находит и выделяет предыдущее совпадение.
		/// </summary>
		public void FindPrevious(ITextEditorElement editor)
		{
			var text = editor.GetText();
			var findWhat = GetFindWhat();

			if (!string.IsNullOrEmpty(text) && !string.IsNullOrEmpty(findWhat))
			{
				var matchCase = GetMatchCase();
				var wholeWord = GetWholeWord();

				var index = text.FindPreviousIndexOf(findWhat, editor.GetSelectionStart(), matchCase, wholeWord);

				if (index < 0)
				{
					index = text.FindPreviousIndexOf(findWhat, text.Length, matchCase, wholeWord);
				}

				if (index >= 0)
				{
					editor.SelectText(index, findWhat.Length);
				}
			}
		}

		/// <summary>
		/// Находит и выделяет следующее совпадение.
		/// </summary>
		public void FindNext(ITextEditorElement editor)
		{
			var text = editor.GetText();
			var findWhat = GetFindWhat();

			if (!string.IsNullOrEmpty(text) && !string.IsNullOrEmpty(findWhat))
			{
				var matchCase = GetMatchCase();
				var wholeWord = GetWholeWord();

				var index = text.FindNextIndexOf(findWhat, editor.GetCaretOffset(), matchCase, wholeWord);

				if (index < 0)
				{
					index = text.FindNextIndexOf(findWhat, 0, matchCase, wholeWord);
				}

				if (index >= 0)
				{
					editor.SelectText(index, findWhat.Length);
				}
			}
		}

		/// <summary>
		/// Заменяет найденное совпадение.
		/// </summary>
		public void Replace(ITextEditorElement editor)
		{
			var text = editor.GetText();
			var findWhat = GetFindWhat();

			if (!string.IsNullOrEmpty(text) && !string.IsNullOrEmpty(findWhat))
			{
				var selectedText = editor.GetSelectedText();

				// Если выделен какой-либо текст
				if (!string.IsNullOrEmpty(selectedText))
				{
					var matchCase = GetMatchCase();
					var wholeWord = GetWholeWord();

					// Если выделена фраза для замены
					if (findWhat.Length == selectedText.Length
						&& selectedText.FindNextIndexOf(findWhat, 0, matchCase, wholeWord) == 0)
					{
						var replaceWith = GetReplaceWith();
						editor.InsertText(replaceWith);
					}
				}

				FindNext(editor);
			}
		}

		/// <summary>
		/// Заменяет все найденные совпадения.
		/// </summary>
		public void ReplaceAll(ITextEditorElement editor)
		{
			var text = editor.GetText();
			var findWhat = GetFindWhat();

			if (!string.IsNullOrEmpty(text) && !string.IsNullOrEmpty(findWhat))
			{
				var matchCase = GetMatchCase();
				var wholeWord = GetWholeWord();
				var replaceWith = GetReplaceWith();

				text = text.Replace(findWhat, replaceWith, matchCase, wholeWord);

				editor.SetText(text);
			}
		}

		public override void Focus()
		{
			_findWhatText.Focus();
		}
	}
}