using System;
using InfinniPlatform.UserInterface.ViewBuilders.Elements;
using InfinniPlatform.UserInterface.ViewBuilders.Scripts;
using InfinniPlatform.UserInterface.ViewBuilders.Views;

namespace InfinniPlatform.UserInterface.ViewBuilders.Designers.ReportDesigner
{
    /// <summary>
    ///     Элемент представления для редактирования отчетов.
    /// </summary>
    internal sealed class ReportDesignerElement : BaseElement<ReportDesignerControl>
    {
        public ReportDesignerElement(View view)
            : base(view)
        {
            Control.EditValueChanged += OnEditValueChanged;
        }

        // Events

        /// <summary>
        ///     Возвращает или устанавливает обработчик события изменения значения.
        /// </summary>
        public ScriptDelegate OnValueChanged { get; set; }

        private void OnEditValueChanged(object sender, EventArgs eventArgs)
        {
            this.InvokeScript(OnValueChanged, arguments => arguments.Value = GetValue());
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
            Control.InvokeControl(() => Control.EditValue = value);
        }
    }
}