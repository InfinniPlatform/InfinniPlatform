using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

using DevExpress.Xpf.Core.Native;

using InfinniPlatform.UserInterface.ViewBuilders.Scripts;
using InfinniPlatform.UserInterface.ViewBuilders.Views;

namespace InfinniPlatform.UserInterface.ViewBuilders.Elements
{
	/// <summary>
	/// Базовый класс элемента представления.
	/// </summary>
	/// <typeparam name="TControl">Реализация элемента представления.</typeparam>
	public abstract class BaseElement<TControl> : IElement where TControl : FrameworkElement, new()
	{
		protected BaseElement(View view)
		{
			Control = new TControl();
			Control.Loaded += (s, e) => this.InvokeScript(OnLoaded);

			// Установка значений по умолчанию

			SetView(view);
			SetEnabled(true);
			SetVisible(true);
			SetVerticalAlignment(ElementVerticalAlignment.Stretch);
			SetHorizontalAlignment(ElementHorizontalAlignment.Stretch);
		}


		protected readonly TControl Control;


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

			Control.ToolTip = value;
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

			Control.SetVisible(value);
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

			switch (value)
			{
				case ElementVerticalAlignment.Top:
					Control.VerticalAlignment = VerticalAlignment.Top;
					break;
				case ElementVerticalAlignment.Center:
					Control.VerticalAlignment = VerticalAlignment.Center;
					break;
				case ElementVerticalAlignment.Bottom:
					Control.VerticalAlignment = VerticalAlignment.Bottom;
					break;
				case ElementVerticalAlignment.Stretch:
					Control.VerticalAlignment = VerticalAlignment.Stretch;
					break;
			}
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

			switch (value)
			{
				case ElementHorizontalAlignment.Left:
					Control.HorizontalAlignment = HorizontalAlignment.Left;
					break;
				case ElementHorizontalAlignment.Center:
					Control.HorizontalAlignment = HorizontalAlignment.Center;
					break;
				case ElementHorizontalAlignment.Right:
					Control.HorizontalAlignment = HorizontalAlignment.Right;
					break;
				case ElementHorizontalAlignment.Stretch:
					Control.HorizontalAlignment = HorizontalAlignment.Stretch;
					break;
			}
		}


		// Events

		/// <summary>
		/// Возвращает или устанавливает обработчик события о том, что элемент загружен.
		/// </summary>
		public ScriptDelegate OnLoaded { get; set; }


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
			if (!Equals(Keyboard.Focus(Control), Control) && (Control.Focusable && Control.IsEnabled))
			{
				var focusScope = FocusManager.GetFocusScope(Control);

				if (focusScope != null)
				{
					FocusManager.SetFocusedElement(focusScope, Control);
				}
			}

			Control.Focus();
		}


		// Control

		public object GetControl()
		{
			return Control;
		}
	}
}