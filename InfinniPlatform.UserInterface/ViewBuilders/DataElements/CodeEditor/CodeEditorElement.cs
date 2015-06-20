using System.Windows;
using InfinniPlatform.UserInterface.Properties;
using InfinniPlatform.UserInterface.ViewBuilders.ActionElements.ToolBar;
using InfinniPlatform.UserInterface.ViewBuilders.DataElements.FindAndReplace;
using InfinniPlatform.UserInterface.ViewBuilders.Elements;
using InfinniPlatform.UserInterface.ViewBuilders.Scripts;
using InfinniPlatform.UserInterface.ViewBuilders.Views;

namespace InfinniPlatform.UserInterface.ViewBuilders.DataElements.CodeEditor
{
    /// <summary>
    ///     Элемент представления для редактора программного кода.
    /// </summary>
    public sealed class CodeEditorElement : BaseElement<CodeEditorControl>, ITextEditorElement
    {
        private readonly FindAndReplaceElement _findAndReplace;

        public CodeEditorElement(View view)
            : base(view)
        {
            // Main Menu
            var menuBar = new ToolBarElement(view);

            // Format
            var formatButton = new ToolBarButtonItem(view);
            formatButton.SetImage("Actions/FormatText_16x16");
            formatButton.SetToolTip(Resources.CodeEditorControlFormatButtonToolTip);
            formatButton.SetHotkey("Ctrl+Shift+F");
            formatButton.OnClick += OnFormatButtonClick;
            menuBar.AddItem(formatButton);

            // Separator0
            var separator0 = new ToolBarSeparatorItem(view);
            menuBar.AddItem(separator0);

            // Undo
            var undoButton = new ToolBarButtonItem(view);
            undoButton.SetImage("Actions/Undo_16x16");
            undoButton.SetToolTip(Resources.CodeEditorControlUndoButtonToolTip);
            undoButton.SetHotkey("Ctrl+Z");
            undoButton.OnClick += OnUndoButtonClick;
            menuBar.AddItem(undoButton);

            // Redo
            var redoButton = new ToolBarButtonItem(view);
            redoButton.SetImage("Actions/Redo_16x16");
            redoButton.SetToolTip(Resources.CodeEditorControlRedoButtonToolTip);
            redoButton.SetHotkey("Ctrl+Y");
            redoButton.OnClick += OnRedoButtonClick;
            menuBar.AddItem(redoButton);

            // Separator1
            var separator1 = new ToolBarSeparatorItem(view);
            menuBar.AddItem(separator1);

            // Cut
            var cutButton = new ToolBarButtonItem(view);
            cutButton.SetImage("Actions/Cut_16x16");
            cutButton.SetToolTip(Resources.CodeEditorControlCutButtonToolTip);
            cutButton.SetHotkey("Ctrl+X");
            cutButton.OnClick += OnCutButtonClick;
            menuBar.AddItem(cutButton);

            // Copy
            var copyButton = new ToolBarButtonItem(view);
            copyButton.SetImage("Actions/Copy_16x16");
            copyButton.SetToolTip(Resources.CodeEditorControlCopyButtonToolTip);
            copyButton.SetHotkey("Ctrl+C");
            copyButton.OnClick += OnCopyButtonClick;
            menuBar.AddItem(copyButton);

            // Paste
            var pasteButton = new ToolBarButtonItem(view);
            pasteButton.SetImage("Actions/Paste_16x16");
            pasteButton.SetToolTip(Resources.CodeEditorControlPasteButtonToolTip);
            pasteButton.SetHotkey("Ctrl+V");
            pasteButton.OnClick += OnPasteButtonClick;
            menuBar.AddItem(pasteButton);

            // Separator2
            var separator2 = new ToolBarSeparatorItem(view);
            menuBar.AddItem(separator2);

            // Find
            var findButton = new ToolBarButtonItem(view);
            findButton.SetImage("Actions/FindAndReplace_16x16");
            findButton.SetToolTip(Resources.CodeEditorControlFindButtonToolTip);
            findButton.SetHotkey("Ctrl+F");
            findButton.OnClick += OnFindButtonClick;
            menuBar.AddItem(findButton);

            // FindAndReplace
            var findAndReplace = new FindAndReplaceElement(view);
            findAndReplace.OnFindPrevious += OnFindPreviousClick;
            findAndReplace.OnFindNext += OnFindNextClick;
            findAndReplace.OnReplace += OnReplaceClick;
            findAndReplace.OnReplaceAll += OnReplaceAllClick;

            // Elements

            Control.MenuBar = menuBar.GetControl<UIElement>();
            Control.Dialog = findAndReplace.GetControl<UIElement>();
            Control.OnEditValueChanged += OnEditValueChangedHandler;

            _findAndReplace = findAndReplace;
        }

