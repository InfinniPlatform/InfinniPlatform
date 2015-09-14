using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ICSharpCode.AvalonEdit.Folding;
using ICSharpCode.AvalonEdit.Highlighting;

namespace InfinniPlatform.UserInterface.ViewBuilders.DataElements.CodeEditor
{
    /// <summary>
    ///     Элемент управления для редактора программного кода.
    /// </summary>
    sealed partial class CodeEditorControl : UserControl
    {
        // Helpers

        private bool _updatingTextOrValue;
        private readonly DelayManager _delayManager;
        private readonly FoldingManager _foldingManager;

        public CodeEditorControl()
        {
            InitializeComponent();

            _foldingManager = FoldingManager.Install(CodeEditor.TextArea);
            _delayManager = new DelayManager(1000, OnValidateTextHandler);

            CurrentLineBackgroundRenderer.AddRendererFor(CodeEditor);
            BracketHighlightRenderer.AddRendererFor(CodeEditor);
        }

        /// <summary>
        ///     Возвращает или устанавливает элемент для отображения меню.
        /// </summary>
        public UIElement MenuBar
        {
            get { return (UIElement) GetValue(MenuBarProperty); }
            set { SetValue(MenuBarProperty, value); }
        }

        /// <summary>
        ///     Возвращает или устанавливает элемент для отображения немодального диалога.
        /// </summary>
        public UIElement Dialog
        {
            get { return (UIElement) GetValue(DialogProperty); }
            set { SetValue(DialogProperty, value); }
        }

        // Text

        /// <summary>
        ///     Возвращает или устанавливает отображаемый текст.
        /// </summary>
        public string Text
        {
            get { return CodeEditor.Text; }
            set { CodeEditor.Text = value; }
        }

        /// <summary>
        ///     Возвращает или устанавливает значение.
        /// </summary>
        public object EditValue
        {
            get { return GetValue(EditValueProperty); }
            set { SetValue(EditValueProperty, value); }
        }

        /// <summary>
        ///     Возвращает или устанавливает способ форматирования.
        /// </summary>
        public IObjectFormatter Formatter
        {
            get { return (IObjectFormatter) GetValue(FormatterProperty); }
            set { SetValue(FormatterProperty, value); }
        }

        /// <summary>
        ///     Возвращает или устанавливает синтаксис языка программирования.
        /// </summary>
        public string Syntax
        {
            get { return (string) GetValue(SyntaxProperty); }
            set { SetValue(SyntaxProperty, value); }
        }

        /// <summary>
        ///     Возвращает или устанавливает сообщение об ошибке.
        /// </summary>
        public IEnumerable<CodeEditorError> ErrorMessages
        {
            get { return (IEnumerable<CodeEditorError>) GetValue(ErrorMessagesProperty); }
            set { SetValue(ErrorMessagesProperty, value); }
        }

        // Properties

        /// <summary>
        ///     Возвращает или устанавливает позицию корретки.
        /// </summary>
        public int CaretOffset
        {
            get { return CodeEditor.CaretOffset; }
            set
            {
                if (value >= 0 && value < CodeEditor.Text.Length)
                {
                    var location = CodeEditor.Document.GetLocation(value);
                    CodeEditor.ScrollTo(location.Line, location.Column);
                    CodeEditor.CaretOffset = value;
                }
            }
        }

        /// <summary>
        ///     Возвращает позицию начала выделения.
        /// </summary>
        public int SelectionStart
        {
            get { return CodeEditor.SelectionStart; }
        }

        /// <summary>
        ///     Возвращает выделенный текст.
        /// </summary>
        public string SelectedText
        {
            get { return CodeEditor.SelectedText; }
        }

        // Handlers

        private void OnCodeEditorTextChanged(object sender, EventArgs e)
        {
            // Обновление маркеров сворачивания текста
            UpdateFoldings();

            if (!_updatingTextOrValue)
            {
                // Отложенный вызов анализа ошибок в тексте
                _delayManager.Delay();
            }
        }

        private static void OnMenuBarPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var editor = d as CodeEditorControl;

            if (editor != null)
            {
                var menuBar = (UIElement) e.NewValue;
                editor.MenuBarPlaceHolder.Child = menuBar;
                editor.MenuBarPlaceHolder.Visibility = (menuBar != null) ? Visibility.Visible : Visibility.Hidden;
            }
        }

        private static void OnDialogPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var editor = d as CodeEditorControl;

