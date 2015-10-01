using System.Windows;
using InfinniPlatform.UserInterface.ViewBuilders.Elements;
using InfinniPlatform.UserInterface.ViewBuilders.Scripts;
using InfinniPlatform.UserInterface.ViewBuilders.Views;

namespace InfinniPlatform.UserInterface.ViewBuilders.DataElements.ToggleButton
{
    /// <summary>
    ///     Элемент представления для отображения и редактирования логического значения в виде переключателя.
    /// </summary>
    public sealed class ToggleButtonElement : BaseElement<ToggleButtonControl>
    {
        // ReadOnly

        private bool _readOnly;
        // TextOff

        private string _textOff;
        // TextOn

        private string _textOn;
        // Value

        private object _value;

        public ToggleButtonElement(View view)
            : base(view)
        {
            Control.OnEditValueChanged += OnEditValueChangedHandler;
        }

        // Events

        /// <summary>
        ///     Возвращает или устанавливает обработчик события изменения значения.
        /// </summary>
        public ScriptDelegate OnValueChanged { get; set; }

        private void OnEditValueChangedHandler(object sender, RoutedEventArgs routedEventArgs)
        {
            _value = Control.EditValue;

            this.InvokeScript(OnValueChanged, args => { args.Value = Control.EditValue; });
        }

        /// <summary>
        ///     Возвращает текст на включенное состояние.
        /// </summary>
        public string GetTextOn()
        {
            return _textOn;
        }

        /// <summary>
        ///     Устанавливает текст на включенное состояние.
        /// </summary>
        public void SetTextOn(string value)
        {
            _textOn = value;

            Control.TextOn = value;
        }

        /// <summary>
        ///     Возвращает текст на выключенное состояние.
        /// </summary>
        public string GetTextOff()
        {
            return _textOff;
        }

        /// <summary>
        ///     Устанавливает текст на выключенное состояние.
        /// </summary>
        public void SetTextOff(string value)
        {
            _textOff = value;

            Control.TextOff = value;
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

            Control.InvokeControl(() => { Control.EditValue = value; });
        }
    }
}