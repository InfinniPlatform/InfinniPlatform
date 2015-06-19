using System.Windows;
using InfinniPlatform.PrintViewDesigner.Controls.PrintViewDesigner;
using InfinniPlatform.UserInterface.ViewBuilders.Elements;
using InfinniPlatform.UserInterface.ViewBuilders.Scripts;
using InfinniPlatform.UserInterface.ViewBuilders.Views;

namespace InfinniPlatform.UserInterface.ViewBuilders.Designers.PrintViewDesigner
{
    /// <summary>
    ///     Элемент представления для редактирования печатного представления.
    /// </summary>
    internal sealed class PrintViewDesignerElement : BaseElement<PrintViewDesignerControl>
    {
        public PrintViewDesignerElement(View view)
            : base(view)
        {
            Control.PrintViewChanged += OnPrintViewChanged;
        }

        // Events

        /// <summary>
        ///     Возвращает или устанавливает обработчик события изменения печатного представления.
        /// </summary>
        public ScriptDelegate OnValueChanged { get; set; }

        // Value

        /// <summary>
        ///     Возвращает печатное представление.
        /// </summary>
        public object GetValue()
        {
            return Control.PrintView;
        }

        /// <summary>
        ///     Устанавливает печатное представление.
        /// </summary>
        public void SetValue(object value)
        {
            Control.PrintView = value;
        }

        private void OnPrintViewChanged(object sender, RoutedEventArgs e)
        {
            this.InvokeScript(OnValueChanged, arguments => arguments.Value = GetValue());
        }
    }
}