            if (editor != null)
            {
                var dialog = (UIElement) e.NewValue;
                editor.DialogPlaceHolderContent.Child = dialog;
            }
        }

        /// <summary>
        ///     Отображает немодальный диалог.
        /// </summary>
        public void ShowDialog()
        {
            DialogPlaceHolder.Visibility = Visibility.Visible;
        }

        /// <summary>
        ///     Закрывает немодальный диалог.
        /// </summary>
        public void CloseDialog()
        {
            DialogPlaceHolder.Visibility = Visibility.Collapsed;
            CodeEditor.Focus();
        }

        private void OnCloseDialogHandler(object sender, RoutedEventArgs e)
        {
            CloseDialog();
        }

        private static object OnEditValuePropertyChanging(DependencyObject d, object newValue)
        {
            var editor = d as CodeEditorControl;

            if (editor != null)
            {
                var oldValue = editor.EditValue;

                if (oldValue == newValue)
                {
                    editor.OnUpdateValueHandler(oldValue);
                }
            }

            return newValue;
        }

        private static void OnEditValuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var editor = d as CodeEditorControl;

            if (editor != null)
            {
                editor.OnUpdateValueHandler(e.NewValue);
                editor.InvokeEditValueChanged(e.OldValue, e.NewValue);
            }
        }

        private static void OnFormatterPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var editor = d as CodeEditorControl;

            if (editor != null)
            {
                editor.OnUpdateValueHandler(editor.EditValue);
            }
        }

        private static void OnSyntaxPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var editor = d as CodeEditorControl;

            if (editor != null)
            {
                var value = e.NewValue as string;

                IHighlightingDefinition syntax = null;

                if (string.IsNullOrEmpty(value) == false)
                {
                    try
                    {
                        syntax = HighlightingManager.Instance.GetDefinition(value);
                    }
                    catch
                    {
                    }
                }

                editor.CodeEditor.SyntaxHighlighting = syntax;
                editor.UpdateFoldings();
            }
        }

        private static void OnErrorMessagePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var editor = d as CodeEditorControl;

            if (editor != null)
            {
                var errorMessages =
                    ((e.NewValue as IEnumerable<CodeEditorError>) ?? Enumerable.Empty<CodeEditorError>())
                        .OrderByDescending(i => i.Category)
                        .ToArray();

                editor.ErrorMessagesList.ItemsSource = errorMessages;
                editor.ErrorMessagesGroup.Visibility = (errorMessages.Length > 0)
                    ? Visibility.Visible
                    : Visibility.Collapsed;
            }
        }

        private void OnErrorMessageClick(object sender, MouseButtonEventArgs e)
        {
            var error = ErrorMessagesList.SelectedItem as CodeEditorError;

            if (error != null)
            {
                try
                {
                    CaretOffset = CodeEditor.Document.GetOffset(error.LineNumber, error.LineColumn);
                    CodeEditor.Focus();
                }
                catch
                {
                }
            }
        }

        /// <summary>
        ///     Событие окончания изменения значения.
        /// </summary>
        public event ValueChangedRoutedEventHandler OnEditValueChanged
        {
            add { AddHandler(OnEditValueChangedEvent, value); }
            remove { RemoveHandler(OnEditValueChangedEvent, value); }
        }

        private void InvokeEditValueChanged(object oldValue, object newValue)
        {
            var valueChangedArgs = new ValueChangedRoutedEventArgs(OnEditValueChangedEvent)
            {
                OldValue = oldValue,
                NewValue = newValue
            };

            RaiseEvent(valueChangedArgs);
        }

        // Methods

        /// <summary>
        ///     Отформатировать текст.
        /// </summary>
        public void Format()
        {
            if (Validate())
            {
                var oldCaretOffset = CaretOffset;
                OnUpdateValueHandler(EditValue);
                CaretOffset = oldCaretOffset;
            }
        }

        /// <summary>
        ///     Отменить последнее действие.
        /// </summary>
        public void Undo()
        {
            if (CodeEditor.CanUndo)
            {
                CodeEditor.Undo();
            }
        }

        /// <summary>
        ///     Повторить последнее действие.
        /// </summary>
        public void Redo()
        {
            if (CodeEditor.CanRedo)
            {
                CodeEditor.Redo();
            }
        }

        /// <summary>
        ///     Вырезать выделенный текст в буфер обмена.
        /// </summary>
        public void Cut()
        {
            CodeEditor.Cut();
        }

        /// <summary>
        ///     Скопировать выделенный текст в буфер обмена.
        /// </summary>
        public void Copy()
        {
            CodeEditor.Copy();
        }

        /// <summary>
        ///     Вставить текст из буфера обмена.
        /// </summary>
        public void Paste()
        {
            CodeEditor.Paste();
        }

        /// <summary>
        ///     Выделяет указанную часть текста.
        /// </summary>
        public void SelectText(int start, int length)
        {
            var textLength = CodeEditor.Text.Length;

            if (start >= 0 && start < textLength && length > 0)
            {
                if (start + length > textLength)
                {
                    length = textLength - start;
                }

                var location = CodeEditor.Document.GetLocation(start);
                CodeEditor.ScrollTo(location.Line, location.Column);
                CodeEditor.Select(start, length);
            }
        }

        /// <summary>
        ///     Вставляет указанный текст в текущую позицию.
        /// </summary>
        public void InsertText(string text)
        {
            if (CodeEditor.SelectionLength > 0)
            {
                CodeEditor.Document.Replace(CodeEditor.SelectionStart, CodeEditor.SelectionLength, text);
            }
            else
            {
                CodeEditor.Document.Insert(CodeEditor.CaretOffset, text);
            }
        }

        /// <summary>
        ///     Вызывает проверку состояния элемента.
        /// </summary>
        public bool Validate()
        {
            OnValidateTextHandler();

            return HasNoErrors(ErrorMessages);
        }

        private void OnUpdateTextHandler()
        {
            if (!_updatingTextOrValue)
            {
                _updatingTextOrValue = true;

                try
                {
                    // Новый текст
                    var newText = Text;

                    // Новое значение
                    IEnumerable<CodeEditorError> errorMessages = null;
                    var newValue = (Formatter != null)
                        ? Formatter.ConvertFromString(newText, out errorMessages)
                        : newText;

                    ErrorMessages = errorMessages;

                    // Если ошибок форматирования нет, обновление значения
                    if (HasNoErrors(errorMessages))
                    {
                        EditValue = newValue;
                    }
                }
                finally
                {
                    _updatingTextOrValue = false;
                }
            }
        }

        private void OnUpdateValueHandler(object newValue)
        {
            if (!_updatingTextOrValue)
            {
                _updatingTextOrValue = true;

                try
                {
                    // Новый текст
                    IEnumerable<CodeEditorError> errorMessages = null;
                    var newText = (Formatter != null)
                        ? Formatter.ConvertToString(newValue, out errorMessages)
                        : ((newValue != null) ? newValue.ToString() : null);

                    ErrorMessages = errorMessages;

                    // Если ошибок форматирования нет, обновление текста
                    if (HasNoErrors(errorMessages))
                    {
                        Text = newText;
                    }
                }
                finally
                {
                    _updatingTextOrValue = false;
                }
            }
        }

        private void OnValidateTextHandler()
        {
            OnUpdateTextHandler();
        }

        private static bool HasNoErrors(IEnumerable<CodeEditorError> errorMessages)
        {
            return (errorMessages == null) || errorMessages.All(i => i.Category != CodeEditorErrorCategory.Error);
        }

        private void UpdateFoldings()
        {
            FoldingStrategy.UpdateFoldings(_foldingManager, CodeEditor.Document);
        }

        // MenuBar

        public static readonly DependencyProperty MenuBarProperty = DependencyProperty.Register("MenuBar",
            typeof (UIElement), typeof (CodeEditorControl),
            new FrameworkPropertyMetadata(null, OnMenuBarPropertyChanged));

        // Dialog

        public static readonly DependencyProperty DialogProperty = DependencyProperty.Register("Dialog",
            typeof (UIElement), typeof (CodeEditorControl), new FrameworkPropertyMetadata(null, OnDialogPropertyChanged));

        // EditValue

        public static readonly DependencyProperty EditValueProperty = DependencyProperty.Register("EditValue",
            typeof (object), typeof (CodeEditorControl),
            new FrameworkPropertyMetadata(null, OnEditValuePropertyChanged, OnEditValuePropertyChanging));

        // Formatter

        public static readonly DependencyProperty FormatterProperty = DependencyProperty.Register("Formatter",
            typeof (IObjectFormatter), typeof (CodeEditorControl),
            new FrameworkPropertyMetadata(null, OnFormatterPropertyChanged));

        // Syntax

        public static readonly DependencyProperty SyntaxProperty = DependencyProperty.Register("Syntax", typeof (string),
            typeof (CodeEditorControl), new FrameworkPropertyMetadata(null, OnSyntaxPropertyChanged));

        // ErrorMessages

        public static readonly DependencyProperty ErrorMessagesProperty = DependencyProperty.Register("ErrorMessages",
            typeof (IEnumerable<CodeEditorError>), typeof (CodeEditorControl),
            new FrameworkPropertyMetadata(null, OnErrorMessagePropertyChanged));

        // OnEditValueChanged

        public static readonly RoutedEvent OnEditValueChangedEvent =
            EventManager.RegisterRoutedEvent("OnEditValueChanged", RoutingStrategy.Bubble,
                typeof (ValueChangedRoutedEventHandler), typeof (CodeEditorControl));

        private static readonly BraceFoldingStrategy FoldingStrategy
            = new BraceFoldingStrategy();
    }
}