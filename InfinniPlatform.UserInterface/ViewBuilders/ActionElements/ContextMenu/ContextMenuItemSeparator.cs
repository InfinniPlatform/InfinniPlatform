using System.Windows.Controls;
using InfinniPlatform.UserInterface.ViewBuilders.Views;

namespace InfinniPlatform.UserInterface.ViewBuilders.ActionElements.ContextMenu
{
    /// <summary>
    ///     Разделитель элементов контекстного меню.
    /// </summary>
    public sealed class ContextMenuItemSeparator : ContextMenuItemBase<Separator>
    {
        public ContextMenuItemSeparator(View view)
            : base(view)
        {
        }
    }
}