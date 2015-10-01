using InfinniPlatform.UserInterface.ViewBuilders.Elements;
using InfinniPlatform.UserInterface.ViewBuilders.Views;

namespace InfinniPlatform.UserInterface.ViewBuilders.Designers.ConfigVerifyDesigner
{
    /// <summary>
    ///     Элемент представления для проверки конфигурации.
    /// </summary>
    internal sealed class ConfigVerifyDesignerElement : BaseElement<ConfigVerifyDesignerControl>
    {
        // Value

        private object _value;

        public ConfigVerifyDesignerElement(View view)
            : base(view)
        {
        }

        /// <summary>
        ///     Возвращает значение.
        /// </summary>
        public object GetValue()
        {
            return _value;
        }

        /// <summary>
        ///     Возвращает значение.
        /// </summary>
        public void SetValue(object value)
        {
            _value = value;

            Control.InvokeControl(() => { Control.Value = value; });
        }
    }
}