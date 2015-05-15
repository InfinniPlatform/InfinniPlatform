using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

using InfinniPlatform.ReportDesigner.Properties;
using InfinniPlatform.ReportDesigner.Views.Editors;

namespace InfinniPlatform.ReportDesigner.Views.Preview
{
	/// <summary>
	/// Представление для выбора нескольких значений из списка.
	/// </summary>
	sealed partial class SelectMultipleValueView : UserControl
	{
		public SelectMultipleValueView()
			: this(null)
		{
		}

		public SelectMultipleValueView(EditorBase editor)
		{
			InitializeComponent();

			Text = Resources.SelectMultipleValueView;

			_editor = editor;

			if (editor != null)
			{
				InitializeEditor(editor);
				ControlPanel.Visible = true;
			}
			else
			{
				ControlPanel.Visible = false;
			}
		}

		private void InitializeEditor(Control editor)
		{
			EditorPanel.Controls.Add(editor);

			editor.Dock = DockStyle.Fill;

			IButtonControl acceptButton = null;

			editor.Enter += (s, e) =>
								{
									var parentForm = editor.FindForm();

									if (parentForm != null)
									{
										acceptButton = parentForm.AcceptButton;
										parentForm.AcceptButton = null;
									}
								};

			editor.Leave += (s, e) =>
								{
									var parentForm = editor.FindForm();

									if (parentForm != null)
									{
										parentForm.AcceptButton = acceptButton;
										acceptButton = null;
									}
								};

			editor.KeyDown += (s, e) =>
								  {
									  if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Return)
									  {
										  OnAdd(s, e);
									  }
								  };
		}


		private readonly EditorBase _editor;


		private IDictionary<string, object> _availableValues;

		/// <summary>
		/// Список значений.
		/// </summary>
		public IDictionary<string, object> AvailableValues
		{
			get
			{
				return _availableValues;
			}
			set
			{
				_availableValues = value;

				ItemsEdit.Items.Clear();

				if (value != null)
				{
					foreach (var item in value)
					{
						ItemsEdit.Items.Add(new ParameterValue(item.Value, item.Key));
					}
				}

				ResetSelection();
			}
		}


		/// <summary>
		/// Список выбранных значений.
		/// </summary>
		public IEnumerable SelectedValues
		{
			get
			{
				return ItemsEdit.CheckedItems.Cast<ParameterValue>().Select(i => i.Value).ToArray();
			}
			set
			{
				var index = 0;
				var checkedItems = (value != null) ? value.Cast<object>().ToList() : null;

				foreach (var item in ItemsEdit.Items.Cast<ParameterValue>().ToArray())
				{
					ItemsEdit.SetItemChecked(index++, checkedItems != null && checkedItems.Contains(item.Value));
				}

				ResetSelection();
			}
		}


		private void ResetSelection()
		{
			if (_editor != null)
			{
				_editor.Value = null;
			}

			ItemsEdit.SelectedIndex = -1;
		}

		private void OnAdd(object sender, EventArgs e)
		{
			var itemValue = _editor.Value;
			var itemLabel = _editor.Text;

			if (itemValue != null && ReferenceEquals(itemValue, string.Empty) == false)
			{
				ItemsEdit.Items.Add(new ParameterValue(itemValue, itemLabel));

				_editor.Value = null;
			}
		}

		private void OnDelete(object sender, EventArgs e)
		{
			var itemIndex = ItemsEdit.SelectedIndex;

			if (itemIndex >= 0)
			{
				ItemsEdit.Items.RemoveAt(itemIndex);

				if (itemIndex != 0)
				{
					ItemsEdit.SelectedIndex = itemIndex - 1;
				}
				else if (ItemsEdit.Items.Count > 0)
				{
					ItemsEdit.SelectedIndex = 0;
				}
			}
		}
	}
}