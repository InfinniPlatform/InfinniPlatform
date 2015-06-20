using InfinniPlatform.UserInterface.ViewBuilders.Designers.ConfigTree.Controls;
using InfinniPlatform.UserInterface.ViewBuilders.Elements;
using InfinniPlatform.UserInterface.ViewBuilders.Views;

namespace InfinniPlatform.UserInterface.ViewBuilders.Designers.ConfigTree.Elements
{
    /// <summary>
    ///     Элемент представления для отображения и редактирования конфигураций.
    /// </summary>
    public sealed class ConfigEditorElement : BaseElement<ConfigEditorControl>
    {
        // EditPanel

        private IConfigElementEditPanel _editPanel;

        public ConfigEditorElement(View view)
            : base(view)
        {
        }

        public IConfigElementEditPanel GetEditPanel()
        {
            return _editPanel;
        }

        public void SetEditPanel(IConfigElementEditPanel value)
        {
            _editPanel = value;

            Control.EditPanel = value;
        }
    }
}