using InfinniPlatform.UserInterface.ViewBuilders.Elements;
using InfinniPlatform.UserInterface.ViewBuilders.Views;

namespace InfinniPlatform.UserInterface.ViewBuilders.DataElements.DataNavigation
{
	sealed class DataNavigationElementBuilder : IObjectBuilder
	{
		public object Build(ObjectBuilderContext context, View parent, dynamic metadata)
		{
			var dataNavigation = new DataNavigationElement(parent);
			dataNavigation.ApplyElementMeatadata((object)metadata);

			// Установка ссылки на источник данных для страниц
			dataNavigation.SetDataSource(metadata.DataSource);

			// Добавление скриптовых подписчиков на события панели
			if (parent != null)
			{
				if (metadata.OnUpdateItems != null)
				{
					dataNavigation.OnUpdateItems += parent.GetScript(metadata.OnUpdateItems);
				}

				if (metadata.OnSetPageNumber != null)
				{
					dataNavigation.OnSetPageNumber += parent.GetScript(metadata.OnSetPageNumber);
				}

				if (metadata.OnSetPageSize != null)
				{
					dataNavigation.OnSetPageSize += parent.GetScript(metadata.OnSetPageSize);
				}
			}

			// Публикация сообщений в шину при возникновении событий
			dataNavigation.NotifyWhenEventAsync(i => i.OnUpdateItems);
			dataNavigation.NotifyWhenEventAsync(i => i.OnSetPageNumber);
			dataNavigation.NotifyWhenEventAsync(i => i.OnSetPageSize);

			// Подписка на сообщения шины от внешних элементов
			dataNavigation.SubscribeOnEvent(OnPageNumberChanged);
			dataNavigation.SubscribeOnEvent(OnPageSizeChanged);

			// Установка номера страницы и ее размеров
			dataNavigation.SetPageNumber((metadata.PageNumber as int?) ?? 0);
			dataNavigation.SetAvailablePageSizes(metadata.AvailablePageSizes);
			dataNavigation.SetPageSize(metadata.PageSize);

			return dataNavigation;
		}


		// Обработчики сообщений шины (внимание: наименования обработчиков совпадают с наименованиями событий)

		private static void OnPageNumberChanged(DataNavigationElement dataNavigation, dynamic arguments)
		{
			if (CanHandle(dataNavigation, arguments))
			{
				dataNavigation.SetPageNumber(arguments.Value);
			}
		}

		private static void OnPageSizeChanged(DataNavigationElement dataNavigation, dynamic arguments)
		{
			if (CanHandle(dataNavigation, arguments))
			{
				dataNavigation.SetPageSize(arguments.Value);
			}
		}

		private static bool CanHandle(DataNavigationElement dataNavigation, dynamic arguments)
		{
			return (dataNavigation.GetDataSource() == arguments.DataSource);
		}
	}
}