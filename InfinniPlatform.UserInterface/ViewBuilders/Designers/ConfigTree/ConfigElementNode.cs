using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

using InfinniPlatform.UserInterface.ViewBuilders.Commands;

namespace InfinniPlatform.UserInterface.ViewBuilders.Designers.ConfigTree
{
	public sealed class ConfigElementNode : INotifyPropertyChanged
	{
		public ConfigElementNode(ConfigElementNode parent, string elementType, dynamic elementMetadata)
		{
			ElementType = elementType;
			ElementMetadata = elementMetadata;

			Key = this;
			Parent = parent;
			Nodes = new List<ConfigElementNode>();
		}

		/// <summary>
		/// Загружено ли содержимое узла.
		/// </summary>
		public bool IsLoaded { get; set; }

		public string ConfigId { get; set; }

		public string DocumentId { get; set; }

        public string Version { get; set; }

		public string ElementId { get; set; }

		public string ElementType { get; set; }

		private string _elementName;

		public string ElementName
		{
			get
			{
				return _elementName;
			}
			set
			{
				if (!Equals(_elementName, value))
				{
					_elementName = value;

					OnPropertyChanged("ElementName");
				}
			}
		}

		public dynamic ElementMetadata { get; set; }
		public string[] ElementChildrenTypes { get; set; }

		public ConfigElementNode Key { get; private set; }
		public ConfigElementNode Parent { get; private set; }
		public List<ConfigElementNode> Nodes { get; private set; }


		/// <summary>
		/// Команда обновления узла.
		/// </summary>
		public ICommand<bool> RefreshCommand { get; set; }

		/// <summary>
		/// Команды добавления узла.
		/// </summary>
		public IEnumerable<ICommand<object>> AddCommands { get; set; }

		/// <summary>
		/// Команды редактирования узла.
		/// </summary>
		public IEnumerable<ICommand<object>> EditCommands { get; set; }

		/// <summary>
		/// Команда удаления узла.
		/// </summary>
		public ICommand<object> DeleteCommand { get; set; }

		/// <summary>
		/// Команда копирования узла.
		/// </summary>
		public ICommand<object> CopyCommand { get; set; }

		/// <summary>
		/// Команда вставки узла.
		/// </summary>
		public ICommand<object> PasteCommand { get; set; }


		// INotifyPropertyChanged

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChanged(string propertyName)
		{
			var handler = PropertyChanged;

			if (handler != null)
			{
				handler(this, new PropertyChangedEventArgs(propertyName));
			}
		}


		public override string ToString()
		{
			return ElementName;
		}

		public string GetNodePath()
		{
			var path = new StringBuilder();

			var parent = this;

			while (parent != null)
			{
				path.Insert(0, '/').Insert(0, parent.ElementName);
				parent = parent.Parent;
			}

			return path.ToString();
		}
	}
}