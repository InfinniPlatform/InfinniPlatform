using System.Windows;
using DevExpress.Xpf.Core;
using InfinniPlatform.UserInterface.ViewBuilders.Elements;

namespace InfinniPlatform.UserInterface.ViewBuilders.Dialogs
{
    internal static class DialogExtensions
    {
        public static void ShowDialog(this IElement target, string title)
        {
            if (target != null)
            {
                var dialogWindow = new DXWindow
                {
                    Title = title ?? string.Empty,
                    Content = target.GetControl(),
                    Width = 600,
                    Height = 300,
                    ShowIcon = false,
                    ShowInTaskbar = true,
                    WindowState = WindowState.Normal,
                    WindowStyle = WindowStyle.ToolWindow,
                    WindowStartupLocation = WindowStartupLocation.CenterScreen
                };

                dialogWindow.ShowDialog();
            }
        }
    }
}