using InfinniPlatform.UserInterface.ViewBuilders.Elements;
using InfinniPlatform.UserInterface.ViewBuilders.Views;

namespace InfinniPlatform.UserInterface.ViewBuilders.Designers.ConfigDeployDesigner
{
    /// <summary>
    ///     Элемент представления для развертывания конфигурации.
    /// </summary>
    internal sealed class ConfigDeployDesignerElement : BaseElement<ConfigDeployDesignerControl>
    {
        // Value

        private object _value;

        public ConfigDeployDesignerElement(View view)
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