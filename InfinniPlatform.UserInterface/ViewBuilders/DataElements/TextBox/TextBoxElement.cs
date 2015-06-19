using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using DevExpress.Xpf.Editors;
using InfinniPlatform.UserInterface.ViewBuilders.Elements;
using InfinniPlatform.UserInterface.ViewBuilders.Scripts;
using InfinniPlatform.UserInterface.ViewBuilders.Views;
using DataFormat = InfinniPlatform.UserInterface.ViewBuilders.DisplayFormats.DataFormat;

namespace InfinniPlatform.UserInterface.ViewBuilders.DataElements.TextBox
{
    /// <summary>
    ///     Элемент представления для текстового поля.
    /// </summary>
    public sealed class TextBoxElement : BaseElement<TextEdit>
    {
        // Format

        private DataFormat _format;
        // HorizontalTextAlignment

        private HorizontalTextAlignment _horizontalTextAlignment;
        // LineCount

        private int _lineCount;
        // Multiline

        private bool _multiline;
        // Placeholder

        private string _placeholder;
        // ReadOnly

        private bool _readOnly;
        // Value

        private object _value;
        // VerticalTextAlignment

        private VerticalTextAlignment _verticalTextAlignment;

        public TextBoxElement(View view)
            : base(view)
        {
            Control.KeyDown += OnKeyDownHandler;
            Control.EditValueChanged += OnTextChangedHandler;
        }

        // Events

        /// <summary>
        ///     Возвращает или устанавливает обработчик события изменения значения.
        /// </summary>
        public ScriptDelegate OnValueChanged { get; set; }

        /// <summary>
        ///     Возвращает или устанавливает обработчик события нажатия на клавишу.
        /// </summary>
        public ScriptDelegate OnKeyDown { get; set; }

        private void OnKeyDownHandler(object sender, KeyEventArgs e)
        {
            this.InvokeScript(OnKeyDown, args => { args.Key = e.Key; });
        }

        private void OnTextChangedHandler(object sender, EditValueChangedEventArgs e)
        {
            _value = e.NewValue;

            this.InvokeScript(OnValueChanged, args => { args.Value = e.NewValue; });
        }

        // Text

        public override void SetText(string value)
        {
            base.SetText(value);

            Control.InvokeControl(() => { Control.Text = value; });
        }

        /// <summary>
        ///     Возвращает отображаемый в случае пустого значения текст.
        /// </summary>
        public string GetPlaceholder()
        {
            return _placeholder;
        }

        /// <summary>
        ///     Устанавливает отображаемый в случае пустого значения текст.
        /// </summary>
        public void SetPlaceholder(string value)
        {
            _placeholder = value;

            Control.NullText = value;
        }

        /// <summary>
        ///     Возвращает формат отображения данных.
        /// </summary>
        public DataFormat GetFormat()
        {
            return _format;
        }

        /// <summary>
        ///     Устанавливает формат отображения данных.
        /// </summary>
        public void SetFormat(DataFormat value)
        {
            _format = value;

            UpdateText();
        }

        /// <summary>
        ///     Возвращает значение.
        /// </summary>
        public object GetValue()
        {
            return _value;
        }

        /// <summary>
        ///     Устанавливает значение.
        /// </summary>
        public void SetValue(object value)
        {
            _value = value;

            UpdateText();
        }

        private void UpdateText()
        {
            string text = null;

            var value = _value;

            if (value != null)
            {
                var format = _format;

                text = (format != null)
                    ? format.Format(value)
                    : value.ToString();
            }

            SetText(text);
        }

        /// <summary>
        ///     Возвращает горизонтальное выравнивание текста внутри элемента.
        /// </summary>
        public HorizontalTextAlignment GetHorizontalTextAlignment()
        {
            return _horizontalTextAlignment;
        }

        /// <summary>
        ///     Устанавливает горизонтальное выравнивание текста внутри элемента.
        /// </summary>
        public void SetHorizontalTextAlignment(HorizontalTextAlignment value)
        {
            if (_horizontalTextAlignment != value)
            {
                _horizontalTextAlignment = value;

                switch (value)
                {
                    case HorizontalTextAlignment.Left:
                        Control.HorizontalContentAlignment = HorizontalAlignment.Left;
                        break;
                    case HorizontalTextAlignment.Right:
                        Control.HorizontalContentAlignment = HorizontalAlignment.Right;
                        break;
                    case HorizontalTextAlignment.Center:
                        Control.HorizontalContentAlignment = HorizontalAlignment.Center;
                        break;
                    default:
                        Control.HorizontalContentAlignment = HorizontalAlignment.Left;
                        break;
                }
            }
        }

        /// <summary>
        ///     Возвращает вертикальное выравнивание текста внутри элемента.
        /// </summary>
        public VerticalTextAlignment GetVerticalTextAlignment()
        {
            return _verticalTextAlignment;
        }

        /// <summary>
        ///     Устанавливает вертикальное выравнивание текста внутри элемента.
        /// </summary>
        public void SetVerticalTextAlignment(VerticalTextAlignment value)
        {
            if (_verticalTextAlignment != value)
            {
                _verticalTextAlignment = value;

                switch (value)
                {
                    case VerticalTextAlignment.Top:
                        Control.VerticalContentAlignment = VerticalAlignment.Top;
                        break;
                    case VerticalTextAlignment.Center:
                        Control.VerticalContentAlignment = VerticalAlignment.Center;
                        break;
                    case VerticalTextAlignment.Bottom:
                        Control.VerticalContentAlignment = VerticalAlignment.Bottom;
                        break;
                    default:
                        Control.VerticalContentAlignment = VerticalAlignment.Center;
                        break;
                }
            }
        }

        /// <summary>
        ///     Возвращает значение, определяющее, запрещено ли редактирование значения.
        /// </summary>
        public bool GetReadOnly()
        {
            return _readOnly;
        }

        /// <summary>
        ///     Устанавливает значение, определяющее, запрещено ли редактирование значения.
        /// </summary>
        public void SetReadOnly(bool value)
        {
            if (_readOnly != value)
            {
                _readOnly = value;

                Control.IsReadOnly = value;
            }
        }

        /// <summary>
        ///     Возвращает значение, определяющее, разрешен ли многострочный текст.
        /// </summary>
        public bool GetMultiline()
        {
            return _multiline;
        }

        /// <summary>
        ///     Устанавливает значение, определяющее, разрешен ли многострочный текст.
        /// </summary>
        public void SetMultiline(bool value)
        {
            if (_multiline != value)
            {
                _multiline = value;

                if (value)
                {
                    Control.AcceptsReturn = true;
                    Control.VerticalContentAlignment = VerticalAlignment.Top;
                    Control.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
                    Control.HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;
                }
                else
                {
                    Control.AcceptsReturn = false;
                    Control.VerticalContentAlignment = VerticalAlignment.Center;
                    Control.VerticalScrollBarVisibility = ScrollBarVisibility.Hidden;
                    Control.HorizontalScrollBarVisibility = ScrollBarVisibility.Hidden;
                }
            }
        }

        /// <summary>
        ///     Возвращает видимое количество строк.
        /// </summary>
        public int GetLineCount()
        {
            return _lineCount;
        }

        /// <summary>
        ///     Устанавливает видимое количество строк.
        /// </summary>
        public void SetLineCount(int value)
        {
            var correct = Math.Max(value, 0);

            if (_lineCount != correct)
            {
                _lineCount = correct;

                Control.Height = (correct > 0)
                    ? correct*Math.Ceiling(Control.FontSize*Control.FontFamily.LineSpacing*96/72)
                    : double.NaN;
            }
        }
    }
}