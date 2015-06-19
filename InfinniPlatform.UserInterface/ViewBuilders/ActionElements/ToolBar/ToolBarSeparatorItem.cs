using DevExpress.Xpf.Bars;
using InfinniPlatform.UserInterface.ViewBuilders.Views;

namespace InfinniPlatform.UserInterface.ViewBuilders.ActionElements.ToolBar
{
    /// <summary>
    ///     Элемент панели инструментов в виде разделителя.
    /// </summary>
    public sealed class ToolBarSeparatorItem : ToolBarItem<BarItemSeparator>
    {
        public ToolBarSeparatorItem(View view)
            : base(view)
        {
        }
    }
}