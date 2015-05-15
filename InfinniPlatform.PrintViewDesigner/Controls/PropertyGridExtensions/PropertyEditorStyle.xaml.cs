using System;
using System.Windows;

using DevExpress.Xpf.Editors;

using InfinniPlatform.PrintViewDesigner.Controls.PropertyGrid;

namespace InfinniPlatform.PrintViewDesigner.Controls.PropertyGridExtensions
{
	public partial class PropertyEditorStyle : PropertyEditorBase
	{
		public PropertyEditorStyle()
		{
			InitializeComponent();
		}


		// PrintViewSource

		public static readonly DependencyProperty PrintViewSourceProperty = DependencyProperty.Register("PrintViewSource", typeof(Func<object>), typeof(PropertyEditorStyle));

		/// <summary>
		/// Печатное представление.
		/// </summary>
		public Func<object> PrintViewSource
		{
			get { return (Func<object>)GetValue(PrintViewSourceProperty); }
			set { SetValue(PrintViewSourceProperty, value); }
		}


		private void OnIsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			if (Equals(e.NewValue, true))
			{
				RefreshItems();
			}
		}

		private void OnPopupOpening(object sender, OpenPopupEventArgs e)
		{
			RefreshItems();
		}

		private void RefreshItems()
		{
			object styles = null;

			if (PrintViewSource != null)
			{
				dynamic printView = PrintViewSource();

				if (printView != null)
				{
					styles = printView.Styles;
				}
			}

			Editor.ItemsSource = styles;
		}
	}
}