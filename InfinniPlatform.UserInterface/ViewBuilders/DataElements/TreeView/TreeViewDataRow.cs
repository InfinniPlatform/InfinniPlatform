using System.ComponentModel;

namespace InfinniPlatform.UserInterface.ViewBuilders.DataElements.TreeView
{
	/// <summary>
	/// Строка древовидного списка.
	/// </summary>
	sealed class TreeViewDataRow : INotifyPropertyChanged
	{
		private object _id;

		/// <summary>
		/// Уникальный идентификатор элемента.
		/// </summary>
		public object Id
		{
			get
			{
				return _id;
			}
			set
			{
				if (!Equals(_id, value))
				{
					_id = value;

					InvokePropertyChanged("Id");
				}
			}
		}


		private object _key;

		/// <summary>
		/// Идентификатор элемента.
		/// </summary>
		public object Key
		{
			get
			{
				return _key;
			}
			set
			{
				if (!Equals(_key, value))
				{
					_key = value;

					InvokePropertyChanged("Key");
				}
			}
		}


		private object _parent;

		/// <summary>
		/// Идентификатор родителя.
		/// </summary>
		public object Parent
		{
			get
			{
				return _parent;
			}
			set
			{
				if (!Equals(_parent, value))
				{
					_parent = value;

					InvokePropertyChanged("Parent");
				}
			}
		}


		private object _image;

		/// <summary>
		/// Изображение элемента.
		/// </summary>
		public object Image
		{
			get
			{
				return _image;
			}
			set
			{
				if (!Equals(_image, value))
				{
					_image = value;

					InvokePropertyChanged("Image");
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
		/// Наименование элемента.
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


		public override int GetHashCode()
		{
			return (Id != null) ? Id.GetHashCode() : 0;
		}

		public override bool Equals(object obj)
		{
			var other = obj as TreeViewDataRow;

			return (other != null && Equals(other.Id, Id));
		}


		public event PropertyChangedEventHandler PropertyChanged;

		private void InvokePropertyChanged(string property)
		{
			var handler = PropertyChanged;

			if (handler != null)
			{
				handler(this, new PropertyChangedEventArgs(property));
			}
		}
	}
}