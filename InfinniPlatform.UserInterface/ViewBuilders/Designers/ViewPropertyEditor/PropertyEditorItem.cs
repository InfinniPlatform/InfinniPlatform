using System;
using System.ComponentModel;
using System.Windows.Media;

namespace InfinniPlatform.UserInterface.ViewBuilders.Designers.ViewPropertyEditor
{
	sealed class PropertyEditorItem : INotifyPropertyChanged
	{
		private PropertyEditor _editor;

		public PropertyEditor Editor
		{
			get
			{
				return _editor;
			}
			set
			{
				if (!Equals(_editor, value))
				{
					_editor = value;

					OnPropertyChanged("Editor");
				}
			}
		}


		private ImageSource _image;

		public ImageSource Image
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

					OnPropertyChanged("Image");
				}
			}
		}


		private string _name;

		public string Name
		{
			get
			{
				return _name;
			}
			set
			{
				if (!Equals(_name, value))
				{
					_name = value;

					OnPropertyChanged("Name");
				}
			}
		}


		private object _item;

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

					OnPropertyChanged("Item");
				}
			}
		}


		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChanged(string propertyName)
		{
			var handler = PropertyChanged;

			if (handler != null)
			{
				handler(this, new PropertyChangedEventArgs(propertyName));
			}
		}


		public PropertyEditorItem Clone()
		{
			var item = (Item is ICloneable) ? ((ICloneable)Item).Clone() : Item;

			return new PropertyEditorItem
				   {
					   Editor = Editor,
					   Image = Image,
					   Name = Name,
					   Item = item
				   };
		}


		public override string ToString()
		{
			return Name;
		}
	}
}