using System;
using System.Windows;

using InfinniPlatform.UserInterface.Properties;
using InfinniPlatform.UserInterface.ViewBuilders.Data;
using InfinniPlatform.UserInterface.ViewBuilders.Views;

namespace InfinniPlatform.UserInterface.ViewBuilders.Actions
{
	sealed class SaveActionBuilder : IObjectBuilder
	{
		public object Build(ObjectBuilderContext context, View parent, dynamic metadata)
		{
			var action = new BaseAction(parent);
			action.SetAction(() => ExecuteAction(parent, metadata));
			return action;
		}

		private static void ExecuteAction(View parent, dynamic metadata)
		{
			if (parent.Validate())
			{
				// Определение источника данных родительского представления
				IDataSource parentDataSource = parent.GetDataSource(metadata.DataSource);

				if (parentDataSource != null)
				{
					// Определение выделенного элемента в родительском представлении 
					var editItem = parentDataSource.GetSelectedItem();

					if (editItem != null)
					{
						// Todo: Клиентская валидация

						try
						{
							// Операция вызывается синхронно, так как нет смысла усложнять этот момент
							parentDataSource.SaveItem(editItem);
							parent.SetDialogResult(DialogResult.Accepted);

							if (metadata.CanClose == null || Convert.ToBoolean(metadata.CanClose))
							{
								parent.Close();
							}
						}
						catch
						{
							// Обработка ошибок сохранения находится на уровне представления
						}
					}
				}
			}
			else
			{
				MessageBox.Show(Resources.SaveValidateViewError, parent.GetText(), MessageBoxButton.OK, MessageBoxImage.Warning);
			}
		}
	}
}