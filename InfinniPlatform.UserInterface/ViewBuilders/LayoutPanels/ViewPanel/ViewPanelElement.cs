using System;
using System.Windows;
using System.Windows.Controls;
using InfinniPlatform.UserInterface.ViewBuilders.Elements;
using InfinniPlatform.UserInterface.ViewBuilders.Scripts;
using InfinniPlatform.UserInterface.ViewBuilders.Views;

namespace InfinniPlatform.UserInterface.ViewBuilders.LayoutPanels.ViewPanel
{
    /// <summary>
    ///     Контейнер элементов представления в виде прямоугольной области, в которую помещается указанное представление.
    /// </summary>
    public sealed class ViewPanelElement : BaseElement<Grid>, ILayoutPanel
    {
        // ContentView

        private View _contentView;
        private readonly Func<View> _contentViewFactory;

        public ViewPanelElement(View parentView, Func<View> contentViewFactory)
            : base(parentView)
        {
            _contentViewFactory = contentViewFactory;

            Control.Loaded += OnLoadedViewPanel;
        }

        // Events

        /// <summary>
        ///     Возвращает или устанавливает обработчик события о том, что представление открывается.
        /// </summary>
        public ScriptDelegate OnOpening { get; set; }

        /// <summary>
        ///     Возвращает или устанавливает обработчик события о том, что представление открыто.
        /// </summary>
        public ScriptDelegate OnOpened { get; set; }

        /// <summary>
        ///     Возвращает или устанавливает обработчик события о том, что представление закрывается.
        /// </summary>
        public ScriptDelegate OnClosing { get; set; }

        /// <summary>
        ///     Возвращает или устанавливает обработчик события о том, что представление закрыто.
        /// </summary>
        public ScriptDelegate OnClosed { get; set; }

        private void OnLoadedViewPanel(object sender, RoutedEventArgs e)
        {
            var contentView = GetContentView();

            if (contentView != null)
            {
                var viewControl = contentView.GetControl<FrameworkElement>();

                if (viewControl != null && viewControl.Parent == null)
                {
                    Control.Children.Add(viewControl);
                }

                contentView.Open();
            }
        }

        public View GetContentView()
        {
            if (_contentView == null)
            {
                _contentView = _contentViewFactory();
            }

            return _contentView;
        }

        // Open

        /// <summary>
        ///     Открывает представление.
        /// </summary>
        public void Open()
        {
            if (OnOpening != null)
            {
                this.InvokeScript(OnOpening);
            }

            if (OnOpened != null)
            {
                this.InvokeScript(OnOpened);
            }
        }

        // Close

        /// <summary>
        ///     Закрывает представление.
        /// </summary>
        public bool Close(bool force = false)
        {
            if (OnClosing != null)
            {
                dynamic arguments = null;

                this.InvokeScript(OnClosing, a =>
                {
                    a.Force = force;
                    arguments = a;
                });

                if (!force && arguments != null && arguments.IsCancel == true)
                {
                    return false;
                }
            }

            if (OnClosed != null)
            {
                this.InvokeScript(OnClosed);
            }

            return true;
        }
    }
}