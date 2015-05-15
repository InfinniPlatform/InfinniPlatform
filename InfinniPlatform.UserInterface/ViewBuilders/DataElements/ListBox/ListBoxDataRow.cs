using System.ComponentModel;

namespace InfinniPlatform.UserInterface.ViewBuilders.DataElements.ListBox
{
	/// <summary>
	/// Строка списка.
	/// </summary>
	sealed class ListBoxDataRow : INotifyPropertyChanged
	{
		private object _item;

		/// <summary>
		/// Ссылка на элемент.
		/// </summary>
		public object Item
		{
			get
			{
				return _item;
			}
			set
			{
				if (!Equals(_item, value))
				{
					_item = value;

					InvokePropertyChanged("Item");
				}
			}
		}


		private bool _checked;

		/// <summary>
		/// Пометка элемента.
		/// </summary>
		public bool Checked
		{
			get
			{
				return _checked;
			}
			set
			{
				if (!Equals(_checked, value))
				{
					_checked = value;

					InvokePropertyChanged("Checked");
				}
			}
		}


		private object _value;

		/// <summary>
		/// Значение элемента.
		/// </summary>
		public object Value
		{
			get
			{
				return _value;
			}
			set
			{
				if (!Equals(_value, value))
				{
					_value = value;

					InvokePropertyChanged("Value");
				}
			}
		}


		private object _display;

		/// <summary>
		/// Отображение элемента.
		/// </summary>
		public object Display
		{
			get
			{
				return _display;
			}
			set
			{
				if (!Equals(_display, value))
				{
					_display = value;

					InvokePropertyChanged("Display");
				}
			}
		}


		public event PropertyChangedEventHandler PropertyChanged;

		private void InvokePropertyChanged(string propertyName)
		{
			if (PropertyChanged != null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}


		public override int GetHashCode()
		{
			return (Item != null) ? Item.GetHashCode() : 0;
		}

		public override bool Equals(object obj)
		{
			var other = obj as ListBoxDataRow;

			return (other != null && Equals(other.Item, Item));
		}
	}
}