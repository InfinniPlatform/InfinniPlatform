using System.Collections.Generic;
using System.Windows;

using InfinniPlatform.UserInterface.ViewBuilders.Elements;
using InfinniPlatform.UserInterface.ViewBuilders.Views;

namespace InfinniPlatform.UserInterface.ViewBuilders.ActionElements.ContextMenu
{
	public abstract class ContextMenuItemBase<TItem> : IContextMenuItem where TItem : FrameworkElement, new()
	{
		protected ContextMenuItemBase(View view)
		{
			Control = new TItem { Tag = this };

			// Установка значений по умолчанию

			SetView(view);
			SetEnabled(true);
			SetVisible(true);
		}


		protected readonly TItem Control;


		// View

		private View _view;

		public View GetView()
		{
			return _view;
		}

		private void SetView(View value)
		{
			_view = value;
		}


		// Name

		private string _name;

		public string GetName()
		{
			return _name;
		}

		public void SetName(string value)
		{
			_name = value;

			Control.Name = value ?? string.Empty;
		}


		// Text

		private string _text;

		public virtual string GetText()
		{
			return _text;
		}

		public virtual void SetText(string value)
		{
			_text = value;
		}


		// ToolTip

		private string _toolTip;

		public virtual string GetToolTip()
		{
			return _toolTip;
		}

		public virtual void SetToolTip(string value)
		{
			_toolTip = value;
		}


		// Enabled

		private bool _enabled;

		public bool GetEnabled()
		{
			return _enabled;
		}

		public void SetEnabled(bool value)
		{
			_enabled = value;

			Control.IsEnabled = value;
		}


		// Visible

		private bool _visible;

		public bool GetVisible()
		{
			return _visible;
		}

		public void SetVisible(bool value)
		{
			_visible = value;

			Control.Visibility = value ? Visibility.Visible : Visibility.Collapsed;
		}


		// VerticalAlignment

		private ElementVerticalAlignment _verticalAlignment;

		public ElementVerticalAlignment GetVerticalAlignment()
		{
			return _verticalAlignment;
		}

		public void SetVerticalAlignment(ElementVerticalAlignment value)
		{
			_verticalAlignment = value;
		}


		// HorizontalAlignment

		private ElementHorizontalAlignment _horizontalAlignment;

		public ElementHorizontalAlignment GetHorizontalAlignment()
		{
			return _horizontalAlignment;
		}

		public void SetHorizontalAlignment(ElementHorizontalAlignment value)
		{
			_horizontalAlignment = value;
		}


		// Hotkey

		private string _hotkey;

		public virtual string GetHotkey()
		{
			return _hotkey;
		}

		public virtual void SetHotkey(string value)
		{
			_hotkey = value;
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
	}
}