        // Events

        /// <summary>
        ///     Возвращает или устанавливает обработчик события изменения значения.
        /// </summary>
        public ScriptDelegate OnValueChanged { get; set; }

        // ITextEditorElement

        public override string GetText()
        {
            return Control.Text;
        }

        public override void SetText(string value)
        {
            Control.Text = value;
        }

        public int GetCaretOffset()
        {
            return Control.CaretOffset;
        }

        public void SetCaretOffset(int value)
        {
            Control.CaretOffset = value;
        }

        public int GetSelectionStart()
        {
            return Control.SelectionStart;
        }

        public string GetSelectedText()
        {
            return Control.SelectedText;
        }

        public void SelectText(int start, int length)
        {
            Control.SelectText(start, length);
        }

        public void InsertText(string text)
        {
            Control.InsertText(text);
        }

        // Handlers

        private void OnEditValueChangedHandler(object sender, ValueChangedRoutedEventArgs e)
        {
            this.InvokeScript(OnValueChanged, a => a.Value = e.NewValue);
        }

        private void OnFormatButtonClick(dynamic context, dynamic arguments)
        {
            Format();
        }

        private void OnUndoButtonClick(dynamic context, dynamic arguments)
        {
            Undo();
        }

        private void OnRedoButtonClick(dynamic context, dynamic arguments)
        {
            Redo();
        }

        private void OnCutButtonClick(dynamic context, dynamic arguments)
        {
            Cut();
        }

        private void OnCopyButtonClick(dynamic context, dynamic arguments)
        {
            Copy();
        }

        private void OnPasteButtonClick(dynamic context, dynamic arguments)
        {
            Paste();
        }

        private void OnFindButtonClick(dynamic context, dynamic arguments)
        {
            Control.ShowDialog();

            _findAndReplace.Focus();
        }

        private void OnFindPreviousClick(dynamic context, dynamic arguments)
        {
            _findAndReplace.FindPrevious(this);
        }

        private void OnFindNextClick(dynamic context, dynamic arguments)
        {
            _findAndReplace.FindNext(this);
        }

        private void OnReplaceClick(dynamic context, dynamic arguments)
        {
            _findAndReplace.Replace(this);
        }

        private void OnReplaceAllClick(dynamic context, dynamic arguments)
        {
            _findAndReplace.ReplaceAll(this);
        }

        // Value

        /// <summary>
        ///     Возвращает значение.
        /// </summary>
        public object GetValue()
        {
            return Control.EditValue;
        }

        /// <summary>
        ///     Устанавливает значение.
        /// </summary>
        public void SetValue(object value)
        {
            Control.EditValue = value;
        }

        // Formatter

        /// <summary>
        ///     Возвращает способ форматирования.
        /// </summary>
        public IObjectFormatter GetFormatter()
        {
            return Control.Formatter;
        }

        /// <summary>
        ///     Устанавливает способ форматирования.
        /// </summary>
        public void SetFormatter(IObjectFormatter value)
        {
            Control.Formatter = value;
        }

        // Syntax

        /// <summary>
        ///     Возвращает синтаксис.
        /// </summary>
        public string GetSyntax()
        {
            return Control.Syntax;
        }

        /// <summary>
        ///     Устанавливает синтаксис.
        /// </summary>
        public void SetSyntax(string value)
        {
            Control.Syntax = value;
        }

        // Methods

        /// <summary>
        ///     Отформатировать текст.
        /// </summary>
        public void Format()
        {
            Control.Format();
        }

        /// <summary>
        ///     Отменить последнее действие.
        /// </summary>
        public void Undo()
        {
            Control.Undo();
        }

        /// <summary>
        ///     Повторить последнее действие.
        /// </summary>
        public void Redo()
        {
            Control.Redo();
        }

        /// <summary>
        ///     Вырезать выделенный текст в буфер обмена.
        /// </summary>
        public void Cut()
        {
            Control.Cut();
        }

        /// <summary>
        ///     Скопировать выделенный текст в буфер обмена.
        /// </summary>
        public void Copy()
        {
            Control.Copy();
        }

        /// <summary>
        ///     Вставить текст из буфера обмена.
        /// </summary>
        public void Paste()
        {
            Control.Paste();
        }

        /// <summary>
        ///     Вызывает проверку состояния элемента.
        /// </summary>
        public override bool Validate()
        {
            return Control.Validate();
        }
    }
}