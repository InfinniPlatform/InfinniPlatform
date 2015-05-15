﻿using System;
using System.Windows.Controls;

namespace InfinniPlatform.UserInterface.ViewBuilders.Designers.ScenarioDesigner
{
	/// <summary>
	/// Элемент управления для редактирования сценариев.
	/// </summary>
	sealed partial class ScenarioDesignerControl : UserControl
	{
		public ScenarioDesignerControl()
		{
			InitializeComponent();

			Designer.OnValueChanged += OnValueChangedHandler;
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