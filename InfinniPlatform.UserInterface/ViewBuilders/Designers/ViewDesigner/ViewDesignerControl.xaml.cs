using System;
using System.Windows;
using System.Windows.Controls;

namespace InfinniPlatform.UserInterface.ViewBuilders.Designers.ViewDesigner
{
	/// <summary>
	/// Элемент управления для редактирования представлений.
	/// </summary>
	sealed partial class ViewDesignerControl : UserControl
	{
		public ViewDesignerControl()
		{
			InitializeComponent();

			IsVisibleChanged += OnIsVisibleChangedHandler;

			Designer.OnValueChanged += OnValueChangedHandler;
		}

		private void OnIsVisibleChangedHandler(object sender, DependencyPropertyChangedEventArgs e)
		{
			Designer.IsVisible = (bool)e.NewValue;
		}

		private void OnValueChangedHandler(object sender, EventArgs e)
		{
			if (OnValueChanged != null)
			{
				OnValueChanged(sender, e);
			}
		}


		public Func<string> ConfigId
		{
			get { return Designer.ConfigId; }
			set { Designer.ConfigId = value; }
		}

		public Func<string> DocumentId
		{
			get { return Designer.DocumentId; }
			set { Designer.DocumentId = value; }
		}

		public object Value
		{
			get { return Designer.Value; }
			set { Designer.Value = value; }
		}


		public event EventHandler OnValueChanged;
	}
}