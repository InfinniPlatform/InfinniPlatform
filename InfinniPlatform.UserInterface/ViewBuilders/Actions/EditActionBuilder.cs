using InfinniPlatform.Api.Dynamic;
using InfinniPlatform.UserInterface.ViewBuilders.Data;
using InfinniPlatform.UserInterface.ViewBuilders.Views;

namespace InfinniPlatform.UserInterface.ViewBuilders.Actions
{
	sealed class EditActionBuilder : IObjectBuilder
	{
		public object Build(ObjectBuilderContext context, View parent, dynamic metadata)
		{
			var action = new BaseAction(parent);
			action.SetAction(() => ExecuteAction(context, parent, metadata));
			return action;
		}

		private static void ExecuteAction(ObjectBuilderContext context, View parent, dynamic metadata)
		{
			// Определение источника данных родительского представления
			IDataSource parentDataSource = parent.GetDataSource(metadata.DataSource);

			if (parentDataSource != null)
			{
				// Определение выделенного элемента в родительском представлении 
				var editItem = parentDataSource.GetSelectedItem();
				var idProperty = parentDataSource.GetIdProperty();

				if (editItem != null && string.IsNullOrEmpty(idProperty) == false)
				{
					// Определение уникального идентификатора выделенного элемента, поскольку родительское
					// представление может отображать только проекцию данных редактируемого агрегата
					var editItemId = editItem.GetProperty(idProperty) as string;

					if (editItemId != null)
					{
						ViewHelper.ShowView(editItemId,
											() => context.Build(parent, metadata.View),
											childDataSource => OnInitializeChildView(parentDataSource, childDataSource, editItemId),
											childDataSource => OnAcceptedChildView(parentDataSource));
					}
				}
			}
		}

		private static void OnInitializeChildView(IDataSource parentDataSource, IDataSource childDataSource, string editItemId)
		{
			childDataSource.SuspendUpdate();
			childDataSource.SetEditMode();
			childDataSource.SetIdFilter(editItemId);
			childDataSource.SetConfigId(parentDataSource.GetConfigId());
			childDataSource.SetDocumentId(parentDataSource.GetDocumentId());
			childDataSource.ResumeUpdate();
		}

		private static void OnAcceptedChildView(IDataSource parentDataSource)
		{
			parentDataSource.UpdateItems();
		}
	}
}