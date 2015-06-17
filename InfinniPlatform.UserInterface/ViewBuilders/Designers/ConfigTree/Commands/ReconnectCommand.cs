using System;

using InfinniPlatform.Api.Dynamic;
using InfinniPlatform.Api.Hosting;

namespace InfinniPlatform.UserInterface.ViewBuilders.Designers.ConfigTree.Commands
{
	sealed class ReconnectCommand : CommandBase<object>
	{
		private readonly ConfigElementNodeBuilder _builder;
		private readonly ConfigElementNode _elementNode;
		private readonly string _elementEditor;

		public ReconnectCommand(ConfigElementNodeBuilder builder, ConfigElementNode elementNode, string elementEditor)
		{
			_builder = builder;
			_elementNode = elementNode;
			_elementEditor = elementEditor;
		}

		public override bool CanExecute(object parameter)
		{
			return true;
		}

		public override void Execute(object parameter)
		{
			dynamic connectionSettings = new DynamicWrapper();
			connectionSettings.ServerScheme = HostingConfig.Default.ServerScheme;
			connectionSettings.ServerName = HostingConfig.Default.ServerName;
			connectionSettings.ServerPort = HostingConfig.Default.ServerPort;

			_builder.EditPanel.EditElement(_elementEditor,
										   _elementNode.GetNodePath(),
										   _elementNode.ConfigId,
										   _elementNode.DocumentId, 
                                           _elementNode.Version,
										   _elementNode.ElementType,
										   _elementNode.ElementId,
										   (object)connectionSettings,
										   () => Reconnect(connectionSettings));
		}

		private void Reconnect(dynamic connectionSettings)
		{
			// Текущая строка подключения
			var oldConnectionString = HostingConfig.Default.ToString();

			// Изменение настроек подключения

			var serverScheme = CastToString(connectionSettings.ServerScheme, Uri.UriSchemeHttp);

			if (!string.Equals(serverScheme, Uri.UriSchemeHttp, StringComparison.OrdinalIgnoreCase)
				&& !string.Equals(serverScheme, Uri.UriSchemeHttps, StringComparison.OrdinalIgnoreCase))
			{
				serverScheme = Uri.UriSchemeHttp;
			}

			HostingConfig.Default.ServerScheme = serverScheme;
			HostingConfig.Default.ServerName = CastToString(connectionSettings.ServerName, HostingConfig.Default.DefaultServerName);
			HostingConfig.Default.ServerPort = CastToInt(connectionSettings.ServerPort, HostingConfig.Default.DefaultServerPort);

			// Получение новой строки подключения
			var newConnectionString = HostingConfig.Default.ToString();

			// Если настройки подключения изменились
			if (!string.Equals(oldConnectionString, newConnectionString, StringComparison.OrdinalIgnoreCase))
			{
				// Закрытие всех редакторов
				_builder.EditPanel.CloseAll();

				// Обновление дерева элементов
				_elementNode.ElementName = newConnectionString;
				_elementNode.RefreshCommand.TryExecute(true);
			}
		}

		private static string CastToString(object value, string defaultResult)
		{
			string result = null;

			if (value != null)
			{
				try
				{
					result = Convert.ToString(value).Trim();
				}
				catch
				{
				}
			}

			if (string.IsNullOrEmpty(result))
			{
				result = defaultResult;
			}

			return result;
		}

		private static int CastToInt(object value, int defaultResult)
		{
			var result = -1;

			if (value != null)
			{
				try
				{
					result = Convert.ToInt32(value);
				}
				catch
				{
				}
			}

			if (result < 0)
			{
				result = defaultResult;
			}

			return result;
		}
	}
}