using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace InfinniPlatform.ReportDesigner.Views.Controls
{
	/// <summary>
	/// Выпадающий список с возможностью форматирования элементов.
	/// </summary>
	sealed partial class FormattedComboBox : UserControl
	{
		public FormattedComboBox()
		{
			InitializeComponent();
		}


		/// <summary>
		/// Выбранный элемент.
		/// </summary>
		[Browsable(false)]
		[DefaultValue(null)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public object SelectedItem
		{
			get
			{
				var selectedItemMetadata = ComboBoxEdit.SelectedItem as ItemMetadata;
				return (selectedItemMetadata != null) ? selectedItemMetadata.Value : null;
			}
			set
			{
				var selectedItemMetadata = ComboBoxEdit.Items.Cast<ItemMetadata>().FirstOrDefault(i => Equals(i.Value, value));
				ComboBoxEdit.SelectedItem = selectedItemMetadata;
			}
		}


		private IEnumerable<object> _items;

		/// <summary>
		/// Список элементов для выбора.
		/// </summary>
		[Browsable(false)]
		[DefaultValue(null)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public IEnumerable Items
		{
			get
			{
				return _items;
			}
			set
			{
				_items = (value != null) ? value.Cast<object>() : null;

				UpdateItems();
			}
		}


		private string _itemFormatString;

		/// <summary>
		/// Строка форматирования элемента.
		/// </summary>
		public string ItemFormatString
		{
			get
			{
				return _itemFormatString;
			}
			set
			{
				_itemFormatString = value;

				UpdateItems();
			}
		}


		/// <summary>
		/// Событие на выбор элемента.
		/// </summary>
		public event EventHandler SelectedIndexChanged;

		private void OnSelectedIndexChanged(object sender, EventArgs e)
		{
			if (SelectedIndexChanged != null)
			{
				SelectedIndexChanged(sender, e);
			}
		}


		private void UpdateItems()
		{
			var selectedItem = SelectedItem;

			ComboBoxEdit.Items.Clear();

			if (_items != null)
			{
				var propertyNames = GetPropertyNames(ItemFormatString);
				var formatString = GetFormatString(ItemFormatString, propertyNames);

				foreach (var item in _items)
				{
					ComboBoxEdit.Items.Add(new ItemMetadata
											   {
												   Value = item,
												   Name = FormatItemName(item, formatString, propertyNames)
											   });
				}

				SelectedItem = selectedItem;
			}
		}

		private static IList<string> GetPropertyNames(string itemFormatString)
		{
			var propertyNames = new List<string>();

			if (string.IsNullOrEmpty(itemFormatString) == false)
			{
				var propertyMatches = Regex.Matches(itemFormatString, @"\{.*?\}", RegexOptions.Compiled);

				foreach (Match propertyMatch in propertyMatches)
				{
					propertyNames.Add(propertyMatch.Value.Trim('{', '}'));
				}
			}

			return propertyNames;
		}

		private static string GetFormatString(string itemFormatString, IList<string> propertyNames)
		{
			var formatString = new StringBuilder(itemFormatString);

			if (propertyNames != null)
			{
				var propertyCount = propertyNames.Count;

				for (var i = 0; i < propertyCount; ++i)
				{
					formatString = formatString.Replace("{" + propertyNames[i] + "}", "{" + i + "}");
				}
			}

			return formatString.ToString();
		}

		private static string FormatItemName(object item, string formatString, IList<string> propertyNames)
		{
			string name = null;

			if (item != null)
			{
				if (string.IsNullOrEmpty(formatString) == false)
				{
					if (propertyNames != null)
					{
						var propertyCount = propertyNames.Count;
						var propertyValues = new object[propertyCount];

						for (var i = 0; i < propertyCount; ++i)
						{
							propertyValues[i] = GerPropertyValue(item, propertyNames[i]);
						}

						name = string.Format(formatString, propertyValues);
					}
					else
					{
						name = formatString;
					}
				}
				else
				{
					name = item.ToString();
				}
			}

			return name;
		}

		private static object GerPropertyValue(object item, string propertyPath)
		{
			var propertyValue = item;

			if (string.IsNullOrEmpty(propertyPath) == false)
			{
				var propertyNames = propertyPath.Split('.');

				foreach (var propertyName in propertyNames)
				{
					if (propertyValue != null)
					{
						var type = propertyValue.GetType();
						var property = type.GetProperty(propertyName);

						propertyValue = (property != null) ? property.GetValue(propertyValue) : null;
					}
					else
					{
						break;
					}
				}
			}

			return propertyValue;
		}


		class ItemMetadata
		{
			public object Value;

			public string Name;

			public override string ToString()
			{
				return Name;
			}
		}
	}
}