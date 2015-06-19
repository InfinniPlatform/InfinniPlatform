using System.Collections.Generic;
using System.Windows;
using InfinniPlatform.UserInterface.ViewBuilders.Elements;
using InfinniPlatform.UserInterface.ViewBuilders.Views;

namespace InfinniPlatform.UserInterface.ViewBuilders.ActionElements.ContextMenu
{
    public abstract class ContextMenuItemBase<TItem> : IContextMenuItem where TItem : FrameworkElement, new()
    {
        // Enabled

        private bool _enabled;
        // HorizontalAlignment

        private ElementHorizontalAlignment _horizontalAlignment;
        // Hotkey

        private string _hotkey;
        // Name

        private string _name;
        // Text

        private string _text;
        // ToolTip

        private string _toolTip;
        // VerticalAlignment

        private ElementVerticalAlignment _verticalAlignment;
        // View

        private View _view;
        // Visible

        private bool _visible;
        protected readonly TItem Control;

        protected ContextMenuItemBase(View view)
        {
            Control = new TItem {Tag = this};

            // Установка значений по умолчанию

            SetView(view);
            SetEnabled(true);
            SetVisible(true);
        }

        public View GetView()
        {
            return _view;
        }

        public string GetName()
        {
            return _name;
        }

        public void SetName(string value)
        {
            _name = value;

            Control.Name = value ?? string.Empty;
        }

        public virtual string GetText()
        {
            return _text;
        }

        public virtual void SetText(string value)
        {
            _text = value;
        }

        public virtual string GetToolTip()
        {
            return _toolTip;
        }

        public virtual void SetToolTip(string value)
        {
            _toolTip = value;
        }

        public bool GetEnabled()
        {
            return _enabled;
        }

        public void SetEnabled(bool value)
        {
            _enabled = value;

            Control.IsEnabled = value;
        }

        public bool GetVisible()
        {
            return _visible;
        }

        public void SetVisible(bool value)
        {
            _visible = value;

            Control.Visibility = value ? Visibility.Visible : Visibility.Collapsed;
        }

        public ElementVerticalAlignment GetVerticalAlignment()
        {
            return _verticalAlignment;
        }

        public void SetVerticalAlignment(ElementVerticalAlignment value)
        {
            _verticalAlignment = value;
        }

        public ElementHorizontalAlignment GetHorizontalAlignment()
        {
            return _horizontalAlignment;
        }

        public void SetHorizontalAlignment(ElementHorizontalAlignment value)
        {
            _horizontalAlignment = value;
        }

        // Elements

        public virtual IEnumerable<IElement> GetChildElements()
        {
            return null;
        }

        public virtual bool Validate()
        {
            return true;
        }

        public virtual void Focus()
        {
            Control.Focus();
        }

        // Control

        public object GetControl()
        {
            return Control;
        }

        private void SetView(View value)
        {
            _view = value;
        }

        public virtual string GetHotkey()
        {
            return _hotkey;
        }

        public virtual void SetHotkey(string value)
        {
            _hotkey = value;
        }
    }
}