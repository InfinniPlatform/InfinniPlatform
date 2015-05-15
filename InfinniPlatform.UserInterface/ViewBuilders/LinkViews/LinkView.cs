using System;
using System.Windows;
using System.Windows.Input;

using DevExpress.Xpf.Core;

using InfinniPlatform.UserInterface.ViewBuilders.Images;
using InfinniPlatform.UserInterface.ViewBuilders.LayoutPanels;
using InfinniPlatform.UserInterface.ViewBuilders.Views;

namespace InfinniPlatform.UserInterface.ViewBuilders.LinkViews
{
	/// <summary>
	/// Ссылка на представление.
	/// </summary>
	public sealed class LinkView
	{
		public LinkView(View appView, View parentView, Func<View> viewFactory)
		{
			_appView = appView;
			_parentView = parentView;
			_viewFactory = viewFactory;
		}


		private readonly View _appView;
		private readonly View _parentView;
		private readonly Func<View> _viewFactory;


		// OpenMode

		private OpenMode _openMode = OpenMode.TabPage;

		/// <summary>
		/// Возвращает способ открытия представления.
		/// </summary>
		public OpenMode GetOpenMode()
		{
			return _openMode;
		}

		/// <summary>
		/// Устанавливает способ открытия представления.
		/// </summary>
		public void SetOpenMode(OpenMode value)
		{
			_openMode = value;
		}


		// View

		/// <summary>
		/// Создает представление.
		/// </summary>
		public View CreateView()
		{
			var view = _viewFactory();
			view.OnOpening += (c, a) => OpenView(view);

			return view;
		}

		private void OpenView(View view)
		{
			if (view != null)
			{
				ViewRegistry.OnOpeningView(view);

				switch (_openMode)
				{
					case OpenMode.None:
						OpenViewInNone(view, _parentView);
						break;
					case OpenMode.TabPage:
						OpenViewInTabPage(view, _parentView);
						break;
					case OpenMode.AppTabPage:
						OpenViewInTabPage(view, _appView);
						break;
					default:
						OpenViewInDialog(view, _parentView);
						break;
				}
			}
		}

		private static bool CloseView(View view)
		{
			if (view != null)
			{
				return ViewRegistry.OnClosingView(view);
			}

			return true;
		}

		private static void OpenViewInNone(View view, View parent)
		{
			var closingHandling = false;
			view.OnClosing += (c, a) => TryExecuteBlock(ref closingHandling, () => a.IsCancel = (a.IsCancel == true) || !CloseView(view));
		}

		private static void OpenViewInDialog(View view, View parent)
		{
			var container = new DXWindow
							{
								Width = 800,
								Height = 500,
								ShowInTaskbar = true,
								Title = view.GetText() ?? string.Empty,
								WindowStartupLocation = WindowStartupLocation.CenterScreen,
								Content = view.GetControl()
							};

			if (parent != null)
			{
				container.ShowIcon = false;
				container.WindowState = WindowState.Normal;
				container.WindowStyle = WindowStyle.ToolWindow;

				// Закрытие диалога по нажатию на Escape
				container.PreviewKeyDown += (s, e) =>
											{
												if (e.Key == Key.Escape)
												{
													container.Close();
												}
											};
			}
			else
			{
				container.ShowIcon = true;
				container.WindowState = WindowState.Maximized;
				container.WindowStyle = WindowStyle.SingleBorderWindow;
				container.Icon = ImageRepository.GetImage(view.GetImage());
			}

			var closingHandling = false;
			var closeHandling = false;
			view.OnClosing += (c, a) => TryExecuteBlock(ref closingHandling, () => a.IsCancel = (a.IsCancel == true) || !CloseView(view));
			view.OnClosed += (c, a) => TryExecuteBlock(ref closeHandling, container.Close);
			container.Closing += (c, a) => TryExecuteBlock(ref closeHandling, () => a.Cancel = a.Cancel || !view.Close());

			var gotFocusHandling = false;
			view.OnGotFocus += (c, a) => TryExecuteBlock(ref gotFocusHandling, () => container.Focus());
			container.GotFocus += (s, e) => TryExecuteBlock(ref gotFocusHandling, () => view.InvokeScript(view.OnGotFocus));

			var lostFocusHandling = false;
			container.LostFocus += (s, e) => TryExecuteBlock(ref lostFocusHandling, () => view.InvokeScript(view.OnLostFocus));

			view.OnTextChanged += (c, a) => container.Title = a.Value;

			container.ShowDialog();
		}

		private static void OpenViewInTabPage(View view, View parent)
		{
			if (parent != null)
			{
				var parentTabPanel = parent.GetLayoutPanel() as ITabPanel;

				if (parentTabPanel != null)
				{
					var container = parentTabPanel.CreatePage(view);
					container.SetCanClose(true);
					container.SetText(view.GetText());
					container.SetToolTip(view.GetToolTip());
					container.SetImage(view.GetImage());
					container.SetLayoutPanel(view);

					parentTabPanel.AddPage(container);
					parentTabPanel.SetSelectedPage(container);

					var closingHandling = false;
					var closeHandling = false;
					view.OnClosing += (c, a) => TryExecuteBlock(ref closingHandling, () => a.IsCancel = (a.IsCancel == true) || !CloseView(view));
					view.OnClosed += (c, a) => TryExecuteBlock(ref closeHandling, () => container.Close());
					container.OnClosing += (c, a) => TryExecuteBlock(ref closeHandling, () => a.IsCancel = (a.IsCancel == true) || !view.Close());

					var gotFocusHandling = false;
					view.OnGotFocus += (c, a) => TryExecuteBlock(ref gotFocusHandling, () => container.Focus());
					container.OnGotFocus += (s, e) => TryExecuteBlock(ref gotFocusHandling, () => view.InvokeScript(view.OnGotFocus));

					var lostFocusHandling = false;
					container.OnLostFocus += (s, e) => TryExecuteBlock(ref lostFocusHandling, () => view.InvokeScript(view.OnLostFocus));

					view.OnTextChanged += (c, a) => container.SetText(a.Value as string);

					return;
				}
			}

			OpenViewInDialog(view, parent);
		}

		private static void TryExecuteBlock(ref bool insideBlock, Action actionBlock)
		{
			if (!insideBlock)
			{
				// ReSharper disable RedundantAssignment
				insideBlock = true;
				// ReSharper restore RedundantAssignment

				try
				{
					actionBlock();
				}
				finally
				{
					insideBlock = false;
				}
			}
		}
	}
}