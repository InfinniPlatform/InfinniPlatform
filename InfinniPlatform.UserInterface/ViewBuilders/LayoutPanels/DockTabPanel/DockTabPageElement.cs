﻿using System.Collections.Generic;
using System.Windows;

using DevExpress.Xpf.Docking;
using DevExpress.Xpf.Docking.Base;

using InfinniPlatform.UserInterface.ViewBuilders.Elements;
using InfinniPlatform.UserInterface.ViewBuilders.Images;
using InfinniPlatform.UserInterface.ViewBuilders.Scripts;
using InfinniPlatform.UserInterface.ViewBuilders.Views;

namespace InfinniPlatform.UserInterface.ViewBuilders.LayoutPanels.DockTabPanel
{
	public sealed class DockTabPageElement : BaseElement<DocumentPanel>, ITabPage
	{
		public DockTabPageElement(View view)
			: base(view)
		{
			SetCanClose(false);

			Control.GotFocus += (s, e) => view.InvokeScript(view.OnGotFocus);
			Control.LostFocus += (s, e) => view.InvokeScript(view.OnLostFocus);
		}


		// Parent

		private ITabPanel _parent;

		/// <summary>
		/// Возвращает родительскую панель закладок.
		/// </summary>
		public ITabPanel GetParent()
		{
			return _parent;
		}

		/// <summary>
		/// Устанавливает родительскую панель закладок.
		/// </summary>
		public void SetParent(ITabPanel value)
		{
			if (!Equals(_parent, value))
			{
				var tabControl = _parent.GetControl<DockTabPanelControl>();

				if (tabControl != null)
				{
					tabControl.PageClosing -= OnTabPageClosing;
				}

				if (value != null)
				{
					tabControl = value.GetControl<DockTabPanelControl>();

					if (tabControl != null)
					{
						tabControl.PageClosing += OnTabPageClosing;
					}
				}

				_parent = value;
			}
		}

		private void OnTabPageClosing(object sender, ItemCancelEventArgs e)
		{
			if (!e.Cancel)
			{
				var parent = GetParent();

				if (parent != null)
				{
					var tabControl = parent.GetControl<DockTabPanelControl>();

					if (tabControl != null && Equals(e.Item, Control))
					{
						Close();

						e.Cancel = true;
					}
				}
			}
		}


		// Text

		public override void SetText(string value)
		{
			base.SetText(value);
			Control.InvokeControl(() => Control.Caption = value);
		}


		// ToolTip

		public override void SetToolTip(string value)
		{
			base.SetToolTip(value);

			// Подсказка хранится в DataContext
			Control.DataContext = value;

			// Это из-за перекрытия стандартного ToolTip в DocumentPanel
			Control.ToolTip = null;
			((FrameworkElement)Control).ToolTip = null;
		}


		// Image

		private string _image;

		/// <summary>
		/// Возвращает изображение заголовка страницы.
		/// </summary>
		public string GetImage()
		{
			return _image;
		}

		/// <summary>
		/// Устанавливает изображение заголовка страницы.
		/// </summary>
		public void SetImage(string value)
		{
			_image = value;

			Control.CaptionImage = ImageRepository.GetImage(value);
		}


		// CanClose

		private bool _canClose;

		/// <summary>
		/// Возвращает значение, определяющее, разрешено ли закрытие страницы.
		/// </summary>
		public bool GetCanClose()
		{
			return _canClose;
		}

		/// <summary>
		/// Устанавливает значение, определяющее, разрешено ли закрытие страницы.
		/// </summary>
		public void SetCanClose(bool value)
		{
			_canClose = value;

			Control.AllowHide = value;
		}


		// LayoutPanel

		private ILayoutPanel _layoutPanel;

		/// <summary>
		/// Возвращает контейнер элементов страницы.
		/// </summary>
		public ILayoutPanel GetLayoutPanel()
		{
			return _layoutPanel;
		}

		/// <summary>
		/// Устанавливает контейнер элементов страницы.
		/// </summary>
		public void SetLayoutPanel(ILayoutPanel layoutPanel)
		{
			_layoutPanel = layoutPanel;

			Control.Content = (layoutPanel != null) ? layoutPanel.GetControl() : null;
		}


		// Close

		/// <summary>
		/// Закрывает страницу.
		/// </summary>
		public bool Close(bool force = false)
		{
			if (GetCanClose())
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

				var parent = GetParent();

				if (parent != null)
				{
					parent.RemovePage(this);
				}

				if (OnClosed != null)
				{
					this.InvokeScript(OnClosed);
				}

				return true;
			}

			return false;
		}


		// Events

		/// <summary>
		/// Возвращает или устанавливает обработчик события о том, что представление получает фокус.
		/// </summary>
		public ScriptDelegate OnGotFocus { get; set; }

		/// <summary>
		/// Возвращает или устанавливает обработчик события о том, что представление теряет фокус.
		/// </summary>
		public ScriptDelegate OnLostFocus { get; set; }

		/// <summary>
		/// Возвращает или устанавливает обработчик события о том, что страница закрывается.
		/// </summary>
		public ScriptDelegate OnClosing { get; set; }

		/// <summary>
		/// Возвращает или устанавливает обработчик события о том, что страница закрыта.
		/// </summary>
		public ScriptDelegate OnClosed { get; set; }


		// Elements

		public override IEnumerable<IElement> GetChildElements()
		{
			var layoutPanel = GetLayoutPanel();

			if (layoutPanel != null)
			{
				return new[] { layoutPanel };
			}

			return null;
		}

		public override void Focus()
		{
			var parent = GetParent();

			if (parent != null)
			{
				var selectedPage = parent.GetSelectedPage();

				if (selectedPage != this)
				{
					if (OnGotFocus != null)
					{
						this.InvokeScript(OnGotFocus);
					}

					parent.SetSelectedPage(this);
				}
			}
		}
	}
}