using InfinniPlatform.UserInterface.ViewBuilders.Data;
using InfinniPlatform.UserInterface.ViewBuilders.Views;

namespace InfinniPlatform.UserInterface.ViewBuilders.Actions
{
	abstract class BaseItemActionBuilder : IObjectBuilder
	{
		public object Build(ObjectBuilderContext context, View parent, dynamic metadata)
		{
			var action = new BaseItemAction(parent);

			IElementDataBinding itemsDataBinding = context.Build(parent, metadata.Items);

			if (itemsDataBinding != null)
			{
				itemsDataBinding.OnPropertyValueChanged += (c, a) => action.SetItems(a.Value);
				action.OnValueChanged += (c, a) => itemsDataBinding.SetPropertyValue(a.Value);

				var sourceItemsDataBinding = itemsDataBinding as ISourceDataBinding;

				if (sourceItemsDataBinding != null)
				{
					var dataSourceName = sourceItemsDataBinding.GetDataSource();
					var propertyName = sourceItemsDataBinding.GetProperty();

					// Публикация сообщений в шину при возникновении событий

					action.NotifyWhenEventAsync(i => i.OnSetSelectedItem, a =>
					                                                      {
						                                                      a.DataSource = dataSourceName;
						                                                      a.Property = propertyName;
						                                                      return true;
					                                                      });

					// Подписка на сообщения шины от внешних элементов

					action.SubscribeOnEvent(OnSetSelectedItem, a => a.DataSource == dataSourceName
																	&& a.Property == propertyName);
				}
			}

			action.SetAction(() => ExecuteAction(context, action, metadata));

			return action;
		}


		protected abstract void ExecuteAction(ObjectBuilderContext context, BaseItemAction action, dynamic metadata);


		// Обработчики сообщений шины (внимание: наименования обработчиков совпадают с наименованиями событий)

		private static void OnSetSelectedItem(BaseItemAction target, dynamic arguments)
		{
			target.SetSelectedItem(arguments.Value);
		}
	}